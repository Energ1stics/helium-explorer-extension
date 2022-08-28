﻿using HeliumApi.Models;

namespace HeliumApi.Services;

public class DailyStatsUpdateHostedService : IHostedService, IDisposable
{
    private readonly ILogger _logger;
    private Timer? _timer = null;

    private readonly DailyStatsService _dailyStatsService;

    public DailyStatsUpdateHostedService(ILogger<DailyStatsUpdateHostedService> logger, DailyStatsService dailyStatsService)
    {
        _logger = logger;
        _dailyStatsService = dailyStatsService;
    }

    public Task StartAsync(CancellationToken token)
    {
        _logger.LogInformation("Update Service is running.");

        _timer = new Timer(ServiceIteration, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

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
            _logger.LogInformation("Data found. Continuing process.");
            date = date.PreviousDay();
            if (date.Equals(lastDayToCheck))
            {
                _logger.LogInformation("Last missing entry was pushed to DB. Pausing Service for five minutes.");
                return;
            }
            successful = await UpdateStats(date);
        }
        _logger.LogInformation("No data was found. Pausing Service for five minutes.");
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