using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing a marketing acknowledgement template rule dialog.
    /// </summary>
    public class MarketingAcknowledgementRuleDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "MarketingAcknowledgementTemplateRuleAddForm", 
                                                         "MarketingAcknowledgementTemplateRuleEditForm3" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Segment", new CrmField("_SEGMENTID_value", FieldType.Searchlist, "SegmentSearch", "_NAME_value")},
            {"Package", new CrmField("_PACKAGEID_value", FieldType.Searchlist, "PackageSearch", "_NAME_value")}
        };

        /// <summary>
        /// Set the fields on the 'Details' tab of the dialog.
        /// </summary>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        public static void SetDetailsTabFields(TableRow fields)
        {
            var dialogId = GetDialogId(DialogIds);
            OpenTab("Details", dialogId);
            SetFields(dialogId, fields, SupportedFields);
        }

        /// <summary>
        /// Save the dialog and click 'Yes' on the subsequent confirmation dialogs.
        /// </summary>
        /// <param name="numOfApprovedChanges">The number of expected confirmation dialogs to accept.</param>
        public static void Save(int numOfApprovedChanges)
        {
            Save();
            for (var i = 0; i < numOfApprovedChanges; i++)
            {
                Yes();
            }
        }
    }
}
