using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Base;
using SystemTest.Common;
using TechTalk.SpecFlow.Assist;
using SpecFlow.Assist.Dynamic;

namespace SystemTest.Core.Crm   
{
    public class ExportDefinitionDialog : Dialog
    {
        private const string visibleDialog =
            "//div[contains(@class,'x-window x-resizable-pinned') and contains(@style,'visible')]";

        //private new const string getXSaveButton =
        //    "//div[contains(@class,'x-window x-resizable-pinned') and contains(@style,'visible')]//button[./text() = 'Save']";

        private new const string getXSaveAndCloseButton =
            "//div[contains(@class,'x-window x-resizable-pinned') and contains(@style,'visible')]//button[./text() = 'Save and close']";

        protected static readonly string DialogId = "exportdefinitiondialog";

        //private IDictionary<string, CrmField> Supportedfields = new Dictionary<string, CrmField>
        //{
        //    {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
        //    {"Description", new CrmField("_DESCRIPTION_value", FieldType.TextArea)},
        //    {"Site", new CrmField("_SITEID_value", FieldType.Dropdown)}
        //};

        /// <summary>
        ///     Given the unique HTML element ids of a dialog and input, return a unique identifier xPath
        ///     to find the INPUT field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_individualSpouseBusinessAddForm'</param>
        /// <param name="inputId">The INPUT element's unique id identifier - i.e. '_LASTNAME_value'</param>
        //public static new string getXInput(string dialogId, string inputId)
        //{
        //    return
        //        String.Format(
        //            "//div[contains(@class,'x-window x-resizable-pinned') and contains(@style,'visible')]//div[contains(@id, '{0}')]//input[contains(@id, '{1}')]",
        //            dialogId, inputId);
        //}

        /// <summary>
        ///     Given the unique HTML element ids of a dialog and textarea, return a unique identifier xPath
        ///     to find the TEXTAREA field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_individualSpouseBusinessAddForm'</param>
        /// <param name="textAreaId">The TEXTAREA element's unique id identifier - i.e. '_DESCRIPTION_value'</param>
        //public static new string getXTextArea(string dialogId, string textAreaId)
        //{
        //    return
        //        String.Format(
        //            "//div[contains(@class,'x-window x-resizable-pinned') and contains(@style,'visible')]//div[contains(@id, '{0}')]//textarea[contains(@id, '{1}')]",
        //            dialogId, textAreaId);
        //}

        /// <summary>
        ///     Given the unique HTML element ids of a dialog and div, return a unique identifier xPath
        ///     to find the IFRAME field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_individualSpouseBusinessAddForm'</param>
        /// <param name="divId">The DIV element's unique id identifier - i.e. 'HTMLNOTE'</param>
        //public static new string getXIFrame(string dialogId, string divId)
        //{
        //    return
        //        String.Format(
        //            "//div[contains(@class,'x-window x-resizable-pinned') and contains(@style,'visible')]//div[contains(@id, '{0}')]//div[contains(@id, '{1}')]//iframe",
        //            dialogId, divId);
        //}

        /// <summary>
        ///     Set any supported field's value for any supported field type.
        /// </summary>
        /// <param name="dialogId">The unique of the dialog.  i.e. 'RevenueBatchConstituentInbatchEditForm'</param>
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
        //public static new void Save()
        //{
        //    WaitClick(getXSaveButton);
        //}

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