using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLib.DependencyInjection;
using MyLib.Services;
using MyLib.LLM;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ChildApp.Tests;

[TestFixture]
public class ChildAppIntegrationTests
{
    [Test]
    public async Task DI_Resolves_HelloService_And_LlmClient()
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection().Build();
        var services = new ServiceCollection();
        services.AddMyLibServices();
        services.AddLlmClient(config);
        var sp = services.BuildServiceProvider();

        var hello = sp.GetRequiredService<IHelloService>();
        var greeting = hello.GetGreeting("Integration");
        Assert.AreEqual("Hello, Integration!", greeting);

        var llm = sp.GetRequiredService<ILLMClient>();
        var res = await llm.QueryAsync("test");
        StringAssert.StartsWith("[local-fallback] test", res);
    }
}
