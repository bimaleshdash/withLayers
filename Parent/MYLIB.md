# MyLib — Library README (Parent/src/MyLib)

This repository contains a small, self-contained C# app created for editor/agent onboarding and demonstration purposes.

Requirements
- .NET 8 SDK (the CI uses `dotnet 8.0.x`).

Selenium
- `Selenium.WebDriver` is included as a dependency of `MyLib` (see `src/MyLib/MyLib.csproj`) so child apps consuming `MyLib` get Selenium transitively. If you want only particular child projects to have Selenium, add the package (or a driver package) to those projects directly.

Structure
- Parent/src/MyLib - base library containing services/models and LLM API
- ChildApp - example consumer/child app that uses `MyLib` either via project or package reference (see `ChildApp/README.md`)
- tests/MyLib.Tests - unit tests

Build & Test (locally):
```bash
dotnet restore
dotnet build
dotnet test
```

Child app tests (UI/POM):
The `ChildApp` project includes a Page Object Model (POM) for the login page. A dedicated unit test project at `tests/ChildApp.Tests` validates the `LoginPage` behavior using Moq (no real browser required).

Run:
```bash
dotnet test tests/ChildApp.Tests
```

Examples: changing project reference to package reference (child consumers)
In `ChildApp/ChildApp.csproj` you can switch the project reference to a package reference to simulate consuming the hosted package:

```xml
<ItemGroup>
	<!-- Project reference used during local development -->
	<!-- <ProjectReference Include="../MyLib/MyLib.csproj" /> -->
	<!-- Package reference used when consuming the NuGet package -->
	<PackageReference Include="MyLib" Version="1.0.0" />
</ItemGroup>
```
Create and publish a package to an Azure Artifacts feed (example):
```bash
# pack
dotnet pack src/MyLib/MyLib.csproj -c Release -o ./artifacts
# push (requires PAT/API key and feed URL)
dotnet nuget push ./artifacts/*.nupkg --source https://pkgs.dev.azure.com/<ORG>/_packaging/<FEED>/nuget/v3/index.json --api-key <PAT>
```

Versioning note: this repo uses `Directory.Build.props` to centralize `PackageVersion` used by library projects. Update `Directory.Build.props` to bump the package version, or override on pack with `dotnet pack /p:Version=1.0.1`.


Continuous Integration: see `.github/workflows/dotnet.yml`.

CI details:
- The workflow includes a `build` job that restores, builds, tests, and packs `MyLib`.
- The workflow also includes a `release-test` job that runs a matrix to verify both a ProjectReference consumer and a PackageReference consumer, confirming the package produced by `MyLib` is usable in the package variant.

Architecture
- Simple service + model + console host.

Purpose
- Provide a small codebase for demonstrations (scaffolding, CI, and Copilot instructions).

Child consumer project
`ChildApp` demonstrates a consumer that uses DI and `MyLib`'s LLM client. It references `MyLib` via ProjectReference for local dev; to use the packaged NuGet, switch to a `PackageReference` and configure `nuget.config` to point to your feed (or use `dotnet add source` to a local folder).

Run the demo consumer:
```bash
dotnet run --project ChildApp
```

Test consuming MyLib as a package (local workflow):
```bash
# This script packs MyLib, adds a local nuget source and tests building ChildApp.Package.csproj
bash ./scripts/test-consume-package.sh
```
