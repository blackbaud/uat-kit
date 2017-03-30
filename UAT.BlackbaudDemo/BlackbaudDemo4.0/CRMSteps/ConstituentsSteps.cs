using System;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using TechTalk.SpecFlow;

namespace BlackbaudDemo40.CRMSteps
{
    [Binding]
    public class ConstituentsSteps : BaseSteps
    {
        [Given(@"staff constituent ""(.*)"" exists")]
        public void GivenStaffConstituentExists(string lastName)
        {
            lastName += uniqueStamp;
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.AddAnIndividual();
            IndividualDialog.SetLastName(lastName);
            IndividualDialog.Save();
            ConstituentPanel.AddConstituency("Staff");
        }

        [When(@"I add constituent ""(.*)""")]
        public void WhenIAddConstituent(string lastName)
        {
            lastName += uniqueStamp;
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.AddAnIndividual();
            IndividualDialog.SetLastName(lastName);
            IndividualDialog.Save();
        }

        [Then(@"constituent ""(.*)"" is created")]
        public void ThenConstituentIsCreated(string lastName)
        {
            lastName += uniqueStamp;
            if (!ConstituentPanel.IsLastName(lastName))
                throw new ArgumentException("Current constituent page does not have the last name " + lastName);
        }

        [Given(@"constituent '(.*)' exists")]
        public void GivenConstituentExists(string lastName)
        {
            WhenIAddConstituent(lastName);
        }

        [When(@"edit the selected constituent")]
        public void WhenEditTheSelectedConstituent(Table fieldMappings)
        {
            if (fieldMappings.RowCount != 1) throw new ArgumentException("Only provide 1 row of field values");
            BatchDialog.EditConstituent(fieldMappings.Rows[0]);
            IndividualDialog.Save();
        }

        [Then(@"constituent '(.*)' has the title '(.*)', birth date '(.*)' and state address '(.*)'")]
        public void ThenConstituentHasTheTitleBirthDateAndStateAddress(string constituent, string title,
            string birthDate,
            string state)
        {
            constituent += uniqueStamp;
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.ConstituentSearch(constituent);
            if (!ConstituentPanel.IsTitle(title))
                throw new ArgumentException(String.Format("'{0}' is not the constituent's title.", title));
            if (!ConstituentPanel.IsBirthDate(birthDate))
                throw new ArgumentException(String.Format("'{0}' is not the constituent's birth date.", birthDate));
            if (!ConstituentPanel.IsStateAddress(state))
                throw new ArgumentException(String.Format("'{0}' is not the constituent's state address.", state));
        }

        [Given(@"constituent exists")]
        public void GivenConstituentExists(Table constituents)
        {
            foreach (var constituent in constituents.Rows)
            {
                if (constituent.ContainsKey("Last name") && !string.IsNullOrEmpty(constituent["Last name"]))
                    constituent["Last name"] += uniqueStamp;
                BBCRMHomePage.OpenConstituentsFA();
                ConstituentsFunctionalArea.AddAnIndividual(constituent, timeout:120);
            }
        }
    }
}