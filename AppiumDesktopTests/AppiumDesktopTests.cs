using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;

namespace AppiumDesktopTests
{
    public class AppiumDesktopTests
    {
        private const string appiumUrl = "http://[::1]:4723/wd/hub";
        private const string appLocation = @"C:\Projects\SofUni_Course\QA_Automation_FrontEnd\RegularExam\ContactBook-DesktopClient.NET6\ContactBook-DesktopClient.exe";
        private WindowsDriver<WindowsElement> driver;
        private WindowsDriver<WindowsElement> driverSearch;
        private AppiumOptions options;
        private AppiumOptions optionsSearch;
        private const string appServer = "https://contactbook.kameliaibowska.repl.co/api";

        [SetUp]
        public void PrepareApp()
        {
            this.options = new AppiumOptions();
            options.AddAdditionalCapability(MobileCapabilityType.App, appLocation);
            options.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Windows");
            this.driver = new WindowsDriver<WindowsElement>(new Uri(appiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            this.optionsSearch = new AppiumOptions();
            optionsSearch.AddAdditionalCapability(MobileCapabilityType.App, "Root");
            optionsSearch.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Windows");
            this.driverSearch = new WindowsDriver<WindowsElement>(new Uri(appiumUrl), optionsSearch);
            driverSearch.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [OneTimeTearDown]
        public void CloseApp()
        {
            foreach (var window in driverSearch.WindowHandles)
            {
                driverSearch.SwitchTo().Window(window);
                driverSearch.Close();
            }
            foreach (var window in driver.WindowHandles)
            {
                driver.SwitchTo().Window(window);
                driver.Close();
            }
        }

        [Test]
        public void Test_SearchAndAssertContact()
        {
            string keyword = "steve";
            
            // Change the URL of the backend
            var inputAppUrl = driver.FindElementByAccessibilityId("textBoxApiUrl");
            inputAppUrl.Clear();
            inputAppUrl.SendKeys(appServer);

            // Press connect button
            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();
            
            // Locate Search window
            var searchContactsForm = driverSearch.FindElementByName("Search Contacts");

            // Search for a keyword
            var serachTextBox = searchContactsForm.FindElementByAccessibilityId("textBoxSearch");
            serachTextBox.SendKeys(keyword);

            var buttonSerach = searchContactsForm.FindElementByAccessibilityId("buttonSearch");
            buttonSerach.Click();

            // Asserts
            var firstName = searchContactsForm.FindElementByName("FirstName Row 0, Not sorted.");
            Assert.That(firstName.Text, Is.EqualTo("Steve"));

            var lasttName = searchContactsForm.FindElementByName("LastName Row 0, Not sorted.");
            Assert.That(lasttName.Text, Is.EqualTo("Jobs"));
        }
    }
}