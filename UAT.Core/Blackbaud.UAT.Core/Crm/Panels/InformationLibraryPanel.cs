using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Information Library' panel.
    /// </summary>
    public class InformationLibraryPanel : Panel
    {

        private static string getXAddHocQueryType(string type)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[./text()='{0}' and contains(@class,'NAME')]", type);
        }

        /// <summary>
        /// Load the dialog to add an Ad-Hoc Query and select the type of query to create.
        /// </summary>
        /// <param name="type">The type of query to add.</param>
        public static void AddAdHocQuery(string type)
        {
            ClickSectionAddButton("Queries", "Add an ad-hoc query");
            WaitClick(getXAddHocQueryType(type));
            WaitClick(Dialog.getXOKButton);
        }

        /// <summary>
        /// XPath for the root of teh query treeview with text "All"
        /// </summary>
        public static string getXAllQueriesTreeRoot = "//*[contains(@class,\"x-tree-node-anchor\")]//span[contains(.,\"All\")]";

        /// <summary>
        /// Return an Xpath for the Query Field on the InformationLibraryPanel.
        /// </summary>
        protected static string getXFilterQueryField = "//*[contains(@class,\"bbui-datalist-pagingbar-search\")]";

        /// <summary>
        /// Set filter based on string passed in
        /// </summary>
        /// <param name="name">Filter to set</param>
        public static void SetFilter(string name)
        {
            SetTextField(getXFilterQueryField, name);
        }

        /// <summary>
        /// Returns an Xpath for the Delete Query menu item on a InformationLibraryPanel.
        /// </summary>
        public static string getXDeleteQuery = "//*[contains(@class,\"x-menu-list-item\") and not(contains(@class,\"x-hide-display\"))]//*[./text()=\"Delete\" and contains(@class,\"x-menu-item-text\")]";

        /// <summary>
        /// Returns an Xpath for the highlighted Query search result.  
        /// </summary>
        /// <param name="text">The text of the result.</param>
        /// <returns></returns>
        public static string getXQuery(string text) { return String.Format("//*[./text()=\"{0}\" and contains(@class,\"bbui-datalist-search-highlight\")]",text); }

        /// <summary>
        /// Create a copy of an existing query as a static selection.
        /// </summary>
        /// <param name="selection">The name to give to the new static selection.</param>
        /// <param name="query">The query to create a copy off.  Mapping of the column captions to a single row's values.</param>
        public static void CopyQueryAsStaticSelection(string selection, IDictionary<string, string> query)
        {
            SelectSectionDatalistRow(query, "Queries");
            WaitClick(getXSelectedDatalistRowButton("Copy"));
            Dialog.OpenTab("Set save options");
            SetTextField(Dialog.getXInput("AdHocQuerySaveOptionsViewDataForm", "_NAME_value"), selection);
            SetCheckbox(Dialog.getXInput("AdHocQuerySaveOptionsViewDataForm", "_SELECTION_TYPE_1"), true);
            WaitClick(Dialog.getXSaveAndCloseButton);
        }

        /// <summary>
        /// Check if a query exists.
        /// </summary>
        /// <param name="query">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the query exists, false otherwise.</returns>
        public static bool QueryExists(IDictionary<string,string> query)
        {
            return SectionDatalistRowExists(query, "Queries");
        }

        /// <summary>
        /// Check if a query exists.
        /// </summary>
        /// <param name="query">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the query exists, false otherwise.</returns>
        public static bool QueryExists(TableRow query)
        {
            return SectionDatalistRowExists(query, "Queries");
        }

        /// <summary>
        /// Delete a query.
        /// </summary>
        /// <param name="query">Mapping of the column captions to a single row's values.</param>
        public static void DeleteQuery(IDictionary<string,string> query)
        {
            SelectSectionDatalistRow(query, "Queries");
            WaitClick(getXSelectedDatalistRowButton("Delete"));
            Dialog.Yes();
        }

        /// <summary>
        /// Delete a query.
        /// </summary>
        /// <param name="query">Mapping of the column captions to a single row's values.</param>
        public static void DeleteQuery(TableRow query)
        {
            SelectSectionDatalistRow(query, "Queries");
            WaitClick(getXSelectedDatalistRowButton("Delete"));
            Dialog.Yes();
        }

        /// <summary>
        /// Select a datalist row for a section.
        /// </summary>
        /// <param name="row">Mapping of the column captions to a single row's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        protected static new void SelectSectionDatalistRow(TableRow row, string sectionCaption)
        {
            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            foreach (string caption in row.Keys)
            {
                rowValues.Add(caption, row[caption]);
            }

            SelectSectionDatalistRow(rowValues, sectionCaption);
        }

        /// <summary>
        /// Select a datalist row for a section.
        /// </summary>
        /// <param name="rowValues">Mapping of the column captions to a single row's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        protected static new void SelectSectionDatalistRow(IDictionary<string, string> rowValues, string sectionCaption)
        {
            IDictionary<string, int> columnIndex = new Dictionary<string, int>();
            foreach (string caption in rowValues.Keys)
            {
                columnIndex.Add(caption, GetDatalistColumnIndex(getXSectionDatalistColumnHeaders(sectionCaption), caption));
            }

            if (ExistsNow(getXSectionDatalistRow(sectionCaption, columnIndex, rowValues)))
            {
                ExpandRow(getXSectionDatalistRow(sectionCaption, columnIndex, rowValues));
            }
            else if (AdditionalDatalistPagesExist(sectionCaption))
            {
                SelectSectionDatalistRow(rowValues, sectionCaption);
            }
            else throw new ArgumentException(String.Format("Could not find row '{0}' to select in section '{1}'", rowValues.Values.ToList(), sectionCaption));
        }

        /// <summary>
        /// Check if a datalist row exists.
        /// </summary>
        /// <param name="row">Mapping of the column captions to a single row's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <returns>True if the row exists, false otherwise.</returns>
        protected static new bool SectionDatalistRowExists(TableRow row, string sectionCaption)
        {
            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            foreach (string caption in row.Keys)
            {
                rowValues.Add(caption, row[caption]);
            }

            return SectionDatalistRowExists(rowValues, sectionCaption);
        }

        /// <summary>
        /// Check if a datalist row exists.
        /// </summary>
        /// <param name="rowValues">Mapping of the column captions to a single row's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <returns>True if the row exists, false otherwise.</returns>
        protected static new bool SectionDatalistRowExists(IDictionary<string, string> rowValues, string sectionCaption)
        {
            IDictionary<string, int> columnIndex = new Dictionary<string, int>();
            foreach (string caption in rowValues.Keys)
            {
                columnIndex.Add(caption, GetDatalistColumnIndex(getXSectionDatalistColumnHeaders(sectionCaption), caption));
            }

            bool exists = ExistsNow(getXSectionDatalistRow(sectionCaption, columnIndex, rowValues));
            if (!exists && AdditionalDatalistPagesExist(sectionCaption))
            {
                return SectionDatalistRowExists(rowValues, sectionCaption);
            }
            return exists;
        }

        /// <summary>
        /// Formats a unique xPath to find a single datalist row.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section that the datalist belongs to.</param>
        /// <param name="columnCaptionToIndex">Mapping of the datalist column captions to their DOM TD index.</param>
        /// <param name="columnCaptionToValue">Mapping of the datalist column captions to their values in a single row.</param>
        protected static new string getXSectionDatalistRow(string sectionCaption, IDictionary<string, int> columnCaptionToIndex, IDictionary<string, string> columnCaptionToValue)
        {
            string finalString = getXSectionDatalistRows(sectionCaption);
            foreach (string columnCaption in columnCaptionToValue.Keys)
            {
                finalString += getXDataListColumnValue(columnCaptionToIndex[columnCaption], columnCaptionToValue[columnCaption]);
            }
            return finalString;
        }

        /// <summary>
        /// Given the DOM index of a datalist column and the value for that column, return an XPath
        /// to append to a datalist XPath for finding that value in the requested column index.
        /// 
        /// No element will be found if the value does not exist in this column.
        /// </summary>
        /// <param name="columnIndex">The DOM index for the TD element representing the desired column.</param>
        /// <param name="columnValue">The value corresponding the provided column.  </param>
        protected static new string getXDataListColumnValue(int columnIndex, string columnValue)
        {
            return String.Format("/td[{0}]//*[text()=\"{1}\" or @title=\"{1}\"]/../../../..", columnIndex, columnValue);
        }
    }
}
