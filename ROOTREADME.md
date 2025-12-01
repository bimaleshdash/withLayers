# Workspace overview — MyLib + ChildApp

Welcome! This repository contains the `MyLib` library under `Parent/src/MyLib` and example consumers (ChildApp and ChildAPI).
See `Parent/README.md` for library-specific docs, `ChildApp/README.md` and `ChildAPI/README.md` for details about the consumers and how to run them.

Quick highlights: see `docs/HIGHLIGHTS.md` for a short, actionable overview of the workspace, CI, packaging, and LLM usage.

Docker quickstart: see `ROOTREADME.DOCKER.md` for a quick root-level Docker guide and `docs/DOCKER.md` for more details.



This workspace contains multiple independent apps; `MyLib` is the base package with services, models and infrastructure. Child apps can consume `MyLib` as a project reference or as a package.

What's installed workspace-wide
 - `Selenium.WebDriver` is included as a dependency of `MyLib` (see `src/MyLib/MyLib.csproj`) so child apps consuming `MyLib` get Selenium transitively.

How to override or add drivers
- To pin a different version, add a `PackageReference` in the project and set your desired `Version`.
- To add a WebDriver binary e.g. ChromeDriver, add `Selenium.WebDriver.ChromeDriver` to the project's csproj.

Running tests (including ChildApp UI / POM tests):
```bash
dotnet test
```

The `tests/ChildApp.Tests` project contains a `LoginPage` unit test that uses Moq to validate the Page Object Model behavior without requiring a browser.




# RestAssured.Net in MyLib

This library includes `RestAssured.Net` as a development/test aid for API contract tests and internal tooling. It is intentionally marked as PrivateAssets so the dependency does not flow transitively to consumers of `MyLib`.

Reasoning:
- `MyLib` can include internal test helpers or utilities for validating APIs.
- Consumers should not automatically receive REST-assured dependencies unless they opt in.

If you want consumers to depend on RestAssured.Net, remove `PrivateAssets="all"` in `MyLib.csproj` and/or add the package to the consumer project's csproj directly.
