using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BlackbaudDemo40.CRMSteps
{
    [Binding]
    public class ConstituentSearchSteps : BaseSteps
    {
        [Given(@"I have opened the constituent search dialog")]
        public void GivenIHaveSelectedTheConstituentSearchDialog()
        {
            BBCRMHomePage.OpenConstituentsFA();
            // Or customise like this ...
            // BBCRMHomePage.OpenFunctionalArea("My custom Functional Area Caption");

            ConstituentsFunctionalArea.OpenConstituentSearchDialog();
            // Or customise like this ...
            //FunctionalArea.OpenLink("My custom link caption");                  
        }

        [When(@"I search for")]
        public void WhenISearchFor(Table table)
        {
            var search = table.CreateInstance<ConstituentSearchCriteria>();
            SearchDialog.SetFirstNameToSearch(search.FirstName);
            SearchDialog.SetLastNameToSearch(search.LastName);
            SearchDialog.Search();
        }

        [When(@"I search with no parameters")]
        public void WhenISearchWithNoParameters()
        {
            SearchDialog.Search();
        }

        [Then(@"I should get (.*) records")]
        public void ThenIShouldGetRecords(int expected)
        {
            SearchDialog.CheckConstituentSearchResultsToolbarContains(expected + " records found");
        }

        [Then(@"I should get more than (.*) records")]
        public void ThenIShouldGetMoreThanRecords(int expected)
        {
            SearchDialog.CheckConstituentSearchResultsToolbarContains("More than " + expected + " records found");
        }

        [Then(@"The results should contain ""(.*)""")]
        public void ThenTheResultsShouldContain(string expected)
        {
            SearchDialog.CheckConstituentSearchResultsContain(expected);
        }
    }

    public class ConstituentSearchCriteria
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
    }
}