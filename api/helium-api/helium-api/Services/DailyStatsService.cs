using helium_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace helium_api.Services;

public class DailyStatsService
{
    private readonly IMongoCollection<DailyStats> _dailyStatisticsCollection;

    private readonly HeliumApiService _heliumApiService;

    public DailyStatsService(
        IOptions<HeliumStatsDatabaseSettings> heliumStatsDatabaseSettings,
        HeliumApiService heliumApiService)
    {
        var mongoClient = new MongoClient(
            heliumStatsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            heliumStatsDatabaseSettings.Value.DatabaseName);

        _dailyStatisticsCollection = mongoDatabase.GetCollection<DailyStats>(
            heliumStatsDatabaseSettings.Value.CollectionName);

        this._heliumApiService = heliumApiService;
    }

    public async Task<DailyStats?> GetAsync(FixedDate date)
    {
        var result = await _dailyStatisticsCollection
            .Find(x => x.Date.Equals(date))
            .FirstOrDefaultAsync();
        if (result == null)
        {
            Console.WriteLine("Day not in Database; Calling Helium API...");
            result = await this._heliumApiService.GetDailyStats(date);
            if (result != null) {
                Console.WriteLine("API retourned data. Data will be pushed to DB.");
                await CreateAsync(result);
            }
        }
        return result;
    }

    public async Task CreateAsync(DailyStats dailyStats)
    {
        await _dailyStatisticsCollection
            .InsertOneAsync(dailyStats);
    }
}
