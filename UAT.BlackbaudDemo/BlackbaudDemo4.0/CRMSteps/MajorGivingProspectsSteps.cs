using System;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using TechTalk.SpecFlow;

namespace BlackbaudDemo40.CRMSteps
{
    [Binding]
    public class MajorGivingProspectsSteps : BaseSteps
    {
        [When(@"I add plan outline ""(.*)"" to major giving setup")]
        public void WhenIAddPlanOutlineToMajorGivingSetup(string planName, Table planSteps)
        {
            planName += uniqueStamp;
            BBCRMHomePage.OpenProspectsFA();
            ProspectsFunctionalArea.MajorGivingSetup();
            MajorGivingSetupPanel.AddPlanOutline(planName, planSteps);
        }

        [Then(@"the plan outline ""(.*)"" is created with ""(.*)"" steps")]
        public void ThenThePlanOutlineIsCreatedWithSteps(string planName, int numberOfSteps)
        {
            planName += uniqueStamp;

            var headers = new string[2];
            headers[0] = "Type";
            headers[1] = "Steps";
            var plans = new Table(headers);
            var firstRow = new string[2];
            firstRow[0] = planName;
            firstRow[1] = numberOfSteps.ToString();
            plans.AddRow(firstRow);
            foreach (var plan in plans.Rows)
            {
                if (!MajorGivingSetupPanel.PlanOutlineExists(plan))
                    throw new ArgumentException("Plan outline " + planName + " does not exist.");
            }
        }

        [When(@"I add prospect constituency to '(.*)'")]
        public void WhenIAddProspectConstituencyTo(string lastName, Table prospects)
        {
            lastName += uniqueStamp;
            GetConstituentPanel(lastName);

            if (prospects == null || prospects.Rows.Count == 0)
                ConstituentPanel.AddConstituency("Major giving prospect");
            else
            {
                foreach (var prospect in prospects.Rows)
                {
                    ConstituentPanel.AddConstituency("Major giving prospect", prospect);
                }
            }
        }

        [Then(@"a prospect constituency is added to '(.*)'")]
        public void ThenAProspectConstituencyIsAddedTo(string lastName, Table prospects)
        {
            lastName += uniqueStamp;
            GetConstituentPanel(lastName);

            foreach (var prospect in prospects.Rows)
            {
                if (!ConstituentPanel.ConstituencyExists(prospect))
                    throw new ArgumentException(String.Format("Constituent page '{0}' does have prospect '{1}'",
                        lastName, prospect));
            }
        }

        [When(@"I add a Note to '(.*)'")]
        public void WhenIAddANoteTo(string lastName, Table notes)
        {
            lastName += uniqueStamp;
            GetConstituentPanel(lastName);
            foreach (var note in notes.Rows)
            {
                if (note.Keys.Contains("Author")) note["Author"] = note["Author"] + uniqueStamp;
                ConstituentPanel.AddNote(note);
            }
        }

        [When(@"add a notification to note '(.*)'")]
        public void WhenAddANotificationToNote(string noteTitle, Table notifications)
        {
            foreach (var notification in notifications.Rows)
            {
                if (notification.Keys.Contains("Selection"))
                    notification["Selection"] = notification["Selection"] + uniqueStamp;
                ConstituentPanel.AddNotification(noteTitle, notification);
            }
        }

        [Then(@"the notification bar displays the Note '(.*)'")]
        public void ThenTheNotificationBarDisplaysTheNote(string noteTitle)
        {
            if (!ConstituentPanel.NotificationExists(noteTitle))
                throw new ArgumentException(
                    String.Format("Current constituent does not have a notification displayed for note '{0}'", noteTitle));
        }

        [Given(@"prospect '(.*)' exists")]
        public void GivenProspectExists(string lastName)
        {
            lastName += uniqueStamp;
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.AddAnIndividual();
            IndividualDialog.SetLastName(lastName);
            IndividualDialog.Save();
            ConstituentPanel.AddConstituency("Major giving prospect");
        }

        [Given(@"fundraiser '(.*)' exists")]
        public void GivenFundraiserExists(string lastName)
        {
            lastName += uniqueStamp;
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.AddAnIndividual();
            IndividualDialog.SetLastName(lastName);
            IndividualDialog.Save();
            ConstituentPanel.AddConstituency("Fundraiser");
        }

