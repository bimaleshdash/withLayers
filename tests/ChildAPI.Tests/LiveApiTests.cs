using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ChildAPI.Tests;

[TestFixture]
[Category("Live")]
public class LiveApiTests
{
    private HttpClient? _client;
    private const string BaseUrl = "https://jsonplaceholder.typicode.com";

    [SetUp]
    public void Setup()
    {
        _client = new HttpClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
    }

    [Test]
    public async Task GetPosts_ReturnsList()
    {
        var res = await _client!.GetAsync(BaseUrl + "/posts");
        res.EnsureSuccessStatusCode();
        var content = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        Assert.IsTrue(doc.RootElement.ValueKind == JsonValueKind.Array, "Expected JSON array");
        Assert.Greater(doc.RootElement.GetArrayLength(), 0, "Expected at least one post in the list");
    }

    [Test]
    public async Task GetPostById_ReturnsExpectedPost()
    {
        var res = await _client!.GetAsync(BaseUrl + "/posts/1");
        res.EnsureSuccessStatusCode();
        var content = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;
        Assert.IsTrue(root.TryGetProperty("id", out var idProp));
        Assert.AreEqual(1, idProp.GetInt32());
        Assert.IsTrue(root.TryGetProperty("title", out var titleProp));
        Assert.IsNotNull(titleProp.GetString());
    }

    [Test]
    public async Task PostPost_ReturnsCreatedItem()
    {
        var post = new { title = "foo", body = "bar", userId = 1 };
        var res = await _client!.PostAsJsonAsync(BaseUrl + "/posts", post);
        // JSONPlaceholder returns 201 with created object
        Assert.IsTrue(res.IsSuccessStatusCode, "Expected success status code for POST");
        var content = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;
        Assert.IsTrue(root.TryGetProperty("id", out var idProp));
        Assert.IsTrue(idProp.GetInt32() > 0);
        // Verify returned fields
        Assert.IsTrue(root.TryGetProperty("title", out var titleProp));
        Assert.AreEqual("foo", titleProp.GetString());
    }

    [Test]
    public async Task PutPost_UpdatesAndReturnsItem()
    {
        var update = new { id = 1, title = "updated", body = "updated body", userId = 1 };
        var res = await _client!.PutAsJsonAsync(BaseUrl + "/posts/1", update);
        Assert.IsTrue(res.IsSuccessStatusCode, "Expected success status code for PUT");
        var content = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;
        Assert.IsTrue(root.TryGetProperty("id", out var idProp));
        Assert.AreEqual(1, idProp.GetInt32());
        Assert.IsTrue(root.TryGetProperty("title", out var titleProp));
        Assert.AreEqual("updated", titleProp.GetString());
    }

    [Test]
    public async Task DeletePost_ReturnsSuccess()
    {
        var res = await _client!.DeleteAsync(BaseUrl + "/posts/1");
        Assert.IsTrue(res.IsSuccessStatusCode, "Expected success status code for DELETE");
        var content = await res.Content.ReadAsStringAsync();
        // JSONPlaceholder returns an empty body for deletes; we verify it's empty or an empty object
        if (!string.IsNullOrWhiteSpace(content))
        {
            using var doc = JsonDocument.Parse(content);
            Assert.IsTrue(doc.RootElement.ValueKind == JsonValueKind.Object);
        }
    }
}
