using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for adding/editing a designations dialog.
    /// </summary>
    public class DesignationsDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "DesignationSplits", "BatchRevenueSplitDesignations" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Designation", new CrmField(null, FieldType.Searchlist, "DesignationSearch", "COMBINEDSEARCH")},
            {"Amount", new CrmField(null, FieldType.TextInput)}
        };

        /// <summary>
        /// Set the designations grid rows starting with the first row and distribute the amount evenly.
        /// </summary>
        /// <param name="designations">Table where each TableRow corresponds to a grid row's values.
        /// A TableRow is a mapping of the column captions to a single row's desired values</param>
        public static void SplitDesignationsEvenly(Table designations)
        {
            SplitDesignations(designations, true);
        }

        /// <summary>
        /// Set the designations grid rows starting with the first row and distribute the amount evenly
        /// if specified.
        /// </summary>
        /// <param name="designations">Table where each TableRow corresponds to a grid row's values.
        /// A TableRow is a mapping of the column captions to a single row's desired values</param>
        /// <param name="evenly">Flag indicating whether to split the designations evenly</param>
        public static void SplitDesignations(Table designations, bool evenly)
        {
            var dialogId = GetDialogId(DialogIds);
            var columnCaptionToIndex = MapColumnCaptionsToIndex(designations.Rows[0].Keys,
                getXGridHeaders(dialogId, "_SPLITS_value"));

            var resetRecognition = false;
            var rowIndex = 1;
            foreach (var designation in designations.Rows)
            {
                if (dialogId == "BatchRevenueSplitDesignations" && designation.ContainsKey("Amount") && designation["Amount"] != string.Empty)
                {
                    SetGridTextField(getXGridCell(dialogId, "_SPLITS_value", rowIndex,
                        columnCaptionToIndex["Amount"]), designation["Amount"]);
                    if (!resetRecognition)
                    {
                        Yes();
                        resetRecognition = true;
                    }
                    designation["Amount"] = null;
                }
                SetGridRow(dialogId, "_SPLITS_value", designation, rowIndex, columnCaptionToIndex, SupportedFields);
                rowIndex++;
            }

            if (evenly) ClickButton("Distribute evenly");
            if (dialogId == "BatchRevenueSplitDesignations" && !resetRecognition) Yes();
        }
    }
}
