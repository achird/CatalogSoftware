using AutoMapper;
using catalog.Core.Locating.Application.Use;
using catalog.Core.Locating.Application.Use.Query.GetShortLocation;
using catalog.Core.Locating.Domain.Location;

namespace catalog.Core.Locating.Application.Interactor.GetShortLocation;

/// <summary>
/// Получить КРАТКУЮ информацию о местоположениях
/// </summary>
public class GetShortLocationsQueryHandler : IGetShortLocationsQueryHandler
{
    private readonly ILocationGetter locationGetter;
    private readonly IMapper mapper;

    public GetShortLocationsQueryHandler(ILocationGetter locationGetter, IMapper mapper)
    {
        this.locationGetter = locationGetter;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyDictionary<long, ShortLocationData>> HandleAsync(GetShortLocationsQuery query)
    {
        var locationIds = query.LocationIds.Select(id => new LocationObjectId(id)).ToList();
        return mapper.Map<IReadOnlyDictionary<long, ShortLocationData>>(await locationGetter.GetAsync(locationIds));
    }
}
