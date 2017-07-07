namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Event Registrant' panel.
    /// </summary>
    public class RegistrantPanel : Panel
    {
        /// <summary>
        /// Check the name of the registrant on the panel.
        /// </summary>
        /// <param name="registrant">The name of the registrant to check for.</param>
        /// <returns>True if the provided name is the name of the registrant.</returns>
        public static bool IsRegistrant(string registrant)
        {
            return Exists(getXPanelHeaderByText(registrant));
        }

        /// <summary>
        /// Check the name of the event the registrant is registered for.
        /// </summary>
        /// <param name="eventName">The name of the even to check for.</param>
        /// <returns>True if the provided name is the name of the associated event.</returns>
        public static bool IsEvent(string eventName)
        {
            return GetDisplayedElement(getXPanelHeaderLink()).Text.Contains(eventName);
        }

        /// <summary>
        /// Check the number of listed guests for the registrant.
        /// </summary>
        /// <param name="numGuests">The number of guests to check for.</param>
        /// <returns>True if the provided number is the listed number of guests.</returns>
        public static bool IsNumGuests(int numGuests)
        {
            return GetDisplayedElement(getXSpan("RegistrantSummaryViewForm2", "_REGISTRANTGUESTCOUNT_value")).Text == numGuests.ToString();
        }
    }
}
