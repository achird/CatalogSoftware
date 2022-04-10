using catalog.Core.Application.Common.Service;
using catalog.Infrastructure.Cache;
using catalog.Infrastructure.IO.AppLocation;
using catalog.Infrastructure.IO.Logger;
using catalog.Infrastructure.Utility.FileSystem;
using catalog.UI.LocatingImportWorker;
using catalog.UI.LocatingImportWorker.Module;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

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


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        // Список зависимых библиотек
        Assembly[] assemblies = GetAssemblies();
        // Add AutoMapper
        services.AddAutoMapper(assemblies);
        // Add base services
        services.AddSingleton<catalog.Core.Application.Common.Service.ILogger, ConsoleFileLogger>();
        services.AddSingleton<IAppLocation>(_ => new AppLocation(@"C:\Repositories\ZmsSoftware"));
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ICacheFactory, CacheFactory>();
        services.AddLocating(hostContext.Configuration);
    })
    .Build();

await host.MigrateDatabaseAsync();
await host.RunAsync();
