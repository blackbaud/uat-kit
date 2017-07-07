using System;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Crm.Panels;
using Blackbaud.UAT.Core.Crm.Dialogs;
using TechTalk.SpecFlow;

namespace BlackbaudDemo40.CRMSteps
{
    [Binding]
    public class RevenueSteps : BaseSteps
    {
        [When(@"I add a pledge")]
        public void WhenIAddAPledge(Table pledges)
        {
            foreach (var pledge in pledges.Rows)
            {
                BBCRMHomePage.OpenRevenueFA();
                if (pledge.ContainsKey("Constituent") && pledge["Constituent"] != string.Empty)
                    pledge["Constituent"] += uniqueStamp;
                RevenueFunctionalArea.AddAPledge(pledge);
                Dialog.Save();
            }
        }

        [Then(@"a pledge exists for constituent ""(.*)"" with amount ""(.*)""")]
        public void ThenAPledgeExistsForConstituentWithAmount(string constituent, string amount)
        {
            constituent += uniqueStamp;
            if (!PledgePanel.IsPledgeConstituent(constituent))
                throw new ArgumentException(String.Format("Current pledge panel is not for constituent '{0}'",
                    constituent));
            if (!PledgePanel.IsPledgeAmount(amount))
                throw new ArgumentException(String.Format("Current pledge panel is not for amount '{0}'", amount));
        }

        [Given(@"designation exists '(.*)'")]
        public void GivenDesignationExists(string designation)
        {
            BBCRMHomePage.OpenFundraisingFA();
            if (!FundraisingFunctionalArea.DesignationExists(designation))
                throw new ArgumentException(String.Format("Designation '{0}' does not exist.", designation));
        }

        [When(@"I start to add a pledge")]
        public void WhenIStartToAddAPledge(Table pledgeValues)
        {
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.AddAPledge();
            foreach (var pledgeValueRow in pledgeValues.Rows)
            {
                if (pledgeValueRow.ContainsKey("Constituent") && pledgeValueRow["Constituent"] != string.Empty)
                    pledgeValueRow["Constituent"] += uniqueStamp;
                PledgeDialog.SetFields(pledgeValueRow);
            }
        }

        [When(@"split the pledge designations evenly")]
        public void WhenSplitThePledgeDesignations(Table designations)
        {
            PledgeDialog.SplitDesignationsEvenly(designations);
        }

        [When(@"save the pledge")]
        public void WhenSaveThePledge()
        {
            Dialog.Save();
        }

        [Then(@"a pledge exists with designations")]
        public void ThenAPledgeExistsWithDesignations(Table designations)
        {
            foreach (var designation in designations.Rows)
            {
                if (!PledgePanel.DesignationExists(designation))
                    throw new ArgumentException(String.Format("Designation '{0}' does not exist for current pledge.",
                        designation));
            }
        }

        [Then(@"a batch exists")]
        public void ThenABatchExists(Table batches)
        {
            foreach (var batch in batches.Rows)
            {
                if (batch.Keys.Contains("Description") && batch["Description"] != null &&
                    batch["Description"] != string.Empty)
                    batch["Description"] += uniqueStamp;
                if (!BatchEntryPanel.UncommittedBatchExists(batch))
                    throw new ArgumentException(String.Format("Uncommitted batch '{0}' does not exist", batch.Values));
            }
        }

        [When(@"I add a batch with template ""(.*)"" and description ""(.*)""")]
        public void WhenIAddABatchWithTemplateAndDescription(string template, string description, Table batchRows)
        {
            description += uniqueStamp;
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.BatchEntry();
            BatchEntryPanel.AddBatch(template, description);
            foreach (var batchRow in batchRows.Rows)
            {
                if (batchRow.Keys.Contains("Constituent") && batchRow["Constituent"] != null &&
                    batchRow["Constituent"] != string.Empty)
                    batchRow["Constituent"] += uniqueStamp;
            }
            BatchDialog.SetGridRows(batchRows);
            Dialog.WaitClick("//button[./text()='Update projected totals']");
            Dialog.WaitClick("//button[./text()='OK']");
            BatchDialog.Validate();
            BatchDialog.SaveAndClose();
        }

        [Given(@"an ""(.*)"" with description ""(.*)"" exists")]
        public void GivenAnWithDescriptionExists(string template, string description, Table batchRows)
        {
            WhenIAddABatchWithTemplateAndDescription(template, description, batchRows);
        }

        [When(@"I commit the batch")]
        public void WhenICommitTheBatch(Table batchRows)
        {
            if (batchRows.RowCount != 1) throw new ArgumentException("Only provide one row to select.");
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.BatchEntry();
            var batchRow = batchRows.Rows[0];
            if (batchRow.ContainsKey("Description") && batchRow["Description"] != null &&
                batchRow["Description"] != string.Empty)
                batchRow["Description"] += uniqueStamp;
            BatchEntryPanel.SelectUncommittedBatch(batchRow);
            BatchEntryPanel.CommitSelectedBatch();
        }

        [When(@"the batch completes successfully")]
        public void WhenTheBatchCompletesSuccessfully()
        {
            ThenTheBatchCommitsWithoutErrorsOrExceptions();
        }

        [Then(@"the batch commits without errors or exceptions")]
        public void ThenTheBatchCommitsWithoutErrorsOrExceptions()
        {
            if (!BusinessProcess.IsCompleted()) throw new Exception("Batch committed with exceptions or errors.");
        }

        [Then(@"the batch commits without errors or exceptions and (.*) record processed")]
        public void ThenTheBatchCommitsWithoutErrorsOrExceptionsAndRecordProcessed(int numRecords)
        {
            if (!BusinessProcess.IsCompleted()) throw new Exception("Batch committed with exceptions or errors.");
            if (!BusinessProcess.IsNumRecordsProcessed(numRecords))
                throw new Exception(String.Format("'{0}' was not the number of records processed.", numRecords));
        }

        [When(@"I start to add a batch with template ""(.*)"" and description ""(.*)""")]
        public void WhenIStartToAddABatchWithTemplateAndDescription(string template, string description, Table batchRows)
        {
            description += uniqueStamp;
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.BatchEntry();
            BatchEntryPanel.AddBatch(template, description);
            foreach (var batchRow in batchRows.Rows)
            {
                if (batchRow.Keys.Contains("Constituent") && batchRow["Constituent"] != null &&
                    batchRow["Constituent"] != string.Empty)
                    batchRow["Constituent"] += uniqueStamp;
            }
            BatchDialog.SetGridRows(batchRows);
            EnhancedRevenueBatchDialog.UpdateProjectedTotals();
        }

        [When(@"split the designations evenly")]
        public void WhenSplitTheDesignationsEvenly(Table designations)
        {
            EnhancedRevenueBatchDialog.SplitDesignations(designations, true);
        }

        [Then(@"the '(.*)' cell value is '(.*)' for row (.*)")]
        public void ThenTheCellValueIsForRow(string caption, string value, int rowNumber)
        {
            if (BatchDialog.GetGridCellValue(caption, rowNumber) != value)
                throw new Exception(String.Format("Row '{0}', column '{1}' does not have the value '{2}'",
                    rowNumber, caption, value));
        }

        [Given(@"the pledge subtype '(.*)' exists")]
        public void GivenThePledgeSubtypeExists(string subtype)
        {
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.PledgeSubtypes();
            if (!PledgeSubtypePanel.SubtypeExists(subtype)) PledgeSubtypePanel.AddSubtype(subtype, false);
        }

        [When(@"navigate to the revenue record for ""(.*)""")]
        public void WhenNavigateToTheRevenueRecordFor(string constituent)
        {
            constituent += uniqueStamp;
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.TransactionSearchByConstituent(constituent);
        }

        [Then(@"the pledge subtype is ""(.*)""")]
        public void ThenThePledgeSubtypeIs(string subtype)
        {
            if (!RevenueRecordPanel.IsSubtype(subtype))
                throw new ArgumentException(String.Format("'{0}' was not the subtype of the revenue record.", subtype));
        }

        [When(@"apply the payment to designations")]
        public void WhenApplyThePaymentToDesignations(Table designations)
        {
            EnhancedRevenueBatchDialog.Apply(designations);
        }

        [When(@"save the batch and commit it")]
        public void WhenSaveTheBatchAndCommitIt(Table batch)
        {
            EnhancedRevenueBatchDialog.UpdateProjectedTotals();
            BatchDialog.Validate();
            BatchDialog.SaveAndClose();
            WhenICommitTheBatch(batch);
        }

        [Then(@"the revenue record for ""(.*)"" has payments")]
        public void ThenTheRevenueRecordForHasPayments(string constituent, Table payments)
        {
            WhenNavigateToTheRevenueRecordFor(constituent);
            foreach (var payment in payments.Rows)
            {
                if (!RevenueRecordPanel.PaymentExists(payment))
                    throw new ArgumentException(String.Format(
                        "Payment '{0}' does not exist on the revenue record panel", payment.Values));
            }
        }

        [Then(@"the revenue record for '(.*)' is marked as receipted")]
        public void ThenTheRevenueRecordForIsMarkedAsReceipted(string constituent)
        {
            WhenNavigateToTheRevenueRecordFor(constituent);
            if (!RevenueRecordPanel.IsReceipted()) throw new ArgumentException("Revenue record not receipted.");
        }

        [Then(@"the revenue record for '(.*)' is marked as acknowledged")]
        public void ThenTheRevenueRecordForIsMarkedAsAcknowledged(string constituent)
        {
            WhenNavigateToTheRevenueRecordFor(constituent);
            if (!RevenueRecordPanel.IsAcknowledged()) throw new ArgumentException("Revenue record not acknowledged.");
        }

        [When(@"split the designations")]
        public void WhenSplitTheDesignations(Table designations)
        {
            EnhancedRevenueBatchDialog.SplitDesignations(designations, false);
        }

        [Then(@"the revenue record for ""(.*)"" has designations")]
        public void ThenTheRevenueRecordForHasDesignations(string constituent, Table designations)
        {
            WhenNavigateToTheRevenueRecordFor(constituent);
            foreach (var designation in designations.Rows)
            {
                if (!PledgePanel.DesignationExists(designation))
                    throw new ArgumentException(String.Format("Designation '{0}' does not exist for current pledge.",
                        designation));
            }
        }

        [Then(@"the recurring gift revenue record for ""(.*)"" has designations")]
        public void ThenTheRecurringGiftRevenueRecordForHasDesignations(string constituent, Table designations)
        {
            WhenNavigateToTheRevenueRecordFor(constituent);
            foreach (var designation in designations.Rows)
            {
                if (!RecurringGiftPanel.DesignationExists(designation))
                    throw new ArgumentException(
                        String.Format("Designation '{0}' does not exist for current recurring gift.", designation));
            }
        }

        [Given(@"pledges exist")]
        public void GivenPledgesExist(Table pledges)
        {
            WhenIAddAPledge(pledges);
        }

        [When(@"set the revenue type for row (.*) to ""(.*)""")]
        public void WhenSetTheRevenueTypeForRowTo(int rowIndex, string paymentMethod)
        {
            BatchDialog.SetGridCell("Revenue type", paymentMethod, rowIndex);
        }

        [When(@"auto apply the payment")]
        public void WhenAutoApplyThePayment()
        {
            Dialog.ClickButton("Auto apply");
            Dialog.OK();
        }

        [Given(@"I start to add a payment")]
        public void GivenIStartToAddAPayment(Table paymentFieldRows)
        {
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.AddAPayment();
            foreach (var paymentFieldValues in paymentFieldRows.Rows)
            {
                if (paymentFieldValues.ContainsKey("Constituent") && paymentFieldValues["Constituent"] != string.Empty)
                    paymentFieldValues["Constituent"] += uniqueStamp;
                PaymentDialog.SetFields(paymentFieldValues);
            }
        }

        [Given(@"add applications to the payment")]
        public void GivenAddApplicationsToThePayment(Table applications)
        {
            foreach (var application in applications.Rows)
            {
                PaymentDialog.AddApplication(application);
            }
        }

        [Given(@"save the the payment")]
        public void GivenSaveTheThePayment()
        {
            Dialog.Save();
        }
    }
}