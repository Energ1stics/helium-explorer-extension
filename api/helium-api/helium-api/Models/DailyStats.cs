using MongoDB.Bson.Serialization.Attributes;

namespace HeliumApi.Models;

public class DailyStats
{
    #region DB Fields
    [BsonId]
    public FixedDate Date { get; set; }

    public long Dc_Burned { get; set; }

    public double Hnt_Minted { get; set; }
    #endregion

    public DailyStats(FixedDate date)
    {
        this.Date = date;
    }

    public DailyStats(FixedDate date, long dc_Burned, double hnt_Minted) : this(date)
    {
        this.Dc_Burned = dc_Burned;
        this.Hnt_Minted = hnt_Minted;
    }
}
