using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions.Json;
using System.Net;
using Newtonsoft.Json.Linq;

namespace HeliumApi.Test;

[TestClass]
public class ApiTests
{
    #region DailyStatsController
    [TestMethod]
    public async Task GetSingleDailyStats()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        var client = webAppFactory.CreateDefaultClient();
        client.BaseAddress = new Uri("https://localhost:7071/api/DailyStats/");

        var response = await client.GetAsync("2022/3/30");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var token = JToken.Parse(result);

        Assert.AreEqual("92035.38998951".ToString(), token["hnt_Minted"]?.ToString());
        Assert.AreEqual("14555763949".ToString(), token["dc_Burned"]?.ToString());
    }
    #endregion
}