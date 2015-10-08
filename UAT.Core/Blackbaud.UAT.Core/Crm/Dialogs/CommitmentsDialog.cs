using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing a commitments dialog.
    /// </summary>
    public class CommitmentsDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "BatchRevenueApplyCommitmentsCustom" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedAddtionalApplicationsGridFields = new Dictionary<string, CrmField>
        {
            {"Additional applications", new CrmField(null, FieldType.Dropdown)},
            {"Applied amount", new CrmField(null, FieldType.TextInput)},
            {"Designation", new CrmField(null, FieldType.Searchlist, "DesignationSearch", "COMBINEDSEARCH")},
            {"Opportunity", new CrmField(null, FieldType.Searchlist, "OpportunitySearch", "KEYNAME")},
            {"Revenue category", new CrmField(null, FieldType.Dropdown)},
        };

        /// <summary>
        /// Set the grid rows for the Additional applications grid starting with the first row.
        /// </summary>
        /// <param name="applications">Table where each TableRow corresponds to a grid row's values.
        /// A TableRow is a mapping of the column captions to a single row's desired values</param>
        public static void SetAdditionalApplicationsGridRows(Table applications)
        {
            var dialogId = GetDialogId(DialogIds);

            var columnCaptionToIndex = MapColumnCaptionsToIndex(applications.Rows[0].Keys,
                getXGridHeaders(dialogId, "_ADDITIONALAPPLICATIONSSTREAM_value"));

            SetGridRows(dialogId, "", applications, 1, columnCaptionToIndex, SupportedAddtionalApplicationsGridFields);
        }
    }
}
