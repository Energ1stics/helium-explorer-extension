using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace helium_api.Models;

public class DailyStats
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public FixedDate Date { get; set; } = FixedDate.Yesterday();

    public long Dc_Burned { get; set; }

    public long Hnt_Minted { get; set; }
}
