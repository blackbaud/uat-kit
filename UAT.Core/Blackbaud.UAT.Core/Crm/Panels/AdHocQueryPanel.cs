namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Ad-hoc Query' panel.
    /// </summary>
    public class AdHocQueryPanel : Panel
    {
        /// <summary>
        /// Check if the Ad-Hoc Query Panel has the provided name in the header.
        /// </summary>
        /// <param name="queryName">The value to check for in the header.</param>
        /// <returns>True if the header contains the value, false otherwise.</returns>
        public static new bool IsPanelHeader(string queryName)
        {
            WaitForPanelType("adhocquery");
            return GetDisplayedElement(getXPanelHeader()).Text.Contains(queryName);
        }
    }
}
