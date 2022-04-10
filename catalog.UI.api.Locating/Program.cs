using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyModel;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Reflection;
using catalog.UI.api.Locating.Module;
using catalog.Core.Application.Common.Service;
using catalog.Infrastructure.Cache;

/// <summary>
/// Список библиотек zms.Core
/// </summary>
Assembly[] GetAssemblies()
{
    var assemblies = new List<Assembly>();
    var dependencies = DependencyContext.Default.RuntimeLibraries;
    foreach (var library in dependencies)
    {
        if (library.Name.StartsWith("catalog.Core.") || library.Dependencies.Any(d => d.Name.StartsWith("catalog.Core.")))
        {
            var assembly = Assembly.Load(new AssemblyName(library.Name));
            assemblies.Add(assembly);
        }
    }
    return assemblies.ToArray();
}

var builder = WebApplication.CreateBuilder(args);
// Список зависимых библиотек
Assembly[] assemblies = GetAssemblies();
// Add base services
builder.Services.AddAutoMapper(assemblies);
builder.Services.AddSingleton<ICacheFactory, CacheFactory>();
// Add services to the container.
builder.Services.AddLocating(builder.Configuration);
builder.Services.AddControllers();
// Add versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = ApiVersion.Default;
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options
    => options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "catalog.UI.api.Locating",
        Version = "v1"
    }));
builder.Services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options
        => options.SwaggerEndpoint(
            url: "/swagger/v1/swagger.json",
            name: "catalog.UI.api.Locating v1"));
}

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

