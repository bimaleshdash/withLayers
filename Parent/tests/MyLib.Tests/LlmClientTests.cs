using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MyLib.LLM;
using NUnit.Framework;

namespace MyLib.Tests;

[TestFixture]
public class LlmClientTests
{
    [Test]
    public async Task QueryAsync_ReturnsFallback_WhenNoBaseAddressConfigured()
    {
        var httpClient = new HttpClient(); // no BaseAddress
        var client = new LlmClient(httpClient);

        var res = await client.QueryAsync("hi");
        StringAssert.Contains("[local-fallback]", res);
    }

    [Test]
    public async Task QueryAsync_PostsToEndpoint_WhenBaseAddressConfigured()
    {
        var handler = new FakeHandler("LLM-Reply");
        var httpClient = new HttpClient(handler) { BaseAddress = new System.Uri("https://api.fake") };
        var client = new LlmClient(httpClient, apiKey: "abc");

        var res = await client.QueryAsync("hello");
        Assert.AreEqual("LLM-Reply", res);
        Assert.AreEqual("POST", handler.LastRequestMethod);
    }

    private class FakeHandler : HttpMessageHandler
    {
        private readonly string _reply;
        public string LastRequestMethod { get; private set; } = string.Empty;

        public FakeHandler(string reply) => _reply = reply;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            LastRequestMethod = request.Method.Method;
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_reply)
            };
            return Task.FromResult(resp);
        }
    }
}
