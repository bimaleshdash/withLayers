using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyLib.LLM;

public class LlmClient : ILLMClient
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;

    public LlmClient(HttpClient httpClient, string? apiKey = null)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<string> QueryAsync(string prompt, CancellationToken cancellationToken = default)
    {
        // Simple fallback: if HttpClient has no BaseAddress, return deterministic response.
        if (_httpClient.BaseAddress == null)
        {
            return $"[local-fallback] {prompt}";
        }

        // Build a simple request payload. APIs differ; this is a generic example.
        var payload = new { prompt };

        using var request = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = JsonContent.Create(payload)
        };

        if (!string.IsNullOrEmpty(_apiKey))
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

        var res = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        res.EnsureSuccessStatusCode();

        // Try to read as plain text
        var text = await res.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return text;
    }
}
