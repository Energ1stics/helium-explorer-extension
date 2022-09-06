using HeliumApi.Models;
using MongoDB.Driver;

namespace HeliumApi.Services;

public class DailyStatsUpdateHostedService : IHostedService, IDisposable
{
    private readonly ILogger _logger;
    private Timer _timer;

    private readonly DailyStatsService _dailyStatsService;

    public DailyStatsUpdateHostedService(ILogger<DailyStatsUpdateHostedService> logger, DailyStatsService dailyStatsService)
    {
        _logger = logger;
        _dailyStatsService = dailyStatsService;
        _timer = new Timer(ServiceIteration, null, Timeout.InfiniteTimeSpan, TimeSpan.FromMinutes(5));
    }

    public Task StartAsync(CancellationToken token)
    {
        _logger.LogInformation("Update Service is running.");

        _timer = new Timer(ServiceIteration, null, TimeSpan.Zero, Timeout.InfiniteTimeSpan);

        return Task.CompletedTask;
    }

    private async void ServiceIteration(object? state)
    {
        if(!Console.IsOutputRedirected) Console.Clear();

        _logger.LogInformation("Update Service is iterating.");

        FixedDate date = FixedDate.Yesterday();
        FixedDate lastDayToCheck = date.AddDays(-100);

        bool successful = await UpdateStats(date);
        while (successful)
        {
            date = date.PreviousDay();
            if (date.Equals(lastDayToCheck))
            {
                _logger.LogInformation("Last missing entry was pushed to DB. Pausing Service for this day.");
                TimeSpan timeSpan = SetTimerToNextDay();
                _logger.LogInformation("Service paused for " + ((int)timeSpan.TotalHours) + " hours");
                return;
            }
            successful = await UpdateStats(date);
        }
        _logger.LogInformation("No data was found. Pausing Service for five minutes.");
        SetTimerToFiveMinutes();
    }

    private TimeSpan SetTimerToNextDay()
    {
        TimeSpan timeToNextDay = DateTime.UtcNow.Date.AddDays(1) - DateTime.UtcNow;
        this._timer.Change(Timeout.InfiniteTimeSpan, timeToNextDay);
        return timeToNextDay;
    }

    private void SetTimerToFiveMinutes()
    {
        this._timer.Change(TimeSpan.Zero, TimeSpan.FromMinutes(5));
    }

    private async Task<bool> UpdateStats(FixedDate date)
    {
        DailyStats? stats = await _dailyStatsService.GetAsync(date);
        return stats != null;
    }

    public Task StopAsync(CancellationToken token)
    {
        _logger.LogInformation("Update Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
