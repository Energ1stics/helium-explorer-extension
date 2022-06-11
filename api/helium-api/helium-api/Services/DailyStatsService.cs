using helium_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace helium_api.Services;

public class DailyStatsService
{
    private readonly IMongoCollection<DailyStats> _dailyStatisticsCollection;

    public DailyStatsService(
        IOptions<HeliumStatsDatabaseSettings> heliumStatsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            heliumStatsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            heliumStatsDatabaseSettings.Value.DatabaseName);

        _dailyStatisticsCollection = mongoDatabase.GetCollection<DailyStats>(
            heliumStatsDatabaseSettings.Value.CollectionName);
    }

    public async Task<DailyStats?> GetAsync(FixedDate date) =>
        await _dailyStatisticsCollection
            .Find(x => x.Date.Equals(date))
            .FirstOrDefaultAsync();

    public async Task CreateAsync(DailyStats dailyStats)
    {
        await _dailyStatisticsCollection
            .InsertOneAsync(dailyStats);
    }
}
