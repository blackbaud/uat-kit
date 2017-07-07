using System.Collections.Generic;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Segments' panel.
    /// </summary>
    public class SegmentsPanel : Panel
    {
        /// <summary>
        /// Start to add a segment of the specified type.
        /// </summary>
        /// <param name="segmentType">The type of segment to add.  Corresponds to the caption of Add menu items.</param>
        public static void AddSegment(string segmentType)
        {
            HeaderLoaded();
            SelectTab("Segments");
            ClickSectionAddButton("Segments");
            WaitClick(getXMenuItem(segmentType));
        }

        /// <summary>
        /// Delete a segment.
        /// </summary>
        /// <param name="segment">Mapping of the column captions to a single row's values.</param>
        public static void DeleteSegment(IDictionary<string, string> segment)
        {
            HeaderLoaded();
            SelectTab("Segments");
            SelectSectionDatalistRow(segment, "Segments");
            WaitClick(getXSelectedDatalistRowButton("Delete"));
            Dialog.Yes();
        }

        /// <summary>
        /// Check if a segment exists.
        /// </summary>
        /// <param name="segment">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the segment exists, false otherwise.</returns>
        public static bool SegmentExists(IDictionary<string, string> segment)
        {
            HeaderLoaded();
            SelectTab("Segments");
            return SectionDatalistRowExists(segment, "Segments");
        }

        /// <summary>
        /// Navigate to the segment panel of a specified segment.
        /// </summary>
        /// <param name="segment">Mapping of the column captions to a single row's values.</param>
        public static void GoToSegment(IDictionary<string, string> segment)
        {
            HeaderLoaded();
            SelectTab("Segments");
            SelectSectionDatalistRow(segment, "Segments");
            WaitClick(getXSelectedDatalistRowLinkByAction("Go to segment"));
        }

        private static void HeaderLoaded()
        {
            GetDisplayedElement(getXPanelHeaderByText("Segments"));
        }
    }
}
