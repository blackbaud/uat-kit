using System;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Query' panel.
    /// </summary>
    public class QueryPanel : Panel
    {
        /// <summary>
        /// Returns an Xpath for the Add button on a QueryPanel.
        /// </summary>
        public const string getXAddButton = "//*[./text()=\"Add\" and @class=\" x-btn-text\"]";

        /// <summary>
        /// Returns an Xpath for the Adhoc Query menu item on a QueryPanel.
        /// </summary>
        public const string getXAdhocQuery = "//*[./text()=\"Ad-hoc query\" and contains(@class,\"x-menu-item-text\")]";

        /// <summary>
        /// Returns an Xpath for the Query Filter field on a QueryPanel.
        /// </summary>
        public const string getXFilterQueryField = "//*[td/label[./text()=\"Name:\"]]/td/input[contains(@class,\"x-form-text\")]";

        /// <summary>
        /// Set filter based on string passed in
        /// </summary>
        /// <param name="name">Filter to set</param>
        public static void SetFilter(string name)
        {
            SetTextField(getXFilterQueryField, name);
        }

        /// <summary>
        /// Returns an XPath for the Delete Query menu item on a QueryPanel.  
        /// 
        /// </summary>
        /// <param name="text">The text of the Query to delete</param>
        /// <returns></returns>
        public static string getXDeleteQuery(string text) { return String.Format("//tbody[//tr/td/a[text()=\"{0}\" and contains(@class,\"bbui-pages-datalistgrid-rowlink\")]]//td[contains(@class,\"x-toolbar-cell\") and not(contains(@class,\"x-hide-display\"))]//*[./text()=\"Delete\"]",text);}

        /// <summary>
        /// Returns an Xpath for a Query in a results row on a QueryPanel.
        /// 
        /// </summary>
        /// <param name="text">The text of the Query.</param>
        /// <returns></returns>
        //public static string getXQuery(string text) { return String.Format("//*[text()=\"{0}\" and contains(@class,\"bbui-pages-datalistgrid-rowlink\")]", text); }
        public static string getXQuery(string text) { return String.Format("//*[./text()=\"{0}\" and contains(@class,\"bbui-datalist-search-highlight\")]", text); }

        /// <summary>
        /// IWebElement representing the Query link element
        /// </summary>
        public static IWebElement QueryLinkElement;

        /// <summary>
        /// IWebElement representing the Delete Query link element
        /// </summary>
        public static IWebElement DeleteQueryLinkElement;

        /// <summary>
        /// IWebElement representing the Yes Delete Element
        /// </summary>
        public static IWebElement YesReallyDeleteElement;
        
        /// <summary>
        /// Delete Query for the input string
        /// </summary>
        /// <param name="name">The query to delete</param>
        public static void DeleteQuery(string name)
        {
            DeleteQuery(getXQuery(name), getXDeleteQuery(name));
        }

        /// <summary>
        /// Delete query
        /// </summary>
        /// <param name="xQuery">?</param>
        /// <param name="xDeleteQuery">?</param>
        public static void DeleteQuery(string xQuery,string xDeleteQuery)
        {

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException));
            Actions clicker = new Actions(Driver); 
            try
            {
                wait.Until(d =>
                {
                    QueryLinkElement = d.FindElement(By.XPath(xQuery));

                    if (QueryLinkElement == null ||
                        QueryLinkElement.Displayed == false ||
                        QueryLinkElement.Enabled == false) return false;                  
                    return true;
                });
            }
            catch (Exception)
            {
                throw new NoSuchElementException();
            }

            wait.Until(d =>
            {
                clicker.ContextClick(QueryLinkElement).Perform(); 
                
                DeleteQueryLinkElement = d.FindElement(By.XPath(xDeleteQuery));
                
                if (DeleteQueryLinkElement == null ||
                    DeleteQueryLinkElement.Displayed == false ||
                    DeleteQueryLinkElement.Enabled == false) return false;
                DeleteQueryLinkElement.Click();
                return true;
            });

            wait.Until(d =>
            {
                YesReallyDeleteElement = d.FindElement(By.XPath(getXButton("Yes")));

                if (YesReallyDeleteElement == null ||
                    YesReallyDeleteElement.Displayed == false) return false;
                YesReallyDeleteElement.Click();
                return true;
            });
        }

        /// <summary>
        /// Check query is present by name
        /// </summary>
        /// <param name="name">Query name</param>
        public static void CheckQueryPresent(string name)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            wait.Until(d =>
            {
                QueryLinkElement = d.FindElement(By.XPath(getXQuery(name)));

                if (QueryLinkElement == null ||
                    QueryLinkElement.Displayed == false ||
                    QueryLinkElement.Text != name) return false;
                return true;
            });
        }

        /// <summary>
        /// Add a new Query
        /// </summary>
        public static void AddQuery()
        {
            WaitClick(getXAddButton);
            WaitClick(getXAdhocQuery);
        }

        /// <summary>
        /// Open the queries tab
        /// </summary>
        public static void OpenQueriesTab()
        {
            OpenTab("Queries");
        }

        /// <summary>
        /// Open the selections tab
        /// </summary>
        public static void OpenSelectionsTab()
        {
            OpenTab("Selections");
        }

    }
}
