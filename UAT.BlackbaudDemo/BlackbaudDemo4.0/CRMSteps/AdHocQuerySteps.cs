using System;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using TechTalk.SpecFlow;

namespace BlackbaudDemo40.CRMSteps
{
    [Binding]
    public class AdHocQuerySteps : BaseSteps
    {
        [When(@"I add ad-hoc query type '(.*)'")]
        public void WhenIAddAd_HocQueryType(string queryType)
        {
            BBCRMHomePage.OpenAnalysisFA();
            AnalysisFunctionalArea.InformationLibrary();
            InformationLibraryPanel.AddAdHocQuery(queryType);
        }

        [When(@"filter by '(.*)'")]
        public void WhenFilterBy(string filter)
        {
            AdHocQueryDialog.FilterBy(filter);
        }

        [When(@"add filter field '(.*)'")]
        public void WhenAddFilterField(string filterField, Table criteria)
        {
            foreach (var criteriaRow in criteria.Rows)
            {
                AdHocQueryDialog.AddFilterField(filterField, criteriaRow);
            }
        }

        [When(@"add output fields")]
        public void WhenAddOutputFields(Table outputFields)
        {
            foreach (var outputField in outputFields.Rows)
            {
                AdHocQueryDialog.FilterBy(outputField["Path"]);
                AdHocQueryDialog.AddOutputField(outputField["Field"]);
            }
        }

        [When(@"set save options")]
        public void WhenSetSaveOptions(Table saveOptions)
        {
            foreach (var option in saveOptions.Rows)
            {
                option["Name"] = option["Name"] + uniqueStamp;
                AdHocQueryDialog.SetSaveOptions(option);
            }
            AdHocQueryDialog.SaveAndClose();
        }

        [Then(@"ad-hoc query '(.*)' is saved")]
        public void ThenAd_HocQueryIsSaved(string queryName)
        {
            if (!AdHocQueryPanel.IsPanelHeader(queryName))
                throw new ArgumentException(
                    String.Format("'{0}' is not the query name of the current ad-hoc query panel.", queryName));
        }
    }
}