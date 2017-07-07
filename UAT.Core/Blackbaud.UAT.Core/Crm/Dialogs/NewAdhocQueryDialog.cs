using System;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Add Ad-hoc Query Dialog functions and interactions in BBCRM.
    /// </summary>
    public class NewAdhocQueryDialog : Dialog
    {
        /// <summary>
        /// Get an XPath for an input field on a NewAdhocQueryDialog.
        /// </summary>
        /// <param name="inputId">The Id of the input field.</param>
        public static string getXSaveOptionsInput(string inputId) { return String.Format("//div[contains(@id, 'AdHocQuerySaveOptionsViewDataForm')]//input[contains(@id, '{0}')]", inputId); }

        /// <summary>
        /// Get an XPath for a TextArea on a NewAdhocQueryDialog.
        /// </summary>
        /// <param name="AreaId">The id of the TextArea.</param>
        public static string getXSaveOptionsTextArea(string AreaId) { return String.Format("//div[contains(@id, 'AdHocQuerySaveOptionsViewDataForm')]//textarea[contains(@id, '{0}')]", AreaId); }
        
        /// <summary>
        /// Constant XPath for the 'Find' Field of an NewAdhocQueryDialog.
        /// </summary>
        public static string getXFindField = "//*[contains(@class,\"bbui-query-container\")]//tr[td/div/span[./text()=\"Find field:\"]]//*[contains(@class,\"x-form-text\")]";

        /// <summary>
        ///Get an XPath for a result field on a NewAdhocQueryDialog.
        /// </summary>
        /// <param name="text">The text of the Result Field.</param>
        public static string getXFieldResult(string text) {return String.Format("//div[@class='x-grid3-viewport']/div/div/div[contains(@id,'gp-category')]/div[@class='x-grid-group-body']/div/table/tbody/tr/td/div[./text()='{0}']",text);}

        /// <summary>
        /// Format a unique XPath for a Input on an AdHocQueryCriteriaForm on the NewAdhocQueryDialog.
        /// </summary>
        /// <param name="inputId">The ID of the input.</param>
        protected static string getXCriteriaInput(string inputId) { return String.Format("//div[contains(@id, 'AdHocQueryCriteriaForm')]//input[contains(@id, '{0}')]", inputId); }

        /// <summary>
        /// Constant XPath for the Add Filter Arrow on a NewAdhocQueryDialog.
        /// </summary>
        protected static string getXAddFilterArrow ="//span[./text()='Include records where:' and @class='x-panel-header-text']/../../../../../../div[@class=' x-panel x-panel-tbar x-panel-tbar-noheader x-border-panel']/div/div/table/tbody/tr/td/em/button[@class=' x-btn-text bbui-adhocquery-movefieldright']";

        /// <summary>
        /// Constant XPath for the Add Filter Arrow on a NewAdhocQueryDialog.
        /// </summary>
        protected static string getXAddOutputFieldArrow = "//span[./text()='Results fields to display:' and @class='x-panel-header-text']/../../../../../../div[@class=' x-panel x-panel-tbar x-panel-tbar-noheader x-border-panel']/div/div/table/tbody/tr/td/em/button[@class=' x-btn-text bbui-adhocquery-movefieldright']";

        /// <summary>
        /// Format a unique XPath for a tab on the Ad-Hoc Query Dialog.
        /// </summary>
        /// <param name="tabCaption">The caption of the tab.</param>
        protected static new string getXTab(string tabCaption) { return String.Format("//span[./text() = \"{0}\"]", tabCaption); }

        /// <summary>
        /// Set the Find field to the input string
        /// </summary>
        /// <param name="field">The field to set</param>
        public static void SetFindField(string field)
        {
            SetTextField(getXFindField, field);            
        }

        /// <summary>
        /// Include record criteria based on field and data in the table
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="criteria">Table of critera</param>
        public static void IncludeRecordCriteria(string field, Table criteria)
        {
            // Select the right field in the find results
            WaitClick(getXFieldResult(field));
            WaitClick(getXAddFilterArrow);
            ApplyCriteria(criteria);
        }

        /// <summary>
        /// Include display field based on string passed in
        /// </summary>
        /// <param name="field">The field to be included</param>
        public static void IncludeDisplayField(string field)
        {
            // Select the right field in the find results
            WaitClick(getXFieldResult(field));
            WaitClick(getXAddOutputFieldArrow);                        
        }

        /// <summary>
        /// Apply criteria to the query based on the table passed in
        /// </summary>
        /// <param name="criteria">Table of criteria to apply</param>
        public static void ApplyCriteria(Table criteria)
        {
            foreach (TableRow criter in criteria.Rows)
            {
                //string field = criteria["field"];
                switch (criter["field"])
                {
                    case "FILTEROPERATOR":
                        SetTextField(getXCriteriaInput(criter["field"]), criter["value"]);
                        break;
                    case "VALUE1":
                        SetTextField(getXCriteriaInput(criter["field"]), criter["value"]);
                        break;
                    case "COMBOVALUE":
                        SetTextField(getXCriteriaInput(criter["field"]), criter["value"]);
                        break;
                    default:
                        throw new NotImplementedException("Criteria field '" + criter["field"] + "' is not implemented yet.");
                }
            }
            WaitClick(getXDialogButton("OK"));
        }

        /// <summary>
        /// Set the values on the 'Set save options' tab
        /// </summary>
        /// <param name="options">Dictionary mapping of the field captions to their desired values</param>
        public static void SetSaveOptions(Table options)
        {
            WaitClick(getXTab("Set save options"));                   
            
            foreach (TableRow option in options.Rows)
            {
                switch (option["field"])
                {
                    case "Name":
                        SetTextField(getXSaveOptionsInput("NAME"), option["value"]);
                        break;
                    case "Description":
                        SetTextField(getXSaveOptionsTextArea("DESCRIPTION"), option["value"]);
                        break;
                    case "Suppress duplicate row":
                        SetCheckbox(getXInput("AdHocQuerySaveOptionsViewDataForm", "SUPPRESSDUPLICATES"), option["value"]);
                        break;
                    case "Create a selection?":
                        SetCheckbox(getXInput("AdHocQuerySaveOptionsViewDataForm", "CREATESELECTION"), option["value"]);
                        break;
                    case "Create a dynamic selection":
                        SetCheckbox(getXInput("AdHocQuerySaveOptionsViewDataForm", "SELECTION_TYPE_0"), option["value"]);
                        break;
                    default:
                        throw new NotImplementedException("Field '" + option["field"] + "' is not implemented for the 'New Ad-hoc Query' dialog's 'Set save options' tab.");
                }
            }
        }

    }
}
