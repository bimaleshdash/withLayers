# ChildAPI — API consumer demo for MyLib

This ASP.NET Core Web API demonstrates how to consume the `MyLib` library via ProjectReference (local dev) or PackageReference (package consumption). It exposes a simple API that uses `IHelloService` and the `ILLMClient` typed client from MyLib.

Run (local / ProjectReference):
```bash
dotnet run --project ChildAPI
```

Endpoints:
- GET /api/hello?name=<name> — returns JSON { message: "Hello, <name>!" }
- POST /api/llm?q=<prompt> — returns a simple reply via the LLM client (or via request body)

Live tests (external):
- The `tests/ChildAPI.Tests` project includes live integration tests under the `LiveApiTests` class that call a public demo API (https://jsonplaceholder.typicode.com) to demonstrate GET/POST/PUT/DELETE usage.
- These tests are marked with the NUnit category `Live` and can be executed with test filters, e.g.:

```bash
dotnet test --filter Category=Live
```

Note: These live tests make real network requests and can fail if the remote service is down or rate-limited. Use them for demos and CI smoke checks with caution.

To consume MyLib as a package (package-consumption scenario), pack `MyLib` and add the package feed to the local NuGet sources, then change the `ProjectReference` to a `PackageReference` or use a separate consumer project for the package variant.
