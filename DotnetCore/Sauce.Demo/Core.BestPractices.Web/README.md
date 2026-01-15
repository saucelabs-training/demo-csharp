# Best Practices for Web Testing

A good testing strategy doesn't only focus on web testing. Instead, it tackles risks at all levels of the system.

In this repository, you will find a cohesive Greybox (we have some insight into the app code) testing strategy using:

✅ Browser testing on desktop

✅ Browser testing on mobile

✅ Visual testing

✅ Accessibility testing

✅ CICD pipeline executed on push and PR

✅ Sauce Labs cloud infrastructure

## Test Strategy

| Expected Behavior  | Tested? | Test Type  | Rationale  | Tech                 |
|---|---|---|---|----------------------|
| Every web page of the app looks correct on desktop | ✅ | Visual test | A visual test efficiently validates app rendering | Selenium             |
| Every web page of the app looks correct on mobile  | ✅ | Visual test | A visual test efficiently validates app rendering | Selenium             |
| A user can successfully check out on desktop  | ✅ | Functional web test  | Functional testing of the most critical functionality is important | Selenium             |
| A user can successfully check out on mobile  | ✅ | Functional mobile test  | Although redundant to a functional web test, it's relatively easy to test this on a mobile device as well  | Appium               |
| App is accessibility friendly  | ✅ | Selenium web test | Accessibility in applications is becoming extremely important  | Selenium, Axe        
| Front-end performance is at least an A  | 🙅‍♂️ | Front-end performance test  | Front-end perf is an important aspect of any digital quality effort | Selenium, Sauce Labs |
| Test code runs on every commit in under 5 minutes  | 🙅‍♂️ | CICD  | Slow feedback makes it hard to iterate  | Github Actions       |
| App is secure  | 🙅‍♂️ | Not covered here, but something to consider for testing strategy  |   |


## ⚙️Setup

* [Download .NET 5](https://dotnet.microsoft.com/download)
* Open terminal and run `dotnet --info` to ensure installation.
* `git clone https://github.com/saucelabs-training/demo-csharp.git`
* Download an IDE of your choice. Probably VS for Mac, or VS Code.

## Running all tests

* Run all the tests inside of the `Core.BestPractices.Web.csproj`

```bash
cd demo-csharp/DotnetCore/Sauce.Demo/Core.BestPractices.Web/
dotnet test
```

### Run accessibility test

```bash
cd demo-csharp/DotnetCore/Sauce.Demo/Core.BestPractices.Web/
dotnet test --filter Name=AccessibilityTest --verbosity normal
```

### Run visual tests

```
cd demo-csharp/DotnetCore/Sauce.Demo/Core.BestPractices.Web/
dotnet test --filter VisualTests --verbosity normal
```


