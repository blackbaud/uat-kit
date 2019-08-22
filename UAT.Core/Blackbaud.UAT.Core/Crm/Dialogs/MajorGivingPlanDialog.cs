using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Panels;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for adding/editing a major giving plan dialog
    /// </summary>
    public class MajorGivingPlanDialog : Dialog
    {
        /// <summary>
        /// Add DialogId html ID
        /// </summary>
        protected const string AddDialogId = "ProspectPlanAddForm";

        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { AddDialogId, "ProspectPlanEditForm2" };


        //FIXME - deprecate and remove - overhaul this table handling with smarter objects?

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Plan name", new CrmField("_PROSPECTPLAN_NAME_value", FieldType.TextInput)},
            {"Plan type", new CrmField("_PROSPECTPLANTYPECODEID_value", FieldType.Dropdown)},
            {"Outlines", new CrmField("_OUTLINELIST_value", FieldType.Dropdown)},
            {"Primary manager", new CrmField("_PRIMARYMANAGERFUNDRAISERID_value", FieldType.Searchlist, "FundraiserSearch" ,"KEYNAME")},
            {"Secondary manager", new CrmField("_SECONDARYMANAGERFUNDRAISERID_value", FieldType.Searchlist, "FundraiserSearch", "KEYNAME")},
            {"Plan participant", new CrmField(null, FieldType.Searchlist, "UniversityofOxfordConstituentSearch", "KEYNAME")},
            {"Role", new CrmField(null, FieldType.Dropdown)},
            {"Expected date", new CrmField(null, FieldType.TextInput)},
            {"Expected start time", new CrmField(null, FieldType.TextInput)},
            {"Expected end time", new CrmField(null, FieldType.TextInput)},
            {"Time zone", new CrmField(null, FieldType.Dropdown)},
            {"Objective", new CrmField(null, FieldType.TextInput)},
            {"Owner", new CrmField(null, FieldType.Searchlist, "FundraiserSearch", "KEYNAME")},
            {"Stage", new CrmField(null, FieldType.Dropdown)},
            {"Status", new CrmField(null, FieldType.Dropdown)},
            {"Actual date", new CrmField(null, FieldType.TextInput)},
            {"Contact method", new CrmField(null, FieldType.Dropdown)}
        };


        //FIXME - deprecate and remove

        /// <summary>
        /// Set the 'Start date' of the plan.
        /// </summary>
        /// <param name="startDate">The date to set the 'Start date' to.</param>
        public static void SetPlanStartDate(string startDate)
        {
            string dialogId = GetDialogId(DialogIds);
            if (dialogId == AddDialogId) OpenTab("Details");
            //requires a custom setter.  see documentation for details.
            SetAddProspectPlanStartDate(getXInput(dialogId, "_STARTDATE_value"), startDate);
        }

        /// <summary>
        /// Set the 'Start date' of the primary manager.
        /// </summary>
        /// <param name="startDate">The date to set the 'Start date' to.</param>
        public static void SetPrimaryManagerStartDate(string startDate)
        {
            string dialogId = GetDialogId(DialogIds);
            if (dialogId == AddDialogId) OpenTab("Details");
            SetTextField(getXInput(dialogId, "_PRIMARYMANAGERDATEFROM_value"), startDate);
        }

        /// <summary>
        /// Set the 'Start date' of the secondary manager.
        /// </summary>
        /// <param name="startDate">The date to set the 'Start date' to.</param>
        public static void SetSecondaryManagerStartDate(string startDate)
        {
            string dialogId = GetDialogId(DialogIds);
            if (dialogId == AddDialogId) OpenTab("Details");
            SetTextField(getXInput(dialogId, "_SECONDARYMANAGERDATEFROM_value"), startDate);
        }


        //FIXME - this needs deprecated and removed

        /// <summary>
        /// The fields on the Details tab of the dialog.
        /// </summary>
        /// <param name="details">Mapping of the field captions to their desired values.</param>
        public static void SetDetails(TableRow details)
        {
            string dialogId = GetDialogId(DialogIds);
            if (dialogId == AddDialogId) OpenTab("Details");
            if (details.ContainsKey("Start date")) SetPlanStartDate(details["Start date"]);
            details["Start date"] = null;
            SetFields(dialogId, details, SupportedFields);
        }

        /// <summary>
        /// Set the participants grid values.
        /// Starts with the first row.
        /// </summary>
        /// <param name="participants">Table where each row represents a participant to add.
        /// A row is a mapping of the column captions to a single row's values.</param>
        public static void SetParticipants(Table participants)
        {
            var dialogId = GetDialogId(DialogIds);
            if (dialogId == AddDialogId) OpenTab("Details");

            var columnCaptionToIndex = MapColumnCaptionsToIndex(participants.Rows[0].Keys,
                getXGridHeaders(dialogId, "PROSPECTPLAN_PARTICIPANTS_value"));

            SetGridRows(dialogId, "PROSPECTPLAN_PARTICIPANTS_value", participants, 1, columnCaptionToIndex, SupportedFields);
        }

        /// <summary>
        /// Set the plan outline.
        /// </summary>
        /// <param name="outline">The type of outline to use.</param>
        public static void SetOutline(string outline)
        {
            var dialogId = GetDialogId(DialogIds);
            if (dialogId == AddDialogId) OpenTab("Steps");
            SetDropDown(getXInput(dialogId, "_OUTLINELIST_value"), outline);
            ClickButton("Add steps from plan outline");
        }

        /// <summary>
        /// Select a row in the Steps grid.
        /// </summary>
        /// <param name="index">The index of the row to select.  The first row's index is 1.</param>
        public static void SelectStepRow(int index)
        {
            string dialogId = GetDialogId(DialogIds);
            if (dialogId == AddDialogId) OpenTab("Steps");
            WaitClick(getXGridRowSelector(dialogId, "_STEPS_value", index));
        }

        /// <summary>
        /// Set a step row's values.
        /// </summary>
        /// <param name="step">Mapping of the column captions to a single row's values.</param>
        /// <param name="index">The index of the row.  The first row's index is 1.</param>
        public static void SetStepRow(TableRow step, int index)
        {
            IDictionary<string, int> columnCaptionToIndex = MapColumnCaptionsToIndex(step.Keys,
                getXGridHeaders(GetDialogId(DialogIds), "_STEPS_value"));

            SetGridRow(GetDialogId(DialogIds), "_STEPS_value", step, index, columnCaptionToIndex, SupportedFields);
        }

        /// <summary>
        /// Select a row and insert a new step above it.
        /// </summary>
        /// <param name="step">Mapping of the column captions to a single row's values.</param>
        /// <param name="index">The index of the row to select before inserting.  The first row's index is 1.</param>
        public static void InsertStep(TableRow step, int index)
        {
            SelectStepRow(index);
            ClickButton("Insert");
            SetStepRow(step, index);
        }

        /// <summary>
        /// Save the dialog.
        /// </summary>
        public new static void Save()
        {
            Dialog.Save();
            GetDisplayedElement(string.Format("{0} | {1}", Panel.getXPanelHeader("individual"), 
                Panel.getXPanelHeader("fa_majorgiving")));
        }


        //FIXME - deprecate and remove - this is taking forever
        // I suspect that the focus thing is a syptom of the element being clicked
        // in other areas I noticed that the 'trigger' element is actually the parent to the field
        // in any case this is extremely not KISS and there must be a better way.

        /// <summary>
        /// Set the 'Start date' of the 'Add prospect plan' dialog.  For whatever reason this requires custom logic
        /// from the default SetField.  Tab over or clicking on the input does NOT change its class to contain the
        /// focused attribute value.  This means it just continues trying to click the field and never sending it a value.
        /// 
        /// If you unfocus from the web browser to another application at any point in this attempt, the field magically starts 
        /// to work and the focus attribute does get added from a click.  I believe this is an underlying product bug that is only
        /// discoverable through automation via a selenium change of field focus.  Stepping through the default setfield implementation. 
        /// causes it to work.  Manually executing the steps shows all the expected element attribute changes when inspecting the field.
        /// </summary>
        /// <param name="xPath">The XPath to find an element for setting the value to.</param>
        /// <param name="value">The desired value of the element.</param>
        private static void SetAddProspectPlanStartDate(string xPath, string value)
        {
            if (value == null) return;

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(ElementClickInterceptedException));
            //removed element from being a public field and moved to scope of method
            waiter.Until(d =>
            {
                CopyToClipBoard(value);

                var innerwaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                innerwaiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(ElementClickInterceptedException));
                try
                {
                    innerwaiter.Until(d1 =>
                    {
                        var fieldElement = d.FindElement(By.XPath(xPath));
                        if (fieldElement.Displayed)
                        {
                            fieldElement.Click();
                            return true;
                        }
                        return false;
                    });

                }
                catch (WebDriverTimeoutException) { return false; }

                try
                {
                    innerwaiter.Until(dd =>
                    {
                        var focusedElement = dd.FindElement(By.XPath(xPath));
                        if (focusedElement == null || !focusedElement.Displayed) return false;
                        focusedElement.SendKeys(Keys.Control + "a");
                        focusedElement.SendKeys(Keys.Control + "v");
                        focusedElement.SendKeys(Keys.Tab);
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                try
                {
                    ElementValueIsSet(xPath, value);
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                return true;
            });
        }
    }
}
