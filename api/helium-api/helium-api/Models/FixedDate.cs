using System.Diagnostics.CodeAnalysis;

namespace helium_api.Models;

public class FixedDate
{
    #region DB Fields
    public int Year { get; init; }

    public int Month { get; init; }

    public int Day { get; init; }
    #endregion

    public FixedDate(int year, int month, int day)
    {
        if(!DateOnly.TryParse(year + "/" + month + "/" + day, out _))
        {
            throw new ArgumentException("The given date is not valid.");
        }
        this.Year = year;
        this.Month = month;
        this.Day = day;
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
