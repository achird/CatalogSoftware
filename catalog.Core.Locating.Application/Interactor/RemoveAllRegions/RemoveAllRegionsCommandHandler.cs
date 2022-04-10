using catalog.Core.Locating.Application.Persistence.Repository;
using catalog.Core.Locating.Application.Use.Command.RemoveAllRegions;

namespace catalog.Core.Locating.Application.Interactor.RemoveAllRegions;

/// <summary>
/// Обработчик команды удаления всех регионов
/// </summary>
public class RemoveAllRegionsCommandHandler : IRemoveAllRegionsCommandHandler
{
    private readonly IRegionRepository regionRepository;

    public RemoveAllRegionsCommandHandler(IRegionRepository regionRepository)
    {
        this.regionRepository = regionRepository;
    }
    public async Task HandleAsync(RemoveAllRegionsCommand command)
    {
        await regionRepository.RemoveAllAsync();
    }
}
