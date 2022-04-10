using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Command.RemoveAllRegions
{
    /// <summary>
    /// Обработчик команды для удаления всех регионов
    /// </summary>
    public interface IRemoveAllRegionsCommandHandler : ICommandHandler<RemoveAllRegionsCommand>
    {
    }
}
