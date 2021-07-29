using Core.Common;
using OpenQA.Selenium;

namespace Core.BestPractices.Web.DesktopWebPageObjects
{
    public class BaseWebPage
    {
        public readonly IWebDriver Driver;

        public BaseWebPage(IWebDriver driver)
        {
            Driver = driver;
            BaseUrl = "https://www.saucedemo.com";
        }

        public IJavaScriptExecutor JavaScriptExecutor => (IJavaScriptExecutor) Driver;

        //public SauceJavaScriptExecutor SauceJsExecutor =>
        //    new SauceJavaScriptExecutor(_driver);

        public Wait Wait => new(Driver);
        public string BaseUrl { get; }

        public void TakeSnapshot()
        {
            JavaScriptExecutor.ExecuteScript("/*@visual.snapshot*/", GetType().Name);
        }
    }
}