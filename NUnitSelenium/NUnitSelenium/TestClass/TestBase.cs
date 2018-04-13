using NUnit.Framework;
using NUnitSelenium.Constants;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Threading;

namespace NUnitSelenium.TestClass
{
    public class TestBase
    {
        protected IWebDriver wdriver;

        [SetUp]
        public void Setup()
        {
            string browser = "chrome";

            switch (browser)
            {
                case "chrome":
                    wdriver = new ChromeDriver(PathProjectConstats.pathChromeDriver); //Launches Browser
                    break;
                case "explorer":
                    wdriver = new InternetExplorerDriver(PathProjectConstats.pathIEDriver);
                    break;
            }
            wdriver.Manage().Window.Maximize(); //Maximize Browser
            wdriver.Navigate().GoToUrl(PathProjectConstats.urlHome);
            Thread.Sleep(5000);
        }

        [TearDown]
        public void Cleanup()
        {
            wdriver.Quit();
        }
    }
}
