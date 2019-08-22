using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Base;
using SystemTest.Common;
using TechTalk.SpecFlow.Assist;
using Blackbaud.UAT.Core.Crm.Dialogs;
using Blackbaud.UAT.Core.Crm.Panels;

namespace SystemTest.Core.Steps
{
    [Binding]
    public sealed class MajorGivingSteps : BaseSteps
    {

        [Then(@"the Prospect Constituency is added")]
        public void ThenTheProspectConstituencyIsAdded(Table table)
        {
            StepHelper.SetTodayDateInTableRow("Date from", table);
            StepHelper.SetTodayDateInTableRow("Date to", table);
            dynamic objectData = table.CreateDynamicInstance();
            DateTime startDate = objectData.DateFrom;
            DateTime endDate = objectData.DateTo;
            //check fields
            Panel.GetEnabledElement(string.Format("//div[contains(@title,'{0}') and ./text()='{0}']", objectData.Description));
            Panel.GetEnabledElement(string.Format("//div[contains(@title,'{0}') and ./text()='{0}']", startDate.ToShortDateString()));
            Panel.GetEnabledElement(string.Format("//div[contains(@title,'{0}') and ./text()='{0}']", endDate.ToShortDateString()));
        }

        [Given(@"I add Constituencies to the following Constituents")]
        public void GivenIAddConstituenciesToTheFollowingConstituents(Table table)
        {
            StepHelper.SetTodayDateInTableRow("Date from", table);
            StepHelper.SetTodayDateInTableRow("Date to", table);
            IList<dynamic> tableData = table.CreateDynamicSet().ToList();
            foreach (dynamic row in tableData)
            {
                //variables
                DateTime startDate = row.DateFrom;
                DateTime endDate = row.DateTo;
                //navigation
                GetConstituentPanel(row.Surname + uniqueStamp);
                ConstituentPanel.SelectInfoTab();
                ConstituentPanel.SelectInnerTab("Constituencies");
                //set values
                Panel.ClickSectionAddButton("Constituencies");
                Panel.WaitClick(string.Format("//div[contains(@style,'visibility: visible')]//span[./text()='{0}']", row.Constituency));
                Dialog.SetTextField("//input[contains(@id,'_DATEFROM_value')]", startDate.ToShortDateString());
                Dialog.SetTextField("//input[contains(@id,'_DATETO_value')]", endDate.ToShortDateString());
                //save
                Dialog.Save();
            }
        }

        [Given(@"Prospect Team role ""(.*)"" exists for ""(.*)""")]
        public void GivenProspectTeamRoleExistsFor(string roleName, string constituentName)
        {
            GetConstituentPanel(constituentName += uniqueStamp);
            ConstituentPanel.AddTeamRole(roleName += uniqueStamp);
        }

        [When(@"I Add team member to ""(.*)""")]
        public void WhenIAddTeamMemberTo(string constituentName, Table teamMembers)
        {
            GetConstituentPanel(constituentName += uniqueStamp);
            foreach (var teamMember in teamMembers.Rows)
            {
                StepHelper.SetTodayDateInTableRow("Start date", teamMember);
                teamMember["Team member"] = teamMember["Team member"] + uniqueStamp;
                teamMember["Role"] = teamMember["Role"] + uniqueStamp;
                ConstituentPanel.AddTeamMember(teamMember);
            }
        }

        [Then(@"The ""(.*)"" Team Member exists")]
        public void ThenTheTeamMemberExists(string groupTitle, Table teamMembers)
        {
            foreach (var teamMember in teamMembers.Rows)
            {
                StepHelper.SetTodayDateInTableRow("Start date", teamMember);
                if (teamMember["Name"] != string.Empty) teamMember["Name"] = teamMember["Name"] + uniqueStamp;
                if (teamMember["Role"] != string.Empty) teamMember["Role"] = teamMember["Role"] + uniqueStamp;
                if (!ConstituentPanel.TeamMemberExists(teamMember))
                    throw new ArgumentException(
                        String.Format("Current constituent page does not have the team member '{0}'", teamMember));
            }
        }

        [Given(@"I add Plan Outline ""(.*)"" to Major Giving Setup")]
        [When(@"I add Plan Outline ""(.*)"" to Major Giving Setup")]
        public void WhenIAddPlanOutlineToMajorGivingSetup(string planName, Table steps)
        {
            planName += uniqueStamp;
            BBCRMHomePage.OpenProspectsFA();
            ProspectsFunctionalArea.MajorGivingSetup();
            MajorGivingSetupPanel.AddPlanOutline(planName, steps);
        }

