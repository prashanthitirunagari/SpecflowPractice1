using NUnit.Framework;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace SpecflowPractice.StepDefinitions
{
    [Binding]
    public class DataTableStesps { 
    

        private Helper _helper;

        public DataTableStesps(Helper helper)
        {
            _helper = helper;
        }

        [Given(@"as a user i should launch the web application")]
        public void GivenAsAUserIShouldLaunchTheWebApplication()
        {
            _helper.driver.Navigate().GoToUrl("http://automationpractice.com/");
        }

        [When(@"user click on SignIn button")]
        public void WhenUserClickOnSignInButton()
        {
            _helper.driver.FindElement(By.LinkText("Sign in")).Click();
        }

        [When(@"user click on Submit button")]
        public void WhenUserClickOnSubmitButton()
        {
            _helper.driver.FindElement(By.Id("SubmitLogin")).Click();
        }


        [When(@"user enter username and password with the details")]
        public void WhenUserEnterUsernameAndPasswordWithTheDetails(Table table)
        {
            foreach(var row in table.Rows)
            {
                _helper.driver.FindElement(By.Id("email")).Clear();
                _helper.driver.FindElement(By.Id("email")).SendKeys(row["Username"]);
                _helper.driver.FindElement(By.Id("passwd")).Clear();
                _helper.driver.FindElement(By.Id("passwd")).SendKeys(row["Password"]);
            }
            

        }

        [When(@"user enter (.*) and (.*) with multiple details")]
        public void WhenUserEnterUsernameAndPasswordWithTheMultipleDetails(string username, string password)
        {
            
                _helper.driver.FindElement(By.Id("email")).Clear();
                _helper.driver.FindElement(By.Id("email")).SendKeys(username);
                _helper.driver.FindElement(By.Id("passwd")).Clear();
                _helper.driver.FindElement(By.Id("passwd")).SendKeys(password);
            
        }
        [Then(@"user should login successfully into the application")]
        public void ThenUserShouldLoginSuccessfullyIntoTheApplication()
        {
            
        }

        [Then(@"user should login or not based on (.*)")]
        public void ThenUserShouldLoginOrNotBasedOnInvalid(string scenarioType)
        {
            if (scenarioType.Equals("Valid"))
            {
                Assert.IsTrue(_helper.driver.FindElement(By.XPath("//a[@title='View my customer account']/span")).Displayed);
            }
            else
            {
                Assert.IsTrue(_helper.driver.FindElement(By.XPath("//div[@class='alert alert-danger']/p")).Displayed);
            }
            
        }

    }
}
