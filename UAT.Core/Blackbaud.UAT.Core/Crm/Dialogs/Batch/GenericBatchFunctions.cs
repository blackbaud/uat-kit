using Blackbaud.UAT.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    internal class GenericBatchFunctions :BatchDialog
    {
        /// <summary>
        /// Set the value of a batch grid cell for a specified row.
        /// </summary>
        /// <param name="caption">The caption of the column.</param>
        /// <param name="value">The desired value of the row.</param>
        /// <param name="row">The index of the row.  The first row's index is 1.</param>
        /// <param name="ColumnToBatchColumn">Dictionary mapping the column to the batch columns</param>
        public static void SetGridCell(string caption, string value, int row, Dictionary<string, CrmField> ColumnToBatchColumn)
        {
            int columnIndex = GetColumnIndexByCaption(caption, ColumnToBatchColumn);
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
                    SetGridSearchList(cellXPath, getXInput(ColumnToBatchColumn[caption].SearchDialogId,
                                ColumnToBatchColumn[caption].SearchDialogFieldId), value);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get the index of the column in the batch grid.
        /// </summary>
        /// <param name="caption">The caption of the column.</param>
        /// <param name="ColumnToBatchColumn">Dictionary mapping the column to the batch columns</param>
        /// <returns>The index of the TD column element within the TR headers.</returns>
        public static int GetColumnIndexByCaption(string caption, Dictionary<string, CrmField> ColumnToBatchColumn)
        {
            string columnId = ColumnToBatchColumn[caption].Id;
            return GetColumnIndexById((columnId));
        }

        /// <summary>
        /// Get the value of a batch grid cell.
        /// </summary>
        /// <param name="caption">The column caption of the cell.</param>
        /// <param name="row">The row index of the cell.  The first row is index 1.</param>
        /// <param name="ColumnToBatchColumn">Dictionary mapping the column to the batch columns</param>
        /// <returns>The value of the cell.</returns>
        public static string GetGridCellValue(string caption, int row, Dictionary<string, CrmField> ColumnToBatchColumn)
        {
            int columnIndex = GetColumnIndexByCaption(caption, ColumnToBatchColumn);
            string cellXPath = getXGridCell(BatchDialogId, BatchDialogGridId, row, columnIndex);

            GetDisplayedElement(cellXPath);
            WaitClick(cellXPath);

            //TODO may need to get the edit cell instead for some situations...
            return GetDisplayedElement(cellXPath).Text;
        }

        /// <summary>
        /// Set the row of a batch grid.
        /// </summary>
        /// <param name="batchRow">Mapping of the batch column captions to a single row's desired values.</param>
        /// <param name="row">The index of the row.  The first row starts at index 1.</param>
        /// <param name="ColumnToBatchColumn">Dictionary mapping the column to the batch columns</param>
        public static void SetGridRow(TableRow batchRow, int row, Dictionary<string, CrmField> ColumnToBatchColumn)
        {
            foreach (string caption in batchRow.Keys)
            {
                string value = batchRow[caption];
                if (value == null) continue;

                SetGridCell(caption, value, row, ColumnToBatchColumn);
            }
        }

        /// <summary>
        /// Set the rows of a batch grid starting with the first row.
        /// </summary>
        /// <param name="batchRows">Table where each TableRow represents a grid row to set.
        /// <param name="ColumnToBatchColumn">Dictionary mapping the column to the batch columns</param>
        /// A TableRow is a mapping of the batch column captions to a single row's desired values.</param>
        public static void SetGridRows(Table batchRows, Dictionary<string, CrmField> ColumnToBatchColumn)
        {
            int rowCount = 1;
            //go through each row
            foreach (TableRow batchRow in batchRows.Rows)
            {
                SetGridRow(batchRow, rowCount, ColumnToBatchColumn);
                rowCount += 1;
            }
        }
    }
}
