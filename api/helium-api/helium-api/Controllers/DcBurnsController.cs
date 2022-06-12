using helium_api.Models;
using helium_api.Models.HeliumApi;
using helium_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace helium_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DcBurnsController : Controller
{
    private readonly ILogger<DcBurnsController> _logger;
    private readonly HeliumApiService _heliumApiService;

    public DcBurnsController(
        ILogger<DcBurnsController> logger,
        HeliumApiService dailyStatsService
        )
    {
        this._logger = logger;
        this._heliumApiService = dailyStatsService;
    }

    [HttpGet("{year}/{month}/{day}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DcBurns>> Get(int year, int month, int day)
    {
        var result = await this._heliumApiService.GetDcBurnsAsync(new FixedDate(year, month, day));
        if(result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
}
