using System;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using TechTalk.SpecFlow;

namespace BlackbaudDemo40.CRMSteps
{
    [Binding]
    public class EventsSteps : BaseSteps
    {
        [Given(@"Location ""(.*)"" exists")]
        public void GivenLocationExists(string locationName)
        {
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.Locations();
            if (!LocationsPanel.LocationExists(locationName))
            {
                LocationsPanel.AddLocation(locationName);
            }
        }

        [When(@"I add events")]
        public void WhenIAddEvents(Table eventsToAdd)
        {
            BBCRMHomePage.OpenEventsFA();
            foreach (var eventToAdd in eventsToAdd.Rows)
            {
                eventToAdd["Name"] += uniqueStamp;
                EventsFunctionalArea.AddEvent(eventToAdd);
            }
        }

        [When(@"I add coordinator '(.*)' to event '(.*)'")]
        public void WhenIAddCoordinatorToEvent(string constituentLastName, string eventName)
        {
            constituentLastName += uniqueStamp;
            eventName += uniqueStamp;
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.EventSearch(eventName);
            EventPanel.AddCoordinator(constituentLastName);
        }

        [Then(@"an event exists with the name '(.*)', start date '(.*)', and location '(.*)'")]
        public void ThenAnEventExistsWithTheNameStartDateAndLocation(string eventName, string startDate, string location)
        {
            eventName += uniqueStamp;
            if (!EventPanel.IsEventName(eventName))
                throw new ArgumentException("Current event does not have the name " + eventName);
            if (!EventPanel.IsStartDate(startDate))
                throw new ArgumentException("Current event does not have the start date " + startDate);
            if (!EventPanel.IsLocation(location))
                throw new ArgumentException("Current event does not have the location " + location);
        }

        [Given(@"an event exists")]
        public void GivenAnEventExists(Table events)
        {
            foreach (var e in events.Rows)
            {
                e["Name"] += uniqueStamp;
                BBCRMHomePage.OpenEventsFA();
                EventsFunctionalArea.AddEvent(e);
            }
        }

        [Then(@"'(.*)' is a coordinator for event '(.*)'")]
        public void ThenIsACoordinatorForEvent(string coordinatorName, string eventName)
        {
            coordinatorName += uniqueStamp;
            eventName += uniqueStamp;

            if (!EventPanel.CoordinatorExists(coordinatorName))
                throw new ArgumentException("Current event panel does not have the coordinator " + coordinatorName);
        }

        [Given(@"event management template ""(.*)"" exists")]
        public void GivenEventManagementTemplateExists(string templateName)
        {
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.EventManagementTemplates();
            if (!EventManagementTemplatesPanel.TemplateExists(templateName))
                EventManagementTemplatesPanel.AddTemplate(templateName);
        }

        [When(@"I create a multi-event using template ""(.*)""")]
        public void WhenICreateAMulti_EventUsingTemplate(string template, Table events)
        {
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.AddMultiEvent();
            MultiLevelEventDialog.SetTemplate(template);

            foreach (var eventToAdd in events.Rows)
            {
                eventToAdd["event"] += uniqueStamp;
                if (eventToAdd["parent"] == string.Empty) MultiLevelEventDialog.AddExistingEvent(eventToAdd["event"]);
                else MultiLevelEventDialog.AddChildEvent(eventToAdd["event"], eventToAdd["parent"] + uniqueStamp);
            }
            Dialog.Save();
        }

        [Then(@"a multi-level event ""(.*)"" exists with hierarchy")]
        public void ThenAMulti_LevelEventExistsWithHierarchy(string multiEventName, Table hierachy)
        {
            multiEventName = multiEventName.Insert(multiEventName.IndexOf(" - Default Summary"), uniqueStamp);
            if (!MultiEventPanel.IsMultiEventName(multiEventName))
                throw new ArgumentException("Current multi event page does not have the name " + multiEventName);
            foreach (var row in hierachy.Rows)
            {
                row["event"] += uniqueStamp;
                if (row["parent"] != string.Empty) row["parent"] += uniqueStamp;
            }
            if (!MultiEventPanel.IsHierarchy(hierachy))
                throw new ArgumentException("Current multi event page does not have the hierarchy " + hierachy);
        }

        [When(@"I add a registration option to event '(.*)'")]
        public void WhenIAddARegistrationOptionToEvent(string eventName, Table registrationOptions)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            foreach (var option in registrationOptions.Rows)
            {
                EventPanel.AddRegistrationOption(option);
            }
        }

