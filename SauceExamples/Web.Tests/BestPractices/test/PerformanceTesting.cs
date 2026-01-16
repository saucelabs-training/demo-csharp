using Common;
using Common.SauceLabs;
using NUnit.Framework;
using OpenQA.Selenium;
using Selenium3.Nunit.Framework.BestPractices.Pages;

namespace Selenium3.Nunit.Framework.BestPractices.test
{
    [TestFixture]
    [Parallelizable]
    [Category("Performance")]
    [Ignore("need to add the capability to set performance testing to true")]
    public class PerformanceTesting
    {
        private SauceDemoLoginPage _loginPage;

        [Test]
        public void LoginPageSpeedIndexIsWithin20Percent()
        {
            _loginPage.Open();
            var performanceMetrics = _loginPage.GetPerformance();

            Assert.That(performanceMetrics["speedIndex"], Is.EqualTo(415).Within(20).Percent);
        }
        [SetUp]
        public void RunBeforeEveryTest()
        {

            Driver = new WebDriverFactory().CreateSauceDriver("chrome", "latest", "windows 10");
            _loginPage = new SauceDemoLoginPage(Driver);
        }

        public IWebDriver Driver { get; set; }
    }
}
