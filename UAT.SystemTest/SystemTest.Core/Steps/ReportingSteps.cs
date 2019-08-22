using System;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class ReportingSteps : BaseSteps
    {

        [When(@"I open and run the ""(.*)"" report for ""(.*)""")]
        public void WhenIOpenAndRunTheReportFor(string reportName, string surname)
        {
            // find the constituent
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.OpenLink(reportName);
            SearchDialog.SetLastNameToSearch(surname + uniqueStamp);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
            // run the report
            Panel.WaitClick("//button[./text()='View report']");
        }

        [Then(@"the ""(.*)"" report is presented for ""(.*)""")]
        public void ThenTheReportIsPresentedFor(string reportName, string name)
        {
            // switch to the reports iframe
            BaseComponent.Driver.SwitchTo().Frame(BaseComponent.GetEnabledElement("//iframe[contains(@class,'bbui-reports-reportcontainer-iframe')]"));

            // verify the report title first and foremost
            BaseComponent.GetEnabledElement(string.Format("//div[./text()='{0}']", reportName));

            // check surname
            BaseComponent.GetEnabledElement(string.Format("//div[./text()='{0}']", name + uniqueStamp));
        }
    }
}
