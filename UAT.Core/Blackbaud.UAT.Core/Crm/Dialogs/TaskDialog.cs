using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing a task dialog.
    /// </summary>
    public class TaskDialog : Dialog
    {

        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "EventTaskAddForm", "EventTaskEditForm" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Event", new CrmField("SELECTEDMULTILEVELEVENTID", FieldType.Dropdown)},
            {"Name", new CrmField("NAME", FieldType.TextInput)},
            {"Comment", new CrmField("COMMENT", FieldType.TextArea)},
            {"Owner", new CrmField("OWNERID", FieldType.Searchlist, "EventTaskOwnerSearch", "KEYNAME")},
            {"Date due", new CrmField("COMPLETEBYDATE", FieldType.TextInput)}
        };

        /// <summary>
        /// Static mapping of supported Grid column captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedRemindersGridFields = new Dictionary<string, CrmField>
        {
            {"Name", new CrmField(null, FieldType.TextInput)},
            {"Date", new CrmField(null, FieldType.TextInput)}
        };

        /// <summary>
        /// Set the fields to their desired values.
        /// </summary>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        public static void SetFields(TableRow fields)
        {
            SetFields(GetDialogId(DialogIds), fields, SupportedFields);
        }

        /// <summary>
        /// Set the reminders or the task.  Always starts at the first row, replacing whatever
        /// existing rows might exist.
        /// </summary>
        /// <param name="reminders">Table where each row represents a reminder to add.  
        /// A row is a mapping of the 'Reminders' grid's column captions to the row's values.  </param>
        public static void SetReminders(Table reminders)
        {
            var dialogId = GetDialogId(DialogIds);

            var columnCaptionToIndex = MapColumnCaptionsToIndex(reminders.Rows[0].Keys,
                getXGridHeaders(dialogId, "REMINDERS_value"));

            SetGridRows(dialogId, "REMINDERS_value", reminders, 1, columnCaptionToIndex, SupportedRemindersGridFields);
        }
    }
}
