using System;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Crm.Panels;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using SpecFlow.Assist.Dynamic;

namespace SystemTest.Core.Steps
{
    [Binding]
    public sealed class AdHocSteps : BaseSteps
    {
        [Given(@"I add a Constituent ad-hoc query")]
        [Given(@"Constituent type Selection exists")]
        public void GivenIAddAConstituentAd_HocQuery()
        {
            //navigate to Analysis>Info Library
            BBCRMHomePage.OpenAnalysisFA();
            AnalysisFunctionalArea.OpenLink("Information library");
            //create new Constituent type query
            Panel.ClickButton("Add an ad-hoc query");
            //QuerySourceViewDialog.SetRecordType("Constituents");
            QuerySourceViewDialog.SelectRecordTypeName("Constituents");
            QuerySourceViewDialog.ClickButton("OK");
        }

        [Given(@"I include ""(.*)"" record ""(.*)"" field with criteria equal to")]
        public void GivenIIncludeRecordFieldWithCriteriaEqualTo(string record, string field, Table criteria)
        {
            //select Constituent fields
            NewAdhocQueryDialog.SetFindField(field);
            //select First name as filter
            NewAdhocQueryDialog.IncludeRecordCriteria(field, criteria);
        }

        [Given(@"I save Query Designer with the following options")]
        public void GivenISaveQueryDesignerWithTheFollowingOptions(Table table)
        {
            string selectPanel = "//div[contains(@id,'AdHocQuerySaveOptionsViewDataForm')]";
            dynamic objectData = table.CreateDynamicInstance();
            //move to correct tab
            Dialog.OpenTab("Set save options");
            //save query selection as per gherkin table
            Dialog.SetTextField(selectPanel + "//input[contains(@id,'NAME_value')]", objectData.Name + uniqueStamp);
            Dialog.SetTextField(selectPanel + "//textarea[contains(@id,'DESCRIPTION_value')]", objectData.Description);
            Dialog.SetCheckbox(selectPanel + "//input[contains(@id,'CREATESELECTION_value')]", objectData.CreateSelection);
            //check if Static selection
            if (!string.IsNullOrEmpty(Convert.ToString(objectData.Static)))
            {
                Dialog.SetCheckbox(Dialog.getXInput("AdHocQuerySaveOptionsViewDataForm", "SELECTION_TYPE_1"), true);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(objectData.ShowInQueryDesigner)))
            {
                //Save as selection available in Query Desgner
                Dialog.SetCheckbox(Dialog.getXInput("AdHocQuerySaveOptionsViewDataForm", "_SHOWINDESIGNER_value"), true);
            }
            Dialog.ClickButton("Save and close");
        }

        [When(@"I include Selection record ""(.*)"" field using criteria equal to")]
        public void WhenIIncludeSelectionRecordFieldWithUniqueStampUsingCriteriaEqualTo(string field, Table criteria)
        {
            //select Constituent fields
            NewAdhocQueryDialog.SetFindField(field + uniqueStamp);
            //select First name as filter
            NewAdhocQueryDialog.IncludeRecordCriteria("Exists in \"" + field + uniqueStamp + " (Ad-hoc Query)\"", criteria);
            //Save and close
            Dialog.ClickButton("Save and close");
        }

        [Given(@"include ""(.*)"" record ""(.*)"" field with criteria")]
        public void GivenIncludeRecordFieldWithCriteria(string record, string field, Table criteria)
        {
            string searchbox = "//div[contains(@id,'SEARCHONEOFVALUES_value')]";
            NewAdhocQueryDialog.SetFindField(field);
            BaseComponent.WaitClick(NewAdhocQueryDialog.getXFieldResult(field));
            BaseComponent.WaitClick("//span[./text()='Include records where:' and @class='x-panel-header-text']/../../../../../../div[@class=' x-panel x-panel-tbar x-panel-tbar-noheader x-border-panel']/div/div/table/tbody/tr/td/em/button[@class=' x-btn-text bbui-adhocquery-movefieldright']");
            Dialog.SetTextField("//input[contains(@id,'FILTEROPERATOR_value')]", criteria.Rows[0]["value"]);
            BaseComponent.WaitClick(searchbox + "//div[contains(@class,'x-grid3-body')]/div/table/tbody/tr/td[2]");
            Dialog.SetTextField(searchbox + "//input[contains(@class,'x-form-text x-form-field x-form-focus')]", criteria.Rows[1]["value"] + uniqueStamp);
            Dialog.SetTextField(searchbox + "//input[contains(@class,'x-form-text x-form-field x-form-focus')]", criteria.Rows[2]["value"] + uniqueStamp);
            Dialog.SetTextField(searchbox + "//input[contains(@class,'x-form-text x-form-field x-form-focus')]", criteria.Rows[3]["value"] + uniqueStamp);
            Dialog.SetTextField(searchbox + "//input[contains(@class,'x-form-text x-form-field x-form-focus')]", criteria.Rows[4]["value"] + uniqueStamp);
            Dialog.OK();
        }

        [When(@"I add a ""(.*)"" ad-hoc query")]
        public void WhenIAddAAd_HocQuery(string queryType)
        {
            // Open the Analysis functional Area
            BBCRMHomePage.OpenAnalysisFA();
            // Open the QueryPanel 4.0 (Information Library)
            AnalysisFunctionalArea.OpenLink("Information library");
            Panel.ClickButton("Add an ad-hoc query");
            // Select the record type we're intersted in
            QuerySourceViewDialog.SetRecordType(queryType);
            QuerySourceViewDialog.SelectRecordTypeName(queryType);
            QuerySourceViewDialog.ClickButton("OK");
        }

        [When(@"save with the following options")]
        public void WhenSaveWithTheFollowingOptions(Table table)
        {
            TableRow row = table.Rows[0];
            row["value"] += uniqueStamp;
            NewAdhocQueryDialog.SetSaveOptions(table);
            Dialog.ClickButton("Save and close");
        }
    }
}
