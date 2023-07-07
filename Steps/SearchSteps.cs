using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System.Threading;
using System.Linq;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowProjectFramework.Steps
{
    [Binding]
    public sealed class SearchStepDefinitions
    {

        
        private IWebDriver driver;

        public SearchStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
        }


        [When(@"URL is entered")]
        public void WhenURLIsEntered()
        {
            driver.Url = "https://www.youtube.com/";
        }

        [Then(@"Search for testers talk")]
        public void ThenSearchForTestersTalk()
        {
            driver.FindElement(By.XPath("//input[@id = 'search']")).SendKeys("Testers talk");
            Thread.Sleep(5000);
            
        }

        [Then(@"Search for keywords")]
        public void ThenSearchForKeywords(Table table)
        {

            var search = table.CreateSet<SearchKeys>();
            foreach(var key in search)
            {
                driver.FindElement(By.XPath("//input[@id = 'search']")).SendKeys(key.keywords);
                Thread.Sleep(5000);
            }
           
        }

        public class SearchKeys
        {

            public string keywords { get; set; }
            public string pwds { get; set; }


        }
    }
}
