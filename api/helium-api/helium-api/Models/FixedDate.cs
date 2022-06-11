using System.Diagnostics.CodeAnalysis;

namespace helium_api.Models;

public class FixedDate
{
    public int Year { get; init; }

    public int Month { get; init; }

    public int Day { get; init; }

    public FixedDate(int year, int month, int day)
    {
        this.Year = year;
        this.Month = month;
        this.Day = day;
    }

    public static FixedDate Yesterday()
    {
        DateTime dateTime = DateTime.UtcNow;
        dateTime = dateTime.AddDays(-1);
        return new FixedDate(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        FixedDate? date = obj as FixedDate;
        if (date is null)
        {
            return false;
        }
        return this.Year == date.Year
            && this.Month == date.Month
            && this.Day == date.Day;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
