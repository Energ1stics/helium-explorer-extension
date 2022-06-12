using HeliumApi.Models;
using HeliumApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeliumApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DailyStatsController : ControllerBase
{
    private readonly ILogger<DailyStatsController> _logger;
    private readonly DailyStatsService _dailyStatsService;

    public DailyStatsController(
        ILogger<DailyStatsController> logger,
        DailyStatsService dailyStatsService
        )
    {
        _logger = logger;
        _dailyStatsService = dailyStatsService;
    }

    [HttpGet("{year}/{month}/{day}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DailyStats>> Get(int year, int month, int day)
    {
        var date = new FixedDate(year, month, day);
        var dailyStats = await this._dailyStatsService.GetAsync(date);
        if (dailyStats is null)
        {
            Console.WriteLine($"No data could be found for date {date}");
            return NotFound();
        }
        return dailyStats;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post(DailyStats dailyStats)
    {
        await _dailyStatsService.CreateAsync(dailyStats);

        return CreatedAtAction(
            nameof(Get),
            new {
                year = dailyStats.Date.Year,
                month = dailyStats.Date.Month,
                day = dailyStats.Date.Day
            },
            dailyStats);
    }
}