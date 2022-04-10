namespace catalog.Core.Locating.Application.Use.Query.GetShortLocation
{
    /// <summary>
    /// Краткая информация о местоположении
    /// </summary>
    public class ShortLocationData
    {
        /// <summary>
        /// Идентификатор местоположения
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Идентификатор родителя
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// Уникальный идентификатор объекта из справочника ФИАС
        /// </summary>
        public Guid Uid { get; set; }
        /// <summary>
        /// Уровень объекта
        /// </summary>
        public int LocationType { get; set; }
        /// <summary>
        /// Код адресного объекта одной строкой без признака актуальности
        /// </summary>
        public string PlainCode { get; set; }
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// OKATO
        /// </summary>
        public string Okato { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Полное наименование объекта
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Имя собственное
        /// </summary>
        public string ProperName { get; set; }
    }
}
