using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using System.Configuration;
using System.Globalization;
using System.Threading;
using Blackbaud.UAT.Core.Crm.Dialogs;
using Blackbaud.UAT.Core.Crm.Panels;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace SystemTest.Common
{
    public class StepHelper : BaseSteps
    {
        private struct LowerCase
        {
            public const string Today = "today";
            public const string Day = " day";
            public const string Days = " days";
            public const string Week = "week";
            public const string Weeks = "weeks";
            public const string Month = "month";
            public const string Months = "months";
            public const string Year = "year";
            public const string Years = "years";
        }
        private const string plus = "+";
        private const string minus = "-";
        private const string startProcessSelectionException = "StartProcess selection exception!";

        private enum TimePeriod
        {
            Day = 0,
            Week = 1,
            Month = 2,
            Year = 3,
            NotSet = 99
        }

        public static int FindRowIndex(TableRow row, string dialogId, string gridId, string uniqueHeader)
        {
            // preprocessing
            int rowIndex = 1;
            string xPath = XpathHelper.xPath.VisibleDialog + $"//div[contains(@id,'{dialogId}')]//div[contains(@id, '{gridId}')]//div[@class='x-grid3-body']/div";
            // wait
            Dialog.GetDisplayedElement(xPath);
            int rowCount = BaseComponent.Driver.FindElements(By.XPath(xPath)).Count();
            // iterate through the rows until you find the row with the matching value in the identifying header
            while (rowIndex <= rowCount && !String.Equals($"{row[uniqueHeader]}", Dialog.GetEnabledElement(Dialog.getXGridCell(dialogId, gridId, rowIndex, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), uniqueHeader))).Text.ToString()))
            {
                rowIndex++;
            }

            return rowIndex;
        }

        public static void SearchAndSelectConstituent(string constituentName)
        {
            bool splitName = false;
            if (constituentName.IndexOf(" ") > 0)
            {
                splitName = true;
            }
            SearchAndSelectConstituent(constituentName, splitName);
        }

        public static void SearchAndSelectConstituent(string constituentName, bool splitName)
        {
            BaseComponent.GetEnabledElement("//button[./text()='Constituents']");
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.OpenConstituentSearchDialog();

            if (splitName)
            {
                var names = new string[2];
                names = constituentName.Split(' ');
                SearchDialog.SetFirstNameToSearch(names[0]);
                SearchDialog.SetLastNameToSearch(names[1] + uniqueStamp);
            }
            else
            {
                SearchDialog.SetLastNameToSearch(constituentName + uniqueStamp);
            }
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }

        public static void SearchAndSelectConstituentNotUnique(string constituentName, bool splitName)
        {
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.OpenConstituentSearchDialog();

            if (splitName)
            {
                var names = new string[2];
                names = constituentName.Split(' ');
                SearchDialog.SetFirstNameToSearch(names[0]);
                SearchDialog.SetLastNameToSearch(names[1]);
            }
            else
            {
                SearchDialog.SetLastNameToSearch(constituentName);
            }
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }

        public static void SelectPageAndExecuteAction(Action action, string ExceptionMessage = null, bool tryLastPageFirst = false)
        {
            try
            {
                // check the page has rendered find the last > in the page control at the bottom of the page
                // this does not appear if the page count is == 1
                Panel.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//div[contains(@class,'x-panel-bbar')]//button[text()='>']", 20);
            }
            catch
            {
                // let's fail fast and move on
            }

            // get actual number of pages
            int actualNumberOfPages = SetActualNumberOfPages();

            if (tryLastPageFirst)
            {
                // let's try the last page first and revert to looping through them if what we want is not found is not on the last page
                if (actualNumberOfPages > 1)
                {
                    BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + $"//button[contains(@data-pagenumber, '{actualNumberOfPages.ToString()}')]", 20);
                }

                try
                {
                    action();
                    return;
                }
                catch (WebDriverTimeoutException)
                {
                    GoToFirstPage();
                }
                catch (Exception Ex)
                {
                    if (Ex.Message != ExceptionMessage)
                    {
                        throw Ex;
                    }
                    // else
                    // eat exception - should just mean it's not visisble on the page yet, so move back to first page if not there already
                    GoToFirstPage();

                }

            }

            // check each page starting at one
            for (int i = 0; i <= actualNumberOfPages; i++)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception Ex)
                {
                    if (Ex.Message != ExceptionMessage)
                    {
                        throw Ex;
                    }
                    // else
                    // eat exception - should just mean it's not visisble on the page yet
                }
                // move to next page if not on the last page
                if (i != actualNumberOfPages)
                {
                    BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + $"//button[contains(@data-pagenumber, '{i + 2}')]");
                }
            }
        }

        private static void GoToFirstPage()
        {
            try
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//button[contains(@data-pagenumber, '1')]", 20);
                BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + "//button[contains(@data-pagenumber, '1')]");
            }
            catch
            {
                // eat exception
            }
        }

        public static void SetAccountSystem(string accountSystem)
        {
            BaseComponent.WaitClick("//a[contains(@id,'_SHOWSYSTEM_action')]"); //select correct account system
            BaseComponent.GetEnabledElement("//div[contains(@class, ' x-window  bbui-dialog') and contains(@style,'visible')]//span[./text()='Select an account system']");
            IDictionary<string, CrmField> Supportedfields = new Dictionary<string, CrmField>
            {
                {"Account System", new CrmField("_PDACCOUNTSYSTEMID_value", FieldType.Dropdown)}
            };
            if (accountSystem == "System Generated Account System")
            {
                Dialog.SetField("SelectAccountSystem", "Account System", accountSystem, Supportedfields);
            }
            else
            {
                Dialog.SetField("SelectAccountSystem", "Account System", accountSystem + uniqueStamp, Supportedfields);
            }

            // OK button
            BaseComponent.WaitClick("//div[contains(@class, 'x-window  bbui-dialog') and contains(@style,'visible')]//button[./text()='OK']");
        }

        public static void SetBenefit(string benefitName, bool isPledge)
        {
            // setup
            var caption = "Benefit";
            var dialogId = "BenefitDetails";
            var gridId = "_BENEFITS_value";
            IDictionary<string, int> columnCaptionToIndex = new Dictionary<string, int>();
            benefitName += uniqueStamp;

            // click button for pop up
            if (isPledge)
            {
                BaseComponent.WaitClick("//a[contains(@id,'_EDITBENEFITSACTION_action')]");
            }
            else
            {
                BaseComponent.WaitClick("//div[contains(@id,'EDITBENEFITSACTION_action')]//button");
            }

            // add benfitname to grid
            columnCaptionToIndex.Add(caption,
                BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), caption));
            string gridXPath = Dialog.getXGridCell(dialogId, gridId, 1, columnCaptionToIndex[caption]);
            string gridRowXPath = Dialog.getXGridRow(dialogId, gridId, 1);
            Dialog.SetGridTextField(gridXPath, benefitName);

            // click OK
            BaseComponent.WaitClick(
                "//div[contains(@class, 'x-window  bbui-dialog') and contains(@style,'visible')]//button[./text()='OK']");
        }

        public static void SetCurrentThreadCultureToConfigValue()
        {
            // lets set the thread culture to get the correct date for the browser
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ConfigurationManager.AppSettings["ChromeDriver.language"]);
        }

        public static int DateIncrementAfterPlus(string dateString)
        {
            return Convert.ToInt32(dateString.Substring(dateString.IndexOf(plus) + 1, 2).Trim());
        }

        public static int DateIncrementAfterMinus(string dateString)
        {
            return -1 * Convert.ToInt32(dateString.Substring(dateString.IndexOf(minus) + 1, 2).Trim());
        }

        public static void SetTodayDateInTableRow(string captionKey, TableRow tableRow)
        {
            TimePeriod timePeriodToAdd = TimePeriod.NotSet;
            int i = 0;

            // set thread to correct culture
            SetCurrentThreadCultureToConfigValue();
            if (tableRow.ContainsKey(captionKey) &&
                !string.IsNullOrEmpty(tableRow[captionKey]) &&
                tableRow[captionKey].ToLower().Contains(LowerCase.Today))
            {

                // get number to increase
                if (tableRow[captionKey].Contains(plus))
                {
                    i = DateIncrementAfterPlus(tableRow[captionKey]);
                }
                else if (tableRow[captionKey].Contains(minus))
                {
                    i = DateIncrementAfterMinus(tableRow[captionKey]);
                }

                // get time unit to increase
                if (tableRow[captionKey].ToLower().Contains(LowerCase.Day) || tableRow[captionKey].ToLower().Contains(LowerCase.Days)) timePeriodToAdd = TimePeriod.Day;
                if (tableRow[captionKey].ToLower().Contains(LowerCase.Week) || tableRow[captionKey].ToLower().Contains(LowerCase.Weeks)) timePeriodToAdd = TimePeriod.Week;
                if (tableRow[captionKey].ToLower().Contains(LowerCase.Month) || tableRow[captionKey].ToLower().Contains(LowerCase.Months)) timePeriodToAdd = TimePeriod.Month;
                if (tableRow[captionKey].ToLower().Contains(LowerCase.Year) || tableRow[captionKey].ToLower().Contains(LowerCase.Years)) timePeriodToAdd = TimePeriod.Year;

                // actually increase
                switch (timePeriodToAdd)
                {
                    case TimePeriod.Day:
                        tableRow[captionKey] = DateTime.Now.AddDays(i).ToShortDateString();
                        break;
                    case TimePeriod.Week:
                        tableRow[captionKey] = DateTime.Now.AddDays(i * 7).ToShortDateString();
                        break;
                    case TimePeriod.Month:
                        tableRow[captionKey] = DateTime.Now.AddMonths(i).ToShortDateString();
                        break;
                    case TimePeriod.Year:
                        tableRow[captionKey] = DateTime.Now.AddYears(i).ToShortDateString();
                        break;
                    case TimePeriod.NotSet:
                        // handle nothing to add
                        tableRow[captionKey] = DateTime.Now.ToShortDateString();
                        break;
                }
            }
        }

        public static DateTime SetTodayDateForVariable(string dateToSet)
        {
            TimePeriod timePeriodToAdd = TimePeriod.NotSet;
            DateTime returnValue = DateTime.MinValue;
            int i = 0;

            // set thread to correct culture
            SetCurrentThreadCultureToConfigValue();
            if (dateToSet.ToLower().Contains(LowerCase.Today))
            {
                // get number to increase
                if (dateToSet.Contains(plus))
                {
                    i = DateIncrementAfterPlus(dateToSet);
                }
                else if (dateToSet.Contains(minus))
                {
                    i = DateIncrementAfterMinus(dateToSet);
                }

                // get time unit to increase
                if (dateToSet.ToLower().Contains(LowerCase.Day) || dateToSet.ToLower().Contains(LowerCase.Days)) timePeriodToAdd = TimePeriod.Day;
                if (dateToSet.ToLower().Contains(LowerCase.Week) || dateToSet.ToLower().Contains(LowerCase.Weeks)) timePeriodToAdd = TimePeriod.Week;
                if (dateToSet.ToLower().Contains(LowerCase.Month) || dateToSet.ToLower().Contains(LowerCase.Months)) timePeriodToAdd = TimePeriod.Month;
                if (dateToSet.ToLower().Contains(LowerCase.Year) || dateToSet.ToLower().Contains(LowerCase.Years)) timePeriodToAdd = TimePeriod.Year;

                // actually increase
                switch (timePeriodToAdd)
                {
                    case TimePeriod.Day:
                        returnValue = DateTime.Now.AddDays(i);
                        break;
                    case TimePeriod.Week:
                        returnValue = DateTime.Now.AddDays(i * 7);
                        break;
                    case TimePeriod.Month:
                        returnValue = DateTime.Now.AddMonths(i);
                        break;
                    case TimePeriod.Year:
                        returnValue = DateTime.Now.AddYears(i);
                        break;
                    case TimePeriod.NotSet:
                        // handle nothing to add
                        returnValue = DateTime.Now;
                        break;
                }
            }
            return returnValue;
        }

        public static void SetTodayDateInTableRow(string captionKey, Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                SetTodayDateInTableRow(captionKey, tableRow);
            }
        }

        public static int SetActualNumberOfPages()
        {
            try
            {
                // First get the number of items then divide by number of items per page to get the actual number of pages
                string numberOfItems =
                    BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel +
                                                    "//div[contains(@class,'xtb-text bbui-pages-section-tbarcaption-detail')]")
                        .Text;
                numberOfItems = numberOfItems.Replace("(", " ");
                numberOfItems = numberOfItems.Replace(")", " ");
                var actualNumberOfItems = double.MinValue;

                if (!double.TryParse(numberOfItems, out actualNumberOfItems))
                {
                    string pageTitle = BaseComponent.Driver.Title.ToString();
                    string returnString = "returnString";
                    if (numberOfItems != null) returnString = numberOfItems;

                    throw new Exception("Error getting page number as double! Value returned from xpath: " + returnString + " page title: " + pageTitle);
                }
                int actualNumberOfPages = Convert.ToInt32(Math.Ceiling(actualNumberOfItems / 30)); // 30 items per page for this list
                return actualNumberOfPages;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SetActualNumberOfPages " + ex.Message);
            }
        }
        public static void SearchByConstituentName(string constituentName, bool splitName)
        {
            if (splitName)
            {
                var names = new string[2];
                names = constituentName.Split(' ');
                SearchDialog.SetFirstNameToSearch(names[0]);
                SearchDialog.SetLastNameToSearch(names[1] + uniqueStamp);
            }
            else
            {
                SearchDialog.SetLastNameToSearch(constituentName + uniqueStamp);
            }
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }

        public static void SelectPageAndStartProcess(string process)
        {
            SelectPageAndStartProcess(process, 0);
        }

        public static void SelectPageAndStartProcess(string process, int expectedRecordCount)
        {
            SelectPageAndExecuteAction(() =>
            {
                StartProcess(process, expectedRecordCount);
            }, startProcessSelectionException, true);


        }

        private static void StartProcess(string process, int expectedRecordCount)
        {
            try
            {
                //if we are not in the right pane this selection with throw an error
                BaseComponent.GetEnabledElement(string.Format(XpathHelper.xPath.VisibleContainerBlock + "//a[text()='{0}']", process), 20);
                BaseComponent.WaitClick(string.Format(XpathHelper.xPath.VisibleContainerBlock + "//a[text()='{0}']", process, 20));
            }
            catch (Exception ex)
            {
                throw new Exception(startProcessSelectionException, ex);
            }

            // Click start process
            BaseComponent.WaitClick("//div[text()='Start process']");
            try
            {
                // click start
                if (process != "Update membership status" + uniqueStamp)
                {
                    BaseComponent.WaitClick(XpathHelper.xPath.VisibleBlock + "//button[text()='Start']", 20);

                }
            }
            catch { }

            // process completes or throw error
            // decide validation based on if we know the actual number of results!
            if (expectedRecordCount == 0)
            {
                WhenRecordsSuccessfullyProcessedIsGreaterThanZero();
            }
            else if (expectedRecordCount > 0)
            {
                WhenRecordsSuccessfullyProcessedIsEqual(expectedRecordCount);
            }
        }

        public static void WhenRecordsSuccessfullyProcessedIsGreaterThanZero()
        {
            // different tabs display two different recent status spellings..accounting for that
            BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + "//span[contains(text(),'Recent Status') or contains(text(),'Recent status')]", 20);
                       
            // wait
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//span[text()='Records successfully processed:']", 480);

            try
            {
                // wait for completed screen
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//span[contains(@id,'_STATUS_caption')]/../..//span[contains(@id,'_STATUS_value') and ./text()='Completed']", 480);
                BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + "//span[contains(@id,'_STATUS_caption')]/../..//span[contains(@id,'_STATUS_value') and ./text()='Completed']", 480);
            }
            catch (Exception ex)
            {
                throw new Exception("Process 'Completed' screen not rendered - process failed to run!", ex);
            }
            // check converts to int
            string xpath = "//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'x-hide-display'))]//span[contains(@id,'_SUCCESSCOUNT_value')]";
            // check it's there to get
            BaseComponent.GetEnabledElement(xpath);
            string processedCount = BaseComponent.GetEnabledElement(xpath).Text.Trim();

            if (!string.IsNullOrEmpty(processedCount))
            {
                int actualProcessedCount = int.MinValue;
                if (!int.TryParse(processedCount, out actualProcessedCount))
                {
                    throw new Exception("Error getting processed count as int! Process count = " + processedCount);
                }
                // check non-negative
                if (actualProcessedCount <= 0)
                {
                    throw new Exception("Processed count is zero (or smaller)! ProcessedCount: " + processedCount);
                }
            }
            else
            {
                throw new Exception("Processed count IsNullOrEmpty");
            }
        }

        public static void WhenRecordsSuccessfullyProcessedIsEqual(int expectedRecordCount)
        {
            try
            {
                // wait for completed screen
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//span[contains(@id,'_STATUS_value') and ./text()='Completed']", 480);
            }
            catch (Exception ex)
            {
                throw new Exception("Process 'Completed' screen not rendered - process failed to run!", ex);
            }

            string xpath = string.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'x-hide-display'))]//span[contains(@id,'_SUCCESSCOUNT_value') and ./text()='{0}']", expectedRecordCount.ToString());
            // check it's there to get
            try
            {
                BaseComponent.GetEnabledElement(xpath, 20);
            }
            catch (WebDriverTimeoutException wdEx)
            {
                throw new Exception(string.Format("Unable to confirm success of Business Process with {0}", expectedRecordCount), wdEx);
            }
        }

        public static void AddEntryOnTheFly()
        {
            // add code table entry if it does not exits
            try
            {
                Dialog.WaitClick("//button[text()='Yes']", 5);
            }
            catch
            {
                // nothing means pop up not spawned
            }
        }

        public static void PageThrough(string pageName, string sectionCaption, string columnName)
        {
            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            rowValues.Add(columnName, pageName + uniqueStamp);

            SelectPageAndExecuteAction(() =>
            {
                if (!Panel.SectionDatalistRowExists(rowValues, sectionCaption))
                {
                    throw new Exception
                        (string.Format("Expected values not in the grid for Solicit code {0}.", pageName + uniqueStamp));
                }
            }, tryLastPageFirst: true);

        }

        public static void VerifyPanelXGridRows(string gridTitle, Table table)
        {
            // preprocessing
            Dictionary<string, int> indices = new Dictionary<string, int>();
            string panelXPath = XpathHelper.xPath.VisiblePanel + $"//div[text()='{gridTitle}']//ancestor::div[contains(@class,'bbui-pages-section-container')]";
            // wait
            Panel.GetEnabledElement(panelXPath + "//thead");
            // find table cells with headers
            IList<IWebElement> elements = BaseComponent.Driver.FindElements(By.XPath(panelXPath + "//thead/tr/td"));
            // generate indices dict
            foreach (var header in table.Header)
            {
                int index = 1;

                foreach (var element in elements)
                {
                    if (string.Equals(element.Text.ToString(), header))
                    {
                        indices.Add(header, index);
                        break;
                    }
                    index++;
                }
            }
            // verify values
            foreach (TableRow row in table.Rows) // by row
            {
                string rowXPath = panelXPath;

                foreach (var header in table.Header) // by column
                {
                    // for each non-empty column, check that the table row includes its value
                    if (!String.IsNullOrEmpty(Convert.ToString(row[header])))
                    {
                        rowXPath += $"//td[{indices[header]}]//*[text()='{row[header]}']//ancestor::tr[1]";
                    }
                }
                Panel.GetDisplayedElement(rowXPath, 15);
            }
        }

        public static string SetXGridHeader(string dialogId, string gridId)
        {
            // this is to take place of Dialog.getXGridHeaders when the visible block that it uses is not needed
            return String.Format("//div[contains(@id,'{0}')]//div[contains(@id, '{1}')]//div[@class='x-grid3-header']//tr", dialogId, gridId);
        }

    }
}
