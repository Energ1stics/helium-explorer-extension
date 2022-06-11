using helium_api.Models;
using helium_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace helium_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DailyStatsController : ControllerBase
    {
        private readonly ILogger<DailyStatsController> _logger;
        private readonly DailyStatsService _dailyStatsService;

        public DailyStatsController(
            ILogger<DailyStatsController> logger,
            DailyStatsService dailyStatsService
            ) {
            _logger = logger;
            _dailyStatsService = dailyStatsService;
        }

        [HttpGet("{year}/{month}/{day}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DailyStats>> Get(int year, int month, int day) {
            var date = new FixedDate(year, month, day);
            var dailyStats = await this._dailyStatsService.GetAsync(date);
            if (dailyStats is null)
            {
                return NotFound();
            }
            return dailyStats;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(DailyStats newDailyStats)
        {
            await _dailyStatsService.CreateAsync(newDailyStats);

            return CreatedAtAction(nameof(Get), new { id = newDailyStats.Id }, newDailyStats);
        }
    }
}