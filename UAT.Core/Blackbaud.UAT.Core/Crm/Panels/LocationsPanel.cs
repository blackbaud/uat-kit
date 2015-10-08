using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Locations' panel.
    /// </summary>
    public class LocationsPanel : Panel
    {
        /// <summary>
        /// Checks if a location exists.
        /// </summary>
        /// <param name="locationName">The name of the location.</param>
        /// <returns>True if the location exists, false otherwise.</returns>
        public static bool LocationExists(string locationName)
        {
            SelectTab("Event Locations");

            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            rowValues.Add("Name", locationName);

            return SectionDatalistRowExists(rowValues, "Event locations");
        }

        /// <summary>
        /// Checks if a location exists.
        /// </summary>
        /// <param name="location">Mapping of the column captions to the desired location's values.</param>
        /// <returns>True if the location exists, false otherwise.</returns>
        public static bool LocationExists(TableRow location)
        {
            SelectTab("Event Locations");

            return SectionDatalistRowExists(location, "Event locations");
        }

        /// <summary>
        /// Add an event location.
        /// </summary>
        /// <param name="locationName">The name of the location.</param>
        public static void AddLocation(string locationName)
        {
            SelectTab("Event Locations");
            ClickSectionAddButton("Event locations");

            SetTextField(Dialog.getXInput("EventLocationAddForm", "NAME"),locationName);
            Dialog.Save();
        }
    }
}
