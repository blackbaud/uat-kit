using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Event' panel.
    /// </summary>
    public class EventPanel : Panel
    {
        /// <summary>
        /// Add a coordinator.
        /// </summary>
        /// <param name="constituentLastName">The last name of the constituent to add as a coordinator.</param>
        public static void AddCoordinator(string constituentLastName)
        {
            SelectTab("Tasks/Coordinators");
            ClickSectionAddButton("Coordinators");
            Dialog.SetSearchList(Dialog.getXInput("EventCoordinatorAddForm", "CONSTITUENTID") ,
                Dialog.getXInput("ConstituentSearch", "KEYNAME"), constituentLastName);
            Dialog.Save();
        }

        /// <summary>
        /// Check if the event record panel has the provided coordinator listed.
        /// </summary>
        /// <param name="coordinatorName">The name of coordinator.</param>
        /// <returns>True if the coordinator exists, false otherwise.</returns>
        public static bool CoordinatorExists(string coordinatorName)
        {
            SelectTab("Tasks/Coordinators");

            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            rowValues.Add("Coordinator", coordinatorName);

            return SectionDatalistRowExists(rowValues, "Coordinators");
        }

        /// <summary>
        /// Check if the event record panel has the provided name.
        /// </summary>
        /// <param name="eventName">The event name value to check for.</param>
        /// <returns>True if the event has the provided name, false otherwise.</returns>
        public static bool IsEventName(string eventName)
        {
            return GetDisplayedElement(getXPanelHeader("fa_events")).Text == eventName;
        }

        /// <summary>
        /// Check if the event record panel starts on the provided start date.
        /// </summary>
        /// <param name="startDate">The start date value to check for.</param>
        /// <returns>True if the event starts on the start date, false otherwise.</returns>
        public static bool IsStartDate(string startDate)
        {
            string startDateXPath = getXSpan("EventSummaryViewForm3", "EVENTDATE");
            startDateXPath = startDateXPath.Remove(startDateXPath.Length - 1);
            return Exists(startDateXPath + " and contains(./text(),'" + startDate + "')]");
        }

        /// <summary>
        /// Check if the event record panel occurs at the provided location.
        /// </summary>
        /// <param name="location">The location to check for.</param>
        /// <returns>True if the event occurs at the location, false otherwise</returns>
        public static bool IsLocation(string location)
        {
            string locationXPath = getXSpan("EventSummaryViewForm3", "LOCATION");
            locationXPath = locationXPath.Remove(locationXPath.Length - 1);
            return Exists(locationXPath + " and contains(./text(),'" + location + "')]");
        }

        /// <summary>
        /// Add a registration option.
        /// </summary>
        /// <param name="option">Mapping of field captions to their desired values.</param>
        public static void AddRegistrationOption(TableRow option)
        {
            SelectTab("Options");
            ClickSectionAddButton("Registration options");

            foreach (string caption in option.Keys)
            {
                switch (caption)
                {
                    case "Registration type":
                        Dialog.SetDropDown(Dialog.getXInput("RegistrationOptionAddForm2", "EVENTREGISTRATIONTYPEID"), option[caption]);
                        break;
                    case "Registration count":
                        SetTextField(Dialog.getXInput("RegistrationOptionAddForm2", "REGISTRATIONCOUNT"), option[caption]);
                        break;
                    case "Registration fee":
                        SetTextField(Dialog.getXInput("RegistrationOptionAddForm2", "AMOUNT"), option[caption]);
                        break;
                    case "Name":
                        SetTextField(Dialog.getXInput("RegistrationOptionAddForm2", "NAME"), option[caption]);
                        break;
                    default:
                        throw new NotImplementedException("Field " + caption + " is not implemented for the 'Add registration option' dialog.");
                }
            }
            Dialog.Save();
        }

        /// <summary>
        /// Check if a registration option exists.
        /// </summary>
        /// <param name="option">Mapping a column headers to a single row's values.</param>
        /// <returns>True if the registration option exists, false otherwise.</returns>
        public static bool RegistrationOptionExists(TableRow option)
        {
            SelectTab("Options");

            return SectionDatalistRowExists(option, "Registration options");
        }

        /// <summary>
        /// Copy the registration options from another existing event.
        /// </summary>
        /// <param name="copyFromEventName">The name of the event to copy the registration options from.</param>
        public static void CopyRegistrationOptions(string copyFromEventName)
        {
            SelectTab("Options");
            ClickSectionAddButton("Registration options", "Copy from");

            Dialog.SetSearchList(Dialog.getXInput("CopyRegistrationOptionEditForm", "SOURCEEVENTID"), Dialog.getXInput("EventSearch", "NAME"), copyFromEventName);
            Dialog.Save();
        }

        /// <summary>
        /// Add an expense under the 'Expenses' tab.
        /// </summary>
        /// <param name="expense">Mapping of the 'Add an expese' dialogs field captions to their desired values.</param>
        public static void AddExpense(TableRow expense)
        {
            SelectTab("Expenses");
            ClickSectionAddButton("Expenses");
            foreach (string caption in expense.Keys)
            {
                switch (caption)
                {
                    case "Event":
                        SetTextField(Dialog.getXInput("EventExpenseAddForm", "SELECTEDEVENTID"), expense[caption]);
                        break;
                    case "Type":
                        SetTextField(Dialog.getXInput("EventExpenseAddForm", "EVENTEXPENSETYPECODEID"), expense[caption]);
                        break;
                    case "Vendor":
                        Dialog.SetSearchList(Dialog.getXInput("EventExpenseAddForm", "VENDORID"), Dialog.getXInput("ConstituentSearch", "KEYNAME"), expense[caption]);
                        break;
                    case "Budgeted amount":
                        SetTextField(Dialog.getXInput("EventExpenseAddForm", "BUDGETEDAMOUNT"), expense[caption]);
                        break;
                    case "Agreed amount":
                        SetTextField(Dialog.getXInput("EventExpenseAddForm", "ACTUALAMOUNT"), expense[caption]);
                        break;
                    case "Amount paid":
                        SetTextField(Dialog.getXInput("EventExpenseAddForm", "AMOUNTPAID"), expense[caption]);
                        break;
                    case "Date due":
                        SetTextField(Dialog.getXInput("EventExpenseAddForm", "DATEDUE"), expense[caption]);
                        break;
                    case "Date paid":
                        SetTextField(Dialog.getXInput("EventExpenseAddForm", "DATEDUE"), expense[caption]);
                        break;
                    case "Comment":
                        SetTextField(Dialog.getXTextArea("EventExpenseAddForm", "COMMENT"), expense[caption]);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Field '{0}' is not implemented for the 'Add an expense' dialog.", caption));
                }
            }
            Dialog.Save();
        }

        /// <summary>
        /// Check if an expense exists in the 'Expenses' tab.
        /// </summary>
        /// <param name="expense">Mapping of the column captions to a row's values.</param>
        /// <returns>True if the expense exists, false otherwise.</returns>
        public static bool ExpenseExists(TableRow expense)
        {
            SelectTab("Expenses");

            return SectionDatalistRowExists(expense, "Expenses");
        }

        /// <summary>
        /// Initiate the dialog for adding a task on the 'Tasks/Coordinators' tab.
        /// 
        /// Note: This is technically a different dialog than the dialog for editing a task.
        /// </summary>
        public static void AddTaskDialog()
        {
            SelectTab("Tasks/Coordinators");
            ClickSectionAddButton("Tasks");
        }

        /// <summary>
        /// Open the 'Edit task' dialog for task in the 'Tasks/Coordinators' tab.
        /// 
        /// Note: This is technically a different dialog than the dialog for adding a task.
        /// </summary>
        /// <param name="taskName">The name of task to edit.</param>
        public static void EditTask(string taskName)
        {
            SelectTask(taskName);
            WaitClick(getXSelectedDatalistRowButton("Edit"));
        }

        /// <summary>
        /// Check if a reminder exists on task in the 'Tasks/Coordinators' tab.
        /// </summary>
        /// <param name="taskName">The name of task to check for the reminder on.</param>
        /// <param name="reminderTitle">The complete title of the reminder to check for.</param>
        /// <returns>True if the reminder exists on the task, false otherwise.</returns>
        public static bool ReminderExists(string taskName, string reminderTitle)
        {
            SelectTask(taskName);

            if (GetDisplayedElement(getXDatalistSpan("EventTaskViewForm", "REMINDERLIST_value")).Text.Contains(reminderTitle)) return true;
            return false;
        }

        /// <summary>
        /// Select a task.
        /// </summary>
        /// <param name="taskName">The row's value for the task under the 'Name' column.</param>
        private static void SelectTask(string taskName)
        {
            SelectTab("Tasks/Coordinators");

            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            rowValues.Add("Name", taskName);

            SelectSectionDatalistRow(rowValues, "Tasks");
        }

        /// <summary>
        /// Add a Preference to an Event under the 'Options' tab
        /// </summary>
        /// <param name="name">The name of the preference to add.</param>
        /// <param name="preferences">Table where each TableRow corresponds to a desired grid row's values in the 'Add a preference' dialog.</param>
        public static void AddPreference(string name, Table preferences)
        {
            SelectTab("Options");
            ClickSectionAddButton("Preferences");
            SetTextField(Dialog.getXInput("EventPreferenceGroupAddForm", "NAME"), name);

            int rowCount = 1;
            //map the column captions to their index
            IDictionary<string, int> columnCaptionToIndex = new Dictionary<string, int>();
            foreach (string caption in preferences.Rows[0].Keys)
            {
                columnCaptionToIndex.Add(caption,
                    GetDatalistColumnIndex(Dialog.getXGridHeaders("EventPreferenceGroupAddForm", "PREFERENCES_value"), caption));
            }

            //add the rows
            foreach (TableRow preference in preferences.Rows)
            {
                foreach (string caption in preference.Keys)
                {
                    //Add if value provided using appropriate field set.  Use rowCount and column caption mapping when making XPath
                    if (preference[caption] == string.Empty) continue;
                    switch (caption)
                    {
                        case "Options":
                            Dialog.SetGridTextField(Dialog.getXGridCell("EventPreferenceGroupAddForm", "PREFERENCES_value", 
                                rowCount, columnCaptionToIndex[caption]), preference[caption]);
                            break;
                        default:
                            throw new NotSupportedException(String.Format("Column '{0}' is not supported by the default 'Add a preference' dialog.  Additional implementation is required.", caption));
                    }
                }
                rowCount++;
            }

            Dialog.Save();
        }

        /// <summary>
        /// Check if a preference exists under the 'Options' tab of the current Event Panel.
        /// </summary>
        /// <param name="preference">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the preference exists, false otherwise.</returns>
        public static bool PreferenceExists(TableRow preference)
        {
            SelectTab("Options");

            return SectionDatalistRowExists(preference, "Preferences");
        }

        /// <summary>
        /// Start to add a registrant by clicking the add button under the 'Registrations' tab.
        /// </summary>
        public static void AddRegistrant()
        {
            SelectTab("Registrations");
            ClickSectionAddButton("Registrations");
        }
    }
}
