using System;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Crm.Panels;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using SpecFlow.Assist.Dynamic;
using SystemTest.Common;
using System.Linq;
using System.Collections.Generic;
using Blackbaud.UAT.Core.Crm.Dialogs;
using Blackbaud.UAT.Core.Crm.Panels;

namespace SystemTest.Core.Steps
{
    [Binding]
    public sealed class BatchSteps : BaseSteps
    {
        [Then(@"the batch commits without errors or exceptions and (.*) record processed")]
        public void ThenTheBatchCommitsWithoutErrorsOrExceptionsAndRecordProcessed(int numRecords)
        {
            if (!BusinessProcess.IsCompleted()) throw new Exception("Batch committed with exceptions or errors.");
            if (!BusinessProcess.IsNumRecordsProcessed(numRecords))
                throw new Exception(String.Format("'{0}' was not the number of records processed.", numRecords));
        }

        [When(@"I add a batch with template ""(.*)"" and description ""(.*)""")]
        public void WhenIAddABatchWithTemplateAndDescription(string template, string description, Table batchRows)
        {
            description += uniqueStamp;
            StepHelper.SetTodayDateInTableRow("Date", batchRows);
            StepHelper.SetTodayDateInTableRow("Installment start date", batchRows);
            BBCRMHomePage.OpenRevenueFA();
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//span[text()='Revenue']");
            RevenueFunctionalArea.BatchEntry();
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//span[text()='Batch Entry']");
            BatchEntryPanel.AddBatch(template, description);
            foreach (var batchRow in batchRows.Rows)
            {
                if (batchRow.Keys.Contains("Constituent") && batchRow["Constituent"] != null &&
                    batchRow["Constituent"] != string.Empty)
                    batchRow["Constituent"] += uniqueStamp;
            }
            //Set fields in batch
            SetGridRows(batchRows);
        }

        [When(@"I select Revenue type ""(.*)""")]
        public void WhenISelectRevenueType(string revenueType)
        {
            BatchDialog.SetGridCell("Revenue type", revenueType, 1);
        }

        [When(@"I add Additional applications")]
        public void WhenIAddAdditionalApplications(Table table)
        {
            //check is visible 
            BaseComponent.GetEnabledElement("//label[contains(@id, '_AMOUNT_caption')]");
            IList<dynamic> objectData = table.CreateDynamicSet().ToList();
            string dialogId = "BatchRevenueApplyCommitmentsCustom";
            string gridId = "_ADDITIONALAPPLICATIONSSTREAM";
            int i = 1;
            foreach (dynamic application in objectData)
            {
                //Add additional application  
                string gridXPath = Dialog.getXGridCell(dialogId, gridId, i, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), "Additional applications"));
                Dialog.SetGridDropDown(gridXPath, application.AdditionalApplications);
                //Add applied amount
                gridXPath = Dialog.getXGridCell(dialogId, gridId, i, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), "Applied amount"));
                Dialog.SetGridTextField(gridXPath, application.AppliedAmount);
                //Add designation
                gridXPath = Dialog.getXGridCell(dialogId, gridId, i, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), "Designation"));
                Dialog.SetGridTextField(gridXPath, application.Designation);
                //Click Ok
                BatchDialog.ClickButton("OK", dialogId);
            }
        }

        [When(@"I prepare the batch for commit")]
        public void WhenIPrepareTheBatchForCommit()
        {
            BatchDialog.OpenTab("Main");
            //Check is visible
            BaseComponent.GetEnabledElement("//button[contains(@class,' x-btn-text') and ./text()='Update projected totals']");
            //Update projected totals
            Dialog.ClickButton("Update projected totals", null);
            BaseComponent.GetEnabledElement("//button[./text()='OK']");
            Dialog.OK();
            //Validate
            BatchDialog.Validate();
            //Save and close
            BatchDialog.SaveAndClose();
        }

        [When(@"I commit the batch")]
        public void WhenICommitTheBatch(Table batchRows)
        {
            if (batchRows.RowCount != 1) throw new ArgumentException("Only provide one row to select.");
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.BatchEntry();
            // reset datalist, if needed
            string resetButtonPath = XpathHelper.xPath.VisiblePanel + "//button[text()='Reset']";
            // ensure that we can find the either enabled or disabled reset button
            try
            {
                BaseComponent.GetEnabledElement(resetButtonPath, 5);
                BaseComponent.WaitClick(resetButtonPath);
            }
            catch
            {
                // eat the exception
            }
            //clear filters button
            string clearFiltersPath = XpathHelper.xPath.VisiblePanel + "//button[text()='Clear all filters']";
            // Click on the button if it is enabled
            try
            {
                BaseComponent.GetEnabledElement(clearFiltersPath, 5);
                BaseComponent.WaitClick(clearFiltersPath);
            }
            catch
            {
                //eat exception as button is not enabled
            }

            var batchRow = batchRows.Rows[0];
            if (batchRow.ContainsKey("Description") && !string.IsNullOrEmpty(batchRow["Description"]))
            {
                batchRow["Description"] += uniqueStamp;
            }
            try
            {
                BatchEntryPanel.SelectUncommittedBatch(batchRow);
            }
            catch
            {
                BatchEntryPanel.SelectUncommittedBatch(batchRow);
            }
            BatchEntryPanel.CommitSelectedBatch();
            ThenTheBatchCommitsWithoutErrorsOrExceptions();
        }

        [Then(@"the batch commits without errors or exceptions")]
        public void ThenTheBatchCommitsWithoutErrorsOrExceptions()
        {
            if (!BusinessProcess.IsCompleted()) throw new Exception("Batch committed with exceptions or errors.");
        }

        public static void SetGridRows(Table batchRows)
        {
            var rowCount = 1;
            foreach (var batchRow in batchRows.Rows)
            {
                SetGridRow(batchRow, rowCount);
                rowCount += 1;
            }
        }

        public static void SetGridRow(TableRow batchRow, int row)
        {
            foreach (var caption in batchRow.Keys)
            {
                var value = batchRow[caption];
                if (value == null) continue;

                BatchDialog.SetGridCell(caption, value, row);

                //Check Revenue type for pop up dialog
                if ("Revenue type" == caption)
                {
                    try
                    {
                        Dialog.ClickButton("OK", 10);
                    }
                    catch { }
                }
            }
        }
    }
}
