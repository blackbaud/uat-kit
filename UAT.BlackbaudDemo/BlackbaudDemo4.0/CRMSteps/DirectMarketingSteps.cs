using System;
using System.Collections.Generic;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Crm.Dialogs;
using TechTalk.SpecFlow;

namespace BlackbaudDemo40.CRMSteps
{
    [Binding]
    public class DirectMarketingSteps : BaseSteps
    {
        [When(@"I create a receipt process")]
        public void WhenICreateAReceiptProcess(Table receipts)
        {
            foreach (var receipt in receipts.Rows)
            {
                BBCRMHomePage.OpenMarketingAndCommunicationsFA();
                MarketingAndCommFunctionalArea.Receipts();
                if (receipt.ContainsKey("Name") && !string.IsNullOrEmpty(receipt["Name"]))
                    receipt["Name"] += uniqueStamp;
                ReceiptsPanel.AddReceipt(receipt);
            }
        }

        [When(@"run receipt process")]
        public void WhenRunReceiptProcess(Table receiptProcesses)
        {
            foreach (var receiptProcess in receiptProcesses.Rows)
            {
                BBCRMHomePage.OpenMarketingAndCommunicationsFA();
                MarketingAndCommFunctionalArea.Receipts();
                if (receiptProcess.ContainsKey("Name") && !string.IsNullOrEmpty(receiptProcess["Name"]))
                    receiptProcess["Name"] += uniqueStamp;
                ReceiptsPanel.RunReceiptProcess(receiptProcess);
                if (!ReceiptBusinessProcess.IsCompleted())
                    throw new Exception("Process ran with exceptions or errors.");
            }
        }

        [Given(@"marketing acknowledgement template does not exist")]
        public void GivenMarketingAcknowledgementTemplateDoesNotExist(Table templates)
        {
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            MarketingAndCommFunctionalArea.MarketingAcknowledgements();
            foreach (var template in templates.Rows)
            {
                if (MarketingAcknowledgementsPanel.TemplateExists(template))
                    MarketingAcknowledgementsPanel.DeleteTemplate(template);
            }
        }

        [When(@"I start to add a marketing acknowledgement template")]
        public void WhenIStartToAddAMarketingAcknowledgementTemplate()
        {
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            MarketingAndCommFunctionalArea.MarketingAcknowledgements();
            MarketingAcknowledgementsPanel.AddTemplate();
        }

        [When(@"set the general tab's fields")]
        public void WhenSetTheGeneralTabSFields(Table generalFields)
        {
            foreach (var fields in generalFields.Rows)
            {
                MarketingAcknowledgementTemplateDialog.SetGeneralTabFields(fields);
            }
        }

        [When(@"set the source code to '(.*)'")]
        public void WhenSetTheSourceCodeTo(string sourceCode)
        {
            MarketingAcknowledgementTemplateDialog.SetSourceCode(sourceCode);
        }

        [When(@"set universe tab to include all records")]
        public void WhenSetUniverseTabToIncludeAllRecords()
        {
            MarketingAcknowledgementTemplateDialog.IncludeAllRecords();
        }

        [When(@"set the activation tab's fields")]
        public void WhenSetTheActivationTabSFields(Table activationFields)
        {
            foreach (var fields in activationFields.Rows)
            {
                MarketingAcknowledgementTemplateDialog.SetActivationTabFields(fields);
            }
        }

        [When(@"save the template")]
        public void WhenSaveTheTemplate()
        {
            Dialog.Save();
        }

        [Then(@"a marketing acknowledgement template exists")]
        public void ThenAMarketingAcknowledgementTemplateExists(Table templates)
        {
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            MarketingAndCommFunctionalArea.MarketingAcknowledgements();
            foreach (var template in templates.Rows)
            {
                if (!MarketingAcknowledgementsPanel.TemplateExists(template))
                    throw new ArgumentException(String.Format("Template '{0}' does not exist.", template.Values));
            }
        }

        [Given(@"unacknowledged revenue query '(.*)' exists")]
        public void GivenUnacknowledgedRevenueQueryExists(string selection)
        {
            BBCRMHomePage.OpenAnalysisFA();
            AnalysisFunctionalArea.InformationLibrary();
            IDictionary<string, string> selectionRow = new Dictionary<string, string> {{"Name", selection}};
            if (InformationLibraryPanel.QueryExists(selectionRow)) return;
            CreateUnacknowledgedRevenueSelection(selection);
        }

