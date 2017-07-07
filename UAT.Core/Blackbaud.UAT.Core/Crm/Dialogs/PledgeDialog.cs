using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for adding/editing pledge dialog.
    /// </summary>
    public class PledgeDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "PledgeAddForm" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Constituent", new CrmField("CONSTITUENTID", FieldType.Searchlist, "RevenueConstituentSearch", "KEYNAME")},
            {"Finder number", new CrmField("FINDERNUMBERSTRING", FieldType.TextInput)},
            {"Amount", new CrmField("AMOUNT", FieldType.TextInput)},
            {"Opportunity", new CrmField("OPPORTUNITYID", FieldType.Searchlist, "OpportunitySearch", "KEYNAME")},
            {"Source code", new CrmField("SOURCECODE", FieldType.TextInput)},
            {"Appeal", new CrmField("APPEALID", FieldType.Searchlist, "RevenueAppealSearch", "NAME")},
            {"Designations", new CrmField("SINGLEDESIGNATIONID", FieldType.Searchlist, "DesignationSearch", "COMBINEDSEARCH")},
            {"Date", new CrmField("DATE", FieldType.TextInput)},
            {"Inbound channel", new CrmField("CHANNELCODEID", FieldType.Dropdown)},
            {"Effort", new CrmField("MAILINGID", FieldType.Searchlist, "MarketingEffortSearch", "NAME")},
            {"Reference", new CrmField("REFERENCE", FieldType.TextInput)},
            {"Revenue category", new CrmField("CATEGORYCODEID", FieldType.Dropdown)},
            {"Pledge subtype", new CrmField("PLEDGESUBTYPEID", FieldType.Dropdown)},
            {"Pledge is anonymous", new CrmField("GIVENANONYMOUSLY", FieldType.Checkbox)},
            {"Starting on", new CrmField("STARTDATE", FieldType.TextInput)},
            {"Frequency", new CrmField("FREQUENCYCODE", FieldType.Dropdown)},
            {"Installment amount", new CrmField("INSTALLMENTAMOUNT", FieldType.TextInput)},
            {"No. installments", new CrmField("NUMBEROFINSTALLMENTS", FieldType.TextInput)},
            {"Post status", new CrmField("POSTSTATUSCODE", FieldType.Dropdown)},
            {"Post date", new CrmField("POSTDATE", FieldType.TextInput)},
            {"Send reminders", new CrmField("SENDPLEDGEREMINDER", FieldType.Checkbox)},
            {"Do not acknowledge", new CrmField("DONOTACKNOWLEDGE", FieldType.Checkbox)},
            {"Card type", new CrmField("CREDITTYPECODEID", FieldType.Dropdown)},
            {"Card number", new CrmField("CREDITCARDNUMBER", FieldType.TextInput)},
            {"Name on card", new CrmField("CARDHOLDERNAME", FieldType.TextInput)},
            {"Expires on", new CrmField("EXPIRESON", FieldType.TextInput)}
        };

        /// <summary>
        /// Split pledge evenly among the provided designations.
        /// </summary>
        /// <param name="designations">Table where each row represents a designation to add.  
        /// A row is a mapping of the 'Split designations' grid's column captions to the row's values.</param>
        public static void SplitDesignationsEvenly(Table designations)
        {
            var dialogId = GetDialogId(DialogIds);
            WaitClick(getXLinkByAction(dialogId, "SPLITSACTION"));

            DesignationsDialog.SplitDesignationsEvenly(designations);
            OK();
        }

        /// <summary>
        /// Set the account system of the pledge.
        /// </summary>
        /// <param name="system">The account system to use.</param>
        public static void SetAccountSystem(string system)
        {
            WaitClick(getXLinkByAction(GetDialogId(DialogIds), "SHOWSYSTEM"));
            SetDropDown(getXInput("SelectAccountSystem", "PDACCOUNTSYSTEMID"), system);
            OK();
        }

        /// <summary>
        /// Click the 'Edit installment designations' button and load the associated dialog.
        /// </summary>
        public static void EditInstallmentDesignations()
        {
            WaitClick(getXButton(GetDialogId(DialogIds), "Edit installment designations"));
        }

        /// <summary>
        /// Set the fields to their desired values
        /// </summary>
        /// <param name="fieldValues">Mapping of the field captions to their desired values.</param>
        public static void SetFields(TableRow fieldValues)
        {
            var dialogId = GetDialogId(DialogIds);
            fieldValues = CustomSetFieldsLogic(fieldValues, dialogId);
            SetFields(dialogId, fieldValues, SupportedFields);
        }

        /// <summary>
        /// Set the fields to their desired values
        /// </summary>
        /// <param name="fieldValues">Mapping of the field captions to their desired values</param>
        /// <param name="customSupportedFields">Mapping of field captions to to a CrmField oject encapsulating
        /// all relevant variables.  Field in the supportedFields mapping can be overridden by including them in this mapping.</param>
        public static void SetFields(TableRow fieldValues, IDictionary<string, CrmField> customSupportedFields)
        {
            var dialogId = GetDialogId(DialogIds);
            fieldValues = CustomSetFieldsLogic(fieldValues, dialogId);
            SetFields(dialogId, fieldValues, SupportedFields, customSupportedFields);
        }

        private static TableRow CustomSetFieldsLogic(TableRow fieldValues, string dialogId)
        {
            if (fieldValues.ContainsKey("Pay installments automatically by"))
            {
                var value = fieldValues["Pay installments automatically by"];
                try
                {
                    var boolValue = ConvertToBool(value);
                    SetCheckbox(getXInput(dialogId, "AUTOPAY"), boolValue);
                }
                catch (NotSupportedException)
                {
                    SetCheckbox(getXInput(dialogId, "AUTOPAY"), true);
                    SetDropDown(getXInput(dialogId, "PAYMENTMETHODCODE"), value);
                }
                fieldValues["Pay installments automatically by"] = null;
            }
            return fieldValues;
        }
    }
}
