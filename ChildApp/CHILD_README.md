# ChildApp — Demo consumer for MyLib

This project demonstrates how to consume `MyLib` both as a project reference (local development) and as a NuGet package (package-consumption smoke test).

Key points:
- Uses DI helpers in `MyLib` via `MyLib.DependencyInjection` (AddMyLibServices / AddLlmClient).
- Demonstrates Selenium POM usage using `ChildApp/Pages/LoginPage.cs` (compile-time behavior validated by unit tests; no real browser required unless you add a driver).
- The package-consumption variant uses `ChildApp/ChildApp.Package.csproj`.

Run (ProjectReference):
```bash
dotnet run --project ChildApp
```

Run (PackageReference):
```bash
# Pack MyLib and add local feed, then run the package variant
dotnet pack Parent/src/MyLib/MyLib.csproj -c Release -o ./artifacts
dotnet nuget add source ./artifacts -n localfeed || true
dotnet restore ./ChildApp/ChildApp.Package.csproj
dotnet build ./ChildApp/ChildApp.Package.csproj
dotnet run --project ./ChildApp/ChildApp.Package.csproj
dotnet nuget remove source localfeed || true
```

Tests:
```bash
dotnet test tests/ChildApp.Tests
```

Notes:
- The `ChildApp` codebase depends on `Parent/src/MyLib` for DI services, models and the LLM client.
- To add a headless Selenium E2E test, add a `Selenium.WebDriver.ChromeDriver` package and configure your CI to install Chrome/Chromedriver.
