using catalog.Core.Exchange.Locating.Use.Command.LoadLocating;

namespace catalog.UI.LocatingImportWorker;

public class Worker : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public Worker(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var loadLocatingCommandHandler = scope.ServiceProvider.GetRequiredService<ILoadLocatingCommandHandler>();
            await loadLocatingCommandHandler.HandleAsync(new LoadLocatingCommand());
        }
        catch (Exception)
        {
            throw;
        }
    }
}
