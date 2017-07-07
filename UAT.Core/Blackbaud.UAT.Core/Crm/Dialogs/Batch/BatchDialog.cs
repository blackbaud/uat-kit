using System;
using System.Collections.Generic;
using System.Linq;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Panels;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Base class to handle the interactions for adding/editing a batch dialog.
    /// </summary>
    public class BatchDialog : Dialog
    {
        /// <summary>
        /// BatchDialogId constant
        /// </summary>
        protected const string BatchDialogId = "_Batch";
        /// <summary>
        /// BatchDialogGridId constant
        /// </summary>
        protected const string BatchDialogGridId = "_BatchForm";

        /// <summary>
        /// Maps batch grid column captions to their required values in order to set their values in rows.
        /// </summary>
        protected static readonly Dictionary<string, CrmField> ColumnToBatchColumn = new Dictionary<string, CrmField> 
        { 
            {"Account system", new CrmField("PDACCOUNTSYSTEMID", FieldType.Dropdown)},
            {"Constituent",new CrmField("CONSTITUENTID", FieldType.Searchlist, "BatchRevenueConstituentSearch", "KEYNAME")},
            {"Lookup ID",new CrmField("CONSTITUENTLOOKUPID", FieldType.Searchlist, "RevenueConstituentSearchbyLookupID", "KEYNAME")},
            {"Amount",new CrmField("AMOUNT", FieldType.TextInput)},
            {"Date",new CrmField("DATE", FieldType.TextInput)},
            {"Revenue type",new CrmField("TYPECODE", FieldType.Dropdown)},
            {"Payment method",new CrmField("PAYMENTMETHODCODE", FieldType.Dropdown)},
            {"Application",new CrmField("APPLICATIONINFO", FieldType.TextInput)},
            {"Designation",new CrmField("SINGLEDESIGNATIONID", FieldType.Searchlist, "DesignationSearch", "COMBINEDSEARCH")},
            {"GL post status",new CrmField("POSTSTATUSCODE", FieldType.Dropdown)},
            {"Installment frequency",new CrmField("INSTALLMENTFREQUENCYCODE", FieldType.Dropdown)},
            {"Installment start date",new CrmField("INSTALLMENTSTARTDATE", FieldType.TextInput)},
            {"Installment end date",new CrmField("INSTALLMENTENDDATE", FieldType.TextInput)},
            {"No. installments",new CrmField("NUMBEROFINSTALLMENTS", FieldType.TextInput)},
            {"Appeal",new CrmField("APPEALID", FieldType.Searchlist, "RevenueAppealSearch", "NAME")},
            {"Opportunity",new CrmField("OPPORTUNITYID", FieldType.Searchlist, "OpportunitySearch", "KEYNAME")},
            {"Source code",new CrmField("SOURCECODE", FieldType.Searchlist, "SourceCodeMapSearch", "SOURCECODE")},
            {"Effort",new CrmField("MAILINGID", FieldType.Searchlist, "MarketingEffortSearch", "NAME")},
            {"Other type",new CrmField("OTHERTYPECODEID", FieldType.TextInput)},
            {"Given anonymously",new CrmField("GIVENANONYMOUSLY", FieldType.Checkbox)},
            {"Pledge subtype", new CrmField("PLEDGESUBTYPEID", FieldType.Dropdown)},
            {"Reference", new CrmField("REFERENCE", FieldType.TextInput)}
        };

        /// <summary>
        /// Unique XPath to get the header columns TR element of a batch's grid.
        /// </summary>
        public const string getXBatchGridHeaders = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'_Batch')]//div[contains(@id, '_BatchForm')]//div[contains(@class,'bbui-forms-collectiongrid') and not(contains(@class,'readonly'))]//div[@class='x-grid3-header']//tr";


        /// <summary>
        /// Get the index of the column in the batch grid.
        /// </summary>
        /// <param name="caption">The caption of the column.</param>
        /// <returns>The index of the TD column element within the TR headers.</returns>
        public static int GetColumnIndexByCaption(string caption)
        {
            string columnId = ColumnToBatchColumn[caption].Id;
            return GetColumnIndexById((columnId));
        }

        /// <summary>
        /// Get the index of the column in the batch grid.
        /// </summary>
        /// <param name="columnId">The unique id of the column.</param>
        /// <returns>The index of the TD column element within the TR headers.</returns>
        public static int GetColumnIndexById(string columnId)
        {
            int columnIndex = -1;

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            waiter.Until(d =>
            {
                IWebElement headersRow = GetDisplayedElement(getXBatchGridHeaders, TimeoutSecs);
                if (headersRow.Displayed != true) return false;
                IList<IWebElement> columns = headersRow.FindElements(By.XPath(".//td")).ToList();

                foreach (IWebElement column in columns)
                {
                    //columns may not be displayed.  do not use displayed as a conditional check
                    if (column.GetAttribute("class").Contains(columnId))
                    {
                        columnIndex = columns.IndexOf(column) + 1;
                        return true;
                    }
                }
                return false;
            });

            if (columnIndex == -1) throw new NoSuchElementException(String.Format("No column found with the id '{0}'", columnId));
            return columnIndex;
        }

        /// <summary>
        /// Get the value of a batch grid cell.
        /// </summary>
        /// <param name="caption">The column caption of the cell.</param>
        /// <param name="row">The row index of the cell.  The first row is index 1.</param>
        /// <returns>The value of the cell.</returns>
        public static string GetGridCellValue(string caption, int row)
        {
            int columnIndex = GetColumnIndexByCaption(caption);
            string cellXPath = getXGridCell(BatchDialogId, BatchDialogGridId, row, columnIndex);

            GetDisplayedElement(cellXPath);
            WaitClick(cellXPath);

            //TODO may need to get the edit cell instead for some situations...
            return GetDisplayedElement(cellXPath).Text;
        }

        /// <summary>
        /// Set the value of a batch grid cell for a specified row.
        /// </summary>
        /// <param name="caption">The caption of the column.</param>
        /// <param name="value">The desired value of the row.</param>
        /// <param name="row">The index of the row.  The first row's index is 1.</param>
        public static void SetGridCell(string caption, string value, int row)
        {
            int columnIndex = GetColumnIndexByCaption(caption);
            string cellXPath = getXGridCell(BatchDialogId, BatchDialogGridId, row, columnIndex);

            //use the caption to determine the grid setter utility method to use
            switch (ColumnToBatchColumn[caption].CellType)
            {
                case FieldType.Checkbox:
                    //TODO implement SetGridCheckbox
                    throw new NotImplementedException();
                case FieldType.Dropdown:
                    SetGridDropDown(cellXPath, value);
                    break;
                case FieldType.TextInput:
                    SetGridTextField(cellXPath, value);
                    break;
                case FieldType.Searchlist:
                    SetGridSearchList(cellXPath,getXInput(ColumnToBatchColumn[caption].SearchDialogId, 
                                ColumnToBatchColumn[caption].SearchDialogFieldId), value);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Set the rows of a batch grid starting with the first row.
        /// </summary>
        /// <param name="batchRows">Table where each TableRow represents a grid row to set.
        /// A TableRow is a mapping of the batch column captions to a single row's desired values.</param>
        public static void SetGridRows(Table batchRows)
        {
            int rowCount = 1;
            //go through each row
            foreach (TableRow batchRow in batchRows.Rows)
            {
                SetGridRow(batchRow, rowCount);
                rowCount += 1;
            }
        }

        /// <summary>
        /// Set the row of a batch grid.
        /// </summary>
        /// <param name="batchRow">Mapping of the batch column captions to a single row's desired values.</param>
        /// <param name="row">The index of the row.  The first row starts at index 1.</param>
        public static void SetGridRow(TableRow batchRow, int row)
        {
            foreach (string caption in batchRow.Keys)
            {
                string value = batchRow[caption];
                if (value == null) continue;

                SetGridCell(caption, value, row);
            }
        }

        /// <summary>
        /// Click the 'Validate' button and click Ok on the confirmation button.
        /// </summary>
        public static void Validate()
        {
            OpenTab("Main");
            ClickButton("Validate", null);
            GetDisplayedElement("//div[contains(@class,'bbui-dialog-msgbox') and contains(@style,'visible')]//span[contains(text(),'Batch')]", 240);
            OK();
        }

        /// <summary>
        /// Click the 'Save and close' button.
        /// </summary>
        public static void SaveAndClose()
        {
            OpenTab("Main");
            ClickButton("Save and close");
            GetDisplayedElement(Panel.getXPanelHeader("fa_batch"));
        }

        /// <summary>
        /// Edit the constituent for the currently selected batch row.
        /// </summary>
        /// <param name="fieldValues">Mapping of the field captions to their desired values.</param>
        public static void EditConstituent(TableRow fieldValues)
        {
            OpenTab("Main");
            ClickButton("Edit", "RevenueBatchConstituentInbatchEditForm");
            IndividualDialog.SetIndividualFields(fieldValues);
        }
    }

}
