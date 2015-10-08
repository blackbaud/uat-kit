using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing an individual dialog.
    /// </summary>
    public class IndividualDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "individualSpouseBusinessAddForm", "RevenueBatchConstituentInbatchEditForm" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Last name", new CrmField("_LASTNAME_value", FieldType.TextInput)},
            {"First name", new CrmField("_FIRSTNAME_value", FieldType.TextInput)},
            {"Middle name", new CrmField("_MIDDLENAME_value", FieldType.TextInput)},
            {"Nickname", new CrmField("_NICKNAME_value", FieldType.TextInput)},
            {"Maiden name", new CrmField("_MAIDENNAME_value", FieldType.TextInput)},
            {"Birth date", new CrmField("_BIRTHDATE_value", FieldType.TextInput)},
            {"City", new CrmField("_ADDRESS_CITY_value", FieldType.TextInput)},
            {"ZIP", new CrmField("_ADDRESS_POSTCODE_value", FieldType.TextInput)},
            {"Phone number", new CrmField("_PHONE_NUMBER_value", FieldType.TextInput)},
            {"Email address", new CrmField("_EMAILADDRESS_EMAILADDRESS_value", FieldType.TextInput)},
            {"Title", new CrmField("_TITLECODEID_value", FieldType.Dropdown)},
            {"Suffix", new CrmField("_SUFFIXCODEID_value", FieldType.Dropdown)},
            {"Marital status", new CrmField("_MARITALSTATUSCODEID_value", FieldType.Dropdown)},
            {"Gender", new CrmField("_GENDERCODE_value", FieldType.Dropdown)},
            {"Address type", new CrmField("_ADDRESS_ADDRESSTYPECODEID_value", FieldType.Dropdown)},
            {"Country", new CrmField("_ADDRESS_COUNTRYID_value", FieldType.Dropdown)},
            {"State", new CrmField("_ADDRESS_STATEID_value", FieldType.Dropdown)},
            {"Reason", new CrmField("_ADDRESS_DONOTMAILREASONCODEID_value", FieldType.Dropdown)},
            {"Phone type", new CrmField("_PHONE_PHONETYPECODEID_value", FieldType.Dropdown)},
            {"Email type", new CrmField("_EMAILADDRESS_EMAILADDRESSTYPECODEID_value", FieldType.Dropdown)},
            {"Information source", new CrmField("_ADDRESS_INFOSOURCECODEID_value", FieldType.Dropdown)},
            {"Do not send mail to this address", new CrmField("_ADDRESS_DONOTMAIL_value", FieldType.Checkbox)},
            {"Address", new CrmField("_ADDRESS_ADDRESSBLOCK_value", FieldType.TextArea)},
            {"Related individual", new CrmField("_SPOUSEID_value", FieldType.Searchlist, "ConstituentSearch", "_KEYNAME_value")},
            {"Copy Individual's primary contact information to the household", 
                new CrmField("_HOUSEHOLDCOPYPRIMARYCONTACTINFO_value", FieldType.Checkbox)},
            {"Individual is the", new CrmField("_SPOUSE_RELATIONSHIPTYPECODEID_value", FieldType.Dropdown)},
            {"Related individual is the", new CrmField("_SPOUSE_RECIPROCALTYPECODEID_value", FieldType.Dropdown)},
            {"Start date", new CrmField("_SPOUSE_STARTDATE_value", FieldType.TextInput)},
            {"This is the spouse relationship for Individual", new CrmField("_ISSPOUSERELATIONSHIP_value", FieldType.Checkbox)},
            {"Copy Individual's primary contact information to Add_Payment_To_Donation", new CrmField("_COPYPRIMARYINFORMATION_value", FieldType.Checkbox)}
        };

        /// <summary>
        /// Set the fields on the 'Household' tab.
        /// </summary>
        /// <param name="fieldValues">Mapping of the field captions to their desired values.  
        /// Empty strings will be considered values and set the field to a blank value.  Null values will be skipped.</param>
        public static void SetHouseholdFields(TableRow fieldValues)
        {
            var dialogId = GetDialogId(DialogIds);
            OpenTab("Household", dialogId);
            SetFields(dialogId, fieldValues, SupportedFields);
        }

        /// <summary>
        /// Set the fields on the 'Individual' tab.
        /// </summary>
        /// <param name="fieldValues">Mapping of the field captions to their desired values.  
        /// Empty strings will be considered values and set the field to a blank value.  Null values will be skipped.</param>
        public static void SetIndividualFields(TableRow fieldValues)
        {
            var dialogId = GetDialogId(DialogIds);
            OpenTab("Individual", dialogId);
            SetFields(dialogId, fieldValues, SupportedFields);
        }

        /// <summary>
        /// Set the fields on the 'Individual' tab.
        /// </summary>
        /// <param name="fieldValues">Mapping of the field captions to their desired values.  
        /// Empty strings will be considered values and set the field to a blank value.  Null values will be skipped.</param>
        /// <param name="CustomSupportedFields">Mapping of field captions to a CrmField oject encapsulating
        /// all relevant variables.  Field in the default SupportedFields mapping can be overridden by including them in this mapping.</param>
        public static void SetIndividualFields(TableRow fieldValues, IDictionary<string, CrmField> CustomSupportedFields) 
        {
            var dialogId = GetDialogId(DialogIds);
            OpenTab("Individual", dialogId);
            SetFields(dialogId, fieldValues, SupportedFields, CustomSupportedFields);
        }

        /// <summary>
        /// Set the 'Last name' field.
        /// </summary>
        /// <param name="lastName">The desired value of the field.</param>
        public static void SetLastName(string lastName)
        {
            SetTextField(getXInput(GetDialogId(DialogIds), SupportedFields["Last name"].Id), lastName);
        }

        /// <summary>
        /// Save the dialog.
        /// </summary>
        public new static void Save()
        {
            WaitClick(getXEntireDialogButton("Save", GetDialogId(DialogIds)));
            if (GetDialogId(DialogIds) == "individualSpouseBusinessAddForm") GetDisplayedElement(Panel.getXPanelHeader("individual"));
        }
    }
}
