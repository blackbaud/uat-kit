using Blackbaud.UAT.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for adding/editing an constituent batch dialog.
    /// </summary>
    public class ConstituentBatchDialog : BatchDialog
    {
        /// <summary>
        /// Maps batch grid column captions to their required values in order to set their values in rows.
        /// </summary>
        protected static Dictionary<string, CrmField> ColumnToBatchColumn = new Dictionary<string, CrmField> 
        { 
            {"Constituent type",new CrmField("ISORGANIZATION", FieldType.Dropdown)},
            {"Last/org/group/household name",new CrmField("KEYNAME", FieldType.TextInput)},
            {"Address type",new CrmField("ADDRESS_ADDRESSTYPECODEID", FieldType.Dropdown)},
            {"Country",new CrmField("ADDRESS_COUNTRYID", FieldType.Dropdown)},
            {"Address",new CrmField("ADDRESS_ADDRESSBLOCK", FieldType.TextInput)},
            {"City",new CrmField("ADDRESS_CITY", FieldType.TextInput)},
            {"State",new CrmField("ADDRESS_STATEID", FieldType.Dropdown)},
            {"ZIP",new CrmField("ADDRESS_POSTCODE", FieldType.TextInput)}

        };

        /// <summary>
        /// Set the value of a batch grid cell for a specified row.
        /// </summary>
        /// <param name="caption">The caption of the column.</param>
        /// <param name="value">The desired value of the row.</param>
        /// <param name="row">The index of the row.  The first row's index is 1.</param>
        public static void SetGridCell(string caption, string value, int row)
        {
            GenericBatchFunctions.SetGridCell(caption, value, row, ColumnToBatchColumn);
        }

        /// <summary>
        /// Get the index of the column in the batch grid.
        /// </summary>
        /// <param name="caption">The caption of the column.</param>
        /// <returns>The index of the TD column element within the TR headers.</returns>
        public static int GetColumnIndexByCaption(string caption)
        {
            return GenericBatchFunctions.GetColumnIndexByCaption(caption, ColumnToBatchColumn);
        }

        /// <summary>
        /// Get the value of a batch grid cell.
        /// </summary>
        /// <param name="caption">The column caption of the cell.</param>
        /// <param name="row">The row index of the cell.  The first row is index 1.</param>
        /// <returns>The value of the cell.</returns>
        public static string GetGridCellValue(string caption, int row)
        {
            return GenericBatchFunctions.GetGridCellValue(caption, row, ColumnToBatchColumn);
        }

        /// <summary>
        /// Set the row of a batch grid.
        /// </summary>
        /// <param name="batchRow">Mapping of the batch column captions to a single row's desired values.</param>
        /// <param name="row">The index of the row.  The first row starts at index 1.</param>
        public static void SetGridRow(TableRow batchRow, int row)
        {
            GenericBatchFunctions.SetGridRow(batchRow, row, ColumnToBatchColumn);
        }

        /// <summary>
        /// Set the rows of a batch grid starting with the first row.
        /// </summary>
        /// <param name="batchRows">Table where each TableRow represents a grid row to set.
        /// A TableRow is a mapping of the batch column captions to a single row's desired values.</param>
        public static void SetGridRows(Table batchRows)
        {
            GenericBatchFunctions.SetGridRows(batchRows, ColumnToBatchColumn);
        }
    }
}
