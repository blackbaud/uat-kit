using System;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Multi-Event' panel.
    /// </summary>
    public class MultiEventPanel : Panel
    {
        /// <summary>
        /// Check if the multi event record page has the provided name.
        /// </summary>
        /// <param name="name">The multi event name value to check for.</param>
        /// <returns>True if the multi event has the provided name, false otherwise.</returns>
        public static bool IsMultiEventName(string name)
        {
            return GetDisplayedElement(getXPanelHeader("fa_events")).Text == name;
        }

        /// <summary>
        /// Check if the multi-event has the provided event hierarchy.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to validate.  A column 'event' should exist for the event
        /// name with a column 'parent' mapping the expected parent of the event.  If no value is provided in the 'parent' column,
        /// it is assumed that this is the root event.</param>
        /// <returns>True if the multi-event has the provided hierarchy, false otherwise.</returns>
        public static bool IsHierarchy(Table hierarchy)
        {
            SelectTab("Events");
            foreach (TableRow eventRow in hierarchy.Rows)
            {
                string eventName = eventRow["event"];
                string parent = eventRow["parent"];
                if (parent == string.Empty && !IsRootEvent(parent)) return false;
                else if (parent != string.Empty && !IsChildOfParent(eventName, parent)) return false;
            }
            return true;
        }

        /// <summary>
        /// Check if the provided event name is displayed as the root event for the Multi-Event page
        /// </summary>
        private static bool IsRootEvent(string eventName)
        {
            return Exists(String.Format("//table[@class='x-treegrid-root-table']/tbody[@class='x-tree-node']/tr[contains(@class,'x-tree-node-el')]/td[1]/a[contains(./text(),'{0}')]", eventName));
        }

        /// <summary>
        /// Check if the provided event is listed as a child of the expected parent event.
        /// </summary>
        private static bool IsChildOfParent(string eventName, string parent)
        {
            return Exists(String.Format("//a[contains(./text(),'{0}')]/../../../../../../../tr[contains(@class,'x-tree-node-el')]/td[1]/a[contains(./text(),'{1}')]", eventName, parent));
        }
    }
}
