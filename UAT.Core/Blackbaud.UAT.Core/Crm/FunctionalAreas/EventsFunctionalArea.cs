using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Events' functional area.
    /// </summary>
    public class EventsFunctionalArea : FunctionalArea
    {

        /// <summary>
        /// Open the dialog to add an event.
        /// </summary>
        public static void AddEvent()
        {
            WaitClick(getXAddMenu("ADDNEWGROUP"));
            WaitClick(getXMenuItem("Event"));
        }

        /// <summary>
        /// Open the dialog to add an event, set the fields to their desired values, and save.
        /// </summary>
        /// <param name="eventValue">Dictionary mapping field caption keys to their values.</param>
        public static void AddEvent(TableRow eventValue)
        {
            WaitClick(getXAddMenu("ADDNEWGROUP"));
            WaitClick(getXMenuItem("Event"));
            EventDialog.SetFields(eventValue);
            Dialog.Save();
        }

        /// <summary>
        /// Open the dialog for adding a 'Multi-level event'.
        /// </summary>
        public static void AddMultiEvent()
        {
            WaitClick(getXAddMenu("ADDNEWGROUP"));
            WaitClick(getXMenuItem("Multi-level event"));
        }

        /// <summary>
        /// Search for an event and select the first matching record.
        /// </summary>
        /// <param name="fieldValue">The desired value of the search criteria field.</param>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Fundraising events".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Event search".</param>
        /// <param name="dialogId">The unique id of the search dialog.  i.e. 'UniversityofOxfordConstituentSearch'  
        /// Defaults to "EventSearch".</param>
        /// <param name="fieldId">The unique id of the search dialog field.  i.e. 'LOOKUPID'  Defaults to "NAME".</param>
        public static void EventSearch(string fieldValue, string groupCaption = "Fundraising events", string taskCaption = "Event search",
            string dialogId = "EventSearch", string fieldId = "NAME")
        {
            CollapseCalendar();
            OpenLink(groupCaption, taskCaption);
            SetTextField(Dialog.getXInput(dialogId, fieldId), fieldValue);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }

        /// <summary>
        /// Navigate to the 'Locations' panel.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Fundraising events".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Locations".</param>
        public static void Locations(string groupCaption = "Fundraising events", string taskCaption = "Locations")
        {
            CollapseCalendar();
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Navigate to the 'Event Management Templates' panel.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Fundraising events".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Event management templates".</param>
        public static void EventManagementTemplates(string groupCaption = "Fundraising events", string taskCaption = "Event management templates")
        {
            CollapseCalendar();
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Collapse the Event Calendar if it is expanded and wait for the collapse to complete.
        /// </summary>
        public static void CollapseCalendar()
        {
            Panel.CollapseSection("Event calendar", "CalendarViewForm");
        }
    }
}
