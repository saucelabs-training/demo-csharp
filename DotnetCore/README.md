# Dotnet Sauce Examples

All examples in this repo are using the latest version of .NET. .NET Framework is now obsolete, but those examples are here.

## ⚙️Setup
* Download [.NET](https://dotnet.microsoft.com/download) (not .NET Core)
* Open terminal and run dotnet --info to ensure installation.
* `git clone https://github.com/saucelabs-training/demo-csharp.git`
* Download an IDE of your choice. Probably VS for Mac, or VS Code.

## TOC

* [Best practices for web testing](./DotnetCore/Sauce.Demo/Core.BestPractices.Web)
* [Selenium test](./DotnetCore/Sauce.Demo/Core.Selenium.Examples/DesktopTests.cs)

## Running tests

* Run all the tests inside of the `Core.BestPractices.Web.csproj`

```bash
cd demo-csharp/DotnetCore/Sauce.Demo/Core.BestPractices.Web/
# run all tests
dotnet test
# run only visual tests
cd demo-csharp/DotnetCore/Sauce.Demo/Core.Selenium.Examples
dotnet test --filter VisualTestOnChrome
```
