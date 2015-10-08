using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing an event dialog.
    /// </summary>
    public class EventDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "EventAddForm", "EventEditForm3" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
            {"Description", new CrmField("_DESCRIPTION_value", FieldType.TextArea)},
            {"Category", new CrmField("_EVENTCATEGORYCODEID_value", FieldType.Dropdown)},
            {"Event is an auction", new CrmField("_ISAUCTION_value", FieldType.Checkbox)},
            {"Do not show event on calendar", new CrmField("_HIDEFROMCALENDAR_value", FieldType.Checkbox)},
            {"Event allows designations on fees", new CrmField("_DESIGNATIONSONFEES_value", FieldType.Checkbox)},
            {"Start date", new CrmField("_STARTDATE_value", FieldType.TextInput)},
            {"Start time", new CrmField("_STARTTIME_value", FieldType.TextInput)},
            {"End date", new CrmField("_ENDDATE_value", FieldType.TextInput)},
            {"End time", new CrmField("_ENDTIME_value", FieldType.TextInput)},
            {"Appeal", new CrmField("_APPEALID_value", FieldType.Searchlist, "AppealSearch", "_NAME_value")},
            {"Location", new CrmField("_EVENTLOCATIONID_value", FieldType.Searchlist, "EventLocationSearch", "_NAME_value")},
            {"Room/Unit:", new CrmField("_EVENTLOCATIONROOMID_value", FieldType.Dropdown)},
            {"Capacity", new CrmField("_CAPACITY_value", FieldType.TextInput)},
            {"Contact", new CrmField("_EVENTLOCATIONCONTACTID_value", FieldType.Searchlist, "ConstituentSearch", "_KEYNAME_value")}
        };

        /// <summary>
        /// Set the fields to their desired values.
        /// </summary>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        public static void SetFields(TableRow fields)
        {
            SetFields(GetDialogId(DialogIds), fields, SupportedFields);
        }
    }
}
