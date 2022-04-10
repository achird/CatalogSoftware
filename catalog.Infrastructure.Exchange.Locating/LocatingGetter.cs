using catalog.Core.Exchange.Locating.Input;
using catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;
using catalog.Infrastructure.Utility.XmlConverter;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.Globalization;
using System.Text.RegularExpressions;

namespace catalog.Infrastructure.Exchange.Locating;

/// <summary>
/// Получить данные регионов.
/// Загрузка данных из xml файлов ГАР (Государственного адресного регистра).
/// </summary>
public class LocatingGetter : ILocatingGetter
{
    private readonly string objectFileName, houseFileName, apartmentFileName, hierarchyFileName, parameterAddName;
    private readonly CultureInfo enUS = new("en-US");
    private readonly ISchema xmlAddressObjectSchema;
    private readonly ISchema xmlHouseObjectSchema;
    private readonly ISchema xmlApartmentObjectSchema;
    private readonly ISchema xmlParameterSchema;
    private readonly ISchema xmlHierarchySchema;
    private readonly string compressFile = "gar_xml.zip";

    public LocatingGetter()
    {
        objectFileName = "AS_ADDR_OBJ";
        houseFileName = "AS_HOUSES";
        apartmentFileName = "AS_APARTMENTS";
        hierarchyFileName = "AS_ADM_HIERARCHY";
        parameterAddName = "PARAM";

        #region XmlSchema
        xmlAddressObjectSchema = new Schema()
            .Add("ADDRESSOBJECTS")
            .Object("OBJECT")
                .PropertyNumber("OBJECTID", "Id")
                .PropertyString("OBJECTGUID", "Uid")
                .PropertyNumber("NEXTID", "NextId")
                .PropertyString("NAME", "ObjectName")
                .PropertyString("TYPENAME", "ObjectType")
                .PropertyNumber("LEVEL", "ObjectLevel");
        xmlHouseObjectSchema = new Schema()
            .Add("HOUSES")
            .Object("HOUSE")
                .PropertyNumber("OBJECTID", "Id")
                .PropertyString("OBJECTGUID", "Uid")
                .PropertyNumber("NEXTID", "NextId")
                .PropertyNumber("HOUSETYPE", "HouseType1")
                .PropertyString("HOUSENUM", "HouseNumber1")
                .PropertyNumber("ADDTYPE1", "HouseType2")
                .PropertyString("ADDNUM1", "HouseNumber2")
                .PropertyNumber("ADDTYPE2", "HouseType3")
                .PropertyString("ADDNUM2", "HouseNumber3");
        xmlApartmentObjectSchema = new Schema()
            .Add("APARTMENTS")
            .Object("APARTMENT")
                .PropertyNumber("OBJECTID", "Id")
                .PropertyString("OBJECTGUID", "Uid")
                .PropertyNumber("NEXTID", "NextId")
                .PropertyNumber("APARTTYPE", "ApartmentType")
                .PropertyString("NUMBER", "ApartmentNumber");
        xmlParameterSchema = new Schema()
            .Add("PARAMS")
            .Object("PARAM")
                .PropertyNumber("OBJECTID", "ObjectId")
                .PropertyNumber("TYPEID", "Type")
                .PropertyString("VALUE", "Value")
                .PropertyDateTime("STARTDATE", "StartDate");
        xmlHierarchySchema = new Schema()
            .Add("ITEMS")
            .Object("ITEM")
                .PropertyNumber("OBJECTID", "Id")
                .PropertyNumber("PARENTOBJID", "ParentId")
                .PropertyDateTime("ENDDATE", "EndDate")
                .PropertyNumber("ISACTIVE", "IsActive");
        #endregion
    }

