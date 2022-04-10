using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Command.ImportCompleteRegion;

/// <summary>
/// Обработчик команды для обновления адресного справочника
/// </summary>
public interface IImportCompleteRegionCommandHandler : ICommandHandler<ImportCompleteRegionCommand>
{
}
