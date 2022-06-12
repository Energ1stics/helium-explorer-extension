using HeliumApi.Models.Helium;
using System.Net.Http.Headers;
using HeliumApi.Models;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace HeliumApi.Services;

public class HeliumApiService
{
    private HttpClient _client = new HttpClient();

    public HeliumApiService()
    {
        this._client.BaseAddress = new Uri("https://api.helium.io/v1/");
        var userAgent = new ProductInfoHeaderValue("HeliumEquilibriumCalculator", "0.1");
        this._client.DefaultRequestHeaders.UserAgent.Add(userAgent);
    }

    public async Task<DailyStats?> GetDailyStats(FixedDate date)
    {
        long? dcBurns = await GetDcBurnsAsync(date);
        double? hntMinted = await GetMintedHntAsync(date);
        if (hntMinted == null || dcBurns == null)
        {
            return null;
        }
        return new DailyStats(date, (long) dcBurns, (double) hntMinted);
    }

    public async Task<long?> GetDcBurnsAsync(FixedDate date)
    {
        long? result = null;
        string url = $"dc_burns/sum?min_time={date.ToQueryString()}&max_time={date.NextDay().ToQueryString()}";
        var response = await this._client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var dataStream = await response.Content.ReadAsStreamAsync();
            result = JsonSerializer.Deserialize<DcBurns>(dataStream)?.Total;
        }
        return result;
    }

    public async Task<double?> GetMintedHntAsync(FixedDate date)
    {
        double? result = null;
        string url = $"rewards/sum?min_time={date.ToQueryString()}&max_time={date.NextDay().ToQueryString()}";
        var response = await this._client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            result = (double?) JObject.Parse(data)["data"]?["total"];
        }
        return result;
    }
}
