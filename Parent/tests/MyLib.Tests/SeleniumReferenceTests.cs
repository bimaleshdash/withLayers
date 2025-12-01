using NUnit.Framework;

namespace MyLib.Tests;

[TestFixture]
public class SeleniumReferenceTests
{
    [Test]
    public void SeleniumTypes_AreAvailableToProjects()
    {
        var t = typeof(OpenQA.Selenium.IWebDriver);
        Assert.NotNull(t);
        Assert.AreEqual("OpenQA.Selenium.IWebDriver", t.FullName);
    }
}
