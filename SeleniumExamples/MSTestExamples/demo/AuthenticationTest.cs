using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace MSTest.demo;

[TestClass]
public class AuthenticationTest : TestBase
{
    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void SetupTest() => StartChromeSession(TestContext);

    [TestCleanup]
    public void TeardownTest() => QuitSession(TestContext);

    [TestMethod]
    public void SignInUnSuccessful()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");

        driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("locked_out_user");
        driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

        var errorElement = driver.FindElement(By.CssSelector("[data-test='error']"));
        Assert.IsTrue(
            errorElement.Text.Contains("Sorry, this user has been locked out"),
            "Error message not found or incorrect"
        );
    }
    
    [TestMethod]
    public void SignInSuccessful()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");

        driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

        Assert.AreEqual("https://www.saucedemo.com/inventory.html", driver.Url, "Login Not Successful");
    }
    
    [TestMethod]
    public void Logout()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");

        driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

        driver.FindElement(By.Id("react-burger-menu-btn")).Click();
        Thread.Sleep(1000);

        driver.FindElement(By.Id("logout_sidebar_link")).Click();

        Assert.AreEqual("https://www.saucedemo.com/", driver.Url, "Logout Not Successful");
    }
}