        [Then(@"event ""(.*)"" has registration option")]
        public void ThenEventHasRegistrationOption(string eventName, Table registrationOptions)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            foreach (var option in registrationOptions.Rows)
            {
                if (!EventPanel.RegistrationOptionExists(option))
                    throw new ArgumentException("'" + option +
                                                "' was not found on the Registration options datalist for event " +
                                                eventName + ".");
            }
        }

        [Given(@"event '(.*)' has registration option")]
        public void GivenEventHasRegistrationOption(string eventName, Table registrationOptions)
        {
            WhenIAddARegistrationOptionToEvent(eventName, registrationOptions);
        }

        [When(@"I copy registration options from '(.*)' to '(.*)'")]
        public void WhenICopyRegistrationOptionsFromTo(string copyFromEvent, string copyToEvent)
        {
            copyFromEvent += uniqueStamp;
            copyToEvent += uniqueStamp;
            GetEventPanel(copyToEvent);
            EventPanel.CopyRegistrationOptions(copyFromEvent);
        }

        [When(@"I add expense to '(.*)'")]
        public void WhenIAddExpenseTo(string eventName, Table expenses)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            foreach (var expense in expenses.Rows)
            {
                if (expense.Keys.Contains("Vendor") && expense["Vendor"] != string.Empty)
                    expense["Vendor"] = expense["Vendor"] + uniqueStamp;
                EventPanel.AddExpense(expense);
            }
        }

        [Then(@"event '(.*)' has expense")]
        public void ThenEventHasExpense(string eventName, Table expenses)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            foreach (var expense in expenses.Rows)
            {
                if (expense.Keys.Contains("Vendor") && expense["Vendor"] != string.Empty)
                    expense["Vendor"] = expense["Vendor"] + uniqueStamp;
                if (!EventPanel.ExpenseExists(expense))
                    throw new ArgumentException(String.Format("Event '{0}' does not have expense '{1}'", eventName,
                        expense));
            }
        }

        [When(@"I add a task to event '(.*)'")]
        public void WhenIAddATaskToEvent(string eventName, Table tasks)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            foreach (var task in tasks.Rows)
            {
                if (task.Keys.Contains("Owner") && task["Owner"] != string.Empty)
                    task["Owner"] = task["Owner"] + uniqueStamp;
                EventPanel.AddTaskDialog();
                TaskDialog.SetFields(task);
                Dialog.Save();
            }
        }

        [When(@"add reminder to task '(.*)' on event '(.*)'")]
        public void WhenAddReminderToTaskOnEvent(string taskName, string eventName, Table reminders)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            EventPanel.EditTask(taskName);
            TaskDialog.SetReminders(reminders);
            Dialog.Save();
        }

        [Then(@"reminder '(.*)' exists for task '(.*)'")]
        public void ThenReminderExistsForTask(string reminder, string task)
        {
            if (!EventPanel.ReminderExists(task, reminder))
                throw new ArgumentException(String.Format("Reminder '{0}' does not exist for task '{1}'", reminder, task));
        }

        [When(@"I start to create a multi-event using template ""(.*)""")]
        public void WhenIStartToCreateAMulti_EventUsingTemplate(string template, Table events)
        {
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.AddMultiEvent();
            MultiLevelEventDialog.SetTemplate(template);

            foreach (var eventToAdd in events.Rows)
            {
                eventToAdd["event"] += uniqueStamp;
                if (eventToAdd["parent"] == string.Empty) MultiLevelEventDialog.AddExistingEvent(eventToAdd["event"]);
                else MultiLevelEventDialog.AddChildEvent(eventToAdd["event"], eventToAdd["parent"] + uniqueStamp);
            }
        }

        [When(@"I copy from a sub-event ""(.*)"" and name it ""(.*)"" under ""(.*)""")]
        public void WhenICopyFromASub_EventAndNameItUnder(string copyFromEventName, string newEventName,
            string parentEventName)
        {
            copyFromEventName += uniqueStamp;
            newEventName += uniqueStamp;
            parentEventName += uniqueStamp;
            MultiLevelEventDialog.CopyFrom(copyFromEventName, parentEventName, newEventName);
            Dialog.Save();
        }

        [When(@"I add preference ""(.*)"" to event ""(.*)""")]
        public void WhenIAddPreferenceToEvent(string preferenceName, string eventName, Table options)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            EventPanel.AddPreference(preferenceName, options);
        }

        [Then(@"event ""(.*)"" has a preference with options")]
        public void ThenEventHasAPreferenceWithOptions(string eventName, Table preferences)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            foreach (var preference in preferences.Rows)
            {
                if (!EventPanel.PreferenceExists(preference))
                    throw new ArgumentException(String.Format("'{0}' is not a preference for event '{1}'", preference,
                        eventName));
            }
        }

        [When(@"I add registrant '(.*)' to event '(.*)'")]
        public void WhenIAddRegistrantToEvent(string registrant, string eventName, Table registrations)
        {
            registrant += uniqueStamp;
            eventName += uniqueStamp;
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.EventSearch(eventName);
            EventPanel.AddRegistrant();
            RegistrantDialog.SetRegistrant(registrant);

            foreach (var registrantRow in registrations.Rows)
            {
                if (registrantRow.ContainsKey("Registrant") && !string.IsNullOrEmpty(registrantRow["Registrant"])
                    && registrantRow["Registrant"] != "(Unnamed guest)") registrantRow["Registrant"] += uniqueStamp;
            }

            RegistrantDialog.SetRegistrants(registrations);
            Dialog.Save();
        }

        [Then(@"registrant record '(.*)' is created for event '(.*)' with (.*) guest")]
        public void ThenRegistrantRecordIsCreatedForEventWithGuest(string registrant, string eventName, int numGuests)
        {
            registrant += uniqueStamp;
            eventName += uniqueStamp;
            if (!RegistrantPanel.IsRegistrant(registrant))
                throw new ArgumentException(String.Format("'{0}' is not the name of the registrant", registrant));
            if (!RegistrantPanel.IsEvent(eventName))
                throw new ArgumentException(String.Format("'{0}' is not the event for the registrant", eventName));
            if (!RegistrantPanel.IsNumGuests(numGuests))
                throw new ArgumentException(String.Format("'{0}' is not the number of guests for the registrant",
                    numGuests));
        }

        private void GetEventPanel(string eventName)
        {
            if (!Panel.IsPanelHeader(eventName))
            {
                BBCRMHomePage.OpenEventsFA();
                EventsFunctionalArea.EventSearch(eventName);
            }
        }
    }
}