        [Given(@"segment '(.*)' does not exist with activated marketing effort template '(.*)'")]
        public void GivenSegmentDoesNotExistWithActivatedMarketingEffortUsingPackage(string segmentName, string template)
        {
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            MarketingAndCommFunctionalArea.Segments();

            IDictionary<string, string> segmentRow = new Dictionary<string, string> {{"Segment", segmentName}};
            if (SegmentsPanel.SegmentExists(segmentRow)) SegmentsPanel.GoToSegment(segmentRow);
            else return;

            IDictionary<string, string> effort = new Dictionary<string, string> {{"Marketing effort", template}};
            if (SegmentPanel.ActivatedMarketingEffortExists(effort))
            {
                SegmentPanel.DeleteActivatedMarketingEffort(template);
                BBCRMHomePage.OpenMarketingAndCommunicationsFA();
                MarketingAndCommFunctionalArea.Segments();
                SegmentsPanel.GoToSegment(segmentRow);
            }
            SegmentPanel.DeleteSegment();
        }

        [Given(@"a static selection copy '(.*)' of query '(.*)' exists")]
        public void GivenAStaticSelectionCopyOfQueryExists(string selection, string query)
        {
            BBCRMHomePage.OpenAnalysisFA();
            AnalysisFunctionalArea.InformationLibrary();

            IDictionary<string, string> queryRow = new Dictionary<string, string> {{"Name", query}};
            if (!InformationLibraryPanel.QueryExists(queryRow)) CreateUnacknowledgedRevenueSelection(query);

            IDictionary<string, string> selectionRow = new Dictionary<string, string> {{"Name", selection}};
            if (InformationLibraryPanel.QueryExists(selectionRow))
            {
                InformationLibraryPanel.DeleteQuery(selectionRow);
                //re-nvaigate should not be necessary.  only done due to filed bug 490200
                BBCRMHomePage.OpenAnalysisFA();
                AnalysisFunctionalArea.InformationLibrary();
            }

            InformationLibraryPanel.CopyQueryAsStaticSelection(selection, queryRow);
        }

        [Given(@"'(.*)' segment exists with selection '(.*)'")]
        public void GivenSegmentExistsWithSelection(string segmentType, string selection, Table detailsFields)
        {
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            MarketingAndCommFunctionalArea.Segments();
            SegmentsPanel.AddSegment(segmentType);
            foreach (var detailsFieldRow in detailsFields.Rows)
            {
                SegmentDialog.SetDetailsFields(detailsFieldRow);
            }
            SegmentDialog.AddSelection(selection);
            Dialog.Save();
        }

        [Given(@"a mail package exists with")]
        public void GivenAMailPackageExistsWith(Table packages)
        {
            foreach (var package in packages.Rows)
            {
                BBCRMHomePage.OpenMarketingAndCommunicationsFA();
                MarketingAndCommFunctionalArea.Packages();

                IDictionary<string, string> packageRow = new Dictionary<string, string> {{"Channel", "Mail"}};
                if (package.ContainsKey("Name") && package["Name"] != string.Empty)
                    packageRow.Add("Name", package["Name"]);
                if (package.ContainsKey("Letter") && package["Letter"] != string.Empty)
                    packageRow.Add("Content", package["Letter"]);
                if (PackagesPanel.PackageExists(packageRow)) PackagesPanel.DeletePackage(packageRow);

                PackagesPanel.AddMailPackage();
                PackageDialog.SetFields(package);
                Dialog.Save();
            }
        }

        [Given(@"package exists")]
        public void GivenPackageExists(Table packages)
        {
            foreach (var package in packages.Rows)
            {
                BBCRMHomePage.OpenMarketingAndCommunicationsFA();
                MarketingAndCommFunctionalArea.Packages();
                if (PackagesPanel.PackageExists(package)) continue;
                PackagesPanel.AddMailPackage();
                PackageDialog.SetFields(package);
            }
        }

        [Given(@"I start to add a marketing acknowledgement template")]
        public void GivenIStartToAddAMarketingAcknowledgementTemplate()
        {
            WhenIStartToAddAMarketingAcknowledgementTemplate();
        }

