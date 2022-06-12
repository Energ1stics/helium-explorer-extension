using System.Text.Json.Serialization;

namespace HeliumApi.Models.Helium;

public class DcBurns
{
    [JsonPropertyName("data")]
    public BurnData Data { get; set; }

    [JsonIgnore]
    public long Total => Data.State_Channel + Data.Routing + Data.Add_Gateway + Data.Assert_Location + Data.Fee;

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
