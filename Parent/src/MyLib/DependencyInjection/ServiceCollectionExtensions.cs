using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyLib.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the LLM client into DI, configured by `LLM_ENDPOINT` and `LLM_API_KEY` in configuration or environment.
    /// The `httpClientName` allows configuration of a named HttpClient in the host.
    /// </summary>
    public static IServiceCollection AddLlmClient(this IServiceCollection services, IConfiguration configuration, string httpClientName = "llm")
    {
        var endpoint = configuration["LLM_ENDPOINT"];
        var apiKey = configuration["LLM_API_KEY"];

        services.AddHttpClient(httpClientName, (sp, client) =>
        {
            if (!string.IsNullOrEmpty(endpoint)) client.BaseAddress = new Uri(endpoint);
        });

    services.AddTransient<MyLib.LLM.ILLMClient>(sp =>
        {
            var clientFactory = sp.GetRequiredService<System.Net.Http.IHttpClientFactory>();
            var httpClient = clientFactory.CreateClient(httpClientName);
            return new MyLib.LLM.LlmClient(httpClient, apiKey);
        });

        return services;
    }

    /// <summary>
    /// Registers MyLib core services like `IHelloService` and other lightweight dependencies.
    /// </summary>
    public static IServiceCollection AddMyLibServices(this IServiceCollection services)
    {
        services.AddTransient<MyLib.Services.IHelloService, MyLib.Services.HelloService>();
        return services;
    }
}
