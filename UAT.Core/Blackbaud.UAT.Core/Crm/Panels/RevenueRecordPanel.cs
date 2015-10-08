using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Revenue Record' panel.
    /// </summary>
    public class RevenueRecordPanel : Panel
    {

        /// <summary>
        /// Check the subtype of the revenue record.
        /// </summary>
        /// <param name="subtype">The subtype to check for.</param>
        /// <returns>True if the subtype is the expected value, false otherwise.</returns>
        public static bool IsSubtype(string subtype)
        {
            SelectTab("Details");
            try
            {
                ElementValueIsSet(getXSpan("PledgeViewForm", "_PLEDGESUBTYPE_value"), subtype);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }

        }

        /// <summary>
        /// Check if a payment exists on the revenue record.
        /// </summary>
        /// <param name="donation">Mapping of the donation view form's captions (minus the colon) to a single donation's values.</param>
        /// <returns>True if the donation exists, false otherwise.</returns>
        public static bool PaymentExists(TableRow donation)
        {
            SelectTab("Details");
            return SectionViewFormExists(donation, "Application details", "RevenueTransactionDetailViewForm");
        }

        /// <summary>
        /// Check if a payment is marked as 'Receipted'
        /// </summary>
        /// <returns>True if the payment is 'Receipted', false otherwise.</returns>
        public static bool IsReceipted()
        {
            return GetDisplayedElement(getXSpan("RevenueTransactionProfileViewForm", "_RECEIPTSTATUS_value")).Text ==
                   "Receipted";
        }

        /// <summary>
        /// Check if a payment is marked as 'Acknowledged'
        /// </summary>
        /// <returns>True if the payment is 'Acknowledged', false otherwise.</returns>
        public static bool IsAcknowledged()
        {
            return GetDisplayedElement(getXSpan("RevenueTransactionProfileViewForm", "_PAYMENTACKNOWLEDGEMENTSTATUS_value")).Text ==
                   "Acknowledged";
        }
    }
}
