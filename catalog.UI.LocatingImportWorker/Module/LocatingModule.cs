using catalog.Core.Acl.Locating.LocatingRemoveAllRegions;
using catalog.Core.Acl.Locating.LocatingSender;
using catalog.Core.Exchange.Locating.Input;
using catalog.Core.Exchange.Locating.Interactor.LoadLocating;
using catalog.Core.Exchange.Locating.Output;
using catalog.Core.Exchange.Locating.Persistence;
using catalog.Core.Exchange.Locating.Use.Command.LoadLocating;
using catalog.Core.Locating.Application.Interactor.ImportCompleteRegion;
using catalog.Core.Locating.Application.Interactor.RemoveAllRegions;
using catalog.Core.Locating.Application.Persistence.Repository;
using catalog.Core.Locating.Application.Use.Command.ImportCompleteRegion;
using catalog.Core.Locating.Application.Use.Command.RemoveAllRegions;
using catalog.Core.Locating.Domain;
using catalog.Core.Locating.Domain.Region;
using catalog.Infrastructure.Exchange.Locating;
using catalog.Persistence.DBLocating;
using catalog.Persistence.DBLocating.Context;
using catalog.Persistence.DBLocating.Repository;
using Microsoft.EntityFrameworkCore;

namespace catalog.UI.LocatingImportWorker.Module
{
    /// <summary>
    /// Модуль данных о местоположениях
    /// </summary>
    public static class LocatingModule
    {
        public static IServiceCollection AddLocating(this IServiceCollection services, IConfiguration configuration)
        {
            // регистрация параметров
            services.AddSingleton(new DbContextOptionsBuilder<LocatingContext>().UseSqlServer(configuration.GetConnectionString("Locating")).Options);
            // регистрация DbContext
            services.AddDbContext<LocatingContext>();

            //регистрация служб хранилища
            services.AddSingleton<IRegionRepository, RegionRepository>();

            //регистрация доменных служб
            services.AddSingleton<RegionSpeificationFactory>();
            services.AddSingleton<IIdGenerator, IdGenerator>();

            //регистрация служб ACL
            services.AddScoped<ILocatingGetter, LocatingGetter>();
            services.AddScoped<ILocatingSender, LocatingSender>();
            services.AddScoped<ILocatingRemoveAllRegions, LocatingRemoveAllRegions>();

            //регистрация обработчиков команд
            services.AddScoped<ILoadLocatingCommandHandler, LoadLocatingCommandHandler>();
            services.AddScoped<IRemoveAllRegionsCommandHandler, RemoveAllRegionsCommandHandler>();
            services.AddScoped<IImportCompleteRegionCommandHandler, ImportCompleteRegionCommandHandler>();
            return services;
        }

        /// <summary>
        /// Создать базу данных и применить миграции
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static async Task MigrateDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            // DbLocating
            var locatingContext = scope.ServiceProvider.GetRequiredService<LocatingContext>();
            await locatingContext.Database.MigrateAsync();
            await locatingContext.Database.EnsureCreatedAsync();
        }
    }
}