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

        public static void SearchAndSelectConstituent(string ConstituentName)
        {
            bool splitName = false;
            if (ConstituentName.IndexOf(" ") > 0)
            {
                splitName = true;
            }
            SearchAndSelectConstituent(ConstituentName, splitName);
        }

        public static void SearchAndSelectConstituent(string ConstituentName, bool SplitName)
        {
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.OpenConstituentSearchDialog();

            if (SplitName)
            {
                var names = new string[2];
                names = ConstituentName.Split(' ');
                SearchDialog.SetFirstNameToSearch(names[0]);
                SearchDialog.SetLastNameToSearch(names[1] + uniqueStamp);
            }
            else
            {
                SearchDialog.SetLastNameToSearch(ConstituentName + uniqueStamp);
            }
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }

        public static void SearchAndSelectConstituentNotUnique(string ConstituentName, bool SplitName)
        {
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.OpenConstituentSearchDialog();

            if (SplitName)
            {
                var names = new string[2];
                names = ConstituentName.Split(' ');
                SearchDialog.SetFirstNameToSearch(names[0]);
                SearchDialog.SetLastNameToSearch(names[1]);
            }
            else
            {
                SearchDialog.SetLastNameToSearch(ConstituentName);
            }
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }

        public static void SetAccountSystem(string AccountSystem)
        {
            BaseComponent.WaitClick("//a[contains(@id,'_SHOWSYSTEM_action')]"); //select correct account system
            BaseComponent.GetEnabledElement("//div[contains(@class, ' x-window  bbui-dialog') and contains(@style,'visible')]//span[./text()='Select an account system']");
            IDictionary<string, CrmField> Supportedfields = new Dictionary<string, CrmField>
            {
                {"Account System", new CrmField("_PDACCOUNTSYSTEMID_value", FieldType.Dropdown)}
            };
            Dialog.SetField("SelectAccountSystem", "Account System", AccountSystem, Supportedfields);
            // OK button
            BaseComponent.WaitClick("//div[contains(@class, 'x-window  bbui-dialog') and contains(@style,'visible')]//button[./text()='OK']");
        }

        public static void SetBenefit(string BenefitName, bool IsPledge)
        {
            // setup
            var caption = "Benefit";
            var dialogId = "BenefitDetails";
            var gridId = "_BENEFITS_value";
            IDictionary<string, int> columnCaptionToIndex = new Dictionary<string, int>();
            BenefitName += uniqueStamp;

            // click button for pop up
            if (IsPledge)
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
            Dialog.SetGridTextField(gridXPath, BenefitName);

            // click OK
            BaseComponent.WaitClick(
                "//div[contains(@class, 'x-window  bbui-dialog') and contains(@style,'visible')]//button[./text()='OK']");
        }

        public static void SetCurrentThreadCultureToConfigValue()
        {
            // lets set the thread culture to get the correct date for the browser
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ConfigurationManager.AppSettings["ChromeDriver.language"]);
        }

        public static int DateIncrementAfterPlus(string DateString)
        {
            return Convert.ToInt32(DateString.Substring(DateString.IndexOf(plus) + 1, 2).Trim());
        }

        public static int DateIncrementAfterMinus(string DateString)
        {
            return -1 * Convert.ToInt32(DateString.Substring(DateString.IndexOf(minus) + 1, 2).Trim());
        }

        public static void SetTodayDateInTableRow(string CaptionKey, TableRow tableRow)
        {
            TimePeriod timePeriodToAdd = TimePeriod.NotSet;
            int i = 0;

            // set thread to correct culture
            SetCurrentThreadCultureToConfigValue();
            if (tableRow.ContainsKey(CaptionKey) &&
                !string.IsNullOrEmpty(tableRow[CaptionKey]) &&
                tableRow[CaptionKey].ToLower().Contains(LowerCase.Today))
            {

                // get number to increase
                if (tableRow[CaptionKey].Contains(plus))
                {
                    i = DateIncrementAfterPlus(tableRow[CaptionKey]);
                }
                else if (tableRow[CaptionKey].Contains(minus))
                {
                    i = DateIncrementAfterMinus(tableRow[CaptionKey]);
                }

                // get time unit to increase
                if (tableRow[CaptionKey].ToLower().Contains(LowerCase.Day) || tableRow[CaptionKey].ToLower().Contains(LowerCase.Days)) timePeriodToAdd = TimePeriod.Day;
                if (tableRow[CaptionKey].ToLower().Contains(LowerCase.Week) || tableRow[CaptionKey].ToLower().Contains(LowerCase.Weeks)) timePeriodToAdd = TimePeriod.Week;
                if (tableRow[CaptionKey].ToLower().Contains(LowerCase.Month) || tableRow[CaptionKey].ToLower().Contains(LowerCase.Months)) timePeriodToAdd = TimePeriod.Month;
                if (tableRow[CaptionKey].ToLower().Contains(LowerCase.Year) || tableRow[CaptionKey].ToLower().Contains(LowerCase.Years)) timePeriodToAdd = TimePeriod.Year;

                // actually increase
                switch (timePeriodToAdd)
                {
                    case TimePeriod.Day:
                        tableRow[CaptionKey] = DateTime.Now.AddDays(i).ToShortDateString();
                        break;
                    case TimePeriod.Week:
                        tableRow[CaptionKey] = DateTime.Now.AddDays(i * 7).ToShortDateString();
                        break;
                    case TimePeriod.Month:
                        tableRow[CaptionKey] = DateTime.Now.AddMonths(i).ToShortDateString();
                        break;
                    case TimePeriod.Year:
                        tableRow[CaptionKey] = DateTime.Now.AddYears(i).ToShortDateString();
                        break;
                    case TimePeriod.NotSet:
                        // handle nothing to add
                        tableRow[CaptionKey] = DateTime.Now.ToShortDateString();
                        break;
                }
            }
        }

        public static DateTime SetTodayDateForVariable(string DateToSet)
        {
            TimePeriod timePeriodToAdd = TimePeriod.NotSet;
            DateTime returnValue = DateTime.MinValue;
            int i = 0;

            // set thread to correct culture
            SetCurrentThreadCultureToConfigValue();
            if (DateToSet.ToLower().Contains(LowerCase.Today))
            {
                // get number to increase
                if (DateToSet.Contains(plus))
                {
                    i = DateIncrementAfterPlus(DateToSet);
                }
                else if (DateToSet.Contains(minus))
                {
                    i = DateIncrementAfterMinus(DateToSet);
                }

                // get time unit to increase
                if (DateToSet.ToLower().Contains(LowerCase.Day) || DateToSet.ToLower().Contains(LowerCase.Days)) timePeriodToAdd = TimePeriod.Day;
                if (DateToSet.ToLower().Contains(LowerCase.Week) || DateToSet.ToLower().Contains(LowerCase.Weeks)) timePeriodToAdd = TimePeriod.Week;
                if (DateToSet.ToLower().Contains(LowerCase.Month) || DateToSet.ToLower().Contains(LowerCase.Months)) timePeriodToAdd = TimePeriod.Month;
                if (DateToSet.ToLower().Contains(LowerCase.Year) || DateToSet.ToLower().Contains(LowerCase.Years)) timePeriodToAdd = TimePeriod.Year;

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

        public static void SetTodayDateInTableRow(string CaptionKey, Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                SetTodayDateInTableRow(CaptionKey, tableRow);
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
        public static void SearchByConstituentName(string ConstituentName, bool SplitName)
        {
            if (SplitName)
            {
                var names = new string[2];
                names = ConstituentName.Split(' ');
                SearchDialog.SetFirstNameToSearch(names[0]);
                SearchDialog.SetLastNameToSearch(names[1] + uniqueStamp);
            }
            else
            {
                SearchDialog.SetLastNameToSearch(ConstituentName + uniqueStamp);
            }
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }

        public static void SelectPageAndStartProcess(string Process)
        {
            SelectPageAndStartProcess(Process, 0);
        }

        public static void SelectPageAndStartProcess(string Process, int ExpectedRecordCount)
        {
            try
            {
                // check the page has rendered find the last > in the page control at the bottom of the page
                // this does not appear if the page count is == 1
                Panel.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//div[contains(@class,'x-panel-bbar')]//button[./text()='>']", 15);
            }
            catch // (Exception ex)
            {
                // lets fail fast and move on
            }

            // get actual number of pages
            int actualNumberOfPages = StepHelper.SetActualNumberOfPages();

            try
            {
                // lets try the last page first as it's normally here
                if (actualNumberOfPages > 1)
                {
                    BaseComponent.WaitClick(string.Format(XpathHelper.xPath.VisiblePanel + "//button[contains(@data-pagenumber, '{0}')]", actualNumberOfPages), 15);
                }
                StartProcess(Process, ExpectedRecordCount);
                return;
            }
            catch
            {
                BaseComponent.WaitClick(string.Format(XpathHelper.xPath.VisiblePanel + "//button[contains(@data-pagenumber, '1')]"));
                // check each page starting at one
                for (var i = 0; i <= actualNumberOfPages; i++)
                {
                    try
                    {
                        StartProcess(Process, ExpectedRecordCount);
                        break;
                    }
                    catch (Exception Ex)
                    {
                        if (Ex.Message != startProcessSelectionException)
                        {
                            throw Ex;
                        }
                        // else
                        // eat exception - should just mean it's not visisble on the page yet
                    }
                    // move to next page if not on the last page
                    if (i != actualNumberOfPages)
                    {
                        BaseComponent.WaitClick(string.Format(XpathHelper.xPath.VisiblePanel + "//button[contains(@data-pagenumber, '{0}')]", i + 2));
                    }
                }
            }
        }

        private static void StartProcess(string Process, int ExpectedRecordCount)
        {
            try
            {
                //if we are not in the right pane this selection with throw an error
                BaseComponent.GetEnabledElement(string.Format(XpathHelper.xPath.VisibleContainerBlock + "//a[text()='{0}']", Process), 15);
            }
            catch (Exception ex)
            {
                throw new Exception(startProcessSelectionException, ex);
            }
            // click process
            BaseComponent.WaitClick(string.Format(XpathHelper.xPath.VisibleContainerBlock + "//a[text()='{0}']", Process), 15);

            // Click start process
            BaseComponent.WaitClick("//div[text()='Start process']");
            // click start
            if (Process != "Update membership status" + uniqueStamp)
            {
                BaseComponent.WaitClick(XpathHelper.xPath.VisibleBlock + "//button[text()='Start']");
            }
           


            // process completes or throw error
            // decide validation based on if we know the actual number of results!
            if (ExpectedRecordCount == 0)
            {
                WhenRecordsSuccessfullyProcessedIsGreaterThanZero();
            }
            else if (ExpectedRecordCount > 0)
            {
                WhenRecordsSuccessfullyProcessedIsEqual(ExpectedRecordCount);
            }
        }

        public static void WhenRecordsSuccessfullyProcessedIsGreaterThanZero()
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

        public static void WhenRecordsSuccessfullyProcessedIsEqual(int ExpectedRecordCount)
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

            string xpath = string.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'x-hide-display'))]//span[contains(@id,'_SUCCESSCOUNT_value') and ./text()='{0}']", ExpectedRecordCount.ToString());
            // check it's there to get
            try
            {
                BaseComponent.GetEnabledElement(xpath, 10);
            }
            catch (WebDriverTimeoutException wdEx)
            {
                throw new Exception(string.Format("Unable to confirm success of Business Process with {0}", ExpectedRecordCount), wdEx);
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
    }
}
