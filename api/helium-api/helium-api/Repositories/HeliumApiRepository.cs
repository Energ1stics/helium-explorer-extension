using helium_api.Models.HeliumApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace helium_api.Respositories
{
    public class HeliumApiRepository
    {
        public async Task<BlockTransactions> GetBlockTransactionsAsync(long height)
        {
            var client = new RestClient($"https://api.helium.io/v1/blocks/{height}/transactions");
            var request = new RestRequest();
            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                Console.WriteLine("Failed getting block transactions: " + response.ErrorMessage);
                return null;
            }
            var content = JsonConvert.DeserializeObject<JToken>(response.Content);
            return new BlockTransactions();
        }
    }
}
