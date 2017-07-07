using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Constituent' panel.
    /// </summary>
    public class ConstituentPanel : Panel
    {
        /// <summary>
        /// XPath for Delete Yes action
        /// </summary>
        public const string getXYesReallyDelete = "//button[contains(@class,'x-btn-text') and .//text()='Yes']";

        /// <summary>
        /// XPath to find a notification on a Constituent panel.
        /// </summary>
        /// <param name="noteTitle">Part of the title that the notification must have.</param>
        protected static string getXNotification(string noteTitle)
        {
            return String.Format("//div[@class='bbui-pages-summary-infobar']/button[contains(./text(), 'Notifications:') and contains(./text(), 'Click here for more information.') and contains (./text(), '{0}')]", noteTitle);
        }

        /// <summary>
        /// Check the Title of the constituent.
        /// </summary>
        /// <param name="title">The Title value to check for.</param>
        /// <returns>True if the provided value is the constituent's Title, false otherwise.</returns>
        public static bool IsTitle(string title)
        {
            return GetDisplayedElement(getXDiv("ConstituentSummaryIndividualInfoTileViewForm",
                "_NAMEROW1_value")).Text.Contains(title);
        }

        /// <summary>
        /// Check the State address of the constituent.
        /// </summary>
        /// <param name="state">The State abbreviation to check for.</param>
        /// <returns>True if the provided value is the constituent's state, false otherwise.</returns>
        public static bool IsStateAddress(string state)
        {
            return GetDisplayedElement(getXDiv("ConstituentSummaryAddressesTileViewForm",
                "_ADDRESSROW1_value")).Text.Contains(state);
        }

        /// <summary>
        /// Check the Birth Date of the constituent.
        /// </summary>
        /// <param name="birthDate">The birth date value to check for.</param>
        /// <returns>True if the provided value is the constituent's birth date, false otherwise.</returns>
        public static bool IsBirthDate(string birthDate)
        {
            SelectTab("Personal Info");
            SelectInnerTab("Personal");
            return GetDisplayedElement(getXSpan("IndividualBiographicalViewForm",
                "_BIRTHDATE_value")).Text.Contains(birthDate);
            /*
             * could also do...
             * 
             * IDictionary<string, string> values = new Dictionary<string, string>() {{"Birth dare", birthdate}};
             * return Exists(getXViewForm("Personal information", "IndividualBiographicalViewForm", null));
             */
        }

        /// <summary>
        /// Checks if the constituent record panel has the provided last name.
        /// </summary>
        /// <param name="lastName">The last name to check for.</param>
        /// <returns>True if the constituent has the provided last name, false otherwise.</returns>
        public static bool IsLastName(string lastName)
        {
            return GetDisplayedElement(getXDiv("ConstituentSummaryIndividualInfoTileViewForm",
                "_NAMEROW3_")).Text == lastName;
        }

        /// <summary>
        /// Check if the Constituent panel header has the expected text.
        /// </summary>
        /// <param name="expectedText">The expected text of the header to check for.</param>
        /// <returns>True if the header has the expected text, false otherwise.</returns>
        public static bool IsConstituentPanelHeader(string expectedText)
        {
            return GetDisplayedElement(getXPanelHeader("individual")).Text == expectedText;
        }

        /// <summary>
        /// Check if a constituency exists on the 'Personal Info' => 'Constituencies' tab.
        /// </summary>
        /// <param name="values">Mapping of column captions to a row's values.</param>
        /// <returns>True if the constituency exists, false otherwise.</returns>
        public static bool ConstituencyExists(TableRow values)
        {
            SelectInfoTab();
            SelectInnerTab("Constituencies");
            return SectionDatalistRowExists(values, "Constituencies");
        }

        /// <summary>
        /// Return the Constituent Type text from the TopBar on the ConstituentPanel
        /// </summary>
        /// <returns></returns>
        public static string GetConstituentTypeText()
        {
            var element = GetEnabledElement("//*[contains(@class,'contentcontainer') and not(contains(@class,'x-hide-display'))]//*[contains(@id,'CONSTITUENTTYPETEXT')]");
            return element.Text;
        }

        /// <summary>
        /// Select Info tab based on constituent type Text in the Panel Header
        /// </summary>
        public static void SelectInfoTab()
        {
            var type = GetConstituentTypeText();

            if (type == "Individual")
            {
                SelectTab("Personal Info");               
            } else if (type == "Organization")
            {
                SelectTab("Organization Info");
            }

        }

        /// <summary>
        /// Delete constituency by constituency
        /// </summary>
        /// <param name="constituency">Constituency to be deleted</param>
        public static void DeleteConstituency(string constituency)
        {
            SelectInfoTab();
            SelectInnerTab("Constituencies");
            WaitClick("//*[contains(@title,'"+constituency+"')]", 30);
            WaitClick("//*[contains(@class,'x-grid3-row-expanded')]//button[contains(@class,'x-btn-text') and ./text()='Delete']");
            WaitClick(getXYesReallyDelete);
        }

        /// <summary>
        /// Add a constituency under the 'Personal Info' => 'Constituencies' tab.
        /// </summary>
        /// <param name="constituency">The type of constituency to add (i.e. 'Staff')</param>
        /// <param name="values">Mapping of the field captions and desired values to set on the 'Add constituency' dialog.  
        /// Defaults to null where no values are set.</param>
        public static void AddConstituency(string constituency, TableRow values = null)
        {
            SelectInfoTab();
            SelectInnerTab("Constituencies");

            ClickSectionAddButton("Constituencies");
            WaitClick(getXMenuItem(constituency));
            if (values == null)
            {
                Dialog.Save();
                return;
            }

            foreach (string caption in values.Keys)
            {
                if (values[caption] == string.Empty) continue;
                // A shortcut to simulate pressing the F3 button (Today)
                if (values[caption] == "F3") values[caption] = DateTime.Today.ToShortDateString();
                switch (caption)
                {
                    case "Date from":
                            SetTextField(Dialog.getXInput("ConstituencyAddEditForm", "DATEFROM"), values[caption]);
                        break;
                    case "Date to":
                            SetTextField(Dialog.getXInput("ConstituencyAddEditForm", "DATETO"), values[caption]);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Field '{0}' is not an implemented field for the 'Add constituency' dialogs.", caption));
                }
            }
            Dialog.Save();
        }

        /// <summary>
        /// Add a Note in the 'Documentation and Interactions' => 'Constituent Documentation' tab
        /// </summary>
        /// <param name="note">Mapping of the field captions for the 'Add a note' dialog to their desired values.</param>
        public static void AddNote(TableRow note)
        {
            SelectTab("Documentation and Interactions");
            SelectInnerTab("Constituent Documentation");

            ClickSectionAddButton("Documentation", "Add note");
            foreach (string caption in note.Keys)
            {
                switch (caption)
                {
                    case "Type":
                        SetTextField(Dialog.getXInput("GenericNoteAddEditForm", "NOTETYPECODEID"), note[caption]);
                        break;
                    case "Date":
                        SetTextField(Dialog.getXInput("GenericNoteAddEditForm", "DATEENTERED"), note[caption]);
                        break;
                    case "Title":
                        SetTextField(Dialog.getXInput("GenericNoteAddEditForm", "TITLE"), note[caption]);
                        break;
                    case "Author":
                        Dialog.SetSearchList(Dialog.getXInput("GenericNoteAddEditForm", "AUTHORID"), Dialog.getXInput("ConstituentSearch", "KEYNAME"), note[caption]);
                        break;
                    case "Notes":
                        Dialog.SetHtmlField(Dialog.getXIFrame("GenericNoteAddEditForm", "HTMLNOTE"), note[caption]);
                        break;
                    default:
                        throw new NotImplementedException("Field '" + caption + "' is not an implemented field for the 'Add a note' dialog.");
                }
            }
            Dialog.Save();
        }

        /// <summary>
        /// Select a Note.
        /// </summary>
        /// <param name="noteTitle">The value of the 'Title' column for the desired row.</param>
        protected static void SelectNote(string noteTitle)
        {
            SelectTab("Documentation and Interactions");
            SelectInnerTab("Constituent Documentation");

            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            rowValues.Add("Title", noteTitle);

            SelectSectionDatalistRow(rowValues, "Documentation");
        }

        /// <summary>
        /// Add a Notification to the specified Note in the 'Documentation and Interactions' => 'Constituent Documentation' tab.
        /// </summary>
        /// <param name="noteTitle">The 'Title' of the Note to add a notification to.</param>
        /// <param name="notification">Mapping of the field captions for the 'Add notification' dialog to their desired values.</param>
        public static void AddNotification(string noteTitle, TableRow notification)
        {
            SelectTab("Documentation and Interactions");
            SelectInnerTab("Constituent Documentation");
            SelectNote(noteTitle);

            WaitClick(getXSelectedDatalistRowButton("Add notification"));
            foreach (string caption in notification.Keys)
            {
                switch (caption)
                {
                    case "End date":
                        SetTextField(Dialog.getXInput("GenericNoteNotificationAddEditForm", "VALIDUNTIL"), notification[caption]);
                        break;
                    case "Display in notification window":
                        SetCheckbox(Dialog.getXInput("GenericNoteNotificationAddEditForm", "DISPLAYNOTIFICATIONWINDOW"), notification[caption]);
                        break;
                    case "Displays for":
                        SetTextField(Dialog.getXInput("GenericNoteNotificationAddEditForm", "APPLYTOCODE"), notification[caption]);
                        break;
                    case "Selection":
                        Dialog.SetSearchList(Dialog.getXInput("GenericNoteNotificationAddEditForm", "VALIDUNTIL"), Dialog.getXInput("SelectionSearch", "NAME"), notification[caption]);
                        break;
                    default:
                        throw new NotImplementedException("Field '" + caption + "' is not an implemented field for the 'Add notification' dialog.");
                }
            }
            Dialog.Save();
        }

        /// <summary>
        /// Check if a Notification exists for the provided Note.
        /// </summary>
        /// <param name="noteTitle">The 'Title' of the Note.</param>
        /// <returns>True if the notification exists, false otherwise.</returns>
        public static bool NotificationExists(string noteTitle)
        {
            return Exists(getXNotification(noteTitle));
        }

        /// <summary>
        /// Check if a Team Member Role exists when adding a Prospect team member through the
        /// 'Add a prospect team member' dialog.
        /// </summary>
        /// <param name="teamRole">The name of the team role to check for.</param>
        /// <returns>True if the team role exists, false otherwise.</returns>
        public static bool TeamRoleExists(string teamRole)
        {
            SelectTab("Prospect");
            SelectInnerTab("Prospect Team");
            ClickSectionAddButton("Prospect team", "Add team member");

            //Save whether the item exists or not, cancel the dialog, then return the result
            bool exists = Dialog.DropdownValueExists(Dialog.getXInput("ProspectTeamAddDataForm", "PROSPECTTEAMROLECODEID"), teamRole);
            Dialog.Cancel();
            return exists;
        }

        /// <summary>
        /// Add Team Member Role through the 'Add a prospect team member' dialog.
        /// </summary>
        /// <param name="teamRole">The name of the team role to add.</param>
        public static void AddTeamRole(string teamRole)
        {
            SelectTab("Prospect");
            SelectInnerTab("Prospect Team");
            ClickSectionAddButton("Prospect team", "Add team member");

            SetTextField(Dialog.getXInput("ProspectTeamAddDataForm", "PROSPECTTEAMROLECODEID"), teamRole);
            Dialog.Yes();
            Dialog.Cancel();
        }

        /// <summary>
        /// Add a prospect team member from the 'Prospect' => 'Prospect Team' tab.
        /// 
        /// Constituent may need to have the 'Prospect' constituency for this tab to be displayed.
        /// </summary>
        /// <param name="teamMember">Mapping of the field captions for the 'Add a prospect team member' dialog to their desired values.</param>
        public static void AddTeamMember(TableRow teamMember)
        {
            SelectTab("Prospect");
            SelectInnerTab("Prospect Team");
            ClickSectionAddButton("Prospect team", "Add team member");
            foreach (string caption in teamMember.Keys)
            {
                switch (caption)
                {
                    case "Team member":
                        Dialog.SetSearchList(Dialog.getXInput("ProspectTeamAddDataForm", "MEMBERID"), Dialog.getXInput("FundraiserSearch", "KEYNAME"), teamMember[caption]);
                        break;
                    case "Role":
                        Dialog.SetDropDown(Dialog.getXInput("ProspectTeamAddDataForm", "PROSPECTTEAMROLECODEID"), teamMember[caption]);
                        break;
                    case "Start date":
                        SetTextField(Dialog.getXInput("ProspectTeamAddDataForm", "DATEFROM"), teamMember[caption]);
                        break;
                    case "End date":
                        SetTextField(Dialog.getXInput("ProspectTeamAddDataForm", "DATETO"), teamMember[caption]);
                        break;
                    default:
                        throw new NotImplementedException("Field '" + caption + "' is not an implemented field for the 'Add a prospect team member' dialog.");
                }
            }
            Dialog.Save();
        }

        /// <summary>
        /// Check if a prospect team member exists.
        /// </summary>
        /// <param name="teamMember">Mapping of column captions to a row's values.</param>
        /// <returns>True if the team member exists, false otherwise</returns>
        public static bool TeamMemberExists(TableRow teamMember)
        {
            SelectTab("Prospect");
            SelectInnerTab("Prospect Team");

            return SectionDatalistRowExists(teamMember, "Prospect team");
        }

        /// <summary>
        /// Add a major giving plan to a prospect constituent.
        /// </summary>
        /// <param name="detailFields">Mapping of the 'Details' tab field captions to their desired values.</param>
        /// <param name="outline">The outline to use.</param>
        public static void AddMajorGivingPlan(TableRow detailFields, string outline)
        {
            SelectTab("Prospect");
            SelectInnerTab("Plans");

            ClickSectionAddButton("Plans");
            WaitClick(getXMenuItem("Add major giving plan"));

            MajorGivingPlanDialog.SetDetails(detailFields);
            MajorGivingPlanDialog.SetOutline(outline);
            Dialog.Save();
            GetDisplayedElement(getXPanelHeader("individual"));
        }

        /// <summary>
        /// Navigate the plan panel of an existing plan for the constituent.
        /// </summary>
        /// <param name="planName">The name of the plan.</param>
        public static void GoToPlan(string planName)
        {
            SelectTab("Prospect");
            SelectInnerTab("Plans");

            IDictionary<string, string> planRow = new Dictionary<string, string> {{"Plan name", planName}};
            SelectSectionDatalistRow(planRow, "Plans");
            WaitClick(getXSelectedDatalistRowLinkByCaption(planName));
            GetDisplayedElement(getXPanelHeader("fa_majorgiving"));
        }

        /// <summary>
        /// Check if a plan exists for the constituent.
        /// </summary>
        /// <param name="plan">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the plan exists, false otherwise.</returns>
        public static bool PlanExists(TableRow plan)
        {
            SelectTab("Prospect");
            SelectInnerTab("Plans");
            return SectionDatalistRowExists(plan, "Plans");
        }

        /// <summary>
        /// Start to add a plan to the prospect constituent.
        /// </summary>
        /// <param name="planType">The caption of the add menu item for the type of plan to add.</param>
        public static void StartToAddAPlan(string planType)
        {
            SelectTab("Prospect");
            SelectInnerTab("Plans");
            ClickSectionAddButton("Plans");
            WaitClick(getXMenuItem(planType));
        }

        /// <summary>
        /// Follow the specified constituency link.
        /// </summary>
        /// <param name="constituency"></param>
        public static void ClickConstituentLink(string constituency)
        {
            WaitClick("//*[contains(@id,'PROSPECTCONSTITUENCYTEXT_value') and not(contains(@style,'display: none') and ./text()='"+constituency+"')]");
        }
    }
}
