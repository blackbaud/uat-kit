namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Analysis' functional area.
    /// </summary>
    public class AnalysisFunctionalArea : FunctionalArea
    {
        /// <summary>
        /// TODO - THIS IS A CUSTOMIZATION IMPLEMENTATION FOR BHF AND SHOULD BE MOVED TO THEIR TEST SOLUTION
        /// </summary>
        public static void OpenQueryPanel()
        {
            OpenLink("Query");
        }

        /// <summary>
        /// Navigate to the 'Information Library' Panel
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Information library".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Information library".</param>
        public static void InformationLibrary(string groupCaption = "Information library",
            string taskCaption = "Information library")
        {
            OpenLink(groupCaption, taskCaption);
        }
    }
}
