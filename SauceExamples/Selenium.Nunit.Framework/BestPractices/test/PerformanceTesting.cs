using NUnit.Framework;
using Selenium.Nunit.Framework.BestPractices.Pages;

namespace Selenium.Nunit.Framework.BestPractices.test
{
    [TestFixture]
    [Parallelizable]
    [Category("Performance")]
    public class PerformanceTesting : BaseTest
    {
        private SauceDemoLoginPage _loginPage;

        public PerformanceTesting(string browser, string version, string os) :
            base(browser, version, os)
        {
        }

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
            _loginPage = new SauceDemoLoginPage(Driver);
        }
    }
}
