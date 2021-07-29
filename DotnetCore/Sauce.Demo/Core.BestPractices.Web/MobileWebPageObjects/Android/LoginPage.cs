using System;
using Core.BestPractices.Web.DesktopWebPageObjects;
using Core.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace Core.BestPractices.Web.MobileWebPageObjects.Android
{
    public class LoginPage
    {
        public LoginPage(AndroidDriver<AndroidElement> driver)
        {
            Driver = driver;
            Wait = new Wait(Driver);
        }

        private By UsernameLocator { get; } = By.CssSelector("#user-name");

        public AndroidDriver<AndroidElement> Driver { get; }
        public Wait Wait { get; }

        public LoginPage Visit()
        {
            Driver.Navigate().GoToUrl(Constants.BaseUrl);
            return this;
        }

        public ProductsPage Login(string username)
        {
            //SauceJsExecutor.LogMessage(
            //    $"Start login with user=>{username} and pass=>{password}");
            var usernameField = Wait.UntilIsVisible(UsernameLocator);
            usernameField.SendKeys(username);
            Driver.FindElement(By.CssSelector("#password")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector(".btn_action")).Click();
            //SauceJsExecutor.LogMessage($"{MethodBase.GetCurrentMethod().Name} success");
            return new ProductsPage(Driver);
        }

        public Action IsVisible()
        {
            return IsElementVisible;
        }

        private void IsElementVisible()
        {
            new Wait(Driver).UntilIsVisible(UsernameLocator);
        }
    }
}