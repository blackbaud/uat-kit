namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    ///  Class to handle interactions for the 'Marketing and Communications' functional area.
    /// </summary>
    public class MarketingAndCommFunctionalArea : FunctionalArea
    {
        /// <summary>
        /// Navigate to the 'Receipts' panel.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Donor relations".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Receipts"</param>
        public static void Receipts(string groupCaption = "Donor relations", string taskCaption = "Receipts")
        {
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Navigate to the 'Marketing Acknowledgements' panel.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Marketing efforts".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Marketing acknowledgements"</param>
        public static void MarketingAcknowledgements(string groupCaption = "Marketing efforts", string taskCaption = "Marketing acknowledgements")
        {
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Navigate to the 'Segments' panel.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Acquisition and segmentation".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Segments"</param>
        public static void Segments(string groupCaption = "Acquisition and segmentation", string taskCaption = "Segments")
        {
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Navigate to the 'Packages' panel and then navigate to the 'View packages' panel.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Packages".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Packages"</param>
        public static void Packages(string groupCaption = "Packages", string taskCaption = "Packages")
        {
            OpenLink(groupCaption, taskCaption);
            OpenLink("Packages", "View packages");
        }
    }
}
