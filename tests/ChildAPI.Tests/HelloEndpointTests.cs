using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Text.Json;

namespace ChildAPI.Tests;

[TestFixture]
public class HelloEndpointTests
{
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task GetHello_ReturnsGreeting()
    {
        var res = await _client!.GetAsync("/api/hello?name=Tester");
        res.EnsureSuccessStatusCode();
        var content = await res.Content.ReadAsStringAsync();
        // Parse JSON and verify `message` property equals expected greeting
        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;
        Assert.IsTrue(root.TryGetProperty("message", out var messageProp), "Response JSON should contain 'message' property");
        var messageValue = messageProp.GetString();
        Assert.AreEqual("Hello, Tester!", messageValue);
    }

    [Test]
    public async Task PostLlm_ReturnsFallbackReply_WhenNoEndpointConfigured()
    {
    var res = await _client!.PostAsync("/api/llm?q=hi", new StringContent(string.Empty, Encoding.UTF8, "text/plain"));
    res.EnsureSuccessStatusCode();
    var content = await res.Content.ReadAsStringAsync();
    // Verify JSON `reply` property contains a fallback marker
    using var doc = JsonDocument.Parse(content);
    var root = doc.RootElement;
    Assert.IsTrue(root.TryGetProperty("reply", out var replyProp), "Response JSON should contain 'reply' property");
    var replyValue = replyProp.GetString();
    Assert.IsNotNull(replyValue);
    Assert.IsTrue(replyValue.Contains("[local-fallback]"), "LLM reply should contain the local fallback prefix");
    }
}
