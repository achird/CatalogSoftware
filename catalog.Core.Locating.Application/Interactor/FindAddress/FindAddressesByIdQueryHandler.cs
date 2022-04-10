using AutoMapper;
using catalog.Core.Locating.Application.Use;
using catalog.Core.Locating.Application.Use.Query.FindAddress;
using catalog.Core.Locating.Domain.Location;

namespace catalog.Core.Locating.Application.Interactor.FindAddress;

/// <summary>
/// Найти адреса по уникальным идентификаторам
/// </summary>
public class FindAddressesByIdQueryHandler : IFindAddressesByIdQueryHandler
{
    private readonly ILocationGetter locationGetter;
    private readonly IMapper mapper;

    public FindAddressesByIdQueryHandler(ILocationGetter locationGetter, IMapper mapper)
    {
        this.locationGetter = locationGetter;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyDictionary<long, AddressData>> HandleAsync(FindAddressesByIdQuery query)
    {
        var locationIds = query.LocationIds.Select(id => new LocationObjectId(id)).ToList();
        return mapper.Map<IReadOnlyDictionary<long, AddressData>>(await locationGetter.GetAsync(locationIds));
    }
}
