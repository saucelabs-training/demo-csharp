using NUnit.Framework;
using OpenQA.Selenium;

namespace NUnitExamples.demo;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class AuthenticationTest : TestBase
{
    [Test]
    public async Task SignInUnSuccessful()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();

            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");

            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("locked_out_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            var errorElement = Driver.FindElement(By.CssSelector("[data-test='error']"));
            Assert.That(errorElement.Text, Does.Contain("Sorry, this user has been locked out"), 
                "Error message not found or incorrect");
        });
    }
    
    [Test]
    public async Task SignInSuccessful()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();

            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");

            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            Assert.That(Driver.Url, Is.EqualTo("https://www.saucedemo.com/inventory.html"), 
                "Login Not Successful");
        });
    }
    
    [Test]
    public async Task Logout()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();

            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");

            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            Driver.FindElement(By.Id("react-burger-menu-btn")).Click();
            await Task.Delay(1000); // Not ideal, but matches the Java code

            Driver.FindElement(By.Id("logout_sidebar_link")).Click();

            Assert.That(Driver.Url, Is.EqualTo("https://www.saucedemo.com/"), 
                "Logout Not Successful");
        });
    }
}