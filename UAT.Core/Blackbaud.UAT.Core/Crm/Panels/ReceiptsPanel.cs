using System;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Receipts' panel.
    /// </summary>
    public class ReceiptsPanel : Panel
    {
        /// <summary>
        /// Add a receipt process.
        /// </summary>
        /// <param name="receipt">Mapping of the field captions to their desired values.</param>
        public static void AddReceipt(TableRow receipt)
        {
            SelectTab("Receipts");
            ClickSectionAddButton("Receipt processes");
            
            foreach (string caption in receipt.Keys)
            {
                if (receipt[caption] == null) continue;
                string value = receipt[caption];
                switch (caption)
                {
                    case "Name":
                        SetTextField(Dialog.getXInput("ReceiptingProcessAddForm3", "_NAME_value"), value);
                        break;
                    case "Output format":
                        Dialog.SetDropDown(Dialog.getXInput("ReceiptingProcessAddForm3", "_BUSINESSPROCESSVIEWID_value"), value);
                        break;
                    case "Mark revenue 'Receipted' when process completes":
                        SetCheckbox(Dialog.getXInput("ReceiptingProcessAddForm3", "_MARKRECEIPTED_value"), value);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Field '{0}' is not implemented for a receipt process dialog.", caption));
                }
            }
            Dialog.Save();
        }

        /// <summary>
        /// Select a receipt process and start it.
        /// </summary>
        /// <param name="receiptProcess">Mapping of the column captions to a single row's values.</param>
        public static void RunReceiptProcess(TableRow receiptProcess)
        {
            SelectTab("Receipts");
            SelectSectionDatalistRow(receiptProcess, "Receipt processes");

            WaitClick(getXSelectedDatalistRowButton("Start process"));
            Dialog.ClickButton(("Start"));
            WaitForPanelHeaderNotContain("Receipts");
            GetDisplayedElement(getXPanelHeader("receipt"));
        }
    }
}
