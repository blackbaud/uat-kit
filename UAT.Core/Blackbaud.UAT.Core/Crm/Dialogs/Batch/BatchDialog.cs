using System;
using System.Collections.Generic;
using System.Linq;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Panels;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Base class to handle the interactions for adding/editing a batch dialog.
    /// </summary>
    abstract public class BatchDialog : Dialog
    {
        /// <summary>
        /// BatchDialogId constant
        /// </summary>
        protected const string BatchDialogId = "_Batch";
        /// <summary>
        /// BatchDialogGridId constant
        /// </summary>
        protected const string BatchDialogGridId = "_BatchForm";

        /// <summary>
        /// Unique XPath to get the header columns TR element of a batch's grid.
        /// </summary>
        public const string getXBatchGridHeaders = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'_Batch')]//div[contains(@id, '_BatchForm')]//div[contains(@class,'bbui-forms-collectiongrid') and not(contains(@class,'readonly'))]//div[@class='x-grid3-header']//tr";

        /// <summary>
        /// Get the index of the column in the batch grid.
        /// </summary>
        /// <param name="columnId">The unique id of the column.</param>
        /// <returns>The index of the TD column element within the TR headers.</returns>
        public static int GetColumnIndexById(string columnId)
        {
            int columnIndex = -1;

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            waiter.Until(d =>
            {
                IWebElement headersRow = GetDisplayedElement(getXBatchGridHeaders, TimeoutSecs);
                if (headersRow.Displayed != true) return false;
                IList<IWebElement> columns = headersRow.FindElements(By.XPath(".//td")).ToList();

                foreach (IWebElement column in columns)
                {
                    //columns may not be displayed.  do not use displayed as a conditional check
                    if (column.GetAttribute("class").Contains(columnId))
                    {
                        columnIndex = columns.IndexOf(column) + 1;
                        return true;
                    }
                }
                return false;
            });

            if (columnIndex == -1) throw new NoSuchElementException(String.Format("No column found with the id '{0}'", columnId));
            return columnIndex;
        }

        /// <summary>
        /// Click the 'Validate' button and click Ok on the confirmation button.
        /// </summary>
        public static void Validate()
        {
            OpenTab("Main");
            ClickButton("Validate", null);
            GetDisplayedElement("//div[contains(@class,'bbui-dialog-msgbox') and contains(@style,'visible')]//span[contains(text(),'Batch')]", 240);
            OK();
        }

        /// <summary>
        /// Click the 'Save and close' button.
        /// </summary>
        public static void SaveAndClose()
        {
            try
            {
                BaseComponent.GetEnabledElement("//span[./text()='Main']", 3);
                OpenTab("Main");
            }
            catch
            { }
            ClickButton("Save and close");
            GetDisplayedElement(Panel.getXPanelHeader("fa_batch"));
        }
    }

}
