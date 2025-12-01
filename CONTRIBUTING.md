# Contributing to this repository

Thanks for contributing! This repository contains a packable library (`MyLib`) under `Parent/src/MyLib` and an example consumer (`ChildApp`) to exercise the package.

This doc covers common dev operations: building, running tests, switching between ProjectReference and PackageReference for the consumer, packing the library and testing package consumption, and publishing to an Azure Artifact feed.

Prerequisites
- Install .NET 8 SDK: https://dotnet.microsoft.com
- Optionally, install Chrome / ChromeDriver if you plan to run real Selenium E2E tests.

Build & Test
```bash
# Restore, build and run all tests in repo
dotnet restore
dotnet build
dotnet test
```

ChildApp - ProjectReference (local dev)
```bash
# Run the child app using the local project reference to MyLib
dotnet run --project ChildApp
```

ChildApp - PackageReference (package-consumer smoke-test)
```bash
# Pack MyLib into a local artifacts folder
dotnet pack Parent/src/MyLib/MyLib.csproj -c Release -o ./artifacts

# Add the local artifacts folder as a nuget source (named 'localfeed')
dotnet nuget add source ./artifacts -n localfeed || true

# Restore / build / run the package-consuming copy of ChildApp
dotnet restore ./ChildApp/ChildApp.Package.csproj
dotnet build ./ChildApp/ChildApp.Package.csproj
dotnet run --project ./ChildApp/ChildApp.Package.csproj

# Remove the local feed when finished
dotnet nuget remove source localfeed || true
```

Testing ChildApp UI/POM (no browser required)
```bash
dotnet test tests/ChildApp.Tests
```
The test suite uses Moq to mock `IWebDriver` / `IWebElement` and validates the Login POM behavior.

CI
- The main CI workflow is `.github/workflows/dotnet.yml` (runs on push / PR against `main` and `master`). The workflow runs unit tests and includes a `release-test` job that validates both the `project` (ProjectReference) and `package` (PackageReference) consumer variants using a matrix job.

Publishing to Azure Artifacts (optional)
1. Pack the library as shown above and place the `.nupkg` into `./artifacts`.
2. Push the package:
```bash
dotnet nuget push ./artifacts/*.nupkg --source "https://pkgs.dev.azure.com/<ORG>/_packaging/<FEED>/nuget/v3/index.json" --api-key <AZURE_API_KEY>
```
In CI, set the secrets `AZURE_ARTIFACTS_FEED_URL` and `AZURE_ARTIFACTS_API_KEY` and the workflow will publish packages automatically.

Versioning
- The repo centralizes `PackageVersion` in `Parent/Directory.Build.props`. Update that file to bump the MyLib release. Alternatively, you can override the version on the `dotnet pack` command with `-p:Version=1.0.1`.

Selenium E2E tests
- If you want to run a real headless browser test, add `Selenium.WebDriver.ChromeDriver` (or another driver) to your consumer project and modify CI to install Chrome/Edge. Example test-run steps may need the runner to provide a browser binary.

Contribution workflow
1. Add tests for new behavior (unit + integration when relevant).
2. Run `dotnet test` locally and confirm all tests pass.
3. Increment `Parent/Directory.Build.props` PackageVersion for public releases.
4. Create a PR with a small, focused change and explain motivation and testing steps.
5. Maintainers will review and run CI; merge once passing.

If you need a hand or are unsure about a step, open an issue or ping a maintainer in your org.
