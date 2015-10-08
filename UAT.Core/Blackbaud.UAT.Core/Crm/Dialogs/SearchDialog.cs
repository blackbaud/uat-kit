using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Base class to handle the interactions for search dialogs.  Inherits the Dialog base class.
    /// </summary>
    public class SearchDialog : Dialog
    {
        /// <summary>
        /// Returns an Xpath for the FirstName Field on a SearchDialog.
        /// </summary>
        public const string getXFirstNameField = "//*[contains(@class,'bbui-dialog-search') and not(contains(@style,'hidden'))]//*[starts-with(@id, 'ctrl_') and contains(@id, '_FIRSTNAME_value')]";

        /// <summary>
        /// Returns an Xpath for the Results Tool bar on a SerachDialog.
        /// </summary>
        public const string getXResultsBar = "//*[contains(@class,'bbui-dialog-search') and not(contains(@style,'hidden'))]//*[contains(@class,'x-toolbar-cell')]/div[contains(@class,'xtb-text')]";

        /// <summary>
        /// Returns an Xpath for the LastName Field on a SerachDialog.
        /// </summary>
        public const string getXLastNameField = "//*[contains(@class,'bbui-dialog-search') and not(contains(@style,'hidden'))]//*[starts-with(@id, 'ctrl_') and contains(@id, '_KEYNAME_value')]";

        /// <summary>
        /// Returns an Xpath for the Grid Name Field on a SerachDialog.
        /// </summary>
        public const string getXGridNameField = "//*[contains(@class,'bbui-dialog-search') and not(contains(@style,'hidden'))]//*[contains(@class,'x-grid3-body')]/div/table/tbody/tr/td[3]/div/span";

        /// <summary>
        /// Returns an Xpath for the Search Results Grid on a SerachDialog.
        /// </summary>
        protected static string getXSearchResultsGrid = "//*[contains(@class,'bbui-dialog-search') and not(contains(@style,'hidden'))]//*[contains(@class,'x-grid3-body')]/div";

        /// <summary>
        /// ConstantXPath value for finding a search dialog's selected row in the results pane.
        /// </summary>
        public const string getXSelectedResultsRow = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'ContentsearchResults')]//div[contains(@class,'x-grid3-row-selected')]";

        /// <summary>
        /// Constant XPath value for checking if no search results were found on a search dialog.
        /// </summary>
        public const string getXNoRecordsFound = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'ContentsearchResults')]//div[./text()='No records found']";

        /// <summary>
        /// Constant XPath value for finding a Search button WebElement.
        /// </summary>
        public const string getXSearchButton = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//button[./text() = 'Search']";

        /// <summary>
        /// Constant XPath value for finding a Select button WebElement.
        /// </summary>
        public const string getXSelectButton = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//button[./text() = 'Select']";

        /// <summary>
        /// Press search on the SearchDialog.
        /// </summary>
        public static void Search()
        {            
            //BaseComponent.WaitClick(String.Format(xButton,"Search"));
            // or could do ...
            ClickButton("Search");
        }

        /// <summary>
        /// Click the 'Select' button
        /// </summary>
        public static void Select()
        {
            WaitClick(getXSelectButton);
        }

        /// <summary>
        /// Checks that the search results tool bar message contains the specified string on a SearchDialog.
        /// Throws an exception on failure.
        /// 
        /// </summary>
        /// <param name="expected">The string to check for.</param>
        public static void CheckConstituentSearchResultsToolbarContains(string expected)
        {
            var actualText = string.Empty;
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));           
            waiter.Until(d => 
            {                
                var constituentSearchResultsToolbarElement = Driver.FindElement(By.XPath(getXResultsBar));
                actualText = constituentSearchResultsToolbarElement.Text;

                //Console.WriteLine(actualText);

                return constituentSearchResultsToolbarElement.Displayed &&
                       actualText.Contains(expected);
            });
            // make a more informative error message
            AssertContains(expected, actualText, "Results contain '" + expected + "'");
        }

        private static void AssertContains(string expectedValue, string actualValue, string elementDescription)
        {
            if (!actualValue.Contains(expectedValue))
            {
                throw new Exception(String.Format("AssertContains Failed: '{0}' didn't match expectations. Expected [{1}], Actual [{2}]", elementDescription, expectedValue, actualValue));
            }
        }

        /// <summary>
        /// Sets the value of the First Name search field.
        /// </summary>
        /// <param name="name">The text to set.</param>
        public static void SetFirstNameToSearch(string name)
        {
            SetTextField(getXFirstNameField, name);
        }
        
        /// <summary>
        /// Sets the value of the Last Name search field.
        /// </summary>
        /// <param name="name">The text to set.</param>
        public static void SetLastNameToSearch(string name)
        {
            SetTextField(getXLastNameField, name);
        }

        
        /// <summary>
        /// Select the first result returned.
        /// If no results are returned, Cancel is clicked and an Exception is thrown.
        /// </summary>
        public static void SelectFirstResult()
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            try
            {
                waiter.Until(d =>
                {
                    try
                    {
                        var noResults = d.FindElement(By.XPath(getXNoRecordsFound));
                        if (noResults != null)
                        {
                            WaitClick(getXCancelButton);
                            throw new Exception("No search results found");
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        var result = d.FindElement(By.XPath(getXSelectedResultsRow));
                        if (result != null)
                        {
                            WaitClick(getXSelectButton);
                            return true;
                        }
                    }
                    return false;
                });
            }
            catch (WebDriverTimeoutException)
            {
                throw new WebDriverTimeoutException("Timed out waiting for search results.");
            }
            
        }


        /// <summary>
        /// Checks that the search results contain the specified string on a SearchDialog.
        /// Throws an exception on failure.
        /// 
        /// </summary>
        /// <param name="expected">The string to check for.</param>
        public static void CheckConstituentSearchResultsContain(string expected)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException));
            ICollection<IWebElement> nameFieldElements = new Collection<IWebElement>();
            waiter.Until(d => 
                {
                    var constituentSearchResultsToolbarElement = Driver.FindElement(By.XPath(getXResultsBar));
                    nameFieldElements = Driver.FindElements(By.XPath(getXGridNameField));
                    var constituentSearchResultsGrid = Driver.FindElement(By.XPath(getXSearchResultsGrid));

                    if ((nameFieldElements == null ||
                         !constituentSearchResultsGrid.Displayed ||
                         !constituentSearchResultsToolbarElement.Text.Contains("found"))) return false;                   

                    var names = from element in nameFieldElements select element.Text;

                    if (!names.Contains(expected)) return false;
                    //{                        
                    //    throw new Exception(String.Format("CheckConstituentResultsContain Failed: Expected Name '{0}' was not found in the Results.", expected));
                    //}

                    return true;    
                });

        }
    }
}
