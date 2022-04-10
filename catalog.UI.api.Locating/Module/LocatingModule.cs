using catalog.Core.Application.Common.Service;
using catalog.Core.Locating.Application.Interactor;
using catalog.Core.Locating.Application.Interactor.FindAddress;
using catalog.Core.Locating.Application.Interactor.GetLongLocation;
using catalog.Core.Locating.Application.Interactor.GetShortLocation;
using catalog.Core.Locating.Application.Persistence.QueryObject;
using catalog.Core.Locating.Application.Use;
using catalog.Core.Locating.Application.Use.Query.FindAddress;
using catalog.Core.Locating.Application.Use.Query.GetLongLocation;
using catalog.Core.Locating.Application.Use.Query.GetShortLocation;
using catalog.Core.Locating.Domain.Location;
using catalog.Persistence.DBLocating.Context;
using catalog.Persistence.DBLocating.QueryObject;
using Microsoft.EntityFrameworkCore;

namespace catalog.UI.api.Locating.Module;

/// <summary>
/// Модуль данных о местоположениях
/// </summary>
public static class LocatingModule
{
    public static IServiceCollection AddLocating(this IServiceCollection services, IConfiguration configuration)
    {
        // регистрация параметров
        services.AddSingleton(new DbContextOptionsBuilder<LocatingContext>().UseSqlServer(configuration.GetConnectionString("Locating")).Options);

        //регистрация служб хранилища
        services.AddSingleton<ILocationObjectQueryObject, LocationObjectQueryObject>();

        //регистрация доменных служб
        services.AddSingleton<LocationObjectSpecificationFactory>();
        services.AddSingleton<ILocationGetter>(provider => new LocationGetter(
            provider.GetRequiredService<LocationObjectSpecificationFactory>(),
            provider.GetRequiredService<ILocationObjectQueryObject>(),
            provider.GetRequiredService<ICacheFactory>().CreateComulativeCache(TimeSpan.FromMinutes(30))));
        services.Decorate<ILocationGetter>((inner, provider) => new CacheLocationGetter(inner,
            provider.GetRequiredService<ICacheFactory>().CreateComulativeCache(TimeSpan.FromMinutes(30)),
            provider.GetRequiredService<ICacheFactory>().CreateComulativeCache(TimeSpan.FromMinutes(30))));

        //регистрация служб ACL

        //регистрация обработчиков команд

        //регистрация обработчиков запросов
        services.AddScoped<IGetShortChildsQueryHandler, GetShortChildsQueryHandler>();
        services.AddScoped<IGetShortLocationsQueryHandler, GetShortLocationsQueryHandler>();
        services.AddScoped<IGetLongChildsQueryHandler, GetLongChildsQueryHandler>();
        services.AddScoped<IGetLongLocationsQueryHandler, GetLongLocationsQueryHandler>();
        services.AddScoped<IFindAddressesByUidQueryHandler, FindAddressesByUidQueryHandler>();
        services.AddScoped<IFindAddressesByIdQueryHandler, FindAddressesByIdQueryHandler>();
        return services;
    }
}
