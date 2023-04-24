using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium;

namespace AppiumMobileTests
{
    public class AppiumMobileTests
    {
        private const string appiumUrl = "http://[::1]:4723/wd/hub";
        private const string appLocation = @"C:\Projects\SofUni_Course\QA_Automation_FrontEnd\RegularExam\contactbook-androidclient.apk";
        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;
        private const string appServer = "https://contactbook.kameliaibowska.repl.co/api";

        [OneTimeSetUp]
        public void PrepareApp()
        {
            options = new AppiumOptions();
            options.AddAdditionalCapability(MobileCapabilityType.App, appLocation);
            options.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            this.driver = new AndroidDriver<AndroidElement>(new Uri(appiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [OneTimeTearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_SearchAndAssertContact()
        {
            string keyword = "steve";

            // Change the URL of the backend
            var inputAppUrl = driver.FindElementById("contactbook.androidclient:id/editTextApiUrl");
            inputAppUrl.Clear();
            inputAppUrl.SendKeys(appServer);

            // Press connect button
            var connectButton = driver.FindElementById("contactbook.androidclient:id/buttonConnect");
            connectButton.Click();

            // Search for a keyword
            var serachTextBox = driver.FindElementById("contactbook.androidclient:id/editTextKeyword");
            serachTextBox.SendKeys(keyword);

            var buttonSerach = driver.FindElementById("contactbook.androidclient:id/buttonSearch");
            buttonSerach.Click();

            // Asserts
            var firstName = driver.FindElementById("contactbook.androidclient:id/textViewFirstName");
            Assert.That(firstName.Text, Is.EqualTo("Steve"));

            var lasttName = driver.FindElementById("contactbook.androidclient:id/textViewLastName");
            Assert.That(lasttName.Text, Is.EqualTo("Jobs"));
        }
    }
}