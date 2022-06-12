using System.Diagnostics.CodeAnalysis;

namespace helium_api.Models;

public class FixedDate
{
    #region DB Fields
    public int Year { get; init; }

    public int Month { get; init; }

    public int Day { get; init; }
    #endregion

    public string DateString => $"{this.Year}/{this.Month}/{this.Day}";

    public FixedDate(int year, int month, int day)
    {
        this.Year = year;
        this.Month = month;
        this.Day = day;
        if(!DateOnly.TryParse(this.DateString, out _))
        {
            throw new ArgumentException("The given date is not valid.");
        }
    }

    public string ToQueryString()
    {
        string year = this.Year.ToString();
        string month = this.Month > 10 ? this.Month.ToString() : "0" + this.Month.ToString();
        string day = this.Day > 10 ? this.Day.ToString() : "0" + this.Day.ToString();
        return $"{year}-{month}-{day}T00:00:00Z";
    }

    public FixedDate NextDay()
    {
        DateOnly date = DateOnly.Parse(this.DateString);
        date = date.AddDays(1);
        return new FixedDate(date.Year, date.Month, date.Day);
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
