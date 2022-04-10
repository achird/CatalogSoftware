using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using catalog.Core.Locating.Application.Use.Query.GetLongLocation;
using catalog.Core.Locating.Application.Use.Query.GetShortLocation;

namespace catalog.UI.api.Locating.Controllers;

//[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LocationController : ControllerBase
{
    private readonly ILogger<LocationController> logger;
    private readonly IGetLongChildsQueryHandler getLongChildsQueryHandler;
    private readonly IGetLongLocationsQueryHandler getLongLocationsQueryHandler;
    private readonly IGetShortChildsQueryHandler getShortChildsQueryHandler;
    private readonly IGetShortLocationsQueryHandler getShortLocationsQueryHandler;

    public LocationController(ILogger<LocationController> logger,
        IGetLongChildsQueryHandler getLongChildsQueryHandler,
        IGetLongLocationsQueryHandler getLongLocationsQueryHandler,
        IGetShortChildsQueryHandler getShortChildsQueryHandler,
        IGetShortLocationsQueryHandler getShortLocationsQueryHandler)
    {
        this.logger = logger;
        this.getLongChildsQueryHandler = getLongChildsQueryHandler;
        this.getLongLocationsQueryHandler = getLongLocationsQueryHandler;
        this.getShortChildsQueryHandler = getShortChildsQueryHandler;
        this.getShortLocationsQueryHandler = getShortLocationsQueryHandler;
    }

    [HttpGet("Long")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<LongLocationData>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IList<LongLocationData>>> GetLongChildsAsync(long parentId)
    {
        var locationData = await getLongChildsQueryHandler.HandleAsync(new GetLongChildsQuery() { ParentId = parentId });

        if (locationData is not { Count: > 0 }) return NoContent();
        return Ok(locationData);
    }

    [HttpPost("Long")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyDictionary<long, LongLocationData>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IReadOnlyDictionary<long, LongLocationData>>> GetLongByIdAsync(List<long> locationIds)
    {
        var locationData = await getLongLocationsQueryHandler.HandleAsync(new GetLongLocationsQuery() { LocationIds = locationIds });

        if (locationData is not { Count: > 0 }) return NoContent();
        return Ok(locationData);
    }

    [HttpGet("Short")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ShortLocationData>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IList<ShortLocationData>>> GetShortChildsAsync(long parentId)
    {
        var locationData = await getShortChildsQueryHandler.HandleAsync(new GetShortChildsQuery() { ParentId = parentId });

        if (locationData is not { Count: > 0 }) return NoContent();
        return Ok(locationData);
    }

    [HttpPost("Short")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyDictionary<long, ShortLocationData>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IReadOnlyDictionary<long, ShortLocationData>>> GetShortByIdAsync(List<long> locationIds)
    {
        var locationData = await getShortLocationsQueryHandler.HandleAsync(new GetShortLocationsQuery() { LocationIds = locationIds });

        if (locationData is not { Count: > 0 }) return NoContent();
        return Ok(locationData);
    }
}
