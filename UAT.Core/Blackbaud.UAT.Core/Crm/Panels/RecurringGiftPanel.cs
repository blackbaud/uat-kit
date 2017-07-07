using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Recurring Gift' panel.
    /// </summary>
    public class RecurringGiftPanel : Panel
    {
        /// <summary>
        /// Check if a designation exists for the recurring gift.
        /// </summary>
        /// <param name="designation">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the designation exists, false otherwise.</returns>
        public static bool DesignationExists(TableRow designation)
        {
            return GridRowExists(designation, "_DESIGNATIONS_value");
        }
    }
}
