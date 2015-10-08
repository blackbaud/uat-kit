using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle the interactions for adding/editing an event registrant dialog.
    /// </summary>
    public class RegistrantDialog : Dialog
    {
        /// <summary>
        /// Static list of supported unique dialog id.
        /// </summary>
        protected static readonly string[] DialogIds = { "RegistrantUnifiedAddForm", "RegistrantUnifiedEditForm" };

        /// <summary>
        /// Static mapping of supported field captions to CrmField objects encapsulating all relevant variables
        /// needed to set the field's value.
        /// </summary>
        protected static readonly IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
        {
            {"Registration option", new CrmField(null, FieldType.Dropdown)},
            {"Registrant", new CrmField(null, FieldType.Searchlist, "ConstituentSearch", "_KEYNAME_value")},
        };

        /// <summary>
        /// Set the registrant.
        /// </summary>
        /// <param name="registrant"></param>
        public static void SetRegistrant(string registrant)
        {
            var dialogId = GetDialogId(DialogIds);
            OpenTab("Single Events", dialogId);

            SetSearchList(getXInput(dialogId, "_CONSTITUENTID_value"), getXInput("ConstituentSearch", "_KEYNAME_value"), registrant);
        }

        /// <summary>
        /// Set the registrants and registration options in the registration grid.
        /// </summary>
        /// <param name="registrants">Mapping of the column captions to a single row's values.</param>
        public static void SetRegistrants(Table registrants)
        {
            var dialogId = GetDialogId(DialogIds);
            OpenTab("Single Events", dialogId);

            var rowCount = 1;
            var columnCaptionToIndex = MapColumnCaptionsToIndex(registrants.Rows[0].Keys,
                getXGridHeaders(dialogId, "_EVENTREGISTRANTS"));

            foreach (TableRow registrant in registrants.Rows)
            {
                if (registrant.ContainsKey("Registrant") && registrant["Registrant"] == "(Unnamed guest)")
                {
                    SetGridDropDown(getXGridCell(dialogId, "_EVENTREGISTRANTS", rowCount,
                        columnCaptionToIndex["Registrant"]), "(Unnamed guest)");
                    registrant["Registrant"] = null;
                }
                SetGridRow(dialogId, "_EVENTREGISTRANTS", registrant, rowCount, columnCaptionToIndex, SupportedFields);
                rowCount++;
            }
        }
    }
}
