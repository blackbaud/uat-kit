using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using System.Linq;


namespace SystemTest.Common.Bbis
{
    [Binding]
    public class BbisContentGallery : BaseComponent
    {    
        public const string getXPartsSearchResultsLine =
            "//*[@id='pagecntnt_tmpltcntnt_partGrid_grid_grid_lblTotalRecs']";

        public const string getXNewPartButton = "//*[contains(@class,'BBAdminButtonLabel') and ./text()='New part']";

        public static string getXDeleteButton =
            "//*[contains(@class,'GridActionToolbarItemLabel') and ./text()='Delete']";

        public static string getXDeleteElements(string name)
        {
            return
                string.Format(
                    "//tbody[tr[contains(@class,'DataGridItem')]//span[text()='{0}']]//input[contains(@title,'delete')]",
                    name);
        }

        // TODO refactor these two following functions and distill out common code and Xpaths
        // TODO move into CORE

        /// <summary>
        /// Delete the named part from the Content Gallery.
        /// </summary>
        /// <param name="name"></param>
        public static void DeletePart(string name)
        {
            SearchPartsByName(name);

            IList<IWebElement> partGridNameFieldElements = null;
            IWebElement partSearchResultsLineElement;

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

            wait.Until(d =>
            {
                partSearchResultsLineElement = Driver.FindElement(By.XPath(getXPartsSearchResultsLine));
                partGridNameFieldElements =
                    Driver.FindElements(By.XPath("//table[contains(@class,'DataGrid')]//span[./text()='" + name + "']"));

                if (partGridNameFieldElements == null ||
                    partSearchResultsLineElement.Displayed == false ||
                    partSearchResultsLineElement.Text.Contains("No"))
                    return false;

                foreach (var element in partGridNameFieldElements)
                {
                    element.Click();
                    WaitClick(getXDeleteButton);

                    IAlert alert = Driver.SwitchTo().Alert();
                    alert.Accept();
                }

                return true;
            });

            // Need to check enabled first for this one !!
            GetEnabledElement("//div[contains(@class,'ExplorerTabs')]//td[./text()='Folders']");

            WaitClick("//div[contains(@class,'ExplorerTabs')]//td[./text()='Folders']");

            WaitClick("//span[./text()='Deleted Parts']");

            GetDisplayedElement("//div[contains(@class,'ExplorerPath')]//span[./text()='Deleted Parts']");

            wait.Until(d =>
            {
                partSearchResultsLineElement = Driver.FindElement(By.XPath(getXPartsSearchResultsLine));
                partGridNameFieldElements =
                    Driver.FindElements(By.XPath("//table[contains(@class,'DataGrid')]//span[./text()='" + name + "']"));

                if (partGridNameFieldElements == null ||
                    partSearchResultsLineElement.Displayed == false ||
                    partSearchResultsLineElement.Text.Contains("No"))
                    return false;

                foreach (var element in partGridNameFieldElements)
                {
                    element.Click();
                    WaitClick(getXDeleteButton);

                    IAlert alert = Driver.SwitchTo().Alert();
                    alert.Accept();
                }
                return true;
            });
        }

