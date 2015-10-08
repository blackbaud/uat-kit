namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Prospects' functional area.
    /// </summary>
    public class ProspectsFunctionalArea : FunctionalArea
    {

        /// <summary>
        /// Navigate to the 'Major Giving Setup' panel.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Configuration".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Major giving setup".</param>
        public static void MajorGivingSetup(string groupCaption = "Configuration", string taskCaption = "Major giving setup")
        {
            CollapseResearchLists();
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Collapse the Research Lists Calendar if it is expanded and wait for the collapse to complete.
        /// </summary>
        public static void CollapseResearchLists()
        {
            Panel.CollapseSection("Research Lists", "ResearchGroupNavigationViewDataForm");
        }
    }
}
