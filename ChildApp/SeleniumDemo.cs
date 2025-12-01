using OpenQA.Selenium;

public static class SeleniumDemo
{
    public static string GetWebDriverType() => typeof(IWebDriver).FullName ?? "Unknown";
}
