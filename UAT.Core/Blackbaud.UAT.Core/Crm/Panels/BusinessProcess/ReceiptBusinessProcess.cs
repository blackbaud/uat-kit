using System.Collections.Generic;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Receipt Business Process' panel.
    /// </summary>
    public class ReceiptBusinessProcess : BusinessProcess
    {
        /// <summary>
        /// Wait for the the business process to finish and validate if it finished with a "Completed" status.
        /// </summary>
        /// <returns>True if the process completed without errors or exceptions, false otherwise.</returns>
        public new static bool IsCompleted()
        {
            SelectTab("Recent status");

            return SectionViewFormExists(new Dictionary<string, string>() { { "Status", "Completed" } }, "Recent status",
                "ReceiptingProcessRecentStatusViewForm");

            /*
             * Could also do...
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout_secs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            try
            {
                waiter.Until(d =>
                {
                    IWebElement statusElement = GetDisplayedElement(getXSpan("ReceiptingProcessRecentStatusViewForm", "_STATUS_value"));
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
