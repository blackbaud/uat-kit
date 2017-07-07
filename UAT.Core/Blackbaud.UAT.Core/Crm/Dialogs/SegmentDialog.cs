using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for adding/editing a segment dialog.
    /// </summary>
    public class SegmentDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "ConstituentSegmentAddForm", "ListSegmentAddForm", "RevenueSegmentAddForm", 
                                                         "MembershipSegmentAddForm", "SponsorshipSegmentAddForm" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
            {"Description", new CrmField("_DESCRIPTION_value", FieldType.TextArea)},
            {"Site", new CrmField("_SITEID_value", FieldType.Searchlist, "SiteSearch", "_SITEID_value")},
            {"Category", new CrmField("_SEGMENTCATEGORYCODEID_value", FieldType.Dropdown)},
            {"Code", new CrmField("_CODEVALUEID_value", FieldType.Dropdown)},
            {"CODE", new CrmField("_CODE_value", FieldType.TextInput)},
        };

        /// <summary>
        /// Set the fields on the 'Details' tab.
        /// </summary>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        public static void SetDetailsFields(TableRow fields)
        {
            var dialogId = GetDialogId(DialogIds);
            OpenTab("Details", dialogId);
            SetFields(dialogId, fields, SupportedFields);
        }

        /// <summary>
        /// Add a marketing selection to the segment.
        /// </summary>
        /// <param name="selection">The selection to add.</param>
        public static void AddSelection(string selection)
        {
            var dialogId = GetDialogId(DialogIds);
            OpenTab("Details", dialogId);
            WaitClick(getXDialogButton("Add", dialogId));
            SetTextField(getXInput("MarketingSelectionSearch", "_NAME_value"), selection);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }
    }
}
