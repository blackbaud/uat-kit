using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace SystemTest.Common.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing recurring gift dialog.
    /// </summary>
    [Binding]
    public class RecurringGiftDialog : Dialog
    {
        /// <summary>
        /// Enumerable of supported Dialog IDs.
        /// </summary>
        public static IEnumerable<string> DialogIds()
        {
            yield return "RecurringGiftAddForm";
            yield return "RecurringGiftEditForm2";
        }
        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// General fields
        /// Needed to set the field's value.
        /// </summary>
        private static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
             {"Constituent",
                new CrmField("_CONSTITUENTID_value", FieldType.Searchlist, "RevenueConstituentSearch", "KEYNAME")},
             {"Finder number", new CrmField("_FINDERNUMBERSTRING_value", FieldType.TextInput)},
             {"Amount", new CrmField("_AMOUNT_value", FieldType.TextInput)},
             {"Source code",new CrmField("_SOURCECODE_value", FieldType.Searchlist, "SourceCodeMapSearch","_SOURCECODE_value")},
             {"Appeal", new CrmField("_APPEALID_value", FieldType.Searchlist,"RevenueAppealSearch", "_NAME_value")},
             {"Designations",new CrmField("_SINGLEDESIGNATIONID_value", FieldType.Searchlist, "DesignationSearch","_COMBINEDSEARCH_value")},
             {"Date", new CrmField("_DATE_value", FieldType.TextInput)},
             {"Inbound channel", new CrmField("_CHANNELCODEID_value", FieldType.Dropdown)},
             {"Effort", new CrmField("_MAILINGID_value", FieldType.Searchlist,"MarketingEffortSearch", "_NAME_value")},
             {"Reference", new CrmField("_REFERENCE_value", FieldType.TextInput)},
             {"Revenue category", new CrmField("_CATEGORYCODEID_value", FieldType.Dropdown)},
             {"Make this recurring gift anonymous", new CrmField("_GIVENANONYMOUSLY_value", FieldType.Checkbox)},
             {"Constituent declines Gift Aid", new CrmField("_DECLINESGIFTAID_value", FieldType.Checkbox)},
             {"Installment frequency", new CrmField("_FREQUENCYCODE_value", FieldType.Dropdown)},
             {"Installment schedule begins", new CrmField("_STARTDATE_value", FieldType.TextInput)},
             {"End date (optional)", new CrmField("_ENDDATE_value", FieldType.TextInput)},
             {"Payment method", new CrmField("_PAYMENTMETHODCODEUI_value", FieldType.Dropdown)},
             {"Send reminders", new CrmField("_SENDREMINDER_value", FieldType.Checkbox)},
             {"Do not acknowledge", new CrmField("_DONOTACKNOWLEDGE_value", FieldType.Checkbox)}

        };

        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// Credit card - pay installments automatically
        /// </summary>
        private static readonly IDictionary<string, CrmField> CCPayInstallmentsSupportedFields = new Dictionary<string, CrmField>
        {
            {"Card type", new CrmField("_CREDITTYPECODEID_value", FieldType.Dropdown)},
            {"Card number", new CrmField("_CREDITCARDNUMBER_value", FieldType.TextInput)},
            {"Name on card", new CrmField("_CARDHOLDERNAME_value", FieldType.TextInput)},
            {"Expires on", new CrmField("_EXPIRESON_value", FieldType.TextInput)}
        };

        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// Credit card - store last 4 digits for reference
        /// </summary>
        private static readonly IDictionary<string, CrmField> CCStoreSupportedFields = new Dictionary<string, CrmField>
        {
             {"Card type", new CrmField("_CREDITTYPECODEID_value", FieldType.Dropdown)},
             {"Last 4 card digits", new CrmField("_CREDITCARDNUMBER_value", FieldType.TextInput)},
             {"Name on card", new CrmField("_CARDHOLDERNAME_value", FieldType.TextInput)},
             {"Expires on", new CrmField("_EXPIRESON_value", FieldType.TextInput)}
        };

        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// Direct debit - pay installments automatically
        /// </summary>
        private static readonly IDictionary<string, CrmField> DDPayInstallmentsSupportedFields = new Dictionary<string, CrmField>
        {
            {"Account", new CrmField("_CONSTITUENTACCOUNTID_value", FieldType.Dropdown)},
            {"Reference date", new CrmField("_REFERENCEDATE_value", FieldType.TextInput)},
            {"Reference number", new CrmField("_REFERENCENUMBER_value", FieldType.TextInput)}
        };


        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// Standing order
        /// </summary>
        private static readonly IDictionary<string, CrmField> StandingOrderSupportedFields = new Dictionary<string, CrmField>
        {
            {"Account", new CrmField("_CONSTITUENTACCOUNTID_value", FieldType.Dropdown)},
            {"Standing order has been setup", new CrmField("_STANDINGORDERSETUP_value", FieldType.Checkbox)},
            {"Setup on", new CrmField("_STANDINGORDERSETUPDATE_value", FieldType.TextInput)},
            {"Automatically generate reference number", new CrmField("_GENERATEREFERENCENUMBER_value", FieldType.Checkbox)},
            {"Reference number", new CrmField("_STANDINGORDERREFERENCENUMBER_value", FieldType.TextInput)}
        };

        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// Other
        /// </summary>
        private static readonly IDictionary<string, CrmField> OtherSupportedFields = new Dictionary<string, CrmField>
        {
            {"Other method", new CrmField("_OTHERPAYMENTMETHODCODEID_value", FieldType.Dropdown)},
            {"Reference date", new CrmField("_REFERENCEDATE_value", FieldType.TextInput)},
            {"Reference number", new CrmField("_REFERENCENUMBER_value", FieldType.TextInput)}
        };


        public static void SetRecurringGiftFields(Table addRecurringGift)
        {
            dynamic objectData = addRecurringGift.CreateDynamicInstance();
            // set default inherited dialog fields
            SetFields(GetDialogId(DialogIds()), addRecurringGift.Rows[0], SupportedFields);
            // handle various payment method types
            string paymentMethod = Convert.ToString(objectData.PaymentMethod);
            switch (paymentMethod.ToLower())
            {
                case "none":
                    break;
                case "cash":
                    break;
                case "check":
                    break;
                case "credit card - pay installments automatically":
                    SetFields(GetDialogId(DialogIds()), addRecurringGift.Rows[0], CCPayInstallmentsSupportedFields);
                    break;
                case "credit card - store last 4 digits for reference":
                    SetFields(GetDialogId(DialogIds()), addRecurringGift.Rows[0], CCStoreSupportedFields);
                    break;
                case "direct debit - pay installments automatically":
                    SetFields(GetDialogId(DialogIds()), addRecurringGift.Rows[0], DDPayInstallmentsSupportedFields);
                    break;
                case "standing order":
                    SetFields(GetDialogId(DialogIds()), addRecurringGift.Rows[0], StandingOrderSupportedFields);
                    break;
                case "other":
                    SetFields(GetDialogId(DialogIds()), addRecurringGift.Rows[0], OtherSupportedFields);
                    break;
                default:
                    throw new NotImplementedException(string.Format("Test failed. Payment method {0} not implemented.", paymentMethod));
            }
        }
    }
}
