# Automation Best Practices for Mobile

This is a repository that shows how to do mobile automation best practices on Sauce Labs.
The code and frameworks that you see here are what Sauce believes are the industry-best
solutions.

## Run the tests

* Clone the code

```bash
cd demo-csharp/SauceExamples/Core.Appium.MsTest.BestPractices
dotnet test --verbosity normal
```

* Output will look something like this
```
Determining projects to restore...
  Restored C:\Source\SauceLabs\demo-csharp\SauceExamples\Core.Appium.MsTest.BestPractices\Core.Appium.Nunit.BestPractices.csproj (in 405 ms).
  You are using a preview version of .NET. See: https://aka.ms/dotnet-core-preview
C:\Source\SauceLabs\demo-csharp\SauceExamples\Common\WebDriverFactory.cs(83,70): warning CS0618: 'DesiredCapabilities' is obsolete: 'Use of DesiredCapabilities has been deprecated in favor of browser-specific Options classes' [C:\Source\SauceLabs\demo-csharp\SauceExamples\Common\Common.csproj]
```
