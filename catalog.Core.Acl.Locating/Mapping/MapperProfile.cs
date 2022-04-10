using AutoMapper;
using catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;
using catalog.Core.Locating.Application.Use.Command.ImportCompleteRegion;

namespace catalog.Core.Acl.Locating.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Преобразование в Dto
        CreateMap<XmlObject, LocationObjectData>(MemberList.Destination).ForMember(l => l.RegionId, opt => opt.Ignore());
        CreateMap<XmlRegion, RegionData>();
    }
}
