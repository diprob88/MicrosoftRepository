using NUnit.Framework;
using NUnitSelenium.Constants;
using NUnitSelenium.Model;
using OpenQA.Selenium;

namespace NUnitSelenium.TestClass
{

    [TestFixture]
    public class NUnitTest : TestBase
    {
        [Test]
        public void Login()
        {
            // create an user for the access
            User user = new User("paolo","calimero");
            //Insert Username and Password with IWebElement
            var username = wdriver.FindElement (By.XPath(XPathPagesConstants.userID));
            username.SendKeys(user.UserName);
            var password = wdriver.FindElement(By.XPath(XPathPagesConstants.passID));
            password.SendKeys(user.Password);
            //Click on Enter Button
            wdriver.FindElement(By.XPath(XPathPagesConstants.EnterButtonXPath)).Click();
            //Element with the write "CALCOLO ABBATTIMENTI"
            var targetElement = wdriver.FindElement(By.XPath(XPathPagesConstants.EnterHomeElementXPath)).Text;
            Assert.AreEqual(targetElement.ToUpper(), XPathPagesConstants.EnterHomeElementText.ToUpper());

            //Thread.Sleep(5000);
        }
    }
}