    /// <summary>
    /// Загрузка объектов из xml-файла
    /// </summary>
    /// <typeparam name="T">Тип объектов</typeparam>
    /// <param name="regionCode">Код региона</param>
    /// <param name="searchPattern">Название файла xml</param>
    /// <param name="objectName">Тип объекта в схеме</param>
    /// <param name="xmlSchema">Схема xml файла</param>
    /// <returns></returns>
    private IEnumerable<IEnumerable<T>> GetObject<T>(string xmlPath, string regionCode, string searchPattern, string objectName, ISchema xmlSchema)
    {
        var xmlFiles = Directory.EnumerateFiles(Path.Combine(xmlPath, regionCode), $"{searchPattern}*.xml");
        if (xmlFiles.Any())
        {
            var xmlFile = xmlFiles.First();
            using (var xmlReader = new XmlObjectReader(xmlSchema, xmlFile))
            {
                while (xmlReader.ReadCollection(objectName, 25000, out IEnumerable<T> collection))
                {
                    yield return collection;
                }
            }
            File.Delete(xmlFile);
        }
    }

    /// <summary>
    /// Распаковать данные региона из архива
    /// </summary>
    /// <param name="xmlPath">Путь к данным</param>
    /// <param name="regionCode">Код региона</param>
    /// <returns></returns>
    private bool TryExtractLocating(string xmlPath, string regionCode)
    {
        if (File.Exists(Path.Combine(xmlPath, compressFile)))
        {
            using var archive = ArchiveFactory.Open(Path.Combine(xmlPath, compressFile));
            var entries = archive.Entries.Where(e =>
                e.Key.StartsWith(regionCode) &&
                (e.Key.Contains(objectFileName) ||
                 e.Key.Contains(houseFileName) ||
                 e.Key.Contains(apartmentFileName) ||
                 e.Key.Contains(hierarchyFileName)));
            if (entries.Count() != 8) return false;
            foreach (var entry in entries)
                entry.WriteToDirectory(xmlPath, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });

            return true;
        }

