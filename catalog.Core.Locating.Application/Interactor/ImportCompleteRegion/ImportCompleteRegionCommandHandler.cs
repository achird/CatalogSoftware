using AutoMapper;
using catalog.Core.Application.Common.Service;
using catalog.Core.Locating.Application.Persistence.Repository;
using catalog.Core.Locating.Application.Use.Command.ImportCompleteRegion;
using catalog.Core.Locating.Domain;
using catalog.Core.Locating.Domain.Region;

namespace catalog.Core.Locating.Application.Interactor.ImportCompleteRegion;

/// <summary>
/// Обработчик команды импорта данных региона справочника ГАР
/// </summary>
public class ImportCompleteRegionCommandHandler : IImportCompleteRegionCommandHandler
{
    private readonly IRegionRepository regionRepository;
    private readonly IIdGenerator idGenerator;
    private readonly ILogger logger;
    private readonly IMapper mapper;

    public ImportCompleteRegionCommandHandler(ILogger logger, IMapper mapper, IIdGenerator idGenerator, IRegionRepository regionRepository)
    {
        this.logger = logger;
        this.regionRepository = regionRepository;
        this.idGenerator = idGenerator;
        this.mapper = mapper;
    }
    public async Task HandleAsync(ImportCompleteRegionCommand command)
    {
        foreach (var regionData in command.Regions)
            regionData.Id = idGenerator.NewId<RegionId>();
        Parallel.ForEach(command.Regions, regionData => Parallel.ForEach(regionData.Objects, o => o.RegionId = regionData.Id));
        await regionRepository.AddAsync(mapper.Map<IList<Region>>(command.Regions));
        logger.Info($"загружены регионы [{string.Join(",", command.Regions.Select(r => r.RegionCode))}] - сохранено {command.Regions.Sum(r => r.Objects.Count)} записей");
    }
}
