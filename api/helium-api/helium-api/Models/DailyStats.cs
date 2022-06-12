using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace helium_api.Models;

public class DailyStats
{
    #region DB Fields
    [BsonId]
    public FixedDate Date { get; set; }

    public long Dc_Burned { get; set; }

    public long Hnt_Minted { get; set; }
    #endregion

    public DailyStats(FixedDate date)
    {
        this.Date = date;
    }
}