        return false;
    }

    /// <summary>
    /// Загрузить регион
    /// </summary>
    /// <param name="xmlPath">Путь к данным</param>
    /// <param name="region">Регион</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private XmlRegion LoadLocatingData(string xmlPath, XmlRegion region)
    {
        if (TryExtractLocating(xmlPath, region.RegionCode))
        {
            var xmlHierarchies = new Dictionary<long, XmlHierarchy>();
            var xmlObjects = new Dictionary<long, XmlObject>();

            // Загрузка иерархии
            foreach (var collection in GetObject<XmlHierarchy>(xmlPath, region.RegionCode, hierarchyFileName, "ITEMS.ITEM", xmlHierarchySchema))
            {
                foreach (var item in collection)
                {
                    if (xmlHierarchies.TryGetValue(item.Id, out XmlHierarchy? hierarchy) &&
                        (hierarchy.IsActive < item.IsActive || hierarchy.EndDate < item.EndDate))
                        xmlHierarchies[item.Id] = item;

                    xmlHierarchies.TryAdd(item.Id, item);
                }
            }

            // Загрузка параметров
            IReadOnlyDictionary<int, XmlParameter> GetParameters(IEnumerable<XmlParameter> parameters)
            {
                var current = new Dictionary<int, XmlParameter>();
                if (parameters == null) return current;

                foreach (var item in parameters)
                {
                    if (current.TryGetValue(item.Type, out XmlParameter? parameter) &&
                        parameter.StartDate < item.StartDate)
                        current[item.Type] = item;
                    current.TryAdd(item.Type, item);
                }
                return current;
            }

            // Загрузка объектов
            void GetObjects<Source>(string startsWith, string addressObject, ISchema schema) where Source : XmlObject
            {
                var parameters = new List<XmlParameter>();
                // Type = 5 (PostCode) || Type = 6 (Okato) || Type = 11 (PlainCode)
                foreach (var collection in GetObject<XmlParameter>(xmlPath, region.RegionCode, $"{startsWith}_{parameterAddName}", "PARAMS.PARAM", xmlParameterSchema))
                    parameters.AddRange(collection.Where(p => p.Type == 5 || p.Type == 6 || p.Type == 11));

                var lookupOverParameters = parameters.ToLookup(p => p.ObjectId);
                foreach (var collection in GetObject<Source>(xmlPath, region.RegionCode, startsWith, addressObject, schema))
                {
                    foreach (var item in collection.Where(o => o.NextId == null || o.NextId == 0))
                    {
                        if (xmlHierarchies.TryGetValue(item.Id, out XmlHierarchy? hierarchy))
                        {
                            item.ParentId = hierarchy.ParentId;
                            // Заполняем параметры
                            var local = GetParameters(lookupOverParameters[item.Id]);
                            if (local.TryGetValue(5, out XmlParameter? parameter)) item.PostCode = parameter.Value;
                            if (local.TryGetValue(6, out parameter)) item.Okato = parameter.Value;
                            if (local.TryGetValue(11, out parameter)) item.PlainCode = parameter.Value;
                            xmlObjects.TryAdd(item.Id, item);
                        }
                    }
                }
            }

            // установить полную иерархию объекта
            void SetHierarchies(XmlObject xmlObject)
            {
                xmlObject.Structure = new List<long>();
                var current = xmlObject;
                do
                {
                    xmlObject.Structure.Add(current.Id);
                } while (xmlObjects.TryGetValue(current.ParentId, out current) && current.Structure is null);
                xmlObject.Structure.Reverse();
                if (current is not null)
                {
                    xmlObject.Structure = current.Structure.Union(xmlObject.Structure).ToList();
                }
            }

            // Загрузка адресных объектов
            GetObjects<XmlLocation>(objectFileName, "ADDRESSOBJECTS.OBJECT", xmlAddressObjectSchema);
            // Загрузка данных о домах
            GetObjects<XmlHouse>(houseFileName, "HOUSES.HOUSE", xmlHouseObjectSchema);
            // Загрузка данных о квартирах
            GetObjects<XmlApartment>(apartmentFileName, "APARTMENTS.APARTMENT", xmlApartmentObjectSchema);
            // Иерархия объектов
            foreach (var xmlObject in xmlObjects.Values)
                SetHierarchies(xmlObject);

            Directory.Delete(Path.Combine(xmlPath, region.RegionCode), true);
            region.Objects = xmlObjects.Values.ToList();
            return region;
        }

        throw new InvalidOperationException($"Регион с кодом {region.RegionCode} не найден");
    }

    /// <summary>
    /// Поиск всех доступных для загрузки регионов
    /// </summary>
    /// <returns></returns>
    private IList<XmlRegion> LoadAvailableLocating(string xmlPath)
    {
        if (!File.Exists(Path.Combine(xmlPath, compressFile)))
            throw new InvalidOperationException($"Архив с данными {compressFile} не найден");

        using var archive = ArchiveFactory.Open(Path.Combine(xmlPath, compressFile));
        var regions = new List<XmlRegion>();
        foreach (var regionCode in archive.Entries.Where(e => Regex.IsMatch(e.Key, @"^[0-9]{2}\\*")).Select(e => e.Key[..2]).Distinct())
        {
            var xmlEntry = archive.Entries.Single(e => e.Key.StartsWith(regionCode) && e.Key.Contains(hierarchyFileName));
            var match = Regex.Match(Path.GetFileNameWithoutExtension(xmlEntry.Key), "_(?<date>[0-9]{8})_");
            if (match.Success && DateTime.TryParseExact(match.Groups["date"].Value, "yyyyMMdd", enUS, DateTimeStyles.None, out DateTime updateDate))
                regions.Add(new XmlRegion()
                {
                    RegionCode = regionCode,
                    Version = updateDate,
                    Size = archive.Entries.Where(e =>
                        e.Key.StartsWith(regionCode) &&
                        (e.Key.Contains(objectFileName) ||
                         e.Key.Contains(houseFileName) ||
                         e.Key.Contains(apartmentFileName) ||
                         e.Key.Contains(hierarchyFileName))).Sum(e => e.Size)
                });
        }
        return regions;
    }

    /// <summary>
    /// Получить список доступных регионов
    /// </summary>
    /// <returns>Данные региона</returns>
    public IList<XmlRegion> GetAllAvailableLocating(string xmlPath)
    {
        return LoadAvailableLocating(xmlPath);
    }

    /// <summary>
    /// Загрузить данные региона
    /// </summary>
    /// <returns>Данные региона</returns>
    public XmlRegion GetLocatingData(string xmlPath, XmlRegion region)
    {
        return LoadLocatingData(xmlPath, new XmlRegion()
        {
            RegionCode = region.RegionCode,
            Version = region.Version,
            Size = region.Size
        });
    }
}
