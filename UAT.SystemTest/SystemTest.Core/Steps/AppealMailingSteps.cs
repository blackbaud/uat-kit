using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Base;
using SystemTest.Common;
using TechTalk.SpecFlow.Assist;
using Blackbaud.UAT.Core.Crm.Dialogs;
using Blackbaud.UAT.Core.Crm.Panels;

namespace SystemTest.Core.Steps
{
    [Binding]
    public sealed class AppealMailingSteps : BaseSteps
    {
        [Given(@"Marketing Export Definitions ""(.*)"" exists")]
        public void GivenMarketingExportDefinitionsExists(string exportName)
        {
            //in this instance we cannot use the default XpathHelper one
            var visibleDialog = "//div[contains(@class,'x-window x-resizable-pinned') and contains(@style,'visible')]";
            exportName += uniqueStamp;
            //navigate
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            MarketingAndCommFunctionalArea.OpenLink("Packages", "Export definitions");
            BaseComponent.GetEnabledElement(
                "//div[contains(@class,'xtb-text bbui-pages-section-tbarcaption') and ./text()='Marketing export definitions']");
            //click Add
            Panel.ClickSectionAddButton("Marketing export definitions");
            BaseComponent.WaitClick(
                "//div[contains(@class, 'x-menu x-menu-floating x-layer') and contains(@style, 'visible')]//span[./text()='Direct marketing']");
            //click save tab
            Dialog.WaitClick("//span[contains(@class,'x-tab-strip-text') and ./text()='Set save options']");
            //set name field
            ExportDefinitionDialog.SetTextField(visibleDialog + "//input[contains(@id,'_NAME_value')]", exportName);
            //give the "save and close" button validation time to catch up
            ExportDefinitionDialog.SetTextField(visibleDialog + "//textarea[contains(@id,'_DESCRIPTION_value')]", exportName);
            ExportDefinitionDialog.SetTextField(visibleDialog + "//textarea[contains(@id,'_DESCRIPTION_value')]", exportName);
            ExportDefinitionDialog.SetTextField(visibleDialog + "//textarea[contains(@id,'_DESCRIPTION_value')]", exportName);
            ExportDefinitionDialog.SaveAndClose();
        }

        [Given(@"Mail Package record ""(.*)"" exists with Export Definition ""(.*)""")]
        public void GivenMailPackageRecordExists(string mailPackageName, string exportDefinition)
        {
            const string dialogId = "PackageAddFormMailChannel";
            mailPackageName += uniqueStamp;
            exportDefinition += uniqueStamp;
            IDictionary<string, CrmField> Supportedfields = new Dictionary<string, CrmField>
            {
                {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
                {"Description", new CrmField("_DESCRIPTION_value", FieldType.TextArea)},
                {"Export definition", new CrmField("_EXPORTDEFINITIONID_value", FieldType.TextInput)}
            };
            NavMailPackage();
            //set fields on dialog
            Dialog.SetField(dialogId, "Name", mailPackageName, Supportedfields);
            Dialog.SetField(dialogId, "Description", "description: from a test", Supportedfields);
            Dialog.SetField(dialogId, "Export definition", exportDefinition, Supportedfields);
            Dialog.Save();
        }

        [Given(@"Appeal ""(.*)"" exists")]
        public void GivenAppealExists(string appealName)
        {
            appealName += uniqueStamp;
            //Navigate to Marketing and Communication
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            //Add Appeal
            MarketingAndCommFunctionalArea.OpenLink("Appeal", "Add an appeal");
            Dialog.SetTextField(Dialog.getXInput("AppealAddForm", "NAME"), appealName);
            Dialog.SetTextField(Dialog.getXTextArea("AppealAddForm", "DESCRIPTION"), appealName);
            Dialog.Save();
        }

        [Given(@"I add an Appeal mailing")]
        public void GivenIAddAnAppealMailing(Table table)
        {
            //setup date field.  StepHelper for date must come before dynamic objects
            StepHelper.SetTodayDateInTableRow("Mail date", table);
            dynamic objectData = table.CreateDynamicInstance();
            //sorts out date format due to datetime adding 00:00:00
            DateTime findDate = objectData.MailDate;
            //Naviate to Appeal Mailings 
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            FunctionalArea.OpenLink("Appeal mailings");
            //Click Add button
            Panel.WaitClick(@"//td[contains(@class,'x-toolbar-cell') and not (contains(@class,'x-hide-display'))]//button[text()='Add']");
            //Populate Add appeal mailing form using Dialog.SetField          
            Dialog.SetTextField("//input[contains(@id,'_NAME_value')]", objectData.Name + uniqueStamp);
            //need to convert date to a string 
            Dialog.SetTextField("//input[contains(@id,'_MAILDATE_value')]", findDate.ToShortDateString());
            Dialog.SetTextField("//input[contains(@id,'_APPEALID_value')]", objectData.Appeal + uniqueStamp);
            //need to appeand (Ad-hoc Query) string in order to match on input
            Dialog.SetTextField("//input[contains(@id,'_IDSETREGISTERID_value')]", objectData.Selection + uniqueStamp + " (Ad-hoc Query)");
            Dialog.SetTextField("//input[contains(@id,'_MKTPACKAGEID_value')]", objectData.Package + uniqueStamp);
            Dialog.Save();
        }

        [When(@"I edit Appeal mailing ""(.*)""")]
        public void WhenIEditAppealMailing(string appealMailing)
        {
            //Click Edit mailing task
            BaseComponent.WaitClick("//button[div[text () ='Edit mailing']]");
        }

        [When(@"I edit the Selection")]
        public void WhenIEditTheSelection()
        {
            //Click the edit Selection pencil button
            Panel.WaitClick("//input[contains(@id,'_IDSETREGISTERID')] /..//img[contains(@class,'x-form-trigger bbui-forms-edit-trigger')]");
        }

        [When(@"I save the Appeal mailing")]
        public void WhenISaveTheAppealMailing()
        {
            Dialog.Save();
        }

        [When(@"I Start mailing")]
        public void WhenIStartMailing()
        {
            BaseComponent.WaitClick("//button/div[./text()='Start mailing']");
            Dialog.ClickButton("Start");
        }

        [When(@"Records successfully processed is greater than zero")]
        public void WhenRecordsSuccessfullyProcessedIsGreaterThanZero()
        {
            try
            {
                //wait for completed screen
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel +
                                                "//span[contains(@id,'_STATUS_value') and ./text()='Completed']", 180);
            }
            catch (Exception ex)
            {

                throw new Exception("Process 'Completed' screen not rendered - process failed to run!", ex);
            }
            //check converts to int
            string processedCount =
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//span[contains(@id,'_SUCCESSCOUNT_value')]", 120)
                .Text;
            var actualProcessedCount = int.MinValue;
            if (!int.TryParse(processedCount, out actualProcessedCount))
            {
                throw new ApplicationException("Error getting processed count as int!");
            }
            //check non-negative
            if (actualProcessedCount <= 0)
            {
                throw new ApplicationException("Processed count is zero (or smaller)!");
            }
        }

        [Then(@"Appeal ""(.*)"" Mailings tab Appeal mailing List shows")]
        public void ThenAppealMailingsTabAppealMailingListShows(string appeal, Table table)
        {
            //Verify Appeal Mailing data displays correctly on Appeal
            //select M&C 
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            //Open Appeal Search
            MarketingAndCommFunctionalArea.OpenLink("Appeal", "Appeal search");
            //search for Appeal in Name field
            Dialog.SetTextField("//input[contains(@id,'_NAME_value')]", appeal + uniqueStamp);
            //click Search button
            Dialog.ClickButton("Search");
            //Select correct result in grid
            SearchDialog.SelectFirstResult();
            //select Mailings tab
            Panel.SelectTab("Mailings");
            //set data to match data list
            TableRow tableRow = table.Rows[0];
            tableRow["Name"] += uniqueStamp;
            tableRow["Package"] += uniqueStamp;
            tableRow["Selection"] += uniqueStamp + " (Ad-hoc Query)";
            StepHelper.SetTodayDateInTableRow("Mail date", tableRow);
            if ((Panel.SectionDatalistRowExists(table.Rows[0], "Appeal mailings") == false))
            {
                throw new Exception("ThenAppealMailingsTabAppealMailingListShows grid not correct!");
            }
        }

        private void NavMailPackage()
        {
            BBCRMHomePage.OpenMarketingAndCommunicationsFA();
            MarketingAndCommFunctionalArea.OpenLink("Packages", "Packages");
            BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + "//button/div[./text()='Add a mail package']");
        }

    }
}
