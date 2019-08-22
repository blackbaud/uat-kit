using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Administration' functional area.
    /// </summary>
    public class AdministrationFunctionalArea : FunctionalArea
    {
        /// <summary>
        /// Open the Enable/disable matching dialog.
        /// </summary>
        public static void EnableDisableMatching()
        {
            OpenLink("Enable/disable matching");
        }

        /// <summary>
        /// Open the Enable/disable matching dialog and set options.
        /// <param name="checkDuplicates">Check for duplicates when adding constituents.</param>
        /// <param name="checkBatchDuplicates">Check for duplicates when adding constituents via batches.</param>
        /// </summary>
        public static void EnableDisableMatching(bool checkDuplicates, bool checkBatchDuplicates)
        {
            EnableDisableMatching();
            SetCheckbox("//div[contains(@id,'dataformdialog') and contains(@style,'block')]//input[contains(@id,'_DUPLICATESEARCHENABLED_value')]", checkDuplicates);
            SetCheckbox("//div[contains(@id,'dataformdialog') and contains(@style,'block')]//input[contains(@id,'_AUTOMATCHENABLED_value')]", checkBatchDuplicates);
            Dialog.Save();
        }

    }
}
