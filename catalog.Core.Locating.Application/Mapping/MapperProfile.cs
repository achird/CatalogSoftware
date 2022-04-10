using AutoMapper;
using catalog.Core.Locating.Application.Use.Query.FindAddress;
using catalog.Core.Locating.Application.Use.Query.GetLongLocation;
using catalog.Core.Locating.Application.Use.Query.GetShortLocation;
using catalog.Core.Locating.Domain.Location;
using catalog.Core.Locating.Domain.Region;

namespace catalog.Core.Locating.Application.Mapping;

/// <summary>
/// Маппинг компонента
/// </summary>
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Преобразование ValueObjects
        CreateMap<long, LocationObjectId>().ConvertUsing(r => new LocationObjectId(r));
        CreateMap<LocationObjectId, long>().ConvertUsing(r => r);
        CreateMap<long, RegionId>().ConvertUsing(r => new RegionId(r));
        CreateMap<string, RegionCode>().ConvertUsing(r => new RegionCode(r));
        CreateMap<DateTime, RegionVersion>().ConvertUsing(r => new RegionVersion(r));
        CreateMap<int, LocationType>().ConvertUsing(new LocationTypeTypeConverter());
        CreateMap<LocationType, int>().ConvertUsing(v => v.Code);
        CreateMap<string, LocationPlainCode>().ConvertUsing(v => LocationPlainCode.Create(v));
        CreateMap<string, LocationPostCode>().ConvertUsing(v => LocationPostCode.Create(v));
        CreateMap<string, LocationOkato>().ConvertUsing(v => LocationOkato.Create(v));

        // DTOs
        CreateMap<Use.Command.ImportCompleteRegion.LocationObjectData, LocationObject>(MemberList.Source);
        CreateMap<Use.Command.ImportCompleteRegion.RegionData, Region>(MemberList.Source);
        CreateMap<Location, AddressData>(MemberList.Destination)
            .ForMember(p => p.House, opt => opt.MapFrom(src => src.House.ProperName))
            .ForMember(p => p.Apartment, opt => opt.MapFrom(src => src.Apartment.ProperName));
        CreateMap<Location, ShortLocationData>(MemberList.Destination);
        CreateMap<Location, LongLocationData>(MemberList.Destination);
        CreateMap<LocationObject, LocationObjectData>(MemberList.Destination);
    }
}
