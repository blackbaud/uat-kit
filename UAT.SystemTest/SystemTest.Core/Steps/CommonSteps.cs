using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Bbis.Pages;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.SpecFlow.Selenium;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Base;

namespace SystemTest.Steps
{
    [Binding]
    public class CommonSteps : BaseSteps
    {
        [Given(@"I have logged into the BBCRM home page")]
        [When(@"I have logged into the BBCRM home page")]
        public void GivenIHaveLoggedIntoTheBBCRMHomePage()
        {
            BBCRMHomePage.Login();
        }

        [Given(@"I have logged into the BBIS home page as user ""(.*)""")]
        [When(@"I have logged into the BBIS home page as user ""(.*)""")]
        [Then(@"I have logged into the BBIS home page as user ""(.*)""")]
        public void GivenIHaveLoggedIntoTheBBISHomePageAsUser(string credentials)
        {
            // credentials should be in form "user:password"
            BaseTest.NewSession();
            BBISHomePage.LoginAs(credentials);
        }

        [Given(@"I have logged into the BBIS home page")]
        public void GivenIHaveLoggedIntoTheBBISHomePage()
        {
            BBISHomePage.Login();
        }

        [Given(@"I have logged into the BBCRM home page as user ""(.*)""")]
        public void GivenIHaveLoggedIntoTheBBCRMHomePageAsUser(string credentials)
        {
            // credentials should be in form "user:password"
            BaseTest.NewSession();
            BBCRMHomePage.LoginAs(credentials);
        }

        [Given(@"I have logged out of BBCRM")]
        public void GivenIHaveLoggedOutOfBBCRM()
        {
            BaseComponent.WaitClick("//table[contains(@class,'x-btn bbui-pages-header-menubutton bbui-pages-header-usernamebutton x-btn-noicon')]//em[contains(@class,'x-btn-arrow x-unselectable')]");
            BaseComponent.WaitClick("//span[text()='Log out']");
            BaseComponent.GetEnabledElement("//div[@id='splash-content' ]");
        }

        [Given(@"I have logged out of BBIS")]
        [When(@"I have logged out of BBIS")]
        [Then(@"I have logged out of BBIS")]
        public void GivenIHaveLoggedOutOfBBIS()
        {
            BaseComponent.WaitClick("//div[contains(@class,'bb_subMenuLogin')]//a[text()='Logout']");
            BaseComponent.GetDisplayedElement("//div[contains(@id,'pnl_login')]");
        }

        [When(@"I navigate to ""(.*)"" page in BBIS")]
        public void WhenINavigateToPageInBBIS(string page)
        {
            //This allows user to navigate to pages WITH unique IDs.
            page += uniqueStamp;
            BaseComponent.Driver.Navigate().GoToUrl(BBISHomePage.BaseUrl.TrimEnd(new char[] { '/' }) + "/" + page);
        }

        [Given(@"I have navigated to ""(.*)"" page")]
        [When(@"I have navigated to ""(.*)"" page")]
        [Then(@"I have navigated to ""(.*)"" page")]
        public void GivenIHaveNavigatedToPage(string page)
        {
            //This allows user to navigate to pages WITHOUT unique IDs.
            BaseComponent.Driver.Navigate().GoToUrl(BBISHomePage.BaseUrl.TrimEnd(new char[] { '/' }) + "/" + page);
        }

        [When(@"I have logged out of BBIS from the front end")]
        [Then(@"I have logged out of BBIS from the front end")]
        public void ThenIHaveLoggedOutOfBBISFromTheFrontEnd()
        {
            BaseComponent.WaitClick("//a[text()='Logout']");
            //check login is visible to confirm logout
            BaseComponent.GetEnabledElement("//a[text()='Login']");
        }
    }
}