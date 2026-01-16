using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Common
{
    public class Wait
    {
        private IWebDriver _driver;
        private WebDriverWait _wait { get; set; }

        private readonly By _locator;

        public Wait(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        }

        public IWebElement UntilIsVisible(By locator)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
        public IWebElement UntilIsVisibleByClass(string className)
        {
            return UntilIsVisible(By.ClassName(className));
        }
        public Wait(IWebDriver driver, By locator)
        {
            _driver = driver;
            _locator = locator;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        }

        public Wait(IWebDriver driver, int timeoutInSeconds)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
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
