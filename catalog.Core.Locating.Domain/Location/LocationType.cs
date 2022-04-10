using catalog.Core.SharedKernel.Base.Domain;

namespace catalog.Core.Locating.Domain.Location
{
    /// <summary>
    /// Указывает уровень местоположения
    /// </summary>
    public class LocationType : ValueObject<LocationType>, IComparable<LocationType>
    {
        public LocationType(int code)
        {
            Code = code;
            Name = code switch
            {
                0 => Undefined.Name,
                1 => Region.Name,
                2 => Area.Name,
                3 => City.Name,
                4 => Place.Name,
                5 => Struct.Name,
                6 => Street.Name,
                7 => House.Name,
                8 => Apartment.Name,
                _ => throw new ArgumentException($"Указанный код {code} не определен для LocationType")
            };
        }
        private LocationType(int code, string name)
        {
            Code = code;
            Name = name;
        }

        /// <summary>
        /// Уровень местоположения
        /// </summary>
        public int Code { get; }
        /// <summary>
        /// Расшифровка уровня местоположения
        /// </summary>
        public string Name { get; }

        public int CompareTo(LocationType other)
        {
            return Code.CompareTo(other.Code);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

        /// <summary>
        /// Уровень местоположения - "не определен"
        /// </summary>
        public static readonly LocationType Undefined = new(0, string.Empty);
        /// <summary>
        /// Уровень местоположения - "Субъект РФ"
        /// </summary>
        public static readonly LocationType Region = new(1, "Субъект РФ");
        /// <summary>
        /// Уровень местоположения - "Административный район"
        /// </summary>
        public static readonly LocationType Area = new(2, "Административный район");
        /// <summary>
        /// Уровень местоположения - "Город"
        /// </summary>
        public static readonly LocationType City = new(3, "Город");
        /// <summary>
        /// Уровень местоположения - "Населенный пункт"
        /// </summary>
        public static readonly LocationType Place = new(4, "Населенный пункт");
        /// <summary>
        /// Уровень местоположения - "Элемент планировочной структуры"
        /// </summary>
        public static readonly LocationType Struct = new(5, "Элемент планировочной структуры");
        /// <summary>
        /// Уровень местоположения - "Элемент улично-дорожной сети"
        /// </summary>
        public static readonly LocationType Street = new(6, "Элемент улично-дорожной сети");
        /// <summary>
        /// Уровень местоположения - "Здание (сооружение)"
        /// </summary>
        public static readonly LocationType House = new(7, "Здание (сооружение)");
        /// <summary>
        /// Уровень местоположения - "Квартира (помещение)"
        /// </summary>
        public static readonly LocationType Apartment = new(8, "Квартира (помещение)");
    }
}
