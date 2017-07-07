using System;
using System.Collections.Generic;
using System.Threading;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for adding/editing an ad-hoc query dialog.
    /// </summary>
    public class AdHocQueryDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "AdHocQueryCriteriaForm", "AdHocQuerySaveOptionsViewDataForm" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"FILTEROPERATOR", new CrmField("FILTEROPERATOR", FieldType.Dropdown)},
            {"VALUE1", new CrmField("VALUE1", FieldType.TextInput)},
            {"COMBOVALUE", new CrmField("COMBOVALUE", FieldType.Dropdown)},
            {"FILTERTYPE_0", new CrmField("FILTERTYPE_0", FieldType.Checkbox)},
            {"FILTERTYPE_1", new CrmField("FILTERTYPE_1", FieldType.Checkbox)},
            {"INCLUDEBLANKS", new CrmField("INCLUDEBLANKS", FieldType.Checkbox)},
            {"OUTPUTFIELD1", new CrmField("OUTPUTFIELD1", FieldType.Dropdown)},
            {"MEETSALLCRITERIA", new CrmField("MEETSALLCRITERIA", FieldType.Checkbox)},
            {"DATEFILTERTYPE", new CrmField("DATEFILTERTYPE", FieldType.Dropdown)},
            {"DATEVALUE1", new CrmField("DATEVALUE1", FieldType.TextInput)},
            {"Name", new CrmField("NAME", FieldType.TextInput)},
            {"Description", new CrmField("DESCRIPTION", FieldType.TextArea)},
            {"Suppress duplicate row", new CrmField("SUPPRESSDUPLICATES", FieldType.Checkbox)},
            {"Create a selection?", new CrmField("CREATESELECTION", FieldType.Checkbox)},
            {"Create a dynamic selection", new CrmField("SELECTION_TYPE_0", FieldType.Checkbox)}
        };

        private static string getXNode(string nodePath)
        {
            return String.Format("//div[@*='{0}']", nodePath);
        }

        private static string getXNodeChildren(string nodePath)
        {
            return String.Format("{0}/../ul[contains(@style,'visibility: visible') and contains(@style,'position: static')]", getXNode(nodePath));
        }

        private static string getXNodeExpander(string nodePath)
        {
            return String.Format("//div[@*='{0}']/img[contains(@class,'x-tree-ec-icon x-tree-elbow')]", nodePath);
        }

        private static string getXField(string caption)
        {
            return String.Format("//div[@class='x-grid3-viewport']/div/div/div[contains(@id,'gp-category')]/div[@class='x-grid-group-body']/div/table/tbody/tr/td/div[./text()='{0}']", caption);
        }

        private const string getXIncludeRecordButton = "//span[./text()='Include records where:' and @class='x-panel-header-text']/../../../../../../div[@class=' x-panel x-panel-tbar x-panel-tbar-noheader x-border-panel']/div/div/table/tbody/tr/td/em/button[@class=' x-btn-text bbui-adhocquery-movefieldright']";

        private const string getXDisplayRecordButton = "//span[./text()='Results fields to display:' and @class='x-panel-header-text']/../../../../../../div[@class=' x-panel x-panel-tbar x-panel-tbar-noheader x-border-panel']/div/div/table/tbody/tr/td/em/button[@class=' x-btn-text bbui-adhocquery-movefieldright']";

        private static void WaitForNodesToLoad(string xPath)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.Until(d =>
            {
                var loaded = d.FindElement(By.XPath(xPath + "/../../ul[contains(@style,'visibility: visible') and contains(@style,'position: static')]"));
                if (loaded != null)
                {
                    /*
                     * Ugly hack sleep line.  Even though the dom lists the child nodes as loaded
                     * based on the syle attribute values, it may still be finalizing the accordion
                     * expand.
                     * 
                     * I cannot find at the moment a different dom attribute that is changing as the
                     * node list expands.
                     */
                    Thread.Sleep(250);
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// Expand a tree node if it is collapsed.
        /// </summary>
        public static void ExpandNode(string xPath)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException));
            waiter.Until(d =>
            {
                var node = GetEnabledElement(xPath);
                var nodeClass = node.GetAttribute("class");
                if (nodeClass.Contains("plus"))
                {
                    node.Click();
                    WaitForNodesToLoad(xPath);
                }
                else
                {
                    WaitClick(xPath + "/..");
                }
                return true;
            });
        }

        /// <summary>
        /// Select a filter category in the 'Browse for fields in' panel
        /// </summary>
        /// <param name="filter">The complete filter path.  Nested filters should be provided as a complete
        /// path (i.e. 'Revenue\Constituent\Spouse'</param>
        public static void FilterBy(string filter)
        {
            OpenTab("Select filter and output fields");
            var filterPath = BuildFilterPath(filter);
            var aggregatePath = string.Empty;
            foreach (var node in filterPath)
            {
                aggregatePath = aggregatePath == string.Empty ? node : String.Format(@"{0}\{1}", aggregatePath, node);
                ExpandNode(getXNodeExpander(aggregatePath));
            }
        }

        private static IEnumerable<string> BuildFilterPath(string filter)
        {
            string[] filterPath = filter.Split(new [] { '\\' });
            filterPath[0] = String.Format("V_QUERY_{0}", filterPath[0].ToUpper());
            return filterPath;
        }

        /// <summary>
        /// Set the fields on the apply criteria dialog that appears when adding a filter field.
        /// </summary>
        /// <param name="criteria">Mapping of the unique field ids to their desired values.</param>
        public static void ApplyCriteria(TableRow criteria)
        {
            var dialogId = GetDialogId(DialogIds);
            SetFields(dialogId, criteria, SupportedFields);
            OK();
        }

        /// <summary>
        /// Select a field and add it as a filter field uing the provided criteria.
        /// </summary>
        /// <param name="fieldCaption">The field to add.</param>
        /// <param name="criteria">Mapping of the unique field ids to their desired values for the criteria dialog.</param>
        public static void AddFilterField(string fieldCaption, TableRow criteria)
        {
            OpenTab("Select filter and output fields");
            WaitClick(getXField(fieldCaption));
            WaitClick(getXIncludeRecordButton);
            ApplyCriteria(criteria);
        }

        /// <summary>
        /// Select a field and add it as an output field to display.
        /// </summary>
        /// <param name="fieldCaption">The field to add.</param>
        public static void AddOutputField(string fieldCaption)
        {
            OpenTab("Select filter and output fields");
            WaitClick(getXField(fieldCaption));
            WaitClick(getXDisplayRecordButton);
        }

        /// <summary>
        /// Set the values on the 'Set save options' tab
        /// </summary>
        /// <param name="options">Dictionary mapping of the field captions to their desired values</param>
        public static void SetSaveOptions(TableRow options)
        {
            OpenTab("Set save options");
            SetFields(GetDialogId(DialogIds), options, SupportedFields);
        }

        /// <summary>
        /// Click the 'Save and close' button
        /// </summary>
        public static void SaveAndClose()
        {
            WaitClick(getXSaveAndCloseButton);
        }

    }
}
