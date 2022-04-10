using AutoMapper;
using catalog.Core.Exchange.Locating.Output;
using catalog.Core.Locating.Application.Use.Command.ImportCompleteRegion;

namespace catalog.Core.Acl.Locating.LocatingSender;

/// <summary>
/// Передать загруженный регион в компонент Locating
/// </summary>
public class LocatingSender : ILocatingSender
{
    private readonly IMapper mapper;
    private readonly IImportCompleteRegionCommandHandler importCompleteRegionCommandHandler;

    public LocatingSender(IMapper mapper, IImportCompleteRegionCommandHandler importCompleteRegionCommandHandler)
    {
        this.mapper = mapper;
        this.importCompleteRegionCommandHandler = importCompleteRegionCommandHandler ?? throw new ArgumentNullException(nameof(importCompleteRegionCommandHandler));
    }

    public async Task SendLocating(IList<Exchange.Locating.Interactor.LoadLocating.XmlEntities.XmlRegion> regions)
    {
        await importCompleteRegionCommandHandler.HandleAsync(new ImportCompleteRegionCommand()
        {
            Regions = mapper.Map<IList<RegionData>>(regions)
        });
    }
}
