# Playwright Examples

This project contains Playwright test examples for the [Sauce Demo](https://www.saucedemo.com/) 
website using C# and NUnit, designed to run on **Sauce Labs** using the Connect over CDP feature.

## Overview

These tests leverage Playwright's CDP (Chrome DevTools Protocol) connection to run tests on 
Sauce Labs' cloud infrastructure. This approach:
- Creates a WebDriver session on Sauce Labs
- Retrieves the CDP endpoint from the session
- Connects Playwright to the remote browser via `ConnectOverCDPAsync`
- Reports test results back to Sauce Labs

## Prerequisites

- .NET 8.0 SDK
- Sauce Labs account with valid credentials
- `SAUCE_USERNAME` and `SAUCE_ACCESS_KEY` environment variables set

## Setup

1. Set your Sauce Labs credentials:
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

Run all tests:
```bash
dotnet test
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

## Viewing Test Results

After tests complete, you can view the results in the [Sauce Labs Dashboard](https://app.saucelabs.com/). 
The console output will include direct links to each test job:

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
├── TestBase.cs          # Sauce Labs CDP connection setup
└── Tests/
    ├── AuthenticationTests.cs
    ├── CartTests.cs
    ├── CheckoutTests.cs
    ├── NavigationTests.cs
    └── SortingTests.cs
```

## Configuration

The Sauce Labs configuration is defined in `TestBase.cs`:

| Setting | Default Value |
|---------|---------------|
| Data Center | US West (`ondemand.us-west-1.saucelabs.com`) |
| Platform | macOS 13 |
| Browser | Chrome |

### Adding Annotations

You can add annotations to your tests that will appear in the Sauce Labs command log:

```csharp
await AnnotateAsync("Starting checkout process");
```
