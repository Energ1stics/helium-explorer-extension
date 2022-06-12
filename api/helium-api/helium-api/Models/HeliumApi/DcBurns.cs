using System.Text.Json.Serialization;

namespace helium_api.Models.HeliumApi;

public class DcBurns
{
    [JsonPropertyName("data")]
    public BurnData Data { get; set; }

    public DcBurns(BurnData data)
    {
        this.Data = data;
    }

    public class BurnData
    {
        [JsonPropertyName("state_channel")]
        public long State_Channel { get; set; }

        [JsonPropertyName("routing")]
        public long Routing { get; set; }

        [JsonPropertyName("fee")]
        public long Fee { get; set; }

        [JsonPropertyName("assert_location")]
        public long Assert_Location { get; set; }

        [JsonPropertyName("add_gateway")]
        public long Add_Gateway { get; set; }
    }
}
