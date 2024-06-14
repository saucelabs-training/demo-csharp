# visual-testing-worker-tests

## Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
- [Usage](#usage)

## About <a name = "about"></a>

These are the webdriver tests using C#.

## Getting Started <a name = "getting_started"></a>

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

#### Using VS Studio

* Create a new NUnit test project

<img src="assets/nunit.png"/>

* Add all the necessary environment variables
  ```
  setx SELENIUM_PROTOCOL "http"
  setx SELENIUM_HOST "staging-hub.screener.io"
  setx SELENIUM_PORT "80"
  setx SAUCE_USERNAME <YOUR_SAUCE_USERNAME>
  setx SAUCE_ACCESS_KEY <YOUR_SAUCE_ACCESS_KEY>
  setx SCREENER_API_KEY <YOUR_SCREENER_API_KEY>
  ```
* Update the default test with the unit test: UnitTest1.cs

## Usage <a name = "usage"></a>

From the folder containing the project run:
```
dotnet test
```
