using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

namespace Core.Appium.Nunit.BestPractices.Screens.iOS
{
    public class LoginScreen : BaseIosScreen
    {
        public LoginScreen(IOSDriver driver) : base(driver)
        {
        }

        public Action IsVisible()
        {
            return IsUsernameFieldVisible;
        }

        private void IsUsernameFieldVisible()
        {
            WaitFor.UntilIsVisible(
                MobileBy.AccessibilityId("test-Username"));
        }

        public void Login(string username, string password)
        {
            var userName = WaitFor.UntilIsVisible(
                MobileBy.AccessibilityId("test-Username"));
            userName.SendKeys(username);

            var passwordField = WaitFor.UntilIsVisible(
                MobileBy.AccessibilityId("test-Password"));
            passwordField.SendKeys(password);

            var login = WaitFor.UntilIsVisible(
                MobileBy.AccessibilityId("test-LOGIN"));
            login.Click();
        }
    }
}