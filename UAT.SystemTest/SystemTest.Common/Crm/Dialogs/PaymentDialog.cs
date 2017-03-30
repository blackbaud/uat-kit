using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
//using Oxford.UAT.Crm;
using TechTalk.SpecFlow;
using System.Configuration;
using System.Globalization;
using System.Threading;
using TechTalk.SpecFlow.Assist;
using System.Linq;
using System.Dynamic;
using SystemTest.Common.Bbis;

namespace SystemTest.Common.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing payment dialog.
    /// </summary>
    [Binding]
    public class PaymentDialog : Dialog
    {
        /// <summary>
        /// Enumerable of supported Dialog IDs.
        /// </summary>
        public static IEnumerable<string> DialogIds()
        {
            yield return "paymentAddForm";
            yield return "paymentEditForm2";
        }
        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// General fields
        /// Needed to set the field's value.
        /// </summary>
        public static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
                {"Constituent", new CrmField("_CONSTITUENTID_value", FieldType.Searchlist, "RevenueConstituentSearch", "_KEYNAME_value")},
                {"Amount", new CrmField("_AMOUNT_value", FieldType.TextInput)},
                {"Date", new CrmField("_DATE_value", FieldType.TextInput)},
                {"Application", new CrmField("_APPLICATIONCODE_value", FieldType.Dropdown)},
                {"Designation",new CrmField("_DONATIONDESIGNATIONID_value", FieldType.Searchlist, "DesignationSearch","_COMBINEDSEARCH_value")},
                {"Gift Aid sponsorship", new CrmField("_ISGIFTAIDSPONSORSHIP_value", FieldType.Checkbox)},
                {"Issuer", new CrmField("_ISSUER_value", FieldType.TextInput)},
                {"Number of units", new CrmField("_NUMBEROFUNITS_value", FieldType.TextInput)},
                {"Price per share", new CrmField("_MEDIANPRICE_value", FieldType.TextInput)},
                {"Subtype", new CrmField("_PROPERTYSUBTYPECODEID_value", FieldType.Dropdown)},
        };
        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// Credit card - pay installments automatically
        /// </summary>
        public static readonly IDictionary<string, CrmField> GIKPaymentSupportedFields = new Dictionary<string, CrmField>
            {
                {"Subtype", new CrmField("_GIFTINKINDSUBTYPECODEID_value", FieldType.Dropdown)},
                {"Item name", new CrmField("_GIFTINKINDITEMNAME_value", FieldType.TextInput)},
                {"Disposition", new CrmField("_GIFTINKINDDISPOSITIONCODE_value", FieldType.Dropdown)},
                {"Number of units", new CrmField("_GIFTINKINDNUMBEROFUNITS_value", FieldType.TextInput)},
                {"Fair market value per unit", new CrmField("_GIFTINKINDFAIRMARKETVALUE_value", FieldType.TextInput)}

            };
        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// Credit card - pay installments automatically
        /// </summary>
        public static readonly IDictionary<string, CrmField> CCPayInstallmentsSupportedFields = new Dictionary<string, CrmField>
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
        public static readonly IDictionary<string, CrmField> CCStoreSupportedFields = new Dictionary<string, CrmField>
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
        public static readonly IDictionary<string, CrmField> DDPayInstallmentsSupportedFields = new Dictionary<string, CrmField>
        {
            {"Account", new CrmField("_CONSTITUENTACCOUNTID_value", FieldType.Dropdown)},
            {"Reference date", new CrmField("_REFERENCEDATE_value", FieldType.TextInput)},
            {"Reference number", new CrmField("_REFERENCENUMBER_value", FieldType.TextInput)}
        };


        /// <summary>
        /// Static mappings of supported field captions to CRMField objects encapsulating all relevant variables
        /// Standing order
        /// </summary>
        public static readonly IDictionary<string, CrmField> StandingOrderSupportedFields = new Dictionary<string, CrmField>
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
        public static readonly IDictionary<string, CrmField> OtherSupportedFields = new Dictionary<string, CrmField>
        {
            {"Other method", new CrmField("_OTHERPAYMENTMETHODCODEID_value", FieldType.Dropdown)},
            {"Reference date", new CrmField("_REFERENCEDATE_value", FieldType.TextInput)},
            {"Reference number", new CrmField("_REFERENCENUMBER_value", FieldType.TextInput)}
        };
        public static void SetPaymentFields(Table AddPayment)
        {
            dynamic objectData = AddPayment.CreateDynamicInstance();
            
            //set default inherited dialog fields
            SetFields(GetDialogId(DialogIds()), AddPayment.Rows[0], SupportedFields);
            //handle various payment method types
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
                    SetFields(GetDialogId(DialogIds()), AddPayment.Rows[0], CCPayInstallmentsSupportedFields);
                    break;
                case "credit card - store last 4 digits for reference":
                    SetFields(GetDialogId(DialogIds()), AddPayment.Rows[0], CCStoreSupportedFields);
                    break;
                case "direct debit - pay installments automatically":
                    SetFields(GetDialogId(DialogIds()), AddPayment.Rows[0], DDPayInstallmentsSupportedFields);
                    break;
                case "standing order":
                    SetFields(GetDialogId(DialogIds()), AddPayment.Rows[0], StandingOrderSupportedFields);
                    break;
                case "other":
                    SetFields(GetDialogId(DialogIds()), AddPayment.Rows[0], OtherSupportedFields);
                    break;
                default:
                    throw new NotImplementedException(string.Format("Test failed. Payment method {0} not implemented.", paymentMethod));
            }
        }
    }
}
