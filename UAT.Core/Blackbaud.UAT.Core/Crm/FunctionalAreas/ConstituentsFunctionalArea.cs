using System;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Dialogs;
using Blackbaud.UAT.Core.Crm.Panels;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Constituents' functional area.
    /// </summary>
    public class ConstituentsFunctionalArea : FunctionalArea
    {
        /// <summary>
        /// Open the Constituent search dialog
        /// </summary>
        public static void OpenConstituentSearchDialog()
        {
            OpenLink("Constituent search");
        }

        /// <summary>
        /// Determine if at least a single matching constituent can be found based on the provided
        /// search field value and select it.  Selects the first returned result, or cancels the search 
        /// dialog if no results are found.
        /// </summary>
        /// <param name="fieldValue">The desired value of the search criteria field.</param>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Constituents".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Constituent search".</param>
        /// <param name="dialogId">The unique id of the search dialog.  i.e. 'UniversityofOxfordConstituentSearch'  
        /// Defaults to "ConstituentSearchbyNameorLookupID".</param>
        /// <param name="fieldId">The unique id of the search dialog field.  i.e. 'LOOKUPID'  Defaults to "KEYNAME".</param>
        /// <returns>True if at least one matching element is found, false otherwise.</returns>
        public static bool ConstituentExists(string fieldValue, string groupCaption = "Constituents", string taskCaption = "Constituent search",
            string dialogId = "ConstituentSearchbyNameorLookupID", string fieldId = "KEYNAME")
        {
            OpenLink(groupCaption, taskCaption);
            SetTextField(Dialog.getXInput(dialogId, fieldId), fieldValue);
            SearchDialog.Search();
            try
            {
                SearchDialog.SelectFirstResult();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Open the dialog to add an individual.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Individuals and households".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Add an individual".</param>
        public static void AddAnIndividual(string groupCaption = "Individuals and households",
            string taskCaption = "Add an individual")
        {
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Open the dialog to add an individual, set the fields to the provided values, and save.
        /// </summary>
        /// <param name="individual">Mapping of the 'Add an individual' dialog's field captions to their desired values.</param>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Individuals and households".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Add an individual".</param>
        public static void AddAnIndividual(TableRow individual, string groupCaption = "Individuals and households",  string taskCaption = "Add an individual")
        {
            AddAnIndividual(individual, groupCaption, taskCaption, 120);
        }

        /// <summary>
        /// Open the dialog to add an individual, set the fields to the provided values, and save.
        /// </summary>
        /// <param name="individual">Mapping of the 'Add an individual' dialog's field captions to their desired values.</param>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Individuals and households".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Add an individual".</param>
        /// <param name="timeout">Time to wait for operation, default is 120 seconds.</param>
        public static void AddAnIndividual(TableRow individual, string groupCaption = "Individuals and households", string taskCaption = "Add an individual", int timeout = 120)
        {
            OpenLink(groupCaption, taskCaption);
            IndividualDialog.SetIndividualFields(individual);
            Dialog.Save();
            GetDisplayedElement(Panel.getXPanelHeader("individual"), timeout);
        }

        /// <summary>
        /// Search for a constituent and navigate to the first returned result.  An Exception is thrown if
        /// no results are found.
        /// </summary>
        /// <param name="fieldValue">The desired value of the search criteria field.</param>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Constituents".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Constituent search".</param>
        /// <param name="dialogId">The unique id of the search dialog.  i.e. 'UniversityofOxfordConstituentSearch'  
        /// Defaults to "ConstituentSearchbyNameorLookupID".</param>
        /// <param name="fieldId">The unique id of the search dialog field.  i.e. 'LOOKUPID'  Defaults to "KEYNAME".</param>
        public static void ConstituentSearch(string fieldValue, string groupCaption = "Constituents", string taskCaption = "Constituent search",
            string dialogId = "ConstituentSearchbyNameorLookupID", string fieldId = "KEYNAME")
        {
            OpenLink(groupCaption, taskCaption);
            SetTextField(Dialog.getXInput(dialogId, fieldId), fieldValue);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }
    }
}
