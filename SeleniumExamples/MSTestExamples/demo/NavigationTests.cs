using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace MSTest.demo;

[TestClass]
public class NavigationTests : TestBase
{
    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void SetupTest() => StartChromeSession(TestContext);

    [TestCleanup]
    public void TeardownTest() => QuitSession(TestContext);

    [TestMethod]
    public void CancelFromCart()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
        driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        driver.FindElement(By.CssSelector("button[data-test='continue-shopping']")).Click();

        Assert.AreEqual("https://www.saucedemo.com/inventory.html", driver.Url);
    }

    [TestMethod]
    public void CancelFromInfoPage()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
        driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
        driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();
        driver.FindElement(By.CssSelector("button[data-test='cancel']")).Click();

        Assert.AreEqual("https://www.saucedemo.com/cart.html", driver.Url);
    }

    [TestMethod]
    public void CancelFromCheckoutPage()
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
        driver.FindElement(By.CssSelector("button[data-test='cancel']")).Click();

        Assert.AreEqual("https://www.saucedemo.com/inventory.html", driver.Url);
    }

    [TestMethod]
    public void StartCheckout()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
        driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
        driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();

        Assert.AreEqual("https://www.saucedemo.com/checkout-step-one.html", driver.Url);
    }
}