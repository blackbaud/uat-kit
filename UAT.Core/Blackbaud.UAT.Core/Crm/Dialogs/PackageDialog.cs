using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing a package dialog.
    /// </summary>
    public class PackageDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "PackageAddFormMailChannel", "PackageAddFormPublicMedia"};

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
            {"Description", new CrmField("_DESCRIPTION_value", FieldType.TextArea)},
            {"Site", new CrmField("_SITEID_value", FieldType.Searchlist, "SiteSearch", "_SITEID_value")},
            {"Category", new CrmField("_CATEGORYCODEID_value", FieldType.Dropdown)},
            {"Package code", new CrmField("_CODEVALUEID_value", FieldType.Dropdown)},
            {"Channel code", new CrmField("_CHANNELSOURCECODEVALUEID_value", FieldType.Dropdown)},
            {"Base cost", new CrmField("_COST_value", FieldType.TextInput)},
            {"Distribution", new CrmField("_COSTDISTRIBUTIONMETHODCODE_value", FieldType.Dropdown)},
            {"Letter", new CrmField("_LETTERCODEID_value", FieldType.Searchlist, "LetterSearch", "_NAME_value")},
            {"Export definition", new CrmField("_EXPORTDEFINITIONID_value", FieldType.Searchlist, "ExportDefinitionSearch2", "_NAME_value")},
        };

        /// <summary>
        /// Set the field values.
        /// </summary>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        public static void SetFields(TableRow fields)
        {
            var dialogId = GetDialogId(DialogIds);
            SetFields(dialogId, fields, SupportedFields);
        }
    }
}
