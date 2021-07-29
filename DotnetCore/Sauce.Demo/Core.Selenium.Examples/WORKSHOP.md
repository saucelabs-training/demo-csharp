# Getting Started With Sauce Workshop

## Pre-reqs

* .NET5 or greater
* C# 9 or greater

## Setup

* Download and install [.NET5](https://dotnet.microsoft.com/download)
* Download and install Visual Studio
* Download [this code branch 30 min before the workshop](https://github.com/saucelabs-training/demo-csharp/tree/2_hr_workshop)
* Get your sauce username and access key and [Store them in environment variables on your machine](https://docs.saucelabs.com/basics/environment-variables/index.html)

## Outline

* Simple desktop test and sauce overview
* Cross-browser massive parallel testing
* Emusim web testing
* Real device web testing
* CICD

## About the author

Nikolay Advolodkin, Principal Solutions Architect, Sauce Labs
- 🔭 I’m the founder of [Ultimate QA](https://ultimateqa.com/)
- 🌱 I’m currently working on [Sauce Bindings](https://github.com/saucelabs/sauce_bindings)
- 🤔 I’m looking for help with [Testing Best Practices](https://github.com/nadvolod/testing-best-practices)
- 💬 Ask me about environmentalism, veganism, testing, and fitness
- 📫 How to reach me:
[Website](https://ultimateqa.com/)
[LinkedIn](https://www.linkedin.com/in/nikolayadvolodkin/)
[Twitter](https://twitter.com/home)
- 😄 Pronouns: he/him
- ⚡ Fun fact: I'm a vegan that's super pasionate about saving the planet, saving animals, and helping underpriveleged communities

## Simple desktop web test

* Open Visual Studio
* Build solution
* Run test called `EdgeW3C` in `Core.Selenium.Examples` project
* Head to [saucelabs.com](https://accounts.saucelabs.com/am/XUI/#login/) to look at the running test

🎖Success is when the test runs

ℹ️ [Web testing best practices tutorial](https://www.youtube.com/watch?v=r9K-2OJUmOE)

## Cross-browser testing

⭐️ Cross-browser functional bugs rarely exist in today's web technology and doing cross-browser functional testing on an entire test suite is inneficient

⭐️ It's more common to find cross-browser rendering issues, especially for responsive web apps. This problem is solved with [visual testing](https://saucelabs.com/platform/visual-testing)

* Go to `Core.Selenium.Examples.CrossBrowser.Start.CrossBrowserTests.cs`
* Find all the `//TODO` and implement them
* Run the tests and notice how fast the parallel build is
* Check out the [Insights Tab](https://app.saucelabs.com/analytics/test-overview)

🎖Success is when all the tests run

ℹ️ [Learn about parallelization with different test runners](https://ultimateqa.com/parallelization-in-csharp/)

## Emusim web testing

* Navigate to `Emusim.Web.Start.AndroidEmusimTests`
* Try to run the android tests using command line

```bash
cd demo-csharp\DotnetCore\Sauce.Demo
dotnet test .\Core.Selenium.Examples --filter TestCategory=android-end
```

🎖The tests should pass

👁Let's take a look at what's going on in this test?

### Your challenge (should you choose to accept it) is to create a similar test but to run on iOS

* ⏰ 10 min
* Go to this class `Core.Selenium.Examples.Emusim.Web.Start.IOSEmusimTests.cs`
* Find all the `//TODO` and implement them
* Run the test

🎖The tests should pass

ℹ️ [Best practices framework with Emusim Web](https://github.com/saucelabs-training/demo-csharp/tree/master/DotnetCore/Sauce.Demo/Core.BestPractices.Web)

## Real devices web testing

* Navigate to `Core.Selenium.Examples.RDC.Web.Start.RealDeviceAndroidWebTests`
* 👁 Let's look at this test and understand it
* Run the test and let's see it in the Sauce dashboard

### Your challenge (should you choose to accept it) is to create a similar test but to run on iOS

* ⏰ 10 min
* Go to this class `Core.Selenium.Examples.RDC.Web.Start.RealDeviceIOSWebTests.cs`
* Find all the `//TODO` and implement them
* Run the test

🎖The tests should pass

## CICD

We have 2 examples. One using [Github Actions](https://github.com/saucelabs-training/demo-csharp/blob/2_hr_workshop/.github/workflows/workshop.yml) and another using [Azure DevOps](https://github.com/saucelabs-training/demo-csharp/blob/2_hr_workshop/workshop.yml).

Let's check out how to create a Github Actions workflow.

* Fork this repo
* Clone this repo
* In the root, add `.github/workflows/ci.yml`
* Copy the code located in [workshop.yml](https://github.com/saucelabs-training/demo-csharp/blob/2_hr_workshop/.github/workflows/workshop.yml) into your file
* Add Secrets to your forked repo
* Push the code to your repo
* You should see staged test execution in CI

🎖The tests should pass

## More resources

We covered 10 days worth of training in 2 hrs! Here are some more important resources for future reference

ℹ️ [Mobile automation training resources](https://github.com/saucelabs-training/demo-java/blob/master/TRAINING.md#mobile-automation-with-appium)

ℹ️ [Testing best practices](https://github.com/nadvolod/testing-best-practices)
