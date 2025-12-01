using Moq;
using OpenQA.Selenium;
using ChildApp.Pages;
using NUnit.Framework;

namespace ChildApp.Tests;

[TestFixture]
public class LoginPageTests
{
    [Test]
    public void LoginAs_CallsFindElementAndActions()
    {
        var mockDriver = new Mock<IWebDriver>();
        var mockUsername = new Mock<IWebElement>();
        var mockPassword = new Mock<IWebElement>();
        var mockLogin = new Mock<IWebElement>();

        // Setup FindElement to return the correct element depending on By.Id
        mockDriver.Setup(d => d.FindElement(It.Is<By>(b => b.ToString().Contains("username"))))
                  .Returns(mockUsername.Object);
        mockDriver.Setup(d => d.FindElement(It.Is<By>(b => b.ToString().Contains("password"))))
                  .Returns(mockPassword.Object);
        mockDriver.Setup(d => d.FindElement(It.Is<By>(b => b.ToString().Contains("login"))))
                  .Returns(mockLogin.Object);

        // Create the LoginPage with the mocked driver
        var page = new LoginPage(mockDriver.Object);

        // Call LoginAs, which should call Clear/SendKeys/Click on the elements
        page.LoginAs("user1", "pass1");

        // Verify interactions
        mockUsername.Verify(e => e.Clear(), Times.Once);
        mockUsername.Verify(e => e.SendKeys("user1"), Times.Once);
        mockPassword.Verify(e => e.Clear(), Times.Once);
        mockPassword.Verify(e => e.SendKeys("pass1"), Times.Once);
        mockLogin.Verify(e => e.Click(), Times.Once);
    }
}
