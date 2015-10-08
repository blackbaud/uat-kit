using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing an enhanced revenue batch dialog.
    /// </summary>
    public class EnhancedRevenueBatchDialog : BatchDialog
    {
        /// <summary>
        /// Click the 'Update projected totals' and click Ok on the confirmation button.
        /// </summary>
        public static void UpdateProjectedTotals()
        {
            OpenTab("Main");
            ClickButton("Update projected totals", null);
            OK();
        }

        /// <summary>
        /// Click the 'Split designations' button and set the desgination grid's rows.  This will
        /// split the designation on the currently selected grid row.  The amount between the designations
        /// will be split evenly.
        /// </summary>
        /// <param name="designations">Table where each row represents a designation to add.
        /// Each row is a mapping of the column captions to the single row's desired values.</param>
        /// <param name="evenly">Flag indicating whether or not to split the designations evenly.</param>
        public static void SplitDesignations(Table designations, bool evenly)
        {
            OpenTab("Revenue");
            ClickButton("Split designations", "BatchRevenueSplitDesignations");

            DesignationsDialog.SplitDesignations(designations, evenly);
            OK();
        }

        /// <summary>
        /// Click 'Apply' under the Revenue tab and set the designations in the 'Apply to commitments' dialog.
        /// </summary>
        /// <param name="applications">Table where each row represents an application to add.
        /// A row is a mapping of the column captions to a single row's desired values.</param>
        public static void Apply(Table applications)
        {
            OpenTab("Revenue");
            ClickButton("Apply", "BatchRevenueApplyCommitmentsCustom");
            CommitmentsDialog.SetAdditionalApplicationsGridRows(applications);
            OK();
        }
    }
}
