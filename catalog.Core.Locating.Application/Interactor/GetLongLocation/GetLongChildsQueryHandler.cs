using AutoMapper;
using catalog.Core.Locating.Application.Use;
using catalog.Core.Locating.Application.Use.Query.GetLongLocation;
using catalog.Core.Locating.Domain.Location;

namespace catalog.Core.Locating.Application.Interactor.GetLongLocation;

/// <summary>
/// Получить ПОЛНУЮ информацию о подчиненных местоположениях
/// </summary>
public class GetLongChildsQueryHandler : IGetLongChildsQueryHandler
{
    private readonly ILocationGetter locationGetter;
    private readonly IMapper mapper;

    public GetLongChildsQueryHandler(ILocationGetter locationGetter, IMapper mapper)
    {
        this.locationGetter = locationGetter;
        this.mapper = mapper;
    }

    public async Task<IList<LongLocationData>> HandleAsync(GetLongChildsQuery query)
    {
        return mapper.Map<IList<LongLocationData>>(await locationGetter.GetChildsAsync(new LocationObjectId(query.ParentId)));
    }
}
