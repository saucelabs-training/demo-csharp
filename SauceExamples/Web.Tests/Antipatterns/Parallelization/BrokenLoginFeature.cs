using FluentAssertions;
using NUnit.Framework;
using Selenium3.Nunit.Framework.BestPractices.Pages;

namespace Selenium3.Nunit.Framework.Antipatterns.Parallelization
{
    [TestFixture]
    [TestFixtureSource(typeof(CrossBrowserData),
        nameof(CrossBrowserData.SimpleConfiguration))]
    [Parallelizable]
    public class BrokenLoginFeature : BrokenBaseTest
    {
        private SauceDemoLoginPage _loginPage;

        public BrokenLoginFeature(string browser, string version, string os) :
            base(browser, version, os)
        {

        }
        [SetUp]
        public void RunBeforeEveryTest()
        {
            _loginPage = new SauceDemoLoginPage(Driver);
        }

        [Test]
        public void LoginPageShouldLoad()
        {
            _loginPage.Open().IsLoaded.Should().BeTrue("the login page should load successfully.");
        }

    }
}
