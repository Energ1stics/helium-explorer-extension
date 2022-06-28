using HeliumApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HeliumApi.Test;

[TestClass]
public class FixedDateTests
{
    [TestMethod]
    public void ValidDateHasCorrectValues()
    {
        FixedDate date = new FixedDate(2022, 3, 31);

        Assert.AreEqual(2022, date.Year);
        Assert.AreEqual(3, date.Month);
        Assert.AreEqual(31, date.Day);
    }

    [TestMethod]
    public void DateQueryStringIsCorrect()
    {
        FixedDate date = new FixedDate(2022, 3, 10);
        string actual = date.ToQueryString();
        string expected = "2022-03-10T00:00:00Z";

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void NextDayIsCorrect()
    {
        FixedDate date = new FixedDate(2022, 3, 31).NextDay();

        Assert.AreEqual(2022, date.Year);
        Assert.AreEqual(4, date.Month);
        Assert.AreEqual(1, date.Day);
    }

    [TestMethod]
    public void PreviousDayIsCorrect()
    {
        FixedDate date = new FixedDate(2022, 3, 1).PreviousDay();

        Assert.AreEqual(2022, date.Year);
        Assert.AreEqual(2, date.Month);
        Assert.AreEqual(28, date.Day);
    }

    [TestMethod]
    public void NextQueryStringIsCorrect()
    {
        FixedDate date = new FixedDate(2022, 3, 31).NextDay();
        string actual = date.ToQueryString();
        string expected = "2022-04-01T00:00:00Z";

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TodayIsCorrect()
    {
        FixedDate date = FixedDate.Today();
        DateTime expectedDate = DateTime.UtcNow;

        Assert.AreEqual(date.Year, expectedDate.Year);
        Assert.AreEqual(date.Month, expectedDate.Month);
        Assert.AreEqual(date.Day, expectedDate.Day);
    }

    public void YesterdayIsCorrect()
    {
        FixedDate date = FixedDate.Today();
        DateTime expectedDate = DateTime.UtcNow;
        expectedDate.AddDays(-1);

        Assert.AreEqual(date.Year, expectedDate.Year);
        Assert.AreEqual(date.Month, expectedDate.Month);
        Assert.AreEqual(date.Day, expectedDate.Day);
    }
}