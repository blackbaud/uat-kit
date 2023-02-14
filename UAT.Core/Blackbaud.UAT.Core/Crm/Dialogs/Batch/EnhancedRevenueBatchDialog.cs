using Blackbaud.UAT.Base;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{
    /// <summary>
    /// Class to handle the interactions for adding/editing an enhanced revenue batch dialog.
    /// </summary>
    public class EnhancedRevenueBatchDialog : BatchDialog
    {
        /// <summary>
        /// Maps batch grid column captions to their required values in order to set their values in rows.
        /// </summary>
        protected static Dictionary<string, CrmField> ColumnToBatchColumn = new Dictionary<string, CrmField> 
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
            {"Reference", new CrmField("REFERENCE", FieldType.TextInput)},
            {"Other method", new CrmField("OTHERPAYMENTMETHODCODEID", FieldType.Dropdown)},
            {"GL post date",new CrmField("td-POSTDATE", FieldType.TextInput)},
            {"Card number",new CrmField("CREDITCARDNUMBER", FieldType.TextInput)},
            {"Card type",new CrmField("CREDITTYPECODEID", FieldType.Dropdown)},
            {"Expires on",new CrmField("EXPIRESON", FieldType.TextInput)},
            {"Name on card",new CrmField("CARDHOLDERNAME", FieldType.TextInput)}
        };

        /// <summary>
        /// Click the 'Update projected totals' and click Ok on the confirmation button.
        /// </summary>
        public static void UpdateProjectedTotals()
        {
            OpenTab("Main");
            ClickButton("Update projected totals", null);
            OK();
        }

        /// <summary>
        /// Click the 'Split designations' button and set the desgination grid's rows.  This will
        /// split the designation on the currently selected grid row.  The amount between the designations
        /// will be split evenly.
        /// </summary>
        /// <param name="designations">Table where each row represents a designation to add.
        /// Each row is a mapping of the column captions to the single row's desired values.</param>
        /// <param name="evenly">Flag indicating whether or not to split the designations evenly.</param>
        public static void SplitDesignations(Table designations, bool evenly)
        {
            OpenTab("Revenue");
            ClickButton("Split designations", "BatchRevenueSplitDesignations");

            DesignationsDialog.SplitDesignations(designations, evenly);
            OK();
        }

        /// <summary>
        /// Click 'Apply' under the Revenue tab and set the designations in the 'Apply to commitments' dialog.
        /// </summary>
        /// <param name="applications">Table where each row represents an application to add.
        /// A row is a mapping of the column captions to a single row's desired values.</param>
        public static void Apply(Table applications)
        {
            OpenTab("Revenue");
            ClickButton("Apply", "BatchRevenueApplyCommitmentsCustom");
            CommitmentsDialog.SetAdditionalApplicationsGridRows(applications);
            OK();
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