        [Given(@"set the general tab's fields")]
        public void GivenSetTheGeneralTabSFields(Table generalTabFields)
        {
            WhenSetTheGeneralTabSFields(generalTabFields);
        }

        [Given(@"set the source code to '(.*)'")]
        public void GivenSetTheSourceCodeTo(string sourceCode)
        {
            WhenSetTheSourceCodeTo(sourceCode);
        }

        [Given(@"set universe tab to include all records")]
        public void GivenSetUniverseTabToIncludeAllRecords()
        {
            WhenSetUniverseTabToIncludeAllRecords();
        }

        [Given(@"set the activation tab's fields")]
        public void GivenSetTheActivationTabSFields(Table activationTabFields)
        {
            WhenSetTheActivationTabSFields(activationTabFields);
        }

        [Given(@"save the template")]
        public void GivenSaveTheTemplate()
        {
            WhenSaveTheTemplate();
        }

        [When(@"I start to add an acknowledgement rule to the current marketing acknowledgement template")]
        public void WhenIStartToAddAnAcknowledgementRuleToTheCurrentMarketingAcknowledgementTemplate(Table fieldRows)
        {
            MarketingAcknowledgementTemplatePanel.AddAcknowledgementRule();
            foreach (var fieldRow in fieldRows.Rows)
            {
                MarketingAcknowledgementRuleDialog.SetDetailsTabFields(fieldRow);
            }
        }

        [When(@"I save the acknowledgement rule accepting the (.*) source code changes")]
        public void WhenISaveTheAcknowledgementRuleAcceptingTheSourceCodeChanges(int numOfApprovedChanges)
        {
            MarketingAcknowledgementRuleDialog.Save(numOfApprovedChanges);
        }

        [When(@"I run marketing acknowledgement process")]
        public void WhenIRunMarketingAcknowledgementProcess()
        {
            MarketingAcknowledgementTemplatePanel.ProcessAcknowledgement();
        }

        [Then(@"the marketing process completes without errors or exceptions")]
        public void ThenTheMarketingProcessCompletesWithoutErrorsOrExceptions()
        {
            if (!MarketingAcknowledgementBusinessProcess.IsCompleted())
                throw new ArgumentException("Marketing process completed with errors or exceptions.");
        }

        private static void CreateUnacknowledgedRevenueSelection(string selectionName)
        {
            InformationLibraryPanel.AddAdHocQuery("Revenue");

            AdHocQueryDialog.FilterBy("Revenue");
            AdHocQueryDialog.AddFilterField("Transaction type", CreateTransactionCriteria());
            AdHocQueryDialog.AddFilterField("Date", CreateDateCriteria());
            AdHocQueryDialog.FilterBy("Revenue\\Revenue Letters");
            AdHocQueryDialog.AddFilterField("Acknowledge date", CreateAcknowledgeDateCriteria());

            AdHocQueryDialog.FilterBy("Revenue");
            AdHocQueryDialog.AddOutputField("Transaction type");
            AdHocQueryDialog.FilterBy("Revenue\\Constituent");
            AdHocQueryDialog.AddOutputField("Last/Organization/Group/Household name");

            AdHocQueryDialog.SetSaveOptions(CreateSaveOptions(selectionName));
            Dialog.Save();
        }

        private static TableRow CreateSaveOptions(string name)
        {
            var table = new Table("Name", "Create a selection?", "Create a dynamic selection");
            table.AddRow(name, "true", "true");
            return table.Rows[0];
        }

        private static TableRow CreateAcknowledgeDateCriteria()
        {
            var table = new Table("FILTEROPERATOR", "INCLUDEBLANKS", "FILTERTYPE_1", "OUTPUTFIELD1", "MEETSALLCRITERIA");
            table.AddRow("Is not", "true", "true", "Date", "true");
            return table.Rows[0];
        }

        private static TableRow CreateDateCriteria()
        {
            var table = new Table("FILTEROPERATOR", "DATEFILTERTYPE", "DATEVALUE1");
            table.AddRow("On or after", "Specific date", "1/1/2015");
            return table.Rows[0];
        }

        private static TableRow CreateTransactionCriteria()
        {
            var table = new Table("FILTEROPERATOR", "COMBOVALUE");
            table.AddRow("Equal to", "Payment");
            return table.Rows[0];
        }
    }
}