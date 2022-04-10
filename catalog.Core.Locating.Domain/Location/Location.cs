using catalog.Core.Locating.Domain.Region;
using catalog.Core.SharedKernel.Base.Domain;

namespace catalog.Core.Locating.Domain.Location
{
    /// <summary>
    /// Информация о местоположении.
    /// Строится на основе списка объектов местомоложений (LocationObject) в соответствии с иерархией объектов
    /// </summary>
    public class Location : Entity<LocationObjectId>
    {
        private readonly ILookup<LocationType, LocationObject> lookupOverObjects;
        private readonly IList<LocationObject> objects;
        private readonly LocationObject locationObject;
        public Location(IList<LocationObject> objects)
        {
            if (objects is null) throw new ArgumentNullException(nameof(objects));
            if (objects.Count == 0) throw new ArgumentException("Местоположение не задано");

            this.objects = objects;
            locationObject = objects.Last();
            lookupOverObjects = objects.ToLookup(o => o.LocationType);
            Id = locationObject.Id;

            // Провепяем адрес
            var parent = LocationObject.Default;
            Okato = LocationOkato.Empty;
            PostCode = LocationPostCode.Empty;
            PlainCode = LocationPlainCode.Empty;
            foreach (var locationObject in objects)
            {
                if (locationObject.ParentId != parent.Id) throw new ArgumentException("Цепочка адреса нарушена");
                if (locationObject.PlainCode != LocationPlainCode.Empty) PlainCode = locationObject.PlainCode;
                if (locationObject.PostCode != LocationPostCode.Empty) PostCode = locationObject.PostCode;
                if (locationObject.Okato != LocationOkato.Empty) Okato = locationObject.Okato;
                parent = locationObject;
            }
        }

        /// <summary>
        /// Идентификатор родителя
        /// </summary>
        public long ParentId => locationObject.ParentId;
        /// <summary>
        /// Уникальный идентификатор объекта из справочника ФИАС
        /// </summary>
        public Guid Uid => locationObject.Uid;
        /// <summary>
        /// Регион
        /// </summary>
        public RegionId RegionId => locationObject.RegionId;
        /// <summary>
        /// Уровень объекта
        /// </summary>
        public LocationType LocationType => locationObject.LocationType;
        /// <summary>
        /// Уровень "Субъект РФ"
        /// </summary>
        public LocationObject Region => GetLocation(LocationType.Region);
        /// <summary>
        /// Уровень "Административный район"
        /// </summary>
        public LocationObject Area => GetLocation(LocationType.Area);
        /// <summary>
        /// Уровень "Город"
        /// </summary>
        public LocationObject City => GetLocation(LocationType.City);
        /// <summary>
        /// Уровень "Населенный пункт"
        /// </summary>
        public LocationObject Place => GetLocation(LocationType.Place);
        /// <summary>
        /// Уровень "Элемент планировочной структуры"
        /// </summary>
        public LocationObject Struct => GetLocation(LocationType.Struct);
        /// <summary>
        /// Уровень "Элемент улично-дорожной сети"
        /// </summary>
        public LocationObject Street => GetLocation(LocationType.Street);
        /// <summary>
        /// Уровень "Здание (сооружение)"
        /// </summary>
        public LocationObject House => GetLocation(LocationType.House);
        /// <summary>
        /// Уровень "Квартира (помещение)"
        /// </summary>
        public LocationObject Apartment => GetLocation(LocationType.Apartment);

        /// <summary>
        /// Адрес до улицы
        /// </summary>
        public string Locality => GetAddress(LocationType.Struct);

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address => objects is not null ? string.Join(", ", objects.Select(o => o.Name)) : string.Empty;
        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public string MailingAddress => PostCode == LocationPostCode.Empty ? Address : string.Join(", ", PostCode.Value, Address);
        /// <summary>
        /// Код адресного объекта одной строкой без признака актуальности
        /// </summary>
        public LocationPlainCode PlainCode { get; }
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public LocationPostCode PostCode { get; }
        /// <summary>
        /// OKATO
        /// </summary>
        public LocationOkato Okato { get; }
        /// <summary>
        /// Польное наименование объекта
        /// </summary>
        public string Name => locationObject.Name;
        /// <summary>
        /// Наименование объекта
        /// </summary>
        public string ProperName => locationObject.ProperName;

        /// <summary>
        /// Получить строку адреса до определенного уровня
        /// </summary>
        /// <param name="locationType">Уровень местоположения</param>
        /// <returns></returns>
        public string GetAddress(LocationType locationType)
        {
            if (objects is null)
                return string.Empty;

            return string.Join(", ", objects.Where(o => o.LocationType.Code <= locationType.Code));
        }

        /// <summary>
        /// Получить указанный уровень местоположения
        /// </summary>
        /// <param name="locationType">Уровень местоположения</param>
        /// <returns></returns>
        private LocationObject GetLocation(LocationType locationType)
        {
            if (lookupOverObjects is not null && lookupOverObjects.Contains(locationType))
                return lookupOverObjects[locationType].First();

            return LocationObject.Default;
        }

        /// <summary>
        /// Представление местоположения в виде строки
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Address;

        /// <summary>
        /// Местоположение по-умолчанию
        /// </summary>
        public static readonly Location Undefined = new(new List<LocationObject>() { LocationObject.Default });
    }
}
