using System;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Crm.Dialogs;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BlackbaudDemo40.CRMSteps
{
    [Binding]
    public class MembershipProgramsSteps : BaseSteps
    {
        [Given(@"there is no existing Membership Program called ""(.*)""")]
        public void GivenThereIsNoExistingMembershipProgramCalled(string name)
        {
            BBCRMHomePage.OpenMembershipsFA();

            try
            {
                AddMembershipProgramDialog.DeleteMembershipProgram(name);
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("No Exisitng Membership Program called " + name + " found.");
            }
        }

        [When(@"I create a new membership with General options")]
        public void WhenICreateANewMembershipWithGeneralOptions(Table table)
        {
            BBCRMHomePage.OpenMembershipsFA();

            var form = table.CreateInstance<AddMembershipProgramGeneralOptions>();

            AddMembershipProgramDialog.CreateMembershipProgram(form.Name);

            if (form.Kind == "Annual")
                AddMembershipProgramDialog.CreateMembershipProgramAnnual();

            if (form.Obtain == "Dues")
                AddMembershipProgramDialog.CreateMembershipProgramDues();

            AddMembershipProgramDialog.CreateMembershipProgramNext();
        }

        [When(@"Level options of")]
        public void WhenLevelOptionsOf(Table table)
        {
            var form = table.CreateInstance<AddMembershipProgramLevelOptions>();

            AddMembershipProgramDialog.CreateMembershipProgramLevelName(form.Name);

            AddMembershipProgramDialog.CreateMembershipProgramLevelPrice(form.Price);

            AddMembershipProgramDialog.CreateMembershipProgramNext();
        }

        [When(@"Benefits options of")]
        public void WhenBenefitsOptionsOf(Table table)
        {
            var form = table.CreateInstance<AddMembershipProgramBenefitsOptions>();

            AddMembershipProgramDialog.CreateMembershipProgramBenefitsOptionsFormat(form.MembershipFormat);

            AddMembershipProgramDialog.CreateMembershipProgramNext();
        }

        [When(@"Dues options of")]
        public void WhenDuesOptionsOf(Table table)
        {
            var form = table.CreateInstance<AddMembershipProgramDuesOptions>();

            if (form.Installments == "Yes")
                AddMembershipProgramDialog.CreateMembershipProgramDuesOptionsInstallments();

            AddMembershipProgramDialog.CreateMembershipProgramNext();
        }

        [When(@"Renewal options of")]
        public void WhenRenewalOptionsOf(Table table)
        {
            var form = table.CreateInstance<AddMembershipProgramRenewalOptions>();

            AddMembershipProgramDialog.CreateMembershipProgramRenewalOptionsTerm(form.Term);

            AddMembershipProgramDialog.CreateMembershipProgramNext();
        }

        [When(@"the Review is Saved")]
        public void WhenTheReviewIsSaved()
        {
            AddMembershipProgramDialog.CreateMembershipProgramReviewSave();
        }

        [Then(@"""(.*)"" should be listed in Membership Programs")]
        public void ThenShouldBeListedInMembershipPrograms(string name)
        {
            AddMembershipProgramDialog.CheckMembershipProgramExists(name);
        }

        private class AddMembershipProgramGeneralOptions
        {
            public string Name { set; get; }
            public string Kind { set; get; }
            public string Obtain { set; get; }
        }

        private class AddMembershipProgramLevelOptions
        {
            public string Name { set; get; }
            public string Price { set; get; }
        }

        private class AddMembershipProgramBenefitsOptions
        {
            public string MembershipFormat { set; get; }
        }

        private class AddMembershipProgramDuesOptions
        {
            public string Installments { set; get; }
        }

        private class AddMembershipProgramRenewalOptions
        {
            public string Term { set; get; }
        }
    }
}