        /// <summary>
        /// Delete all parts with the passed name in their title from the Content Gallery.
        /// c.f. name*
        /// </summary>
        /// <param name="name"></param>
        public static void DeleteAllParts(string name)
        {
            SearchPartsByName(name);

            IWebElement partSearchResultsLineElement;
            IWebElement selectAllCheckBoxElement;

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

            wait.Until(d =>
            {
                partSearchResultsLineElement = Driver.FindElement(By.XPath(getXPartsSearchResultsLine));
                selectAllCheckBoxElement = Driver.FindElement(By.XPath("//tr[contains(@class,'DataGridHeader')]//input[contains(@type,'checkbox')]"));

                if (selectAllCheckBoxElement == null ||
                    partSearchResultsLineElement.Displayed == false ||
                    partSearchResultsLineElement.Text.Contains("No"))
                    return false;

                WaitClick("//tr[contains(@class,'DataGridHeader')]//input[contains(@type,'checkbox')]");
                WaitClick(getXDeleteButton);

                var innerwaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                innerwaiter.IgnoreExceptionTypes(typeof(NoAlertPresentException));
                innerwaiter.Until(dd =>
                {
                    IAlert alert = Driver.SwitchTo().Alert();
                    alert.Accept();
                    return true;
                });

                return true;
            });

            // Need to check enabled first for this one !!
            GetEnabledElement("//div[contains(@class,'ExplorerTabs')]//td[./text()='Folders']");

            WaitClick("//div[contains(@class,'ExplorerTabs')]//td[./text()='Folders']");

            WaitClick("//span[./text()='Deleted Parts']");

            GetDisplayedElement("//div[contains(@class,'ExplorerPath')]//span[./text()='Deleted Parts']");

            wait.Until(d =>
            {
                partSearchResultsLineElement = Driver.FindElement(By.XPath(getXPartsSearchResultsLine));
                selectAllCheckBoxElement = Driver.FindElement(By.XPath("//tr[contains(@class,'DataGridHeader')]//input[contains(@type,'checkbox')]"));

                if (selectAllCheckBoxElement == null ||
                    partSearchResultsLineElement.Displayed == false ||
                    partSearchResultsLineElement.Text.Contains("No"))
                    return false;

                WaitClick("//tr[contains(@class,'DataGridHeader')]//input[contains(@type,'checkbox')]");
                WaitClick(getXDeleteButton);

                var innerwaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                innerwaiter.IgnoreExceptionTypes(typeof(NoAlertPresentException));
                innerwaiter.Until(dd =>
                {
                    IAlert alert = Driver.SwitchTo().Alert();
                    alert.Accept();
                    return true;
                });

                return true;
            });
        }

        // TODO - move Search into a Generic Gallery Parent Class (and others that use this standard layout)
        public static void SearchPartsByName(string name)
        {
            BBISHomePageCustom.WaitClick("//div[contains(@class,'ExplorerTabs')]//td[./text()='Search']", 15);

            const string getXfilterByNameField = "//input[contains(@id,'_txtName')]";

            BBISHomePageCustom.SetTextField(getXfilterByNameField, name);

            WaitClick("//input[contains(@class,'SearchButton')]");

            GetDisplayedElement("//div[contains(@class,'ExplorerPath')]//span[text()[contains(.,'Search Results:')]]");
        }

        /// <summary>
        /// Creates a new User Login part with default mandatory values and table of optional field values.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fields"></param>
        public static void NewUserLoginPart(string name, List<dynamic> fields)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            string handle = Driver.CurrentWindowHandle;

            WaitClick(getXNewPartButton);
            waiter.Until(d => d.WindowHandles.Count == 2);
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            WaitClick("//*[contains(@class,'PartDropDownHit')]");
            WaitClick("//a[contains(@id,'_ContentTypeSelector') and ./text()='User Login']");
            BBISHomePageCustom.SetTextField("//*[contains(@class,'FieldPartName')]", name);
            WaitClick("//*[contains(@class,'BBAdminButtonLabel') and ./text()='Next']");

            WaitClick("//span[contains(@class,'BBAdminButtonLabel') and ./text()='Forgotten Password/Username Email']");
            waiter.Until(d => d.WindowHandles.Count == 3);
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            BBISHomePageCustom.SetTextField("//input[contains(@id,'FromAddress')]", "gherkin@blackbaud.com");
            BBISHomePageCustom.SetTextField("//input[contains(@id,'DisplayName')]", "gherkin");

            WaitClick("//*[contains(@class,'BBAdminButtonLabel') and ./text()='Save']");
            waiter.Until(d => d.WindowHandles.Count == 2);
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());

            WaitClick("//span[contains(@class,'BBAdminButtonLabel') and ./text()='New User Registration Email']");
            waiter.Until(d => d.WindowHandles.Count == 3);
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            BBISHomePageCustom.SetTextField("//input[contains(@id,'FromAddress')]", "gherkin@blackbaud.com");
            BBISHomePageCustom.SetTextField("//input[contains(@id,'DisplayName')]", "gherkin");

            WaitClick("//*[contains(@class,'BBAdminButtonLabel') and ./text()='Save']");
            waiter.Until(d => d.WindowHandles.Count == 2);
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());

            foreach (var f in fields)
            {
                WaitClick("//tr[td[text()='" + f.AdditionalField + "']]//input[contains(@type,'checkbox') and contains(@id,'chkFieldInclude')]");
            }

            WaitClick("//*[contains(@class,'BBAdminButtonLabel') and ./text()='Save']");
            Driver.SwitchTo().Window(handle);
        }

    }
}