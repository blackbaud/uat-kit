using System;
using Blackbaud.UAT.Core.Crm.Dialogs;
using Blackbaud.UAT.Core.Crm.Panels;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Fundraising' functional area.
    /// </summary>
    public class FundraisingFunctionalArea : FunctionalArea
    {
        /// <summary>
        /// Check if a designation exists and navigate to it if so.
        /// </summary>
        /// <param name="designation">The name of the designation.</param>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Fundraising".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Designation search"</param>
        /// <param name="dialogId">The unique id of the search dialog.  i.e. 'UniversityofOxfordConstituentSearch'  
        /// Defaults to "DesignationSearch".</param>
        /// <param name="fieldId">The unique id of the search dialog field.  i.e. 'LOOKUPID'  Defaults to "COMBINEDSEARCH".</param>
        /// <returns>True if the designation exists, false otherwise.</returns>
        public static bool DesignationExists(string designation, string groupCaption = "Fundraising",
            string taskCaption = "Designation search", string dialogId = "DesignationSearch", string fieldId = "COMBINEDSEARCH")
        {
            OpenLink(groupCaption, taskCaption);
            SetTextField(Dialog.getXInput(dialogId, fieldId), designation);
            try
            {
                SearchDialog.Search();
                SearchDialog.SelectFirstResult();
                GetDisplayedElement(Panel.getXPanelHeader("fa_fundraisinginitiatives"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