        [Given(@"prospect team role '(.*)' exists for '(.*)'")]
        public void GivenProspectTeamRoleExistsFor(string teamRole, string lastName)
        {
            lastName += uniqueStamp;
            GetConstituentPanel(lastName);
            if (!ConstituentPanel.TeamRoleExists(teamRole)) ConstituentPanel.AddTeamRole(teamRole);
        }

        [When(@"I add team member to '(.*)'")]
        public void WhenIAddTeamMemberTo(string lastName, Table teamMembers)
        {
            lastName += uniqueStamp;
            GetConstituentPanel(lastName);
            foreach (var teamMember in teamMembers.Rows)
            {
                teamMember["Team member"] = teamMember["Team member"] + uniqueStamp;
                ConstituentPanel.AddTeamMember(teamMember);
            }
        }

        [Then(@"the '(.*)' team member exists")]
        public void ThenTheTeamMemberExists(string groupCaption, Table teamMembers)
        {
            foreach (var teamMember in teamMembers.Rows)
            {
                if (teamMember["Name"] != string.Empty) teamMember["Name"] = teamMember["Name"] + uniqueStamp;
                if (!ConstituentPanel.TeamMemberExists(teamMember))
                    throw new ArgumentException(
                        String.Format("Current constituent page does not have the team member '{0}'", teamMember));
            }
        }

        [Given(@"major giving plan exists")]
        public void GivenMajorGivingPlanExists(Table plans)
        {
            foreach (var plan in plans.Rows)
            {
                BBCRMHomePage.OpenProspectsFA();
                ProspectsFunctionalArea.MajorGivingSetup();
                if (!MajorGivingSetupPanel.PlanOutlineExists(plan))
                    throw new Exception(String.Format("Major giving plan '{0}' does not exist.", plan.Values));
            }
        }

        [Given(@"prospect '(.*)' is associated with major giving plan")]
        public void GivenProspectIsAssociatedWithMajorGivingPlan(string prospect, Table plans)
        {
            prospect += uniqueStamp;
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.ConstituentSearch(prospect);
            foreach (var plan in plans.Rows)
            {
                if (plan.ContainsKey("Plan name") && !string.IsNullOrEmpty(plan["Plan name"]))
                    plan["Plan name"] += uniqueStamp;
                if (plan.ContainsKey("Outlines"))
                {
                    var outline = plan["Outlines"];
                    plan["Outlines"] = null;
                    ConstituentPanel.AddMajorGivingPlan(plan, outline);
                }
                else ConstituentPanel.AddMajorGivingPlan(plan, string.Empty);
            }
        }

        [When(@"I go to the plan '(.*)' for prospect '(.*)'")]
        public void WhenIGoToThePlanForProspect(string plan, string prospect)
        {
            prospect += uniqueStamp;
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.ConstituentSearch(prospect);
            plan += uniqueStamp;
            ConstituentPanel.GoToPlan(plan);
        }

        [When(@"add a step")]
        public void WhenAddAStep(Table steps)
        {
            foreach (var step in steps.Rows)
            {
                PlanPanel.AddCompletedStep(step);
            }
        }

        [Then(@"a completed step is saved")]
        public void ThenACompletedStepIsSaved(Table steps)
        {
            foreach (var step in steps.Rows)
            {
                if (!PlanPanel.CompletedStepExists(step))
                    throw new ArgumentException(String.Format("Completed step '{0}' does not exist.", step.Values));
            }
        }

        private void GetConstituentPanel(string lastName)
        {
            if (!ConstituentPanel.IsLastName(lastName))
            {
                BBCRMHomePage.OpenConstituentsFA();
                ConstituentsFunctionalArea.ConstituentSearch(lastName);
            }
            //if (!Panel.IsPanelType("individual") || !ConstituentPanel.IsConstituentPanelHeader(lastName))
            //{
            //    BBCRMHomePage.OpenConstituentsFA();
            //    ConstituentsFunctionalArea.ConstituentSearch(lastName);
            //}
        }
    }
}