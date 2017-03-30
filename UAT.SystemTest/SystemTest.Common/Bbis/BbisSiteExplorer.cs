using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
//using Oxford.UAT.Crm;
using TechTalk.SpecFlow;
using System.Configuration;
using System.Globalization;
using System.Threading;
using TechTalk.SpecFlow.Assist;
using System.Linq;

namespace SystemTest.Common.Bbis
{
    [Binding]
    public class BbisSiteExplorer : BaseComponent
    {
        public const string getXPagesSearchResultsLine =
            "//*[@id='pagecntnt_tmpltcntnt_pageGrid_grid_grid_lblTotalRecs']";

        public static string getXDeleteButton =
            "//*[contains(@class,'GridActionToolbarItemLabel') and ./text()='Delete']";

        public static string getXNewPageButton =
            "//*[contains(@class,'BBAdminButtonLabel') and ./text()='New page']";

        public static string getXDeleteElements(string name)
        {
            return
                string.Format(
                    "//tbody[tr[contains(@class,'DataGridItem')]//span[text()='{0}']]//input[contains(@title,'delete')]",
                    name);
        }

        public static void DeleteAllPages(string name)
        {
            SearchPagesByName(name);

            IWebElement partSearchResultsLineElement;
            IWebElement selectAllCheckBoxElement;

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

            try
            {
                wait.Until(d =>
                {
                    partSearchResultsLineElement = Driver.FindElement(By.XPath(getXPagesSearchResultsLine));
                    selectAllCheckBoxElement =
                        Driver.FindElement(
                            By.XPath("//tr[contains(@class,'DataGridHeader')]//input[contains(@type,'checkbox')]"));

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
            catch (WebDriverException)
            {
            }

            // Need to check enabled first for this one !!
            GetEnabledElement("//div[contains(@class,'ExplorerTabs')]//td[./text()='Folders']");

            WaitClick("//div[contains(@class,'ExplorerTabs')]//td[./text()='Folders']");

            WaitClick("//span[./text()='Deleted Pages/Templates']");

            GetDisplayedElement("//div[contains(@class,'ExplorerPath')]//span[./text()='Deleted Pages/Templates']");

            wait.Until(d =>
            {
                partSearchResultsLineElement = Driver.FindElement(By.XPath(getXPagesSearchResultsLine));
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

        public static void SearchPagesByName(string name)
        {
            WaitClick("//div[contains(@class,'ExplorerTabs')]//td[./text()='Search']");

            const string getXfilterByNameField = "//input[contains(@id,'_txtSearch')]";

            BBISHomePageCustom.SetTextField(getXfilterByNameField, name);

            WaitClick("//input[contains(@id,'btnSearch')]");

            GetDisplayedElement("//div[contains(@class,'ExplorerPath')]//span[text()[contains(.,'Search Results:')]]");
        }

        public static void NewPage(string name)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            string handle = Driver.CurrentWindowHandle;
            string template = "QA template";

            WaitClick(getXNewPageButton);
            waiter.Until(d => d.WindowHandles.Count == 2);
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            BBISHomePageCustom.SetTextField("//input[contains(@id,'_PageName')]", name);

            WaitClick(string.Format("//option[text()='{0}']", template));

            WaitClick("//*[contains(@class,'BBAdminButtonLabel') and ./text()='Next']");
            Driver.SwitchTo().Window(handle);
        }

    }
}