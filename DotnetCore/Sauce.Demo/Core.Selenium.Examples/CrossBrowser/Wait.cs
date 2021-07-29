using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Core.Selenium.Examples.CrossBrowser
{
    public class Wait
    {
        private readonly By _locator;

        public Wait(IWebDriver driver)
        {
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        public Wait(IWebDriver driver, By locator)
        {
            _locator = locator;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        public Wait(IWebDriver driver, int timeoutInSeconds)
        {
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        }

        private WebDriverWait _wait { get; }

        public IWebElement UntilIsVisible(By locator)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public IWebElement UntilIsVisibleByClass(string className)
        {
            return UntilIsVisible(By.ClassName(className));
        }

        public bool IsVisible()
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(_locator)).Displayed;
        }

        public IWebElement UntilIsVisibleById(string id)
        {
            return UntilIsVisible(By.Id(id));
        }

        //This type of method is really useful for negative assertions
        //To wait until something isn't displayed and then assert on it
        public bool UntilIsDisplayedById(string id)
        {
            bool isDisplayed;
            try
            {
                isDisplayed = UntilIsVisible(By.Id(id)).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }

            return isDisplayed;
        }

        public IWebElement UntilIsVisibleByCss(string css)
        {
            return UntilIsVisible(By.CssSelector(css));
        }
    }
}