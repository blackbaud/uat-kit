using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Blackbaud.UAT.Core.Crm.Panels;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Base class to handle interactions for the 'Business Process' panel.
    /// </summary>
    public class BusinessProcess : Panel
    {
        /// <summary>
        /// Get a formatted xPath for an element whose text represents the commit status of the business process.
        /// </summary>
        public static string getXCommitStatus =
            getXSpan("BusinessProcessParameterSetRecentStatusViewForm", "_STATUS_value");

        /// <summary>
        /// Get a formatted xPath for an element whose test represents the number of records processed for the business process.
        /// </summary>
        public static string getXRecordsProcessed = 
            getXSpan("BusinessProcessParameterSetRecentStatusViewForm", "_TOTALNUMBERPROCESSED_value");

        /// <summary>
        /// Wait for the business process to complete and check if the status finished as 'Completed'.
        /// </summary>
        /// <returns>True if the business process completes without errors or exceptions, false otherwise.</returns>
        public static bool IsCompleted()
        {
            return IsCompleted(120);
        }

        /// <summary>
        /// Wait for the business process to complete and check if the status finished as 'Completed'.
        /// </summary>
        /// <returns>True if the business process completes without errors or exceptions, false otherwise.</returns>
        /// <param name="timeout">Time to wait for operation, default is 120 seconds.</param>
        public static bool IsCompleted(int timeout)
        {
            WebDriverWait waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            try
            {
                waiter.Until(d =>
                {
                    IWebElement statusElement = GetDisplayedElement(getXCommitStatus, timeout);
                    if (statusElement == null || statusElement.Text != "Completed") return false;
                    return true;
                });
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Check for the total number of records processed.
        /// </summary>
        /// <param name="numRecords">The expected number of total records processed by the business process.</param>
        /// <returns>True if the posted number of total processed records equals the provided expected amount, false otherwise.</returns>
        public static bool IsNumRecordsProcessed(int numRecords)
        {
            try
            {
                ElementValueIsSet(getXRecordsProcessed, numRecords.ToString());
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
                
            }
        }
    }
}
