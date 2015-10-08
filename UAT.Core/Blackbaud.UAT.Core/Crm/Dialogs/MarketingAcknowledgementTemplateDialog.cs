using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for adding/editing a marketing acknowledgement template dialog.
    /// </summary>
    public class MarketingAcknowledgementTemplateDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "MarketingAcknowledgementTemplateAddForm" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
            {"Description", new CrmField("_DESCRIPTION_value", FieldType.TextArea)},
            {"Site", new CrmField("_SITEID_value", FieldType.Searchlist, "SiteSearch", "_SITEID_value")},
            {"Assign letters based on segmentation", new CrmField("_LETTERS_0", FieldType.Checkbox)},
            {"Allow multiple letters per revenue", new CrmField("_LETTERS_1", FieldType.Checkbox)},
            {"Mark letters 'Acknowledged/Receipted' when process completes", 
                new CrmField("_MARKLETTERSACKNOWLEDGED_value", FieldType.Checkbox)},
            {"Activate and export marketing acknowledgement when template processing completes", 
                new CrmField("_RUNACTIVATEANDEXPORT_value", FieldType.Checkbox)},
            {"Appeal", new CrmField("_INDIVIDUALAPPEAL_value", FieldType.Searchlist, "AppealSearch", "_NAME_value")},
            {"Capture source analysis rule data", new CrmField("_CACHESOURCEANALYSISRULEDATA_value", FieldType.Checkbox)},
            {"Run marketing exclusions report", new CrmField("_RUNMARKETINGEXCLUSIONSREPORT_value", FieldType.Checkbox)}
        };
        /// <summary>
        /// Set the fields on the 'General' tab.
        /// </summary>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        public static void SetGeneralTabFields(TableRow fields)
        {
            string dialogId = GetDialogId(DialogIds);
            OpenTab("General", dialogId);

            if (fields.ContainsKey("Date"))
            {
                if (fields["Date"] == "Today") SetDropDown(getXInput(dialogId, "_ACKNOWLEDGEDATETYPECODE_value"), fields["Date"]);
                else
                {
                    SetDropDown(getXInput(dialogId, "_ACKNOWLEDGEDATETYPECODE_value"), "<Specific date>");
                    SetTextField(getXInput(dialogId, "_ACKNOWLEDGEDATE_value"), fields["Date"]);
                }
                fields["Date"] = null;
            }
            SetFields(dialogId, fields, SupportedFields);
        }

        /// <summary>
        /// Set the 'Source code' field on the 'Source Code' tab.
        /// </summary>
        /// <param name="sourceCode">The desired value of the field.</param>
        public static void SetSourceCode(string sourceCode)
        {
            string dialogId = GetDialogId(DialogIds);
            OpenTab("Source Code", dialogId);
            SetDropDown(getXInput(dialogId, "_SOURCECODEID_value"), sourceCode);
        }

        /// <summary>
        /// Select the 'Include all records' box on the 'Universe' tab.
        /// </summary>
        public static void IncludeAllRecords()
        {
            string dialogId = GetDialogId(DialogIds);
            OpenTab("Universe", dialogId);
            SetCheckbox(getXInput(dialogId, "_INCLUDERECORDS_1"), true);
        }

        /// <summary>
        /// Set the fields on the 'Activation' tab.
        /// </summary>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        public static void SetActivationTabFields(TableRow fields)
        {
            string dialogId = GetDialogId(DialogIds);
            OpenTab("Activation", dialogId);
            SetFields(dialogId, fields, SupportedFields);
        }
    }
}
