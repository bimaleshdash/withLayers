using MyLib.Services;
using NUnit.Framework;

namespace MyLib.Tests;

[TestFixture]
public class HelloServiceTests
{
    [Test]
    public void GetGreeting_ReturnsExpectedMessage()
    {
        var service = new HelloService();
        var res = service.GetGreeting("Tester");
        Assert.AreEqual("Hello, Tester!", res);
    }
}
