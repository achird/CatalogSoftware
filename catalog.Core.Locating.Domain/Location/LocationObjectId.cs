using catalog.Core.SharedKernel.Base.Domain;

namespace catalog.Core.Locating.Domain.Location
{
    /// <summary>
    /// Идентификатор информации о местоположении
    /// </summary>
    public class LocationObjectId : EntityId<LocationObjectId>
    {
        public LocationObjectId(long value)
        {
            Value = value;
        }

        /// <summary>
        /// Значение по умолчанию
        /// </summary>
        public static readonly LocationObjectId Default = new LocationObjectId(0);
    }
}
