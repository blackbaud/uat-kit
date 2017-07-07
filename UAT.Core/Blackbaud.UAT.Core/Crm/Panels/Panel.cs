using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Base class to handle interactions for panels.
    /// </summary>
    public class Panel : BaseComponent
    {
        /// <summary>
        /// Xpath for tab specified by the input text
        /// </summary>
        /// <param name="text">The tab text</param>
        /// <returns>formated XPath for the tab text passed in</returns>
        public static string getXPanelTab(string text) { return String.Format("//*[./text()=\"{0}\" and contains(@class,\"x-tab-strip-text\")]", text); }

        /// <summary>
        /// Given the unique HTML element ids of a dialog and span, return a unique identifier XPath
        /// to find the SPAN field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_EventSummaryViewForm3'</param>
        /// <param name="spanId">The SPAN element's unique id identifier - i.e. '_LOCATION_value'</param>
        public static string getXSpan(string dialogId, string spanId)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//div[contains(@id, '{0}')]//span[contains(@id, '{1}')]", dialogId, spanId);
        }

        /// <summary>
        /// XPath for Datalist Span based on dialog and span name
        /// </summary>
        /// <param name="dialogId">DialogID</param>
        /// <param name="spanId">Html span ID</param>
        /// <returns>Formated XPath</returns>
        public static string getXDatalistSpan(string dialogId, string spanId)
        {
            return String.Format("//div[contains(@class,'bbui-pages-section-datalist')]/div[contains(@id, '{0}')]//span[contains(@id, '{1}')]", dialogId, spanId);
        }

        /// <summary>
        /// Given the unique HTML element ids of a dialog and div, return a unique identifier XPath
        /// to find the DIV field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_individualSpouseBusinessAddForm'</param>
        /// <param name="divId">The DIV element's unique id identifier - i.e. '_LASTNAME_value'</param>
        public static string getXDiv(string dialogId, string divId)
        {
            return String.Format("//div[not(contains(@class,'x-hide-display'))]/div[contains(@class,'summarycontainer')]//div[contains(@style,'visible')]//div[contains(@id, '{0}')]//div[contains(@id, '{1}')]", dialogId, divId);
        }

        /// <summary>
        /// Format an XPath to find all top-right indexing buttons for a section's datalist.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section.</param>
        public static string getXSectionDatalistPageIndexButtons(string sectionCaption)
        {
            return String.Format("{0}/../../../../../../../../../../..//div[contains(@class,'x-panel-tbar')]//div[contains(@class,'bbui-datalist-pagingbar')]//tr[@class='x-toolbar-right-row']//button[contains(@class, 'bbui-linkbutton')]", getXSection(sectionCaption));
        }

        /// <summary>
        /// Format an XPath to find the top-right index DIV whose text will be the current page index of a section's datalist.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section</param>
        public static string getXSectionDatalistCurrentPageIndex(string sectionCaption)
        {
            return String.Format("{0}/../../../../../../../../../../..//div[contains(@class,'x-panel-tbar')]//div[contains(@class,'bbui-datalist-pagingbar')]//tr[@class='x-toolbar-right-row']//div[@class='bbui-numericpaging-button-selected']", getXSection(sectionCaption));
        }

        /// <summary>
        /// Format an XPath for handling a button on the selected datalist row.
        /// </summary>
        /// <param name="caption">The caption of the button.</param>
        public static string getXSelectedDatalistRowButton(string caption)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//div[contains(@class,'x-grid3-row-selected')]//button[./text()='{0}']", caption);
        }

        /// <summary>
        /// Format an XPath for handling a link on the selected datalist row.
        /// </summary>
        /// <param name="action">The tooltip caption of the action displayed when hovering over the link. i.e. 'Go to plan:'</param>
        public static string getXSelectedDatalistRowLinkByAction(string action)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//div[contains(@class,'x-grid3-row-selected')]//a[contains(@class,'datalistgrid-rowlink') and contains(@title,'{0}')]", action);
        }

        /// <summary>
        /// Format an xPath for handling a link on any datalist row.
        /// </summary>
        /// <param name="action">The result of the action. i.e. 'Go to segment:'</param>
        /// <param name="caption">The caption of the link.</param>
        public static string getXDatalistRowLinkByActionAndCaption(string action, string caption)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//a[contains(@class,'datalistgrid-rowlink') and (contains(@title,'{0}') or contains(text(),'{1}'))]", action, caption);
        }

        /// <summary>
        /// Format an XPath for handling a link on the selected datalist row.
        /// </summary>
        /// <param name="id">The id of the link that is the value of the 'data-fieldid' attribute.  i.e. 'PLANNAME'</param>
        public static string getXSelectedDatalistRowLinkById(string id)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//div[contains(@class,'x-grid3-row-selected')]//a[contains(@class,'datalistgrid-rowlink') and contains(@data-fieldid,'{0}')]", id);
        }

        /// <summary>
        /// Format an XPath for handling a link on the selected datalist row.
        /// </summary>
        /// <param name="caption">The caption text of the link.</param>
        public static string getXSelectedDatalistRowLinkByCaption(string caption)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//div[contains(@class,'x-grid3-row-selected')]//a[contains(@class,'datalistgrid-rowlink') and contains(text(),'{0}')]",caption);
        }

        /// <summary>
        /// Format an XPath for finding a link based on its caption located in the header of a panel.
        /// </summary>
        /// <param name="linkCaption"></param>
        public static string getXPanelHeaderLink(string linkCaption)
        {
            return String.Format("//div[not(contains(@class,'x-hide-display'))]/div[contains(@class,'summarycontainer')]//div[contains(@id,'headerpanel')]//button[text()='{0}']", linkCaption);
        }

        /// <summary>
        /// Format an XPath for finding a context link located in the header of a panel.
        /// </summary>
        public static string getXPanelHeaderLink()
        {
            return "//div[not(contains(@class,'x-hide-display'))]/div[contains(@class,'summarycontainer')]//div[contains(@id,'headerpanel')]//div[contains(@class,'header-contextlinks')]/button";
        }

        /// <summary>
        /// Formatter for an XPath to find a panel header with a specific image type.
        /// </summary>
        /// <param name="imageName">Unique name of the image associated with the panel header.</param>
        public static string getXPanelHeader(string imageName)
        {
            return String.Format("//div[not(contains(@class,'x-hide-display'))]/div[contains(@class,'summarycontainer')]//div[contains(@style, '{0}')]//*[contains(@class,'pages-header')]/span", imageName);
        }

        /// <summary>
        /// Generic XPath to get the panel header.
        /// </summary>
        public static string getXPanelHeader()
        {
            return "//div[not(contains(@class,'x-hide-display'))]/div[contains(@class,'summarycontainer')]//*[contains(@class,'pages-header')]/span";
        }

        /// <summary>
        /// Format an XPath to find a panel header containing the provided text.
        /// </summary>
        /// <param name="text">The value to check for in the panel header.</param>
        public static string getXPanelHeaderByText(string text)
        {
            return String.Format("//div[not(contains(@class,'x-hide-display'))]/div[contains(@class,'summarycontainer')]//*[contains(@class,'pages-header')]/span[contains(text(),'{0}')]", text);
        }

        /// <summary>
        /// Given the DOM index of a datalist column and the value for that column, return an XPath
        /// to append to a datalist XPath for finding that value in the requested column index.
        /// 
        /// No element will be found if the value does not exist in this column.
        /// </summary>
        /// <param name="columnIndex">The DOM index for the TD element representing the desired column.</param>
        /// <param name="columnValue">The value corresponding the provided column.  </param>
        public static string getXDataListColumnValue(int columnIndex, string columnValue)
        {
            return String.Format("/td[{0}]/div[contains(text(),\"{1}\") or contains(@title,\"{1}\")]/../..", columnIndex, columnValue);
        }

        /// <summary>
        /// XPath to get the TR datalist rows of a section.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section that the datalist belongs to.</param>
        public static string getXSectionDatalistRows(string sectionCaption)
        {
            return String.Format("{0}/../../../../../../../../../../..//div[@class='x-grid3-body']//div[contains(@class, 'x-grid3-row')]/table/tbody/tr", getXSection(sectionCaption));
        }

        /// <summary>
        /// Formats a unique XPath to find a single datalist row.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section that the datalist belongs to.</param>
        /// <param name="columnCaptionToIndex">Mapping of the datalist column captions to their DOM TD index.</param>
        /// <param name="columnCaptionToValue">Mapping of the datalist column captions to their values in a single row.</param>
        public static string getXSectionDatalistRow(string sectionCaption, IDictionary<string, int> columnCaptionToIndex, IDictionary<string, string> columnCaptionToValue)
        {
            string finalString = getXSectionDatalistRows(sectionCaption);
            foreach (string columnCaption in columnCaptionToValue.Keys)
            {
                finalString += getXDataListColumnValue(columnCaptionToIndex[columnCaption], columnCaptionToValue[columnCaption]);
            }
            return finalString;
        }

        /// <summary>
        /// Format an XPath to find a single view form in a section.
        /// 
        /// Field values will be checked to CONTAIN (not match) the provided values.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section that the view form belongs to.</param>
        /// <param name="viewFormId">The unique id for the type of view form to look for.  i.e. 'RevenueTransactionDetailViewForm'</param>
        /// <param name="viewFormValues">Mapping of a single view form's captions (excluding the colon) to their desired value.
        /// Field values will be checked to CONTAIN (not match) the provided values.</param>
        public static string getXViewForm(string sectionCaption, string viewFormId,
            IDictionary<string, string> viewFormValues)
        {
            string finalString = getXSectionViewForm(sectionCaption, viewFormId);
            foreach (string caption in viewFormValues.Keys)
            {
                finalString += getXSectionViewFormSpan(caption, viewFormValues[caption]);
            }
            return finalString;
        }

        /// <summary>
        /// Format an XPath to append to a section view form that has an item with the provided
        /// caption and CONTAINING (not matching) the value.
        /// 
        /// No element will be found if the caption does not exist with this value.
        /// </summary>
        /// <param name="caption">The caption of the view form field.</param>
        /// <param name="value">The expected value associated with the provided caption.</param>
        public static string getXSectionViewFormSpan(string caption, string value)
        {
            return string.Format("//span[text()='{0}:']/../..//span[contains(@id,'_value') and contains(text(),'{1}')]/../../..", caption, value);
        }

        /// <summary>
        /// Format an XPath for a section's view form root.
        /// </summary>
        /// <param name="secionCaption">The caption of the section.</param>
        /// <param name="viewFormId">The unique id for the type of view form to look for.  i.e. 'RevenueTransactionDetailViewForm'</param>
        public static string getXSectionViewForm(string secionCaption, string viewFormId)
        {
            return String.Format("{0}/../../../../../../../../../../..//div[contains(@id,'{1}')]", getXSection(secionCaption), viewFormId);
        }

        /// <summary>
        /// XPath to get the TR row of column headers for a section's datalist.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section that the datalist belongs to.</param>
        public static string getXSectionDatalistColumnHeaders(string sectionCaption)
        {
            return String.Format("{0}/../../../../../../../../../../..//div[@class='x-grid3-header']//tr", getXSection(sectionCaption));
        }

        /// <summary>
        /// XPath to get the TR row of column headers for a section's datalist when multiple sections with matching
        /// captions exists for a single panel on different tabs.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section that the datalist belongs to.</param>
        public static string getXDuplicateSectionDatalistColumnHeaders(string sectionCaption)
        {
            return String.Format("{0}/../../../../../../../../../../../../../../../div[not(contains(@class,'hide-display'))]//div[@class='x-grid3-header']//tr", getXSection(sectionCaption));
        }

        /// <summary>
        /// Given the caption header of a section, formats an XPath for handling the Add
        /// button.
        /// </summary>
        /// <param name="sectionName">The caption head of the section.</param>
        /// <param name="addCaption">The caption of the button defaulting to "Add".  
        /// A unique caption value can be specified if the button caption is not "Add".</param>
        /// <returns></returns>
        public static string getXSectionAddButton(string sectionName, string addCaption = "Add")
        {
            return String.Format("{0}/../..//td/table/tbody/tr/td/em/button[./text() = '{1}']", getXSection(sectionName), addCaption);
        }

        /// <summary>
        /// Given the caption header of a section, return a unique identifier XPath
        /// for handling the section WebElement.
        /// </summary>
        /// <param name="sectionName">The caption header of the section to reference</param>
        public static string getXSection(string sectionName)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//div[not(contains(@class,'x-hide-display')) and contains(@class,'bbui-pages-pagesection') and not(contains(@class,'row'))]//div[contains(@id,'pageSection')]/div/table/tbody/tr//td/table/tbody/tr//td/div[./text() = '{0}']", sectionName);
        }

        /// <summary>
        /// Constant XPath for getting the right scroller on the top Tab bar.
        /// </summary>
        public const string getXTabsBarRightScroller = "//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]/div[contains(@class,'bbui-pages-tabpanel')]/div[contains(@class,'x-tab-panel-header') and not(contains(@style,'display: none'))]/div[contains(@class,'tab-scroller-right')]";

        /// <summary>
        /// Constant XPath for getting an element that indicates the top Tab bar has loaded an active Tab.
        /// </summary>
        public const string getXTabsBar = "//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]/div[contains(@class,'bbui-pages-tabpanel')]/div[contains(@class,'x-tab-panel-header') and not(contains(@style,'display: none'))]//ul[contains(@class,'x-tab-strip')]/li[contains(@class,'x-tab-strip-active')]/..";

        /// <summary>
        /// Constant XPath for getting an element corresponds to the currently active Tab in the top Tab bar.
        /// </summary>
        public const string getXTabsBarActiveTab = "//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]/div[contains(@class,'bbui-pages-tabpanel')]/div[contains(@class,'x-tab-panel-header') and not(contains(@style,'display: none'))]//ul[contains(@class,'x-tab-strip')]/li[contains(@class,'x-tab-strip-active')]//span[contains(@class,'x-tab-strip-text')]";

        /// <summary>
        /// Constant XPath for getting an element that indicates an inner Tab bar has loaded an active Tab.
        /// </summary>
        //public const string getXInnerTabsBar = "//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//div[contains(@class,'bbui-pages-pagesection-withchildtabs')]//div[contains(@class,'bbui-pages-tabpanel')]//ul[contains(@class,'x-tab-strip')]/li[contains(@class,'x-tab-strip-active')]/..";
        public const string getXInnerTabsBar = "//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//div[contains(@class,'bbui-pages-pagesection-withchildtabs') and not(contains(@class,'x-hide-display'))]//div[contains(@class,'bbui-pages-tabpanel')]//ul[contains(@class,'x-tab-strip')]";

        /// <summary>
        /// Formats an XPath for a Tab in a top Tab bar.
        /// </summary>
        /// <param name="caption">The caption of the tab.</param>
        public static string getXTab(string caption)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]/div[contains(@class,'bbui-pages-tabpanel')]/div[contains(@class,'x-tab-panel-header') and not(contains(@style,'display: none'))]//ul[contains(@class,'x-tab-strip')]//span[text()='{0}']", caption);
        }

        /// <summary>
        /// Formats an XPath for a Tab in an inner Tab bar.
        /// </summary>
        /// <param name="caption">The caption of the tab.</param>
        public static string getXInnerTab(string caption)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//div[contains(@class,'bbui-pages-pagesection-withchildtabs')]//div[contains(@class,'bbui-pages-tabpanel')]//ul[contains(@class,'x-tab-strip')]//span[text()='{0}']", caption);
        }

        /// <summary>
        /// Formats an XPath to get the expand/collpase button of a section
        /// </summary>
        /// <param name="sectionCaption">The caption of the section.</param>
        public static string getXSectionExpandButton(string sectionCaption)
        {
            return String.Format("{0}/../../td[not(contains(@class,'x-hide-display'))]//button[contains(@class,'bbui-pages-section') and not(contains(@class,'refresh'))]", getXSection(sectionCaption));
        }

        /// <summary>
        /// Returns an XPath to get the DIV parent of the Research Lists UI widget whose 'stlye' attribute contains visibility info about the widget.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <param name="widgetId">A unique part of the widget element's id in the DOM.  i.e. 'ResearchGroupNavigationViewDataForm'</param>
        public static string getXSectionWidget(string sectionCaption, string widgetId)
        {
            return String.Format("{0}/../../../../../../../../../../..//div[contains(@id,'{1}')]/../..", getXSection(sectionCaption), widgetId);
        }

        /// <summary>
        /// Collapse a section if it is expanded and a collapse/expand button exists.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section</param>
        /// <param name="widgetId">A unique part of the widget element's id in the DOM.  i.e. 'ResearchGroupNavigationViewDataForm'</param>
        public static void CollapseSection(string sectionCaption, string widgetId)
        {
            if (GetEnabledElement(getXSectionExpandButton(sectionCaption)).GetAttribute("class").Contains("collapsesection"))
            {
                WaitClick(getXSectionExpandButton(sectionCaption));
                WebDriverWait waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
                waiter.Until(d =>
                {
                    IWebElement element = d.FindElement(By.XPath(getXSectionWidget(sectionCaption, widgetId)));
                    string style = element.GetAttribute("style");
                    if (element != null && style.Contains("display: none") && !style.Contains("height"))
                    {
                        /*
                         * Ugly hack sleep line.  Even though the dom lists the widget as completly collapsed
                         * based on the syle attribute values, it may still be finalizing the collapse.  This 
                         * causes the click on a task item below the Research List to occur on a different item as 
                         * the task is moving between the get and the click.
                         * 
                         * TODO - investigate a different dom attribute that is changing as the widget
                         * state fluctuates.
                         */
                        Thread.Sleep(250);
                        return true;
                    }
                    return false;
                });
            }

        }

        /// <summary>
        /// Select a row in a section's datalist.  
        /// If the row is already selected, no action is taken.
        /// </summary>
        /// <param name="rowValues">Mapping of the column caption to the single row's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <param name="columnCaptionToIndex">Mapping of the column captions to their TD index.</param>
        public static void SelectSectionDatalistRow(IDictionary<string, string> rowValues, string sectionCaption,
            IDictionary<string, int> columnCaptionToIndex)
        {
            if (ExistsNow(getXSectionDatalistRow(sectionCaption, columnCaptionToIndex, rowValues)))
            {
                if (ExistsNow(String.Format("{0}//button[contains(@class,'bbui-pages-section-expandbutton')]",
                    getXSectionDatalistRow(sectionCaption, columnCaptionToIndex, rowValues))))
                    ExpandRow(getXSectionDatalistRow(sectionCaption, columnCaptionToIndex, rowValues));
                else WaitClick(String.Format("{0}/../..", getXSectionDatalistRow(sectionCaption, columnCaptionToIndex, rowValues)));
            }
            else if (AdditionalDatalistPagesExist(sectionCaption))
            {
                SelectSectionDatalistRow(rowValues, sectionCaption, columnCaptionToIndex);
            }
            else throw new ArgumentException(String.Format("Could not find row '{0}' to select in section '{1}'", rowValues.Values.ToList(), sectionCaption));
        }

        /// <summary>
        /// Select a row in a section's datalist.  
        /// If the row is already selected, no action is taken.
        /// </summary>
        /// <param name="rowValues">Mapping of the column caption to the single row's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <param name="columnCaptionToIndex">Mapping of the column captions to their TD index.</param>
        public static void SelectSectionDatalistRow(TableRow rowValues, string sectionCaption,
            IDictionary<string, int> columnCaptionToIndex)
        {
            IDictionary<string, string> row = new Dictionary<string, string>();
            foreach (string caption in rowValues.Keys)
            {
                row.Add(caption, row[caption]);
            }

            SelectSectionDatalistRow(row, sectionCaption, columnCaptionToIndex);
        }

        /// <summary>
        /// Select a row in a section's datalist.  If the row is already selected, no action is taken.
        /// </summary>
        /// <param name="rowValues">Dictionary mapping of the column caption to the single row's values.   Keys represent column captions.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        public static void SelectSectionDatalistRow(IDictionary<string, string> rowValues, string sectionCaption)
        {
            IDictionary<string, int> columnIndex = new Dictionary<string, int>();
            foreach (string caption in rowValues.Keys)
            {
                columnIndex.Add(caption, GetDatalistColumnIndex(getXSectionDatalistColumnHeaders(sectionCaption), caption));
            }

            if (Exists(getXSectionDatalistRow(sectionCaption, columnIndex, rowValues), 2))
            {
                if (ExistsNow(String.Format("{0}//button[contains(@class,'bbui-pages-section-expandbutton')]", 
                    getXSectionDatalistRow(sectionCaption, columnIndex, rowValues)))) 
                    ExpandRow(getXSectionDatalistRow(sectionCaption, columnIndex, rowValues));
                else WaitClick(String.Format("{0}/../..", getXSectionDatalistRow(sectionCaption, columnIndex, rowValues)));
            }
            else if (AdditionalDatalistPagesExist(sectionCaption))
            {
                SelectSectionDatalistRow(rowValues, sectionCaption);
            }
            else throw new ArgumentException(String.Format("Could not find row '{0}' to select in section '{1}'", rowValues.Values.ToList(), sectionCaption));
        }

        /// <summary>
        /// Select a row in a section's datalist.  If the row is already selected, no action is taken.
        /// </summary>
        /// <param name="row">Mapping of the column caption to the single row's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        public static void SelectSectionDatalistRow(TableRow row, string sectionCaption)
        {
            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            foreach (string caption in row.Keys)
            {
                rowValues.Add(caption, row[caption]);
            }

            SelectSectionDatalistRow(rowValues, sectionCaption);
        }


        // FIXME :( 

        /// <summary>
        /// Check whether a row exists in a section's datalist.
        /// </summary>
        /// <param name="rowValues">Dictionary mapping the datalist columns to a single row's values.  Keys represent column captions.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <returns>True if the row exists, false otherwise.</returns>
        public static bool SectionDatalistRowExists(IDictionary<string, string> rowValues, string sectionCaption)
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
        /// Check whether a row exists in a section's datalist.
        /// </summary>
        /// <param name="row">Mapping of the datalist columns to a single row's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <returns>True if the row exists, false otherwise.</returns>
        public static bool SectionDatalistRowExists(TableRow row, string sectionCaption)
        {
            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            foreach (string caption in row.Keys)
            {
                rowValues.Add(caption, row[caption]);
            }

            bool exists = SectionDatalistRowExists(rowValues, sectionCaption);

            return exists;
        }

        /// <summary>
        /// Check whether a row exists in a section's datalist.
        /// </summary>
        /// <param name="row">Mapping of the datalist columns to a single row's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <param name="columnCaptionToIndex">Mapping of the column captions to their TD index.</param>
        /// <returns>True if the row exists, false otherwise.</returns>
        public static bool SectionDatalistRowExists(TableRow row, string sectionCaption,
            IDictionary<string, int> columnCaptionToIndex)
        {
            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            foreach (string caption in row.Keys)
            {
                rowValues.Add(caption, row[caption]);
            }
            return SectionDatalistRowExists(rowValues, sectionCaption, columnCaptionToIndex);
        }

        /// <summary>
        /// Check whether a row exists in a section's datalist.
        /// </summary>
        /// <param name="rowValues">Dictionary mapping the datalist columns to a single row's values.  Keys represent column captions.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <param name="columnCaptionToIndex">Mapping of the column captions to their TD index.</param>
        /// <returns>True if the row exists, false otherwise.</returns>
        public static bool SectionDatalistRowExists(IDictionary<string, string> rowValues, string sectionCaption,
            IDictionary<string, int> columnCaptionToIndex)
        {
            GetDisplayedElement(getXSectionDatalistColumnHeaders(sectionCaption));
            bool exists = ExistsNow(getXSectionDatalistRow(sectionCaption, columnCaptionToIndex, rowValues));
            if (!exists && AdditionalDatalistPagesExist(sectionCaption))
            {
                return SectionDatalistRowExists(rowValues, sectionCaption);
            }
            return exists;
        }

        /// <summary>
        /// Check whether a view form exists in a section CONTAINING (not identically matching) the provided values.
        /// </summary>
        /// <param name="viewForm">Mapping of the view form captions (excluding the colon) to a single view form's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <param name="viewFormId">The unique id for the type of view form to look for.  i.e. 'RevenueTransactionDetailViewForm'</param>
        /// <returns>True if the view form exists, false otherwise.</returns>
        public static bool SectionViewFormExists(TableRow viewForm, string sectionCaption, string viewFormId)
        {
            IDictionary<string, string> viewFormValues = new Dictionary<string, string>();
            foreach (string caption in viewForm.Keys)
            {
                viewFormValues.Add(caption, viewForm[caption]);
            }
            return SectionViewFormExists(viewFormValues, sectionCaption, viewFormId);
        }

        /// <summary>
        /// Check whether a view form exists in a section CONTAINING (not identically matching) the provided values.
        /// </summary>
        /// <param name="viewForm">Mapping of the view form captions (excluding the colon) to a single view form's values.</param>
        /// <param name="sectionCaption">The caption of the section.</param>
        /// <param name="viewFormId">The unique id for the type of view form to look for.  i.e. 'RevenueTransactionDetailViewForm'</param>
        /// <returns>True if the view form exists, false otherwise.</returns>
        public static bool SectionViewFormExists(IDictionary<string, string> viewForm, string sectionCaption, string viewFormId)
        {
            return Exists(getXViewForm(sectionCaption, viewFormId, viewForm));
        }

        /// <summary>
        /// Check if a grid/datalist row exists.
        /// </summary>
        /// <param name="row">Mapping of the grid/datalist column captions to a single row's values.</param>
        /// <param name="gridId">The unique id of the grid/datalist.  i.e. '_DESIGNATIONS_value'</param>
        /// <returns>True if the row exists, false otherwise.</returns>
        public static bool GridRowExists(TableRow row, string gridId)
        {
            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            foreach (string caption in row.Keys)
            {
                rowValues.Add(caption, row[caption]);
            }
            return GridRowExists(rowValues, gridId);
        }

        /// <summary>
        /// Check if a grid/datalist row exists.
        /// </summary>
        /// <param name="row">Dictionary mapping the grid/datalist columns to a single row's values.  Keys represent column captions.</param>
        /// <param name="gridId">The unique id of the grid/datalist.  i.e. '_DESIGNATIONS_value'</param>
        /// <returns>True if the row exists, false otherwise.</returns>
        public static bool GridRowExists(IDictionary<string, string> row, string gridId)
        {
            IDictionary<string, int> columnIndex = new Dictionary<string, int>();
            foreach (string caption in row.Keys)
            {
                columnIndex.Add(caption, GetDatalistColumnIndex(getXGridHeaders(gridId), caption));
            }
            
            return Exists(getXGridRow(gridId, columnIndex, row));
        }

        /// <summary>
        /// Formats an XPath to find a single row in a grid/datalist.
        /// </summary>
        /// <param name="gridId">The unique id of the grid/datalist.  i.e. '_DESIGNATIONS_value'</param>
        /// <param name="columnCaptionToIndex">Mapping of the column captions to their index in the TD.</param>
        /// <param name="columnCaptionToValue">Mapping of the column captions to a single row's values.</param>
        public static string getXGridRow(string gridId, IDictionary<string, int> columnCaptionToIndex, IDictionary<string, string> columnCaptionToValue)
        {
            string finalString = getXGridRows(gridId);
            foreach (string columnCaption in columnCaptionToValue.Keys)
            {
                finalString += getXDataListColumnValue(columnCaptionToIndex[columnCaption], columnCaptionToValue[columnCaption]);
            }
            return finalString;
        }

        /// <summary>
        /// Format an XPath to find the TR rows of a grid/datalist.
        /// </summary>
        /// <param name="gridId">The unique id of the grid/datalist.  i.e. '_DESIGNATIONS_value'</param>
        public static string getXGridRows(string gridId)
        {
            return String.Format("{0}//div[@class='x-grid3-body']//div[contains(@class, 'x-grid3-row')]/table/tbody/tr", getXGrid(gridId));
        }

        /// <summary>
        /// Format an XPath to find the TR header row of a grid/datalist that contains the column captions.
        /// </summary>
        /// <param name="gridId">The unique id of the grid/datalist.  i.e. '_DESIGNATIONS_value'</param>
        public static string getXGridHeaders(string gridId)
        {
            return String.Format("{0}//div[@class='x-grid3-header']//tr", getXGrid(gridId));
        }

        /// <summary>
        /// Format an XPath to find a grid/datalist.
        /// </summary>
        /// <param name="gridId">The unique id of the grid/datalist.  i.e. '_DESIGNATIONS_value'</param>
        public static string getXGrid(string gridId)
        {
            return String.Format("//div[not(contains(@class,'x-hide-display'))]/div[contains(@class,'summarycontainer')]//div[contains(@id, '{0}')]", gridId);
        }

        /// <summary>
        /// Select a Tab from the top Tab bar.
        /// </summary>
        /// <param name="caption">The caption of the tab.</param>
        public static void SelectTab(string caption)
        {
            GetDisplayedElement(getXTabsBar);

            if (ExistsNow(getXTab(caption))) { 
                //retry logic.  wait until the clicked tab is actually selected.
                //can sometimes click it too fast and then no tab load occurs
                WebDriverWait waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
                waiter.IgnoreExceptionTypes(typeof(InvalidOperationException));
                waiter.Until(d =>
                {
                    WaitClick(getXTab(caption));
                    if (d.FindElement(By.XPath(getXTabsBarActiveTab)).Text != caption) return false;
                    return true;
                });
            }
            //tab does not exist or is hidden by the scroll-arrow
            //click scroll-arrow if possible and try again or throw exception.
            else if (ExistsNow(getXTabsBarRightScroller) &&
                !GetDisplayedElement(getXTabsBarRightScroller).GetAttribute("class").Contains("disabled"))
            {

                WaitClick(getXTabsBarRightScroller);
                SelectTab(caption);
            }
            else throw new WebDriverException(String.Format("Tab '{0}' does not exist on the current Panel's top Tab bar.", caption));
        }

        /// <summary>
        /// Select a Tab from an inner Tab bar.
        /// </summary>
        /// <param name="caption">The caption of the tab.</param>
        public static void SelectInnerTab(string caption)
        {
            GetDisplayedElement(getXInnerTabsBar);
            WaitClick(getXInnerTab(caption));
        }

        /// <summary>
        /// Open tab for the passed in string
        /// </summary>
        /// <param name="caption">The name of the tab to be opened</param>
        public static void OpenTab(string caption)
        {
            WaitClick(getXPanelTab(caption));
        }

        /// <summary>
        /// Click the 'Add' button of a section.
        /// </summary>
        /// <param name="sectionCaption">The caption of the section that the button belongs to.</param>
        /// <param name="addButtonCaption">The caption of the button to click.  Defaults to 'Add'.</param>
        public static void ClickSectionAddButton(string sectionCaption, string addButtonCaption = "Add")
        {
            //wait for the section's datalist headers to be displayed.  Clicking
            //the add button too early can fire a javascript error where the button
            //is not tied to an action yet.
            GetDisplayedElement(getXSectionDatalistColumnHeaders(sectionCaption));
            //still getting the error with BHF occassionaly where it claims it clicked the button but nothing happens...
            WaitClick(getXSectionAddButton(sectionCaption, addButtonCaption));
        }

        /// <summary>
        /// Check if the panel header's text matches the expected value.
        /// </summary>
        /// <param name="expectedValue">The expected value of the header.</param>
        /// <returns>True if the text of the header matches the provided expected value, false otherwise.</returns>
        public static bool IsPanelHeader(string expectedValue)
        {
            return GetDisplayedElement(getXPanelHeader()).Text == expectedValue;
        }

        /// <summary>
        /// Check if the panel header's image contains the provided image type.
        /// </summary>
        /// <param name="panelImage">The unique image name of the panel.  i.e. 'individual.png'</param>
        /// <returns>True if the panel header image contains the provided image type, false otherwise.</returns>
        public static bool IsPanelType(string panelImage)
        {
            return GetDisplayedElement(getXPanelHeader() + "/../..").GetAttribute("style").Contains(panelImage);
        }

        /// <summary>
        /// Wait until the panel header's image contains the provided image type.
        /// A WebDriverTimeoutException is thrown is no displayed panel header loads with the image.
        /// </summary>
        /// <param name="panelImage">The unique image name of the panel.  i.e. 'individual.png'</param>
        public static void WaitForPanelType(string panelImage)
        {
            GetDisplayedElement(getXPanelHeader(panelImage));
        }

        /// <summary>
        /// Wait the default time for the panel header to not contain the provided value.
        /// </summary>
        /// <param name="header">The value to wait until not displayed in the header.</param>
        public static void WaitForPanelHeaderNotContain(string header)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            waiter.Until(d =>
            {
                var panelHeader = GetDisplayedElement(getXPanelHeader());
                return !panelHeader.Text.Contains(header);
            });
        }

        /// <summary>
        /// Expand a row if it is not already expanded.
        /// </summary>
        public static void ExpandRow(string XPath)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            waiter.Until(d =>
            {
                var expandButton = GetEnabledElement(XPath + "//button[contains(@class,'bbui-pages-section-expandbutton')]");
                if (!expandButton.GetAttribute("class").Contains("bbui-pages-section-collapsebutton"))
                {
                    expandButton.Click();
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Check if additional pages exist for a datalist to load.  If so load the page.
        /// </summary>
        /// <returns>Returns true if an additional page was found and clicked, false otherwise.</returns>
        public static bool AdditionalDatalistPagesExist(string sectionCaption)
        {
            WebDriverWait waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(1));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            bool additionalPage = false;

            //get current datalist page number
            int datalistPage = Int32.MaxValue;
            try
            {
                waiter.Until(d =>
                {
                    IWebElement currentDatalistPage =
                        d.FindElement(By.XPath(getXSectionDatalistCurrentPageIndex(sectionCaption)));
                    if (currentDatalistPage != null && currentDatalistPage.Displayed)
                    {
                        datalistPage = Int32.Parse(currentDatalistPage.Text);
                        return true;
                    }
                    return false;
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }

            //see if button exists with higher page index and click it
            waiter.Until(d =>
            {
                ICollection<IWebElement> datalistPageButtons = d.FindElements(By.XPath(getXSectionDatalistPageIndexButtons(sectionCaption)));
                foreach (IWebElement button in datalistPageButtons)
                {
                    try
                    {
                        int nextPageIndex = Int32.Parse(button.Text);
                        if (button.Displayed && button.Enabled && nextPageIndex == datalistPage + 1)
                        {
                            additionalPage = true;
                            button.Click();
                            //wait until the current page index is the next page
                            WebDriverWait innerWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
                            innerWaiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
                            innerWaiter.Until(dd =>
                            {
                                IWebElement currentIndex = d.FindElement(By.XPath(getXSectionDatalistCurrentPageIndex(sectionCaption)));
                                if (currentIndex != null && currentIndex.Displayed 
                                    && Int32.Parse(currentIndex.Text) == nextPageIndex) return true;
                                return false;
                            });
                            return true;
                        }
                    }
                    catch (FormatException)
                    {
                    }
                }
                return false;
            });

            return additionalPage;
        }

        /// <summary>
        /// XPath for Event search results grid name fields
        /// </summary>
        protected const string getXEventSearchResultsGridNameFields = "//*[contains(@class,\"bbui-dialog\") and contains(@style,\"visible\")]//*[contains(@class,\"x-grid3-body\")]/div/table/tbody/tr[1]/td[3]/div";

        /// <summary>
        /// Xpath for Event search results bar
        /// </summary>
        protected const string getxEventSearchResultsBar = "//*[contains(@class,\"bbui-dialog\") and contains(@style,\"visible\")]//*[contains(@class,'x-toolbar-cell')]/div[contains(@class,'xtb-text')]";

        /// <summary>
        /// Xpath for search results grid
        /// </summary>
        protected const string getXSearchResultsGrid = "//*[contains(@class,\"bbui-dialog\") and contains(@style,\"visible\")]//*[contains(@class,'x-grid3-body')]/div";

        /// <summary>
        /// Check grid results contain items in parameters
        /// </summary>
        /// <param name="expected">?</param>
        /// <param name="XnameField">?</param>
        /// <param name="XresultsBar">?</param>
        /// <param name="resultsBarText">?</param>
        /// <param name="XresultsGrid">?</param>
        public static void CheckGridResultsContain(string expected, string XnameField = getXEventSearchResultsGridNameFields, string XresultsBar = getxEventSearchResultsBar,string resultsBarText = "found", string XresultsGrid = getXSearchResultsGrid)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException));
            ICollection<IWebElement> nameFieldElements = new Collection<IWebElement>();
            waiter.Until(d =>
            {
                var searchResultsToolbarElement = Driver.FindElement(By.XPath(XresultsBar));
                nameFieldElements = Driver.FindElements(By.XPath(XnameField));
                var searchResultsGrid = Driver.FindElement(By.XPath(XresultsGrid));

                if ((nameFieldElements == null ||
                     !searchResultsGrid.Displayed ||
                     !searchResultsToolbarElement.Text.Contains(resultsBarText))) return false;

                var names = from element in nameFieldElements select element.Text;

                if (!names.Contains(expected)) return false;

                return true;
            });
        }

    }
}
