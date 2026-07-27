# Playwright Examples

This project contains Playwright test examples for the [Sauce Demo](https://www.saucedemo.com/)
website using C# and NUnit. Tests run either against a **locally launched browser** (the default)
or on **Sauce Labs** via its native Playwright WebSocket endpoint - switching between them is a
single environment variable, and everything else about the test (context/page isolation, session
pooling, browser engine selection) behaves identically either way.

## Overview

`TARGET` picks where the browser lives:
- `TARGET` unset or `local` (default): launches a browser on the machine running the tests, via
  `IBrowserType.LaunchAsync`. No Sauce Labs account needed.
- `TARGET=sauce`: connects to Sauce Labs' native Playwright WebSocket endpoint instead - creates a
  native Playwright session on Sauce Labs, connects via `ConnectAsync` to the returned WebSocket
  endpoint, and reports test results back to Sauce Labs.

`BROWSER` picks the engine (`chromium`, `firefox` or `webkit`, default `chromium`) - it means the
same thing for both targets, so `BROWSER=firefox` runs Firefox whether you're local or on Sauce.

`GROUPING` picks how sessions are shared across tests:
- `GROUPING` unset or `class` (default): one session per test class/fixture, named after the class.
  Every test class is `[Parallelizable(ParallelScope.Fixtures)]`, so different classes run in
  parallel with each other, but tests *within* one class run sequentially - meaning exactly one
  test at a time ever uses a given class's session.
- `GROUPING=test`: one dedicated, never-shared session per test, named after the test itself.

### Session lifecycle

A session (local browser or Sauce Labs job) is checked out for a test and, on teardown, either kept
alive for reuse (test passed) or closed immediately (test failed) - so a failing test never leaves
a possibly-dirty browser behind for whatever test picks up that session next. Each test still gets
its own fresh, isolated `BrowserContext`/`Page` regardless of session reuse. Whatever sessions are
still open once the whole run finishes are closed out by `SauceSessionsTeardown` in `TestBase.cs`.
This applies the same way regardless of `TARGET` or `GROUPING`.

## Prerequisites

- .NET 8.0 SDK
- For `TARGET=sauce`: a Sauce Labs account, with `SAUCE_USERNAME` and `SAUCE_ACCESS_KEY`
  environment variables set

## Setup

1. If running against Sauce Labs, set your credentials:
   ```bash
   export SAUCE_USERNAME="your-username"
   export SAUCE_ACCESS_KEY="your-access-key"
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

## Running Tests

Run all tests locally (default):
```bash
dotnet test
```

Run all tests on Sauce Labs:
```bash
TARGET=sauce dotnet test
```

Run locally with a specific browser engine:
```bash
BROWSER=firefox dotnet test
```

Run with one session per test instead of one per class:
```bash
GROUPING=test dotnet test
```

Run tests with detailed output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

Run specific test class:
```bash
dotnet test --filter "FullyQualifiedName~AuthenticationTests"
```

Run a single test:
```bash
dotnet test --filter "Name=SignInSuccessful"
```

Run with an HTML report (built into the .NET test platform, no extra package needed):
```bash
dotnet test --logger "html;LogFileName=report.html"
```
This writes `TestResults/report.html` with a pass/fail/duration table for the run. It works the
same way regardless of `TARGET`.

## Viewing Test Results

When `TARGET=sauce`, you can view the results in the [Sauce Labs Dashboard](https://app.saucelabs.com/).
The console output will include direct links to each test job (not printed for local runs, since
there's no Sauce job to link to):

```
SauceOnDemandSessionID=<session-id> job-name=<test-name>
Test Job Link: https://app.saucelabs.com/tests/<session-id>
```

## Test Categories

- **AuthenticationTests**: Login and logout functionality
- **CartTests**: Add/remove items from cart
- **CheckoutTests**: Complete checkout flow and validation
- **NavigationTests**: Site navigation and menu functionality
- **SortingTests**: Product sorting functionality

## Project Structure

```
PlaywrightExamples/
├── PlaywrightExamples.csproj
├── README.md
├── TestBase.cs          # Local/Sauce Labs session setup (class-grouped or per-test)
└── Tests/
    ├── AuthenticationTests.cs
    ├── CartTests.cs
    ├── CheckoutTests.cs
    ├── NavigationTests.cs
    └── SortingTests.cs
```

## Configuration

Configuration is defined in `TestBase.cs` and controlled via environment variables:

| Variable | Default | Description |
|----------|---------|--------------|
| `TARGET` | `local` | Where the browser runs: `local` or `sauce` |
| `BROWSER` | `chromium` | Browser engine: `chromium`, `firefox`, `webkit` - same meaning for both targets |
| `GROUPING` | `class` | Session sharing: `class` (one session per test class) or `test` (one per test) |
| `SAUCE_REGION` | `us-west-1` | Data center for all Sauce Labs URLs (e.g. `us-east-1`, `eu-central-1`). Only used when `TARGET=sauce` |

Examples:
```bash
BROWSER=firefox dotnet test
GROUPING=test TARGET=sauce BROWSER=webkit SAUCE_REGION=eu-central-1 dotnet test
```

Note: `GROUPING` only controls how `TestBase.cs` shares Sauce/local sessions across tests - it
can't change how many tests NUnit actually runs concurrently, since that's governed by each test
class's `[Parallelizable]` attribute, a compile-time setting. Test classes are currently
`ParallelScope.Fixtures` (classes run in parallel with each other, tests within one class run
sequentially), which is what makes `GROUPING=class` safe - if the intra-class concurrency needs to
change, that's a separate edit to `Tests/*.cs`, not something `GROUPING` can flip at runtime. To
control the number of concurrent NUnit workers directly, use `dotnet test -- NUnit.NumberOfTestWorkers=N`.
