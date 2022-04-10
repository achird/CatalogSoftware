using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using catalog.Core.Locating.Application.Use.Query.FindAddress;

namespace catalog.UI.api.Locating.Controllers;

//[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AddressController : ControllerBase
{
    private readonly ILogger<AddressController> logger;
    private readonly IFindAddressesByIdQueryHandler findAddressesByIdQueryHandler;
    private readonly IFindAddressesByUidQueryHandler findAddressesByUidQueryHandler;

    public AddressController(ILogger<AddressController> logger, IFindAddressesByIdQueryHandler findAddressesByIdQueryHandler, IFindAddressesByUidQueryHandler findAddressesByUidQueryHandler)
    {
        this.logger = logger;
        this.findAddressesByIdQueryHandler = findAddressesByIdQueryHandler;
        this.findAddressesByUidQueryHandler = findAddressesByUidQueryHandler;
    }

    [HttpPost("Id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyDictionary<long, AddressData>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IReadOnlyDictionary<long, AddressData>>> GetByIdAsync(List<long> locationIds)
    {
        var locationData = await findAddressesByIdQueryHandler.HandleAsync(new FindAddressesByIdQuery() { LocationIds = locationIds });

        if (locationData is not { Count: > 0 }) return NoContent();
        return Ok(locationData);
    }

    [HttpPost("Uid")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyDictionary<Guid, AddressData>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IReadOnlyDictionary<Guid, AddressData>>> GetByUidAsync(List<Guid> locationUids)
    {
        var locationData = await findAddressesByUidQueryHandler.HandleAsync(new FindAddressesByUidQuery() { LocationUids = locationUids });

        if (locationData is not { Count: > 0 }) return NoContent();
        return Ok(locationData);
    }
}
