# Highlights — MyLib workspace (docs)

A concise guide to the most important aspects of this repository.

## Overview
- MyLib: a small, packable C# library that contains Services, Models, and an LLM client. The library is in `Parent/src/MyLib` and is the canonical source for core behavior.
- ChildApp: a simple console/demo consumer that demonstrates registering MyLib services and using the LLM client.
- ChildAPI: an ASP.NET Core Web API consumer with integration tests that demonstrate how to register and use MyLib in an API project.

## Key files
- `Parent/src/MyLib` — library project
- `ChildApp` — console/sample consumer
- `ChildAPI` — Web API consumer
- `tests/*.Tests` — test projects
- `scripts/test-consume-package.sh` — local smoke-test for packaging
- `.github/workflows/dotnet.yml` — CI workflow for build/test/pack

## Quick commands
- Restore, build, test: `dotnet restore && dotnet build && dotnet test`
- Run the sample app: `dotnet run --project ChildApp`
- Pack the library: `dotnet pack Parent/src/MyLib/MyLib.csproj -c Release -o ./artifacts`

## LLM & DI
- The library exposes `MyLib.LLM.ILLMClient` and `MyLib.LLM.LlmClient`. The LLM client falls back to a local response when `LLM_ENDPOINT` or `LLM_API_KEY` aren't configured.
- Use `services.AddMyLibServices()` and `services.AddLlmClient(Configuration)` to register DI services in consumers.

## Packaging & CI
- The library is configured to generate a NuGet package on build: `<GeneratePackageOnBuild>true</GeneratePackageOnBuild>`.
- CI validates both a `project` (ProjectReference) and `package` (PackageReference) consumer scenario using a release-test matrix.
- To publish to Azure Artifacts, set `AZURE_ARTIFACTS_FEED_URL` and `AZURE_ARTIFACTS_API_KEY` in repo secrets. The CI workflow will push the package if configured.

## Dockerization
- Dockerfiles are available for `ChildAPI` and `ChildApp`:
   - `ChildAPI/Dockerfile` (multi-stage, uses dotnet SDK to build and ASP.NET runtime for final image)
   - `ChildApp/Dockerfile` (multi-stage, uses dotnet SDK to build and .NET runtime for the final image)
- `docker-compose.yml` is provided for running both services together locally. See `docs/DOCKER.md` for full usage instructions and CI suggestions.

## Testing & Conventions
- Tests use NUnit across the repository. Tests are named descriptively (e.g., `Method_Condition_Expected`).
- The `tests/*` projects include both integration and unit tests; `tests/ChildApp.Tests` contains page-object-model unit tests using Moq for UI logic without a browser.

- The `tests/ChildAPI.Tests` project includes a `LiveApiTests` class (NUnit Category `Live`) demonstrating how to write tests that call external APIs (GET/POST/PUT/DELETE) using `https://jsonplaceholder.typicode.com`.

## RestAssured.Net
- `RestAssured.Net` is included in `Parent/src/MyLib` for internal API testing as a test helper and is marked with `PrivateAssets="all"` to avoid transitive consumption by child projects. Move it to a test project if you'd rather consumers not receive it.

## Good benefits
- Small, modular library with clear layers (Services/Models), making it easy to reason about and extend.
- Packable by design and configured to generate NuGet packages automatically during builds.
- DI-friendly architecture with helper registration methods (`AddMyLibServices`, `AddLlmClient`) that simplify host integration.
- LLM client with sensible defaults and local fallback behavior, making development and local testing predictable.
- CI workflow that validates both ProjectReference and PackageReference consumers via a release-test matrix — helps detect packaging issues early.
- Tests and example consumer apps (ChildApp, ChildAPI) that show real-world consumption and patterns.


## Backlogs (priority recommendations / next steps)
1. High priority: Move test-only dependencies out of `MyLib` and into the appropriate `tests/*` projects.
   - Why: Prevents consumers from implicitly receiving tools they don't need; keeps library surface minimal.
2. High priority: Add a CI job for headless Selenium E2E tests (or a separate workflow) with necessary drivers for Chrome/Firefox.
   - Why: Ensure UI and POM behaviors are validated in CI. This reduces regression risk when making UI changes.
3. High priority: Add a `publish-on-tag` job to CI to push packages to Azure Artifacts or a configured feed.
   - Why: Simplifies release flow and lets consumers rely on published packages.
4. Medium priority: Standardize test frameworks (NUnit) across the repo and migrate tests accordingly.
   - Why: Consistent testing reduces cross-framework knowledge overhead and increases maintainability.
5. Medium priority: Add integration tests that cover `POST /api/llm` and verify structured JSON reply (message + reply fields).
   - Why: Validates end-to-end LLM behavior and DI mocking in WebApplicationFactory contexts.
6. Medium priority: Add code coverage reporting and static analysis to CI.
   - Why: Improves the code quality bar and helps identify untested code paths.
7. Low priority: Flesh out README docs, add a `docs/` entry to the repo with deep dives for LLM usage and DI.
   - Why: Better onboarding for contributors.
8. Low priority: Consider moving Selenium from package transitive to per-project addition, or add explicit driver packages only in consumer projects if heavy.
9. Add GitHub Actions steps to build and push Docker images (I can implement a snippet).
10. Add health-check and readiness endpoints for ChildAPI and include those checks in docker-compose.
11. Update ChildAPI to accept a --urls environment variable and expose a health endpoint if required by your monitoring checks.
