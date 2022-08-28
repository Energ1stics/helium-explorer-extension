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
    public async Task<ActionResult<DailyStats>> GetDailyStats(int year, int month, int day)
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

    private async Task<ActionResult<DailyStats>> GetDailyStats(FixedDate date)
    {
        return await GetDailyStats(date.Year, date.Month, date.Day);
    }

    [HttpGet("{year}/{month}/{day}/{timeInDays}")]
    public ActionResult<long> GetMinValueForToken(int year, int month, int day, int timeInDays)
    {
        var date = new FixedDate(year, month, day);
        double burnedDc = 0;
        double mintedHNT = 0;

        for (int i = 1; i <= timeInDays; i++)
        {
            DailyStats? stats = GetDailyStats(date.AddDays(-i)).Result.Value;
            if(stats is null) return NotFound();
            burnedDc += stats.Dc_Burned;
            mintedHNT += stats.Hnt_Minted;
        }

        return (long) ((burnedDc/1000.0)/mintedHNT);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post(DailyStats dailyStats)
    {
        await _dailyStatsService.CreateAsync(dailyStats);

        return CreatedAtAction(
            nameof(GetDailyStats),
            new {
                year = dailyStats.Date.Year,
                month = dailyStats.Date.Month,
                day = dailyStats.Date.Day
            },
            dailyStats);
    }
}