        [Given(@"I start to add a major giving plan to '(.*)'")]
        [When(@"I start to add a major giving plan to '(.*)'")]
        public void WhenIStartToAddAMajorGivingPlanTo(string constituent)
        {
            StepHelper.SearchAndSelectConstituent(constituent);
            ConstituentPanel.StartToAddAPlan("Add major giving plan");
        }

        [Given(@"set the details")]
        [When(@"set the details")]
        public void WhenSetTheDetails(Table table)
        {
            if (table.RowCount != 1) throw new ArgumentException("Only provide one row for the details.");
            foreach (var details in table.Rows)
            {
                string startDate = details["Start date"].ToString();
                StepHelper.SetTodayDateInTableRow("Start date", details);
                if (details.ContainsKey("Primary manager") && !string.IsNullOrEmpty(details["Primary manager"])) details["Primary manager"] += uniqueStamp;
                if (details.ContainsKey("Secondary manager") && !string.IsNullOrEmpty(details["Secondary manager"])) details["Secondary manager"] += uniqueStamp;
                if (details.ContainsKey("Plan name") && !string.IsNullOrEmpty(details["Plan name"])) details["Plan name"] += uniqueStamp;
                MajorGivingPlanDialog.SetDetails(details);
                DateTime actualStartDate = DateTime.MinValue;

                if (startDate.ToLower().Equals("today"))
                {
                    actualStartDate = DateTime.Now;
                }
                MajorGivingPlanDialog.SetPrimaryManagerStartDate(actualStartDate.ToShortDateString());
                if (details.ContainsKey("Secondary manager") && !string.IsNullOrEmpty(details["Secondary manager"]))
                {
                    MajorGivingPlanDialog.SetSecondaryManagerStartDate(actualStartDate.ToShortDateString());
                }
            }
        }

        [When(@"add plan participants")]
        public void WhenAddPlanParticipants(Table participants)
        {
            foreach (TableRow participant in participants.Rows)
            {
                if (participant.ContainsKey("Plan participant") && !string.IsNullOrEmpty(participant["Plan participant"])) participant["Plan participant"] += uniqueStamp;
            }
            MajorGivingPlanDialog.SetParticipants(participants);
        }

        [Given(@"set the Steps with outline '(.*)'")]
        [When(@"set the Steps with outline '(.*)'")]
        public void WhenSetTheStepsWithOutline(string outline)
        {
            MajorGivingPlanDialog.SetOutline(outline + uniqueStamp);
        }

        [When(@"insert a step on row (.*)")]
        public void WhenInsertAStepOnRow(int index, Table steps)
        {
            if (steps.RowCount != 1) throw new ArgumentException("Only provide one step row.");
            foreach (var step in steps.Rows)
            {
                StepHelper.SetTodayDateInTableRow("Expected date", step);
                StepHelper.SetTodayDateInTableRow("Actual date", step);
                if (step.ContainsKey("Owner") && !string.IsNullOrEmpty(step["Owner"])) step["Owner"] += uniqueStamp;
                MajorGivingPlanDialog.InsertStep(step, index);
            }
        }

        [Given(@"save the plan")]
        [When(@"save the plan")]
        public void WhenSaveThePlan()
        {
            MajorGivingPlanDialog.Save();
        }

        [Then(@"the plan exists on the constituent")]
        public void ThenThePlanExistsOnTheConstituent(Table plans)
        {
            foreach (var plan in plans.Rows)
            {
                if (plan.ContainsKey("Constituent") && !string.IsNullOrEmpty(plan["Constituent"])) plan["Constituent"] += uniqueStamp;
                if (plan.ContainsKey("Plan name") && !string.IsNullOrEmpty(plan["Plan name"])) plan["Plan name"] += uniqueStamp;
                if (!ConstituentPanel.PlanExists(plan))
                    throw new ArgumentException(String.Format("Plan '{0}' does not exist on the current constituent",
                        plan.Values));
            }
        }

        [Given(@"Prospect ""(.*)"" has an individual relationship with constituent ""(.*)""")]
        public void GivenProspectHasAnIndividualRelationshipWithConstituent(string prospect, string relationshipName)
        {
            //select relationship tab
            Panel.WaitClick("//span[contains(@class,'x-tab-strip-text ') and ./text()='Relationships']");
            Panel.WaitClick("//div[./text()='Relationships']/../../td[not(contains(@class,'x-hide-display'))]//button[./text()='Add individual']");
            //check if visible
            BaseComponent.GetEnabledElement("//span[contains(@class,'x-window-header-text') and ./text()='Add a relationship']");
            //Select Related individual search
            BaseComponent.WaitClick("//div[contains(@id,'dataformdialog') and contains(@style,'block')]//img[contains(@class,'x-form-trigger x-form-search-trigger')]");
            //Must use split for search
            string[] names = relationshipName.Split(' ');
            BaseComponent.SetTextField("//div[contains(@id,'searchdialog') and contains(@style,'block')]//input[contains(@id,'_KEYNAME_value')]", names[1] + uniqueStamp);
            BaseComponent.SetTextField("//div[contains(@id,'searchdialog') and contains(@style,'block')]//input[contains(@id,'_FIRSTNAME_value')]", names[0]);
            //Click search and select
            Dialog.ClickButton("Search");
            Dialog.ClickButton("Select");
            Dialog.SetTextField("//input[contains(@id,'RELATIONSHIPTYPECODEID_value')]", "Friend");
            Dialog.SetTextField("//input[contains(@id,'RECIPROCALTYPECODEID_value')]", "Friend");
            //added comment to allow AJAX DDL's time to auto complete allowing correct validation before the save
            Dialog.SetTextField("//textarea[contains(@id,'_COMMENTS_value')]", "None");
            //save
            Dialog.Save();
        }

        [When(@"I add a Planned Gift to Prospect ""(.*)""")]
        public void WhenIAddAPlannedGiftToProspect(string prospectName, Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            prospectName += uniqueStamp;
            //move to prospect tab
            Panel.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//h2[contains(.,'{0}')]", prospectName));
            Panel.SelectTab("Prospect");
            //select plnned gift sub tab
            Panel.SelectInnerTab("Planned Gifts");
            //add
            Panel.ClickSectionAddButton("Planned gifts");
            //fill form
            Dialog.SetDropDown(XpathHelper.xPath.VisibleDialog + "//input[contains(@id,'VEHICLECODE_value')]", objectData.PlannedGiftVehicle);
            Dialog.SetTextField(XpathHelper.xPath.VisibleDialog + "//input[contains(@id,'STATUSCODE_value') and not(contains(@id,'PROBATE'))]", objectData.Status);
            Dialog.SetTextField(XpathHelper.xPath.VisibleDialog + "//input[contains(@id,'EXPECTEDGIFTAMOUNT_value')]", Convert.ToString(objectData.GiftAmount));
            Dialog.SetDropDown(XpathHelper.xPath.VisibleDialog + "//input[contains(@id,'PROBATESTATUSCODE_value')]", objectData.ProbateStatus);
            Dialog.Save();
        }

        [When(@"I add to planned Gift Details")]
        public void WhenIAddToPlannedGiftDetails(Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            //edit planned gift
            BaseComponent.WaitClick("//div[./text()='Edit planned gift']");
            BaseComponent.WaitClick("//div[contains(@class,'x-grid3-cell-inner x-grid3-col-DESIGNATIONID')]");
            Dialog.SetTextField("//input[contains(@class, 'x-form-text x-form-field x-form-focus')]", objectData.Designation);
            Dialog.SetTextField("//input[contains(@class,'x-form-text x-form-field bbui-forms-collectiongrid-right')]", Convert.ToString(objectData.Amount));
            Dialog.Save();
        }

        [When(@"I add to Planned Giving Relationships")]
        public void WhenIAddToPlannedGivingRelationships(Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            string testString = objectData.Constituent + uniqueStamp + " - Friend";
            //edit planned gift
            BaseComponent.WaitClick("//div[./text()='Edit planned gift']");
            BaseComponent.WaitClick(XpathHelper.xPath.VisibleDialog + "//span[./text()='Planned Giving Relationships']");
            try
            {
                Dialog.SetTextField("//input[contains(@class,'x-form-text x-form-field x-form-text-required x-form-focus')]", objectData.Constituent);
            }
            catch
            {
                //eat exception
            }
            Dialog.GetEnabledElement(string.Format("//div[contains(@class,'x-grid3-cell-inner x-grid3-col-RELATIONSHIPID') and ./text()='{0}']", testString));
            Dialog.Save();
        }

        [When(@"I add to Assests tab")]
        public void WhenIAddToAssestsTab(Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            //edit planned gift
            BaseComponent.WaitClick("//div[./text()='Edit planned gift']");
            BaseComponent.WaitClick(XpathHelper.xPath.VisibleDialog + "//span[./text()='Assets']");
            Dialog.WaitClick(XpathHelper.xPath.VisibleDialog + "//div[contains(@class,'x-grid3-cell-inner x-grid3-col-ASSETTYPECODEID')]");
            Dialog.SetTextField(XpathHelper.xPath.VisibleDialog + "//input[contains(@class,'x-form-text x-form-field x-form-focus')]", objectData.Type);
            StepHelper.AddEntryOnTheFly();
            Dialog.SetTextField(XpathHelper.xPath.VisibleDialog + "//input[contains(@class,'x-form-text x-form-field x-form-text-required')]", objectData.Description);
            Dialog.SetTextField(XpathHelper.xPath.VisibleDialog + "//input[contains(@class,'x-form-text x-form-field bbui-forms-collectiongrid-right')]", Convert.ToString(objectData.Value));
            Dialog.GetEnabledElement(XpathHelper.xPath.VisibleDialog + "//td[contains(@class,'x-grid3-col x-grid3-cell x-grid3-td-VALUE')]/div[./text()='$100.00']");
            Dialog.GetEnabledElement(XpathHelper.xPath.VisibleDialog + "//div[contains(@class,'x-grid3-cell-inner x-grid3-col-ASSETVALUATIONMETHODCODEID')]");
            Dialog.WaitClick(XpathHelper.xPath.VisibleDialog + "//div[contains(@class,'x-grid3-cell-inner x-grid3-col-ASSETVALUATIONMETHODCODEID')]");
            Dialog.SetTextField(XpathHelper.xPath.VisibleDialog + "//input[contains(@class,'x-form-text x-form-field x-form-focus')]", objectData.ValuationMethod);
            StepHelper.AddEntryOnTheFly();
            Dialog.Save();
        }

        [Then(@"the Planned Gift can be seen with the details")]
        public void ThenThePlannedGiftCanBeSeenWithTheDetails(Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            //check fields
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//button[./text()='{0}']", objectData.Constituent + uniqueStamp));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'STATUS_value') and ./text()='{0}']", objectData.Status));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'VEHICLENAME_value') and ./text()='{0}']", objectData.Vehicle));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'TOTALGIFTAMOUNT_value') and ./text()='{0}']", objectData.Amount));
        }

        [Given(@"a major giving plan is started on ""(.*)""")]
        public void GivenAMajorGivingPlanIsStartedOn(string constituent, Table table)
        {
            WhenIStartToAddAMajorGivingPlanTo(constituent);
            foreach (var planValues in table.Rows)
            {
                StepHelper.SetTodayDateInTableRow("Start date", planValues);
                if (planValues.ContainsKey("Outlines"))
                {
                    MajorGivingPlanDialog.SetOutline(planValues["Outlines"]);
                    planValues["Outlines"] = null;
                }
                MajorGivingPlanDialog.SetDetails(planValues);
            }
        }

        [When(@"I go to plan ""(.*)"" for prospect ""(.*)""")]
        public void WhenIGoToPlanForProspect(string plan, string prospect)
        {
            prospect += uniqueStamp;
            GetConstituentPanel(prospect);
            ConstituentPanel.GoToPlan(plan);
        }

        [When(@"I have selected Add from the Opportunities tab")]
        public void WhenIHaveSelectedAddFromTheOpportunitiesTab(Table table)
        {
            var sectionCaption = "Opportunities";
            //lets set the thread culture to get the correct date for the browser
            StepHelper.SetCurrentThreadCultureToConfigValue();
            dynamic objectData = table.CreateDynamicInstance();
            //select tab
            Panel.SelectTab(sectionCaption);
            BaseComponent.GetEnabledElement(Panel.getXSectionAddButton(sectionCaption));
            //select add dialog
            BaseComponent.WaitClick(Panel.getXSectionAddButton(sectionCaption));
            //adding check is visible for page load.
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisibleBlock + "//label[text()='Plan name:']");
            //set fields
            Dialog.SetDropDown(XpathHelper.xPath.VisibleBlock + "//input[contains(@id,'_STATUSCODE_value')]", objectData.Status);
            Dialog.SetTextField(XpathHelper.xPath.VisibleBlock + "//input[contains(@id,'_EXPECTEDASKAMOUNT_value')]", objectData.ExpectedAskAmount);
            Dialog.SetTextField(XpathHelper.xPath.VisibleBlock + "//input[contains(@id,'_EXPECTEDASKDATE_value')]", DateTime.Now.AddDays(Convert.ToInt32(objectData.ExpectedAskDateFromNow)).ToShortDateString());
            Dialog.Save();
        }

