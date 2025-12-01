using OpenQA.Selenium;

namespace ChildApp.Pages;

public class LoginPage
{
    private readonly IWebDriver _driver;

    public LoginPage(IWebDriver driver)
    {
        _driver = driver ?? throw new System.ArgumentNullException(nameof(driver));
    }

    public IWebElement UsernameInput => _driver.FindElement(By.Id("username"));
    public IWebElement PasswordInput => _driver.FindElement(By.Id("password"));
    public IWebElement LoginButton => _driver.FindElement(By.Id("login"));

    public void EnterUsername(string username)
    {
        UsernameInput.Clear();
        UsernameInput.SendKeys(username);
    }

    public void EnterPassword(string password)
    {
        PasswordInput.Clear();
        PasswordInput.SendKeys(password);
    }

    public void ClickLogin()
    {
        LoginButton.Click();
    }

    public void LoginAs(string username, string password)
    {
        EnterUsername(username);
        EnterPassword(password);
        ClickLogin();
    }
}
