using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for ExportDefinitionDialog dialog.
    /// </summary>
    public class ExportDefinitionDialog : Dialog
    {
        private const string visibleDialog = "//div[contains(@class,'x-window x-resizable-pinned') and contains(@style,'visible')]";

        private new const string getXSaveAndCloseButton = "//div[contains(@class,'x-window x-resizable-pinned') and contains(@style,'visible')]//button[./text() = 'Save and close']";

        /// <summary>
        /// Add DialogId html ID
        /// </summary>
        protected static readonly string DialogId = "exportdefinitiondialog";

        /// <summary>
        ///     Set any supported field's value for any supported field type.
        /// </summary>
        /// <param name="caption">The caption of the field.</param>
        /// <param name="value">The desired value of the field.</param>
        /// <param name="supportedFields">
        ///     Mapping of the supported field captions to a CrmField object encapsulating all relevant variables
        ///     needed to set the field's value.
        /// </param>
        public static void SetField(string caption, string value, IDictionary<string, CrmField> supportedFields)
        {
            if (!supportedFields.ContainsKey(caption))
                throw new NotImplementedException(String.Format("Field '{0}' is not implemented.", caption));
            var crmField = supportedFields[caption];
            switch (crmField.CellType)
            {
                case FieldType.Checkbox:
                    BaseComponent.SetCheckbox(getXInput(DialogId, crmField.Id), value);
                    break;
                case FieldType.Dropdown:
                    Dialog.SetDropDown(getXInput(DialogId, crmField.Id), value);
                    break;
                case FieldType.Searchlist:
                    Dialog.SetSearchList(getXInput(DialogId, crmField.Id),
                        getXInput(crmField.SearchDialogId, crmField.SearchDialogFieldId), value);
                    break;
                case FieldType.TextInput:
                    BaseComponent.SetTextField(getXInput(DialogId, crmField.Id), value);
                    break;
                case FieldType.TextArea:
                    BaseComponent.SetTextField(getXTextArea(DialogId, crmField.Id), value);
                    break;
                case FieldType.TextIframe:
                    BaseComponent.SetTextField(getXIFrame(DialogId, crmField.Id), value);
                    break;
                default:
                    throw new NotImplementedException(String.Format("Field type '{0}' is not implemented.",
                        crmField.CellType));
            }
        }

        /// <summary>
        ///     Click the 'Save' button
        /// </summary>
        public static void SaveAndClose()
        {
            //check name is populated
            if (
                string.IsNullOrEmpty(
                    BaseComponent.GetEnabledElement(visibleDialog + "//input[contains(@id,'_NAME_value')]")
                        .GetAttribute("value")
                        .ToString()))
            {
                throw new ApplicationException(
                    "ExportDefinitionDialog: SaveAndClose() triggered before name field filled!");
            }
            WaitClick(getXSaveAndCloseButton);
        }
    }
}