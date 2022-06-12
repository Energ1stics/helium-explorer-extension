using helium_api.Models;
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
        FixedDate date = new FixedDate(2022, 3, 31);
        string actual = date.ToQueryString();
        string expected = "2022-03-31T00:00:00Z";

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
    public void NextQueryStringIsCorrect()
    {
        FixedDate date = new FixedDate(2022, 3, 31).NextDay();
        string actual = date.ToQueryString();
        string expected = "2022-04-01T00:00:00Z";

        Assert.AreEqual(expected, actual);
    }
}