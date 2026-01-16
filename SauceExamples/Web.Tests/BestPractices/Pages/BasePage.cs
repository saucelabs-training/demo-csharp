using Common;
using Common.SauceLabs;
using OpenQA.Selenium;

namespace Selenium3.Nunit.Framework.BestPractices.Pages
{
    public class BasePage
    {
        public readonly IWebDriver _driver;
        private readonly string _baseUrl;

        public SauceJavaScriptExecutor SauceJsExecutor =>
            new SauceJavaScriptExecutor(_driver);

        public Wait Wait => new Wait(_driver);
        public string BaseUrl => _baseUrl;

        public BasePage(IWebDriver driver)
        {
            _driver = driver;
            _baseUrl = "https://www.saucedemo.com";
        }
    }
}