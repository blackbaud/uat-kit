using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing a payment dialog.
    /// </summary>
    public class PaymentDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "paymentAddForm"};

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Constituent", new CrmField("_CONSTITUENTID_value", FieldType.Searchlist, "RevenueConstituentSearch", "_KEYNAME_value")},
            {"Amount", new CrmField("_AMOUNT_value", FieldType.TextInput)},
            {"Date", new CrmField("_DATE_value", FieldType.TextInput)},
            {"Finder number", new CrmField("_FINDERNUMBER_value", FieldType.TextInput)},
            {"Inbound channel", new CrmField("_CHANNELCODEID_value", FieldType.Dropdown)},
            {"Payment is anonymous", new CrmField("_GIVENANONYMOUSLY_value", FieldType.Checkbox)},
            {"Application", new CrmField("_APPLICATIONCODE_value", FieldType.Dropdown)},
            {"Applied", new CrmField("_DONATIONAPPLIED_value", FieldType.TextInput)},
            {"Designation", new CrmField("_DONATIONDESIGNATIONID_value", FieldType.Searchlist, "DesignationSearch", "_COMBINEDSEARCH_value")},
            {"Payment method", new CrmField("_PAYMENTMETHODCODE_value", FieldType.Dropdown)}
        };

        /// <summary>
        /// Mapping of the 'Application' dropdown options to their 'Add' button id.
        /// </summary>
        private static readonly IDictionary<string, string> ApplicationToAddButtonId = new Dictionary<string, string>
        {
            {"All", "_ADDSELECTEDCOMMITMENT_action"}, { "Donation", "_DONATIONADDACTION_action" }, {"Other","_OTHERADDACTION_action"},
            {"Pledge", "_ADDSELECTEDCOMMITMENT_action"}
        };

        /// <summary>
        /// Set the fields to their desired values.
        /// </summary>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        public static void SetFields(TableRow fields)
        {
            var dialogId = GetDialogId(DialogIds);
            SetFields(dialogId, fields, SupportedFields);
        }

        /// <summary>
        /// Add an application to the payment.
        /// </summary>
        /// <param name="application">Mapping of the field captions to their desired values in the various application types.</param>
        public static void AddApplication(TableRow application)
        {
            var dialogId = GetDialogId(DialogIds);
            if (!application.ContainsKey("Application")) throw new ArgumentException("Include an 'Application' TableRow key for AddApplication");
            var applicationType = application["Application"];

            SetFields(dialogId, application, SupportedFields);

            if (ApplicationToAddButtonId.Keys.Contains(applicationType)) 
                WaitClick(getXDialogButton("Add", ApplicationToAddButtonId[applicationType]));
            else 
                throw new NotImplementedException(String.Format("Application type '{0}' is not implemented on the payment dialog.", applicationType));
            
        }
    }
}
