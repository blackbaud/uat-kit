using System.Collections.Generic;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Marketing Acknowledgement Business Process' panel.
    /// </summary>
    public class MarketingAcknowledgementBusinessProcess : BusinessProcess
    {
        /// <summary>
        /// Check for the number of records successfully processed.
        /// </summary>
        /// <param name="numRecords">The number of records to check for.</param>
        /// <returns>True if the provided number matches the listed amount of successfully processed records, false otherwise.</returns>
        public new static bool IsNumRecordsProcessed(int numRecords)
        {
            GetDisplayedElement(getXPanelHeaderByText("Marketing Acknowledgement Process Status"));
            SelectTab("Recent Status");

            return SectionViewFormExists(new Dictionary<string, string>() { { "Records successfully processed", numRecords.ToString() } }, 
                "Recent status", "MarketingAcknowledgementProcessRecentStatusViewForm");
        }

        /// <summary>
        /// Wait for the the business process to finish and validate if it finished with a "Completed" status.
        /// </summary>
        /// <returns>True if the process completed without errors or exceptions, false otherwise.</returns>
        public new static bool IsCompleted()
        {
            GetDisplayedElement(getXPanelHeaderByText("Marketing Acknowledgement Process Status"));
            SelectTab("Recent Status");

            return SectionViewFormExists(new Dictionary<string, string>() { { "Status", "Completed" } }, "Recent status",
                "MarketingAcknowledgementProcessRecentStatusViewForm");

            /*
             * Could also do...
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout_secs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            try
            {
                waiter.Until(d =>
                {
                    IWebElement statusElement = GetDisplayedElement(getXSpan("MarketingAcknowledgementProcessRecentStatusViewForm", "_STATUS_value"));
                    if (statusElement == null || statusElement.Text != "Completed") return false;
                    return true;
                });
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
             */
        }
    }
}
