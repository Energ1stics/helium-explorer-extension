using HeliumApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HeliumApi.Services;

public class DailyStatsService
{
    private readonly IMongoCollection<DailyStats> _dailyStatisticsCollection;

    private readonly HeliumApiService _heliumApiService;

    private readonly ILogger _logger;

    public DailyStatsService(
        IOptions<HeliumStatsDatabaseSettings> heliumStatsDatabaseSettings,
        HeliumApiService heliumApiService,
        ILogger<DailyStatsService> logger)
    {
        var mongoClient = new MongoClient(
            heliumStatsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            heliumStatsDatabaseSettings.Value.DatabaseName);

        _dailyStatisticsCollection = mongoDatabase.GetCollection<DailyStats>(
            heliumStatsDatabaseSettings.Value.CollectionName);

        _heliumApiService = heliumApiService;

        _logger = logger;
    }

    public async Task<DailyStats?> GetAsync(FixedDate date)
    {
        _logger.LogInformation($"Getting data for {date.DateString}.");

        var result = await _dailyStatisticsCollection
            .Find(x => x.Date.Equals(date))
            .FirstOrDefaultAsync();

        if (result != null)
        {
            _logger.LogInformation($"Data for {date.DateString} found in DB.");
            return result;
        }

        _logger.LogInformation($"{date.DateString} not in Database; Calling Helium API...");
        result = await this._heliumApiService.GetDailyStats(date);
        if (result != null)
        {
            _logger.LogInformation($"API retourned data. {date.DateString} will be pushed to DB.");
            await CreateAsync(result);
        }
        return result;
    }

    public async IAsyncEnumerable<DailyStats?> GetMultipleAsync(FixedDate from, FixedDate to)
    {
        yield return await GetAsync(from);
        while (from != to)
        {
            from = from.NextDay();
            yield return await GetAsync(from);
        }
    }

    public async Task CreateAsync(DailyStats dailyStats)
    {
        await _dailyStatisticsCollection
            .InsertOneAsync(dailyStats);
    }
}
