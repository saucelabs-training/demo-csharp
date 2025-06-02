using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace MSTest.demo;

[TestClass]
public class CartTest : TestBase
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task AddFromProductPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync(TestContext);

            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-bolt-t-shirt']")).Click();

            Assert.AreEqual(
                "1",
                driver.FindElement(By.ClassName("shopping_cart_badge")).Text,
                "Item not correctly added to cart");
        });
    }

    [TestMethod]
    public async Task RemoveFromProductPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync(TestContext);

            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-bolt-t-shirt']")).Click();
            driver.FindElement(By.CssSelector("button[data-test='remove-sauce-labs-bolt-t-shirt']")).Click();

            Assert.IsTrue(
                driver.FindElements(By.ClassName("shopping_cart_badge")).Count == 0,
                "Item not correctly removed from cart");
        });
    }

    [TestMethod]
    public async Task AddFromInventoryPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync(TestContext);

            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();

            Assert.AreEqual(
                "1",
                driver.FindElement(By.ClassName("shopping_cart_badge")).Text,
                "Item not correctly added from inventory");
        });
    }

    [TestMethod]
    public async Task RemoveFromInventoryPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync(TestContext);

            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-bike-light']")).Click();
            driver.FindElement(By.CssSelector("button[data-test='remove-sauce-labs-bike-light']")).Click();

            Assert.IsTrue(
                driver.FindElements(By.ClassName("shopping_cart_badge")).Count == 0,
                "Shopping Cart is not empty");
        });
    }

    [TestMethod]
    public async Task RemoveFromCartPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync(TestContext);

            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-backpack']")).Click();
            driver.FindElement(By.ClassName("shopping_cart_link")).Click();
            driver.FindElement(By.CssSelector("button[data-test='remove-sauce-labs-backpack']")).Click();

            Assert.IsTrue(
                driver.FindElements(By.ClassName("shopping_cart_badge")).Count == 0,
                "Shopping Cart is not empty");
        });
    }
}