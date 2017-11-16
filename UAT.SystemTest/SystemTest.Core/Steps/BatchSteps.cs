using System;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
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
        RevenueSteps RevenueSteps = new RevenueSteps();

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
            EnhancedRevenueBatchDialog.SetGridCell("Revenue type", revenueType, 1);
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

        [When(@"split the designations evenly")]
        public void WhenSplitTheDesignationsEvenly(Table designations)
        {
            EnhancedRevenueBatchDialog.SplitDesignations(designations, true);
        }

        [Then(@"the '(.*)' cell value is '(.*)' for row (.*)")]
        public void ThenTheCellValueIsForRow(string caption, string value, int rowNumber)
        {
            if (EnhancedRevenueBatchDialog.GetGridCellValue(caption, rowNumber) != value)
            {
                throw new Exception(String.Format("Row '{0}', column '{1}' does not have the value '{2}'", rowNumber, caption, value));
            }
        }

        [When(@"apply the payment to designations")]
        public void WhenApplyThePaymentToDesignations(Table designations)
        {
            EnhancedRevenueBatchDialog.Apply(designations);
        }

        [Then(@"the revenue record for ""(.*)"" has donations")]
        public void ThenTheRevenueRecordForHasDonations(string constituent, Table donations)
        {
            RevenueSteps.WhenNavigateToTheRevenueRecordFor(constituent);
            foreach (var donation in donations.Rows)
            {
                if (!RevenueRecordPanel.PaymentExists(donation))
                    throw new ArgumentException(
                        String.Format("Donation '{0}' does not exist on the revenue record panel", donation.Values));
            }
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

                EnhancedRevenueBatchDialog.SetGridCell(caption, value, row);

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

        [When(@"I search for the transaction")]
        public void WhenISearchForTheTransaction(Table table)
        {
            #region data setup
            //setup date field.  StepHelper for date must come before dynamic objects
            StepHelper.SetTodayDateInTableRow("Date", table);
            dynamic objectData = table.CreateDynamicInstance();
            var dialogId = "TransactionSearch";
            string groupCaption = "Transactions";
            string taskCaption = "Transaction search";
            string transactionType = objectData.TransactionType;
            objectData.LastName += uniqueStamp;
            //sorts out date format due to datetime adding 00:00:00
            DateTime findDate = objectData.Date;
            //set fields for Transaction search fields on form
            IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
            {
                {"Last/Org/Group name", new CrmField("_KEYNAME_value", FieldType.TextInput)},
                {"Transaction type", new CrmField("_TRANSACTIONTYPE_value", FieldType.Dropdown)}

            };
            #endregion
            //search for pledge transaction
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.OpenLink(groupCaption, taskCaption);
            BaseComponent.GetEnabledElement("//div[contains(@id,'searchdialog') and contains(@style,'visible')]//span[text()='Transaction Search']", 30);
            //Set search fields
            Dialog.SetField(dialogId, "Last/Org/Group name", objectData.LastName, SupportedFields);
            Dialog.SetField(dialogId, "Transaction type", transactionType, SupportedFields);
            //Click Search and select first result
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
            switch (transactionType.ToLower())
            {
                case "payment":
                    string paymentName = string.Format("{0} Payment: {1}", findDate.ToShortDateString(), objectData.Amount);
                    BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[./text()='{0}']", paymentName), 15);
                    break;
                case "pledge":
                    string pledgeName = string.Format("{0} Pledge: {1}", findDate.ToShortDateString(), objectData.Amount);
                    BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[./text()='{0}']", pledgeName), 15);
                    break;
                case "recurring gift":
                    string RecurringGiftName = string.Empty;
                    if (objectData.SpecificType == "Sponsorship")
                    {
                        RecurringGiftName = string.Format("{0} Sponsorship recurring gift: {1}", findDate.ToShortDateString(), objectData.Amount);
                    }
                    else
                    {
                        RecurringGiftName = string.Format("{0} Recurring gift: {1}", findDate.ToShortDateString(), objectData.Amount);
                    }
                    BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[./text()='{0}']", RecurringGiftName), 15);
                    break;
                default:
                    FailTest(string.Format("Test failed checking transaction {0}.", transactionType));
                    break;
            }
        }

        [Then(@"Revenue Transaction Page Transaction Summary for batch payment shows")]
        public void ThenRevenueTransactionPageTransactionSummaryForBatchPaymentShows(Table table)
        {
            RevenueSteps.RevenueTransactionSummary(table);
            //need to figure out that the batch number is not null
            string batchnumber = BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//span[contains(@id,'_BATCHNUMBER_value')]", 15).Text.ToString();
            if (string.IsNullOrEmpty(batchnumber))
            {
                throw new Exception("Batch Number is null or empty");
            }
        }

        [Given(@"An Event exists that includes Registration Option")]
        public void GivenAnEventExistsThatIncludesRegistrationOption(Table table)
        {
            #region data setup
            //setup date field.  StepHelper for date must come before dynamic objects
            StepHelper.SetTodayDateInTableRow("Start date", table);
            dynamic objectData = table.CreateDynamicInstance();
            var eventDialogId = "EventAddForm";
            var registrationDialogID = "RegistrationOptionAddForm2";
            objectData.Name += uniqueStamp;
            //sorts out date format due to datetime adding 00:00:00
            DateTime findDate = objectData.StartDate;
            //set the thread culture to get the correct date for the browser
            StepHelper.SetCurrentThreadCultureToConfigValue();
            //fields for add an event
            IDictionary<string, CrmField> EventSupportedFields = new Dictionary<string, CrmField>
            {
                {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
                {"Start date", new CrmField("_STARTDATE_value", FieldType.TextInput)},
                {"Category", new CrmField("_EVENTCATEGORYCODEID_value", FieldType.Dropdown)}
            };
            //fields for Registration form
            IDictionary<string, CrmField> RegistrationSupportedFields = new Dictionary<string, CrmField>
            {
                {"Registration type", new CrmField("_EVENTREGISTRATIONTYPEID_value", FieldType.Dropdown)},
                {"Registration count", new CrmField("_REGISTRATIONCOUNT_value", FieldType.TextInput)},
                {"Registration fee", new CrmField("_AMOUNT_value", FieldType.TextInput)}
            };
            #endregion
            //navigation
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.AddEvent();
            //set fields for Add an Event
            Dialog.SetField(eventDialogId, "Name", objectData.Name, EventSupportedFields);
            Dialog.SetField(eventDialogId, "Start date", findDate.ToShortDateString(), EventSupportedFields);
            Dialog.SetField(eventDialogId, "Category", objectData.Category, EventSupportedFields);
            Dialog.Save();
            //add registration
            Panel.SelectTab("Options");
            Panel.ClickSectionAddButton("Registration options");
            //add registration options
            Dialog.SetField(registrationDialogID, "Registration type", objectData.RegistrationType, RegistrationSupportedFields);
            Dialog.SetField(registrationDialogID, "Registration count", Convert.ToString(objectData.RegistrationCount), RegistrationSupportedFields);
            Dialog.SetField(registrationDialogID, "Registration fee", Convert.ToString(objectData.RegistrationFee), RegistrationSupportedFields);
            //Save dialog
            Dialog.Save();
        }

        [Given(@"I add organization\(s\)")]
        public void GivenIAddOrganizationS(Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            objectData.Name += uniqueStamp;
            var organizationDialogId = "OrganizationAddForm";
            //fields for add an organization
            IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
            {
                {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
                {"Industry", new CrmField("_INDUSTRYCODEID_value", FieldType.Dropdown)}
            };
            //navigate to Constituents > Add organization
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.OpenLink("Add an organization");
            //set fields 
            Dialog.SetField(organizationDialogId, "Name", objectData.Name, SupportedFields);
            Dialog.SetField(organizationDialogId, "Industry", objectData.Industry, SupportedFields);
            Dialog.Save();
        }

        [Given(@"I add an Organization relationship")]
        public void GivenIAddAnOrganizationRelationship(Table table)
        {
            #region data setup
            //setup date field.  StepHelper for date must come before dynamic objects
            StepHelper.SetTodayDateInTableRow("Start date", table);
            dynamic objectData = table.CreateDynamicInstance();
            var organizationDialogId = "RelationshipIndividualtoOrganizationAddForm2";
            objectData.RelatedOrganization += uniqueStamp;
            //setup fields for Add organizational relationship
            IDictionary<string, CrmField> OrganizationSupportedFields = new Dictionary<string, CrmField>
            {
                {"Related organization",new CrmField("_RECIPROCALCONSTITUENTID_value", FieldType.Searchlist, "OrganizationSearch","_KEYNAME_value")},
                {string.Format(objectData.Constituent + uniqueStamp + " is the"), new CrmField("_RELATIONSHIPTYPECODEID_value", FieldType.Dropdown)},
                {string.Format(objectData.RelatedOrganization + uniqueStamp + " is the"), new CrmField("_RECIPROCALTYPECODEID_value", FieldType.Dropdown)},
                {"Start date", new CrmField("_RELATIONSHIPSTARTDATE_value", FieldType.TextInput)}
            };
            DateTime startDate = objectData.StartDate;
            #endregion
            //Search for constituent
            StepHelper.SearchAndSelectConstituent(objectData.Constituent, true);
            //click Relationship tab and Add individual relationship
            Panel.SelectTab("Relationships");
            Panel.ClickButton("Add organization");
            //check if visible
            BaseComponent.GetEnabledElement("//span[contains(@class,'x-window-header-text') and ./text()='Add a relationship']");
            //set fields
            Dialog.SetField(organizationDialogId, "Related organization", objectData.RelatedOrganization, OrganizationSupportedFields);
            Dialog.SetField(organizationDialogId, objectData.Constituent + uniqueStamp + " is the", objectData.ConstituentRelationshipType, OrganizationSupportedFields);
            Dialog.SetField(organizationDialogId, "Start date", startDate.ToShortDateString(), OrganizationSupportedFields);
            Dialog.SetCheckbox("//input[contains(@id,'_ISMATCHINGGIFTRELATIONSHIP_value')]", objectData.TheOrganizationWillMatchContributionsForThisRelationship);
            Dialog.Save();
        }

        [When(@"I load commitments for constituent ""(.*)"" and apply amount of ""(.*)""")]
        public void WhenILoadCommitmentsForConstituentAndApplyAmountOf(string constituentName, string amount)
        {
            string applyCommitments = "BatchRevenueApplyCommitmentsCustom";
            //navigate to Revenue tab > Apply
            BatchDialog.OpenTab("Revenue");
            BatchDialog.ClickButton("Apply", applyCommitments);
            //Click Load commitments
            BatchDialog.ClickButton("Load commitments", applyCommitments);
            //search for and select constituent
            //TODO - need to refactor this bit of code for searching and selecting a constituent
            string[] names = constituentName.Split(' ');
            Dialog.SetTextField("//div[contains(@id,'searchdialog') and contains(@style,'visible')]//input[contains(@id,'_KEYNAME_value')]", names[1] + uniqueStamp);
            Dialog.SetTextField("//div[contains(@id,'searchdialog') and contains(@style,'visible')]//input[contains(@id,'_FIRSTNAME_value')]", names[0]);
            SearchDialog.Search();
            SearchDialog.Select();
            //Click Auto apply
            BatchDialog.ClickButton("Auto apply", applyCommitments);
            //Check if visible and click ok
            BaseComponent.GetEnabledElement(string.Format(("//td[contains(@class,'x-grid3-col x-grid3-cell x-grid3-td-APPLIED')]/div[text()='{0}']"), Convert.ToString(amount)));
            BatchDialog.ClickButton("OK", applyCommitments);
        }

    }
}
