using System.Collections.Generic;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Segment' panel.
    /// </summary>
    public class SegmentPanel : Panel
    {
        /// <summary>
        /// Check if an activated marketing effort exists.
        /// </summary>
        /// <param name="effort">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the effort exists, false otherwise.</returns>
        public static bool ActivatedMarketingEffortExists(IDictionary<string, string> effort)
        {
            GetDisplayedElement(getXPanelHeader("segment_revenue"));
            SelectTab("Marketing Efforts");
            return SectionDatalistRowExists(effort, "Activated marketing efforts that use this segment");
        }

        /// <summary>
        /// Delete an activated marketing effort using the specified template.
        /// </summary>
        /// <param name="marketingTemplate">The name of the template that the effort uses.</param>
        public static void DeleteActivatedMarketingEffort(string marketingTemplate)
        {
            GetDisplayedElement(getXPanelHeader("segment_revenue"));
            SelectTab("Marketing Efforts");
            WaitClick(getXDatalistRowLinkByActionAndCaption("Go to effort", marketingTemplate));
            WaitClick(BBCRMHomePage.getXTask("Delete effort"));
            Dialog.Yes();
        }

        /// <summary>
        /// Delete the current segment.
        /// </summary>
        public static void DeleteSegment()
        {
            GetDisplayedElement(getXPanelHeader("segment_revenue"));
            WaitClick(BBCRMHomePage.getXTask("Delete segment"));
            Dialog.Yes();
        }
    }
}
