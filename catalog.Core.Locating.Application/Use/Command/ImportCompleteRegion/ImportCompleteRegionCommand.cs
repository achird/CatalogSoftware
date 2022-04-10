using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Command.ImportCompleteRegion;

/// <summary>
/// Команда для запуска обновления адресного справочника
/// </summary>
public class ImportCompleteRegionCommand : ICommand
{
    public IList<RegionData> Regions { get; set; }
}
