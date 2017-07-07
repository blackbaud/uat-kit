using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Base;
using SystemTest.Common;
using TechTalk.SpecFlow.Assist;
using SpecFlow.Assist.Dynamic;
using Blackbaud.UAT.Core.Crm.Dialogs;
using Blackbaud.UAT.Core.Crm.Panels;

namespace SystemTest.Core.Steps
{
    [Binding]
    public sealed class ConstituentSteps : BaseSteps
    {
        [Given(@"I add individual")]
        [Given(@"Individual constituent exists with an address")]
        [Given(@"I add an individual")]
        [Given(@"a constituent record exists")]
        [Given(@"I add individual\(s\)")]
        [When(@"I add individual\(s\) with address")]
        public void WhenIAddIndividualSWithAddress(Table individualAddress)
        {
            foreach (TableRow individual in individualAddress.Rows)
            {
                individual["Last name"] = individual["Last name"] + uniqueStamp;
                BBCRMHomePage.OpenConstituentsFA();
                ConstituentsFunctionalArea.AddAnIndividual(individual, timeout: 120);
            }
        }

        [Then(@"individual constituent is created named ""(.*)""")]
        public void ThenIndividualConstituentIsCreatedNamed(string fullName)
        {
            fullName += uniqueStamp;
            Panel.WaitForPanelType("individual");
            Panel.GetEnabledElement(string.Format("//span[./text()='{0}']", fullName));
        }

        [Then(@"An address is displayed on contact tab")]
        public void ThenAnAddressIsDisplayedOnContactTab(Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            Panel.SelectTab("Contact");
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(.,'{0}')]", objectData.ContactInformation));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[contains(@title,'{0}') and ./text()='{0}']", objectData.Type));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[contains(@title,'{0}') and ./text()='{0}']", objectData.Primary));
        }

        [Then(@"Personal information is displayed on Personal Info tab")]
        public void ThenPersonalInformationIsDisplayedOnPersonalInfoTab(Table table)
        {
            IList<dynamic> individuals = table.CreateDynamicSet().ToList();
            foreach (dynamic individual in individuals)
            {
                ConstituentPanel.SelectTab("Personal Info");
                individual.LastName += uniqueStamp;
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_FIRSTNAME_value') and ./text()='{0}']", individual.FirstName));
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_KEYNAME_value') and ./text()='{0}']", individual.LastName));
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_TITLECODE_value') and ./text()='{0}']", individual.Title));
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_NICKNAME_value') and ./text()='{0}']", individual.Nickname));
            }
        }

        [When(@"I add the address to the current individual")]
        public void WhenIAddTheAddressToTheCurrentIndividual(Table table)
        {
            //create dynamic table object
            dynamic objectData = table.CreateDynamicInstance();
            //Select tab and add address
            Panel.SelectTab("Contact");
            Panel.ClickSectionAddButton("Addresses");
            Dialog.SetTextField("//input[contains(@id,'_ADDRESSTYPECODEID_value')]", objectData.AddressType);
            Dialog.SetTextField("//input[contains(@id,'_COUNTRYID_value')]", objectData.Country);
            Dialog.SetTextField("//textarea[contains(@id,'_ADDRESSBLOCK_value')]", objectData.Address);
            Dialog.SetTextField("//input[contains(@id,'_CITY_value')]", objectData.City);
            Dialog.SetTextField("//input[contains(@id,'_POSTCODE_value')]", objectData.ZIP);
            Dialog.Save();
        }

        [When(@"I edit the address")]
        public void WhenIEditTheAddressToIndividualConstituent(Table table)
        {
            //set up date before dynamic object
            StepHelper.SetTodayDateInTableRow("Start date", table);
            //create dynamic table object
            dynamic objectData = table.CreateDynamicInstance();
            DateTime startDate = Convert.ToDateTime(objectData.StartDate);
            //set fields
            foreach (var EditAddress in table.Rows)
            {
                Dialog.SetTextField("//textarea[contains(@id,'_ADDRESSBLOCK_value')]", objectData.Address);
                Dialog.SetTextField("//input[contains(@id,'_POSTCODE_value')]", objectData.Postcode);
                Dialog.SetTextField("//input[contains(@id,'_HISTORICALSTARTDATE_value')]", startDate.ToShortDateString());
                Dialog.Save();
            }
        }

        [When(@"I select a row under Addresses")]
        public void WhenISelectARowUnderAddresses(Table table)
        {
            //create dynamic table object
            dynamic objectData = table.CreateDynamicInstance();
            Panel.SelectTab("Contact");
            Panel.WaitClick(string.Format("//div[contains(@title,'{0}') and ./text()='{0}']", objectData.Type));
            Panel.WaitClick(Panel.getXSelectedDatalistRowButton("Edit"));
        }

        [Given(@"a Fundraiser exists")]
        public void GivenAFundraiserExists(Table table)
        {
            WhenIAddIndividualSWithAddress(table);
            ConstituentPanel.AddConstituency("Fundraiser");
        }

    }
}
