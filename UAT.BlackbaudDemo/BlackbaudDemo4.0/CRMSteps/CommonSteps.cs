using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using TechTalk.SpecFlow;

namespace BlackbaudDemo40.CRMSteps
{
    [Binding]
    public class CommonSteps : BaseSteps
    {
        [Given(@"I have logged into the BBCRM home page")]
        public void GivenIHaveLoggedIntoTheBBCRMHomePage()
        {
            //BBCRMHomePageBasicAuthenticate.LoginAs();
            BBCRMHomePage.Login();
        }
    }
}