using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Pledge' panel.
    /// </summary>
    public class PledgePanel : Panel
    {
        /// <summary>
        /// Check if the pledge is associated with the provided constituent.
        /// Verifies by checking which constituent the header link navigates to.
        /// </summary>
        /// <param name="constituent">The last name of the constituent.</param>
        /// <returns>True if the pledge is associated with the provided constituent, false otherwise.</returns>
        public static bool IsPledgeConstituent(string constituent)
        {
            GetDisplayedElement(getXPanelHeader("fa_revenue"));
            return Exists(getXPanelHeaderLink(constituent));
        }

        /// <summary>
        /// Check if the pledge is for the provided amount.
        /// </summary>
        /// <param name="amount">The amount to check for.</param>
        /// <returns>True if the pledge is for the provided amount, false otherwise.</returns>
        public static bool IsPledgeAmount(string amount)
        {
            return GetDisplayedElement(getXSpan("RevenueTransactionProfileViewForm", "_AMOUNT_value")).Text.Contains(amount);
        }

        /// <summary>
        /// Check if the pledge has the provided subtype.
        /// </summary>
        /// <param name="subtype">The subtype to check for.</param>
        /// <returns>True if the pledge has the subtype, false otherwise.</returns>
        public static bool IsPledgeSubtype(string subtype)
        {
            OpenTab("Details");
            return GetDisplayedElement(getXSpan("PledgeViewForm", "_PLEDGESUBTYPE_value")).Text == subtype;
        }

        /// <summary>
        /// Check if a designation exists.
        /// </summary>
        /// <param name="designation">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the designation exists, false otherwise.</returns>
        public static bool DesignationExists(TableRow designation)
        {
            SelectTab("Details");

            return SectionDatalistRowExists(designation, "Designations");
        }
    }
}
