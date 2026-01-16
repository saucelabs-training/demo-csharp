using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace MSTest.demo;

[TestClass]
public class CheckoutTests : TestBase
{
    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void SetupTest() => StartChromeSession(TestContext);

    [TestCleanup]
    public void TeardownTest() => QuitSession(TestContext);

    [TestMethod]
    public void BadInfo()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
        driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
        driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();
        driver.FindElement(By.CssSelector("input[data-test='continue']")).Click();

        var classAttr = driver.FindElement(By.CssSelector("input[data-test='firstName']")).GetAttribute("class");
        Assert.IsTrue(classAttr.Contains("error"), "Expected error not found on page");
    }

    [TestMethod]
    public void GoodInfo()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
        driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
        driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();

        driver.FindElement(By.CssSelector("input[data-test='firstName']")).SendKeys("Luke");
        driver.FindElement(By.CssSelector("input[data-test='lastName']")).SendKeys("Perry");
        driver.FindElement(By.CssSelector("input[data-test='postalCode']")).SendKeys("90210");
        driver.FindElement(By.CssSelector("input[data-test='continue']")).Click();

        Assert.AreEqual("https://www.saucedemo.com/checkout-step-two.html", driver.Url, "Information Submission Unsuccessful");
    }

    [TestMethod]
    public void CompleteCheckout()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
        driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
        driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();

        driver.FindElement(By.CssSelector("input[data-test='firstName']")).SendKeys("Luke");
        driver.FindElement(By.CssSelector("input[data-test='lastName']")).SendKeys("Perry");
        driver.FindElement(By.CssSelector("input[data-test='postalCode']")).SendKeys("90210");
        driver.FindElement(By.CssSelector("input[data-test='continue']")).Click();
        driver.FindElement(By.CssSelector("button[data-test='finish']")).Click();

        Assert.AreEqual("https://www.saucedemo.com/checkout-complete.html", driver.Url);
        Assert.IsTrue(driver.FindElement(By.ClassName("complete-text")).Displayed);
    }
}