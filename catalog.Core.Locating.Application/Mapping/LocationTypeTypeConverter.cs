using AutoMapper;
using catalog.Core.Locating.Domain.Location;

namespace catalog.Core.Locating.Application.Mapping;

internal class LocationTypeTypeConverter : ITypeConverter<int, LocationType>
{
    Dictionary<int, LocationType> values = new();

    public LocationTypeTypeConverter()
    {
        values.Add(0, LocationType.Undefined);
        values.Add(1, LocationType.Region);
        values.Add(2, LocationType.Area);
        values.Add(3, LocationType.City);
        values.Add(4, LocationType.Place);
        values.Add(5, LocationType.Struct);
        values.Add(6, LocationType.Street);
        values.Add(7, LocationType.House);
        values.Add(8, LocationType.Apartment);
    }

    public LocationType Convert(int source, LocationType destination, ResolutionContext context)
    {
        if (!values.TryGetValue(source, out var locationType))
            locationType = LocationType.Undefined;

        return locationType;
    }
}
