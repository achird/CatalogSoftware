using catalog.Core.Exchange.Locating.Persistence;
using catalog.Core.Locating.Application.Use.Command.RemoveAllRegions;

namespace catalog.Core.Acl.Locating.LocatingRemoveAllRegions;

public class LocatingRemoveAllRegions : ILocatingRemoveAllRegions
{
    private readonly IRemoveAllRegionsCommandHandler removeAllRegionsCommandHandler;

    public LocatingRemoveAllRegions(IRemoveAllRegionsCommandHandler removeAllRegionsCommandHandler)
    {
        this.removeAllRegionsCommandHandler = removeAllRegionsCommandHandler;
    }

    public async Task RemoveAllAsync()
    {
        await removeAllRegionsCommandHandler.HandleAsync(new RemoveAllRegionsCommand());
    }
}
