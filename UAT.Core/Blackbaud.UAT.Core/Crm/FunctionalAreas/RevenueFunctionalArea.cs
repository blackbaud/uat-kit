using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Dialogs;
using Blackbaud.UAT.Core.Crm.Panels;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Revenue' functional area.
    /// </summary>
    public class RevenueFunctionalArea : FunctionalArea
    {
        /// <summary>
        /// Open the dialog to add a payment.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Transactions".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Add a payment".</param>
        public static void AddAPayment(string groupCaption = "Transactions", string taskCaption = "Add a payment")
        {
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Open the dialog to add a pledge.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Transactions".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Add a pledge".</param>
        public static void AddAPledge(string groupCaption = "Transactions", string taskCaption = "Add a pledge")
        {
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Open the dialog to add a pledge, set its values, and save.
        /// </summary>
        /// <param name="pledge">Mapping of the 'Add a pledge' dialog's field captions to their desired values.</param>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Transactions".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Add a pledge".</param>
        public static void AddAPledge(TableRow pledge, string groupCaption = "Transactions", string taskCaption = "Add a pledge")
        {
            OpenLink(groupCaption, taskCaption);
            PledgeDialog.SetFields(pledge);
        }

        /// <summary>
        /// Navigate to the 'Batch Entry' panel
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Transactions".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Batch entry".</param>
        public static void BatchEntry(string groupCaption = "Transactions", string taskCaption = "Batch entry")
        {
            OpenLinkToPanel(groupCaption, taskCaption, "Batch Entry");
        }

        /// <summary>
        /// Navigate to the 'Pledge Subtypes' panel
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Configuration".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Pledge subtypes".</param>
        public static void PledgeSubtypes(string groupCaption = "Configuration", string taskCaption = "Pledge subtypes")
        {
            OpenLink(groupCaption, taskCaption);
        }

        /// <summary>
        /// Search for a revenue record by all records associated with a constituent
        /// and select the first returned result.
        /// </summary>
        /// <param name="constituent">The last name of the constituent.</param>
        /// <param name="groupCaption">The group header caption of the task.  Defaults to "Transactions".</param>
        /// <param name="taskCaption">The link caption of the task.  Defaults to "Transaction search".</param>
        public static void TransactionSearchByConstituent(string constituent, string groupCaption = "Transactions",
            string taskCaption = "Transaction search")
        {
            OpenLink(groupCaption, taskCaption);
            SetTextField(Dialog.getXInput("TransactionSearch", "KEYNAME"), constituent);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
            GetDisplayedElement(Panel.getXPanelHeader("fa_revenue"));
        }
    }
}
