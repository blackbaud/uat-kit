using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Marketing Acknowledgements' panel.
    /// </summary>
    public class MarketingAcknowledgementsPanel : Panel
    {
        /// <summary>
        /// Start to add a new marketing acknowledgement template.
        /// </summary>
        public static void AddTemplate()
        {
            SelectTab("Templates");
            ClickSectionAddButton("Acknowledgement templates");
        }

        /// <summary>
        /// Check if a marketing acknowledgement template exists.
        /// </summary>
        /// <param name="template">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the template exists, false otherwise.</returns>
        public static bool TemplateExists(TableRow template)
        {
            SelectTab("Templates");
            return SectionDatalistRowExists(template, "Acknowledgement templates");
        }

        /// <summary>
        /// Delete a marketing acknowledgement template.
        /// </summary>
        /// <param name="template">Mapping of the column captions to a single row's values.</param>
        public static void DeleteTemplate(TableRow template)
        {
            SelectTab("Templates");
            SelectSectionDatalistRow(template, "Acknowledgement templates");
            WaitClick(getXSelectedDatalistRowButton("Delete"));
            Dialog.Yes();
        }
    }
}
