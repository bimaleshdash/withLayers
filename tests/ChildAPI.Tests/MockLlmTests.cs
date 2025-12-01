using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit.Framework;
using MyLib.LLM;

namespace ChildAPI.Tests;

[TestFixture]
public class MockLlmTests
{
    [Test]
    public async Task PostLlm_ReturnsMockReply_WhenMockingLlmClient()
    {
        // Arrange
        var mockLlm = new Mock<ILLMClient>();
        mockLlm.Setup(m => m.QueryAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
               .ReturnsAsync("[mocked-reply] hello from mock");

        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing ILLMClient registration(s) and replace with our mock
                services.RemoveAll(typeof(ILLMClient));
                services.AddSingleton(typeof(ILLMClient), mockLlm.Object);
            });
        });

        using var client = factory.CreateClient();

        // Act
        var res = await client.PostAsync("/api/llm?q=test", new StringContent(string.Empty, Encoding.UTF8, "text/plain"));
        res.EnsureSuccessStatusCode();
        var content = await res.Content.ReadAsStringAsync();

        // Assert: parse JSON and assert reply equals mock
        using var doc = System.Text.Json.JsonDocument.Parse(content);
        var root = doc.RootElement;
        Assert.IsTrue(root.TryGetProperty("reply", out var replyProp));
        Assert.AreEqual("[mocked-reply] hello from mock", replyProp.GetString());
    }
}
