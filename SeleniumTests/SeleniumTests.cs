using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    public class SeleniumTests
    {
        private WebDriver driver;
        private const string BaseUrl = "https://contactbook.kameliaibowska.repl.co/";

        [SetUp]
        public void OpenWebApp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = BaseUrl;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void CloseWebApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_CheckFirstContactInTheList()
        {
            var contactsLink = driver.FindElement(By.XPath("//a[contains(.,'Contacts')]"));
            contactsLink.Click();

            var textBoxFirstName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td"));
            Assert.That(textBoxFirstName.Text, Is.EqualTo("Steve"));
            
            var textBoxLastName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td"));
            Assert.That(textBoxLastName.Text, Is.EqualTo("Jobs"));
        }

        [Test]
        public void Test_SearchContactsByKeyword()
        {
            string keyword = "albert";

            var searchLink = driver.FindElement(By.XPath("//a[contains(.,'Search')]"));
            searchLink.Click();

            var keywordTextBox = driver.FindElement(By.Id("keyword"));
            keywordTextBox.SendKeys(keyword);

            var buttonSearch = driver.FindElement(By.Id("search"));
            buttonSearch.Click();

            // Assert
            var searchResultsDiv = driver.FindElement(By.Id("searchResult"));
            Assert.That(searchResultsDiv.Text.Contains("contacts found"));

            var textBoxFirstName = driver.FindElement(By.CssSelector("table tr.fname > td"));
            Assert.That(textBoxFirstName.Text, Is.EqualTo("Albert"));
            
            var textBoxLastName = driver.FindElement(By.CssSelector("table tr.lname > td"));
            Assert.That(textBoxLastName.Text, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_SearchContactsByMissingKeyword_NoResults()
        {
            string keyword = $"missing{DateTime.Now.Ticks}";

            var searchLink = driver.FindElement(By.XPath("//a[contains(.,'Search')]"));
            searchLink.Click();

            var keywordTextBox = driver.FindElement(By.Id("keyword"));
            keywordTextBox.SendKeys(keyword);

            var buttonSearch = driver.FindElement(By.Id("search"));
            buttonSearch.Click();

            // Assert
            var searchResultsDiv = driver.FindElement(By.Id("searchResult"));
            Assert.That(searchResultsDiv.Text, Is.EqualTo("No contacts found."));
        }

        [TestCase("", "Petrov", "p.petrov@gmail.com", "Error: First name cannot be empty!")]
        [TestCase("Peter", "", "p.petrov@gmail.com", "Error: Last name cannot be empty!")]
        [TestCase("Peter", "Petov", "", "Error: Invalid email!")]
        public void Test_CreateNewContact_InvalidData(string firstName, string lastName, string email, string errorMessage)
        {
            var createContacthLink = driver.FindElement(By.XPath("//a[contains(.,'Create')]"));
            createContacthLink.Click();

            var textBoxFirstName = driver.FindElement(By.Id("firstName"));
            textBoxFirstName.SendKeys(firstName);

            var textBoxLastName = driver.FindElement(By.Id("lastName"));
            textBoxLastName.SendKeys(lastName);

            var textBoxEmail = driver.FindElement(By.Id("email"));
            textBoxEmail.SendKeys(email);

            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();

            // Assert
            var errMessage = driver.FindElement(By.ClassName("err"));
            Assert.That(errMessage.Text, Is.EqualTo(errorMessage));
        }

        [TestCase("Peter", "Petov", "p.petrov@gmail.com")]
        [TestCase("Ivana", "Koleva", "ivana.koleva@yahoo.com")]
        public void Test_CreateNewContact_ValidData(string firstName, string lastName, string email)
        {
            var createContacthLink = driver.FindElement(By.XPath("//a[contains(.,'Create')]"));
            createContacthLink.Click();

            var textBoxFirstName = driver.FindElement(By.Id("firstName"));
            textBoxFirstName.SendKeys(firstName);

            var textBoxLastName = driver.FindElement(By.Id("lastName"));
            textBoxLastName.SendKeys(lastName);

            var textBoxEmail = driver.FindElement(By.Id("email"));
            textBoxEmail.SendKeys(email);

            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();

            // Assert
            var pageHeading = driver.FindElement(By.XPath("//h1[contains(.,'View Contacts')]"));
            Assert.That(pageHeading.Text, Is.EqualTo("View Contacts"));

            var allContacts = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContactTable = allContacts[allContacts.Count - 1];
            var textContactFirstName = lastContactTable.FindElement(By.CssSelector("tr.fname > td"));
            Assert.That(textContactFirstName.Text, Is.EqualTo(firstName));

            var textContactLastName = lastContactTable.FindElement(By.CssSelector("tr.lname > td"));
            Assert.That(textContactLastName.Text, Is.EqualTo(lastName));

            var textContactEmail = lastContactTable.FindElement(By.CssSelector("tr.email > td"));
            Assert.That(textContactEmail.Text, Is.EqualTo(email));
        }
    }
}