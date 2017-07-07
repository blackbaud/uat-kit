using System;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Marketing Acknowledgement Template' panel.
    /// </summary>
    public class MarketingAcknowledgementTemplatePanel : Panel
    {
        /// <summary>
        /// Start to add an acknowledgement rule to the template.
        /// </summary>
        public static void AddAcknowledgementRule()
        {
            GetDisplayedElement(getXPanelHeaderLink("Marketing Acknowledgements"));
            SelectTab("Acknowledgement Rules");

            GetDisplayedElement(getXSectionDatalistColumnHeaders("Acknowledgement rules"));
            WaitClick(getXSectionAddButton("Acknowledgement rules"));
        }

        /// <summary>
        /// Start the process for the current acknowledgement template.
        /// </summary>
        public static void ProcessAcknowledgement()
        {
            WaitClick(BBCRMHomePage.getXTask("Process acknowledgements")); 
            Dialog.ClickButton("Start");
        }

        private new static string getXSectionDatalistColumnHeaders(string sectionCaption)
        {
            return String.Format("{0}/../../../../../../../../../../../../..//div[@class='x-grid3-header']//tr", getXSection(sectionCaption));
        }
    }
}
