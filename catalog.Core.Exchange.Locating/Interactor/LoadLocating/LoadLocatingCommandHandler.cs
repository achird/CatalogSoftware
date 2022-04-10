using catalog.Core.Application.Common.Service;
using catalog.Core.Exchange.Locating.Input;
using catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;
using catalog.Core.Exchange.Locating.Output;
using catalog.Core.Exchange.Locating.Persistence;
using catalog.Core.Exchange.Locating.Use.Command.LoadLocating;

namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating;

public class LoadLocatingCommandHandler : ILoadLocatingCommandHandler
{
    private readonly ILogger logger;
    private readonly ILocatingRemoveAllRegions locatingRemoveAllRegions;
    private readonly ILocatingGetter locatingGetter;
    private readonly ILocatingSender locatingSender;
    private readonly IAppLocation appLocation;
    private readonly string pathName = "Locating";

    public LoadLocatingCommandHandler(ILogger logger, ILocatingRemoveAllRegions locatingRemoveAllRegions, ILocatingGetter locatingGetter, ILocatingSender locatingSender, IAppLocation appLocation)
    {
        this.logger = logger;
        this.locatingRemoveAllRegions = locatingRemoveAllRegions;
        this.locatingGetter = locatingGetter ?? throw new ArgumentNullException(nameof(locatingGetter));
        this.locatingSender = locatingSender ?? throw new ArgumentNullException(nameof(locatingSender));
        this.appLocation = appLocation ?? throw new ArgumentNullException(nameof(appLocation));
    }

    public async Task HandleAsync(LoadLocatingCommand command)
    {
        // очистить хранилище
        logger.Info($"Удаляем данные всех регионов");
        await locatingRemoveAllRegions.RemoveAllAsync();
        var xmlPath = Path.Combine(appLocation.ExchangePath, pathName);
        foreach (var region in locatingGetter.GetAllAvailableLocating(xmlPath))
        {
            // загрузить регион из Xml и отдаем в обменник Acl
            await locatingSender.SendLocating(new List<XmlRegion>() { locatingGetter.GetLocatingData(xmlPath, region) });
        }
        logger.Info($"Загрузка данных завершена");

        //тут можно сохранить данные в хранилище - может быть полезно для реестров/страхования/информирования/..., для Locating не полезно
        //можно также вести журнал загрузки и регистрировать все загруженные файлы
    }
}
