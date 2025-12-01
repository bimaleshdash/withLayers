using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLib.DependencyInjection;
using MyLib.Services;
using MyLib.LLM;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configuration and DI
builder.Services.AddMyLibServices();
builder.Services.AddLlmClient(builder.Configuration);

builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/api/hello", (string? name, IHelloService hello) =>
{
    var result = hello.GetGreeting(name ?? "World");
    return Results.Ok(new { message = result });
});

app.MapPost("/api/llm", async (LLM.ILLMClient llm, LLM.LlmClient? _dummy, HttpRequest req) =>
{
    // For demo: read query parameter "q" or request body
    string? q = req.Query["q"].ToString();
    if (string.IsNullOrEmpty(q))
    {
        using var sr = new StreamReader(req.Body);
        q = await sr.ReadToEndAsync();
    }
    var reply = await llm.QueryAsync(q ?? "Hello");
    return Results.Ok(new { reply });
});

app.Run();
