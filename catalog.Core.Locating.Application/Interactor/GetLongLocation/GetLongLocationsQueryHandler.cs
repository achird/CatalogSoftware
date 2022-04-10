using AutoMapper;
using catalog.Core.Locating.Application.Use;
using catalog.Core.Locating.Application.Use.Query.GetLongLocation;
using catalog.Core.Locating.Domain.Location;

namespace catalog.Core.Locating.Application.Interactor.GetLongLocation;

/// <summary>
/// Получить ПОЛНУЮ информацию о местоположениях
/// </summary>
public class GetLongLocationsQueryHandler : IGetLongLocationsQueryHandler
{
    private readonly ILocationGetter locationGetter;
    private readonly IMapper mapper;

    public GetLongLocationsQueryHandler(ILocationGetter locationGetter, IMapper mapper)
    {
        this.locationGetter = locationGetter;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyDictionary<long, LongLocationData>> HandleAsync(GetLongLocationsQuery query)
    {
        var locationIds = query.LocationIds.Select(id => new LocationObjectId(id)).ToList();
        return mapper.Map<IReadOnlyDictionary<long, LongLocationData>>(await locationGetter.GetAsync(locationIds));
    }
}
