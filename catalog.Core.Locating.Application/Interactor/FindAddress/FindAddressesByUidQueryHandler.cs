using AutoMapper;
using catalog.Core.Locating.Application.Use;
using catalog.Core.Locating.Application.Use.Query.FindAddress;

namespace catalog.Core.Locating.Application.Interactor.FindAddress;

/// <summary>
/// Найти адреса по уникальным идентификаторам
/// </summary>
public class FindAddressesByUidQueryHandler : IFindAddressesByUidQueryHandler
{
    private readonly ILocationGetter locationGetter;
    private readonly IMapper mapper;

    public FindAddressesByUidQueryHandler(ILocationGetter locationGetter, IMapper mapper)
    {
        this.locationGetter = locationGetter;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyDictionary<Guid, AddressData>> HandleAsync(FindAddressesByUidQuery query)
    {
        return mapper.Map<IReadOnlyDictionary<Guid, AddressData>>(await locationGetter.GetAsync(query.LocationUids));
    }
}
