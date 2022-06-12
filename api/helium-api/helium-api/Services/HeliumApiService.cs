using helium_api.Models.HeliumApi;
using System.Net.Http.Headers;
using System.Text.Json;
using helium_api.Models;

namespace helium_api.Services;

public class HeliumApiService
{
    private HttpClient _client = new HttpClient();

    public HeliumApiService()
    {
        this._client.BaseAddress = new Uri("https://api.helium.io/v1/dc_burns/");
        var userAgent = new ProductInfoHeaderValue("HeliumEquilibriumCalculator", "0.1");
        this._client.DefaultRequestHeaders.UserAgent.Add(userAgent);
    }

    public async Task<DcBurns?> GetDcBurnsAsync(FixedDate date)
    {
        DcBurns? result = null;
        string url = $"sum?min_time={date.ToQueryString()}&max_time={date.NextDay().ToQueryString()}";
        var response = await this._client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var dataStream = await response.Content.ReadAsStreamAsync();
            result = JsonSerializer.Deserialize<DcBurns>(dataStream);
        }
        return result;
    }
}
