using System;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for adding/editing an add a multi-level event dialog.
    /// </summary>
    public class MultiLevelEventDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "MultilevelEventAddDataForm" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static string getXAddedEvent(string existingEvent)
        {
            return String.Format("//li[@class='x-tree-node']//span[text()=\"{0}\"]", existingEvent);
        }

        /// <summary>
        /// Set the 'Event management template' dropdown.
        /// </summary>
        /// <param name="templateName">The name of the template to set.</param>
        public static void SetTemplate(string templateName)
        {
            SetDropDown(getXInput(GetDialogId(DialogIds), "EVENTMANAGEMENTTEMPLATEID"), templateName);
        }

        /// <summary>
        /// Add an event to the multi-level event.  If an event if currently selected in
        /// the hierachy, then the new event will be added a child to the selected event.
        /// </summary>
        /// <param name="eventName">The name of the event to add.</param>
        public static void AddExistingEvent(string eventName)
        {
            WaitClick(getXButton(GetDialogId(DialogIds), "Add existing"));
            SetTextField(getXInput("EventSearch", "NAME"), eventName);
            WaitClick(SearchDialog.getXSearchButton);
            SearchDialog.SelectFirstResult();
        }

        /// <summary>
        /// Add an event to the multi-level event as a child to a specified event
        /// already added to the multi-level event.
        /// </summary>
        /// <param name="eventName">The name of the event to add.</param>
        /// <param name="parentEventName">The name of the parent event.</param>
        public static void AddChildEvent(string eventName, string parentEventName)
        {
            WaitClick(getXAddedEvent(parentEventName));
            AddExistingEvent(eventName);
        }

        /// <summary>
        /// Select a parent event to add a new child event to.  The child event will be copied from an existing event.
        /// </summary>
        /// <param name="copyFromEventName">The name of the event to copy.</param>
        /// <param name="parentEventName">The name of the parent event to add the new event as a child to.</param>
        /// <param name="newEventName">The name to set for the new event.</param>
        public static void CopyFrom(string copyFromEventName, string parentEventName, string newEventName)
        {
            string dialogId = GetDialogId(DialogIds);
            WaitClick(getXAddedEvent(parentEventName));
            WaitClick(getXButton(dialogId, "Copy from"));
            SetSearchList(getXInput("MultilevelEventAddEventAddForm", "_COPYFROMEVENTID_value"), 
                getXInput("EventSearch", "NAME"), copyFromEventName);
            SetTextField(getXInput("MultilevelEventAddEventAddForm", "NAME"), newEventName);
            OK();
        }
    }
}
