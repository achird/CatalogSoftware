using AutoMapper;
using catalog.Core.Locating.Application.Use;
using catalog.Core.Locating.Application.Use.Query.GetShortLocation;
using catalog.Core.Locating.Domain.Location;

namespace catalog.Core.Locating.Application.Interactor.GetShortLocation;

/// <summary>
/// Получить КРАТКУЮ информацию о подчиненных местоположениях
/// </summary>
public class GetShortChildsQueryHandler : IGetShortChildsQueryHandler
{
    private readonly ILocationGetter locationGetter;
    private readonly IMapper mapper;

    public GetShortChildsQueryHandler(ILocationGetter locationGetter, IMapper mapper)
    {
        this.locationGetter = locationGetter;
        this.mapper = mapper;
    }

    public async Task<IList<ShortLocationData>> HandleAsync(GetShortChildsQuery query)
    {
        return mapper.Map<IList<ShortLocationData>>(await locationGetter.GetChildsAsync(new LocationObjectId(query.ParentId)));
    }
}
