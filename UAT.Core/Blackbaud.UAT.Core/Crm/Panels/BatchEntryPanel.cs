using System;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Batch Entry' panel.
    /// </summary>
    public class BatchEntryPanel : Panel
    {
        /// <summary>
        /// Get Xpath for search input html field
        /// </summary>
        protected const string getXSearchInput = "//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//input[@placeholder='Search']";

        /// <summary>
        /// Create a new batch using the specified template and load the batch dialog.
        /// </summary>
        /// <param name="batchTemplate">The template to use for the new batch.</param>
        public static void AddBatch(string batchTemplate)
        {
            OpenTab("Uncommitted Batches");
            ClickSectionAddButton("Uncommitted batches");
            Dialog.SetDropDown(Dialog.getXInput("Batch2AddForm", "BATCHTEMPLATEID"), batchTemplate);
            Dialog.Save();
        }

        /// <summary>
        /// Create a new batch with the specified template and description.
        /// </summary>
        /// <param name="batchTemplate">The template to use for the new batch.</param>
        /// /// <param name="description">The description to use for the new batch.</param>
        public static void AddBatch(string batchTemplate, string description)
        {
            OpenTab("Uncommitted Batches");
            ClickSectionAddButton("Uncommitted batches");
            Dialog.SetDropDown(Dialog.getXInput("Batch2AddForm", "BATCHTEMPLATEID"), batchTemplate);
            SetTextField(Dialog.getXTextArea("Batch2AddForm", "DESCRIPTION"), description);
            Dialog.Save();
        }

        /// <summary>
        /// Check if a batch exists as a row in the uncommitted batches datalist.
        /// </summary>
        /// <param name="batch">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the batch exists, false otherwise.</returns>
        public static bool UncommittedBatchExists(TableRow batch)
        {
            OpenTab("Uncommitted Batches");
            return SectionDatalistRowExists(batch, "Uncommitted batches");
        }

        /// <summary>
        /// Select an uncommitted batch.
        /// </summary>
        /// <param name="batchRow">Mapping of the column captions to a single row's values.</param>
        public static void SelectUncommittedBatch(TableRow batchRow)
        {
            OpenTab("Uncommitted Batches");
            if (batchRow.ContainsKey("Description") && !String.IsNullOrEmpty(batchRow["Description"]))
            {
                SetTextField(getXSearchInput, batchRow["Description"]);
                GetDisplayedElement(getXSearchInput).SendKeys(Keys.Tab);
            }
            SelectSectionDatalistRow(batchRow, "Uncommitted batches");
        }

        /// <summary>
        /// Commit the currently selected batch.
        /// </summary>
        public static void CommitSelectedBatch()
        {
            WaitClick(getXSelectedDatalistRowButton("Commit"));
            Dialog.ClickButton(("Start"));
            GetDisplayedElement(getXPanelHeader("fa_batch"));
        }
    }
}