        [Then(@"an Opportunity is associated with the Major Giving Plan called ""(.*)""")]
        public void ThenAnOpportunityIsAssociatedWithTheMajorGivingPlanCalled(string majorGivingPlan, Table table)
        {
            //lets set the thread culture to get the correct date for the browser
            StepHelper.SetCurrentThreadCultureToConfigValue();
            dynamic objectData = table.CreateDynamicInstance();
            var date90Days = DateTime.Now.AddDays(Convert.ToInt32(objectData.ExpectedAskDateFromNow)).ToShortDateString();
            //check the plan matches
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//h2[contains(@class, 'bbui-pages-header')]//span[./text()='Major giving - {0}']", majorGivingPlan + uniqueStamp));
            //check there is an opportunity
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//button[contains(@class, 'x-btn-text') and ./text()='Go to opportunity']");
            //check values that were entered in the previous step are actually displayed
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id, '_STATUS_value') and ./text()='{0}']", objectData.Status));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id, '_EXPECTEDASKAMOUNT_value') and ./text()='{0}']", objectData.ExpectedAskAmount));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id, '_EXPECTEDASKDATE_value') and ./text()='{0}']", date90Days));

        }

        [When(@"I edit the planned steps")]
        public void WhenIEditThePlannedSteps(Table table)
        {
            StepHelper.SetTodayDateInTableRow("Actual date", table);
            IList<dynamic> objectData = table.CreateDynamicSet().ToList();
            string dialogId = "ProspectPlanEditForm2";
            string gridId = "STEPS";
            int i = 1;
            Panel.ClickButton("Edit steps");
            foreach (dynamic steps in objectData)
            {
                DateTime actualDate = steps.ActualDate;
                //check objective in row entry
                string gridXPath = Dialog.getXGridCell(dialogId, gridId, i, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), "Objective"));
                BaseComponent.GetEnabledElement(string.Format(gridXPath + string.Format("//div[text()='{0}']", steps.Objective)), 5);
                //change status
                gridXPath = Dialog.getXGridCell(dialogId, gridId, i, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), "Status"));
                Dialog.SetGridDropDown(gridXPath, steps.Status);
                //change status                
                gridXPath = Dialog.getXGridCell(dialogId, gridId, i, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), "Actual date"));
                Dialog.SetGridTextField(gridXPath, actualDate.ToShortDateString());
                i++;
            }
            Dialog.Save();
        }

        [Then(@"Completed steps displays")]
        public void ThenCompletedStepsDisplays(Table table)
        {
            StepHelper.SetTodayDateInTableRow("Date", table);
            IList<dynamic> objectData = table.CreateDynamicSet().ToList();
            string sectionCaption = "Completed steps"; 
            foreach (dynamic completedSteps in objectData)
            {
                DateTime findDate = Convert.ToDateTime(completedSteps.Date);
               IDictionary <string, string> rowValues = new Dictionary<string, string>();
                rowValues.Add("Status", completedSteps.Status);
                rowValues.Add("Date", findDate.ToShortDateString());
                rowValues.Add("Objective", completedSteps.Objective);
                rowValues.Add("Stage", completedSteps.Stage);
                if(!string.IsNullOrEmpty(completedSteps.Owner))
                {
                    rowValues.Add("Owner", completedSteps.Owner + uniqueStamp);
                }
                //check rows
                if(!Panel.SectionDatalistRowExists(rowValues, sectionCaption))
                {
                    throw new Exception(string.Format("Expected values not in the grid for completed step {0}.", completedSteps.Objective));
                }
            }
        }

        private void GetConstituentPanel(string lastName)
        {
            if (!ConstituentPanel.IsLastName(lastName))
            {
                BBCRMHomePage.OpenConstituentsFA();
                ConstituentsFunctionalArea.ConstituentSearch(lastName);
            }
        }
    }
}
