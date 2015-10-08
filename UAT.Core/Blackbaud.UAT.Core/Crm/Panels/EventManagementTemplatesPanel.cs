using OpenQA.Selenium;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Event Management Templates' panel.
    /// </summary>
    public class EventManagementTemplatesPanel : Panel
    {
        /// <summary>
        /// Check if an event management template exists.
        /// </summary>
        /// <param name="templateName">The name of the event management template.</param>
        /// <returns>True if the template exists, false otherwise.</returns>
        public static bool TemplateExists(string templateName)
        {            
            const string templateNameFields = "//*[contains(@style,\"visible\")]//*[contains(@class,\"x-grid3-body\")]/div/table/tbody/tr[1]/td[3]/div";
            const string templateBar = "//*[contains(@class,\"bbui-pages-section-tbarcaption\")]";
            const string templateGrid = "//*[contains(@class,'x-grid3-body')]";

            try
            {
                CheckGridResultsContain(templateName, templateNameFields, templateBar, "Templates", templateGrid);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }

            return true;

            //IDictionary<string, string> rowValues = new Dictionary<string, string>();
            //rowValues.Add("Name", templateName);

            //return SectionDatalistRowExists(rowValues, "Event management templates");
        }

        /// <summary>
        /// Add an event management template with a single level labeled 'Default'
        /// </summary>
        /// <param name="templateName">The name of the template to add.</param>
        public static void AddTemplate(string templateName)
        {
            ClickSectionAddButton("Event management templates");
            SetTextField(Dialog.getXInput("EventManagementTemplateAddDataForm", "NAME"), templateName);
            WaitClick(Dialog.getXButton("EventManagementTemplateAddDataForm", "Add"));
            SetTextField(Dialog.getXInput("EventManagementTemplateAddLevelAddForm", "NAME"), "Default");
            Dialog.OK();
            Dialog.Save();
        }
    }
}
