using System;
using System.Collections.Generic;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Base class to handle the interactions for dialogs. 
    /// </summary>
    public class Dialog : BaseComponent
    {
        /// <summary>
        /// Constant xPath for the tab strip of a dialog.
        /// </summary>
        public const string getXTabStrip = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@class,'x-tab-panel-header')]//li[contains(@class,'x-tab-strip-active')]/..";

        /// <summary>
        /// Format a unique xPath for a tab on a dialog.
        /// </summary>
        /// <param name="tabCaption">The caption of the tab.</param>
        protected static string getXTab(string tabCaption)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@class,'x-tab-panel-header')]//span[./text() = '{0}']", tabCaption);
        }

        /// <summary>
        /// Format a unique xPath for a tab on a dialog.
        /// </summary>
        /// <param name="tabCaption">The caption of the tab.</param>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'RevenueBatchConstituentInbatchEditForm'</param>
        protected static string getXTab(string tabCaption, string dialogId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]//div[contains(@class,'x-tab-panel-header')]//span[text() = '{1}']", dialogId, tabCaption);
        }

        /// <summary>
        /// Open the tab of a dialog.
        /// </summary>
        /// <param name="caption">The caption of the tab.</param>
        public static void OpenTab(string caption)
        {
            //need to add support for multiple rows of tabs and tabs hidden by additional tabs arrow
            //GetDisplayedElement(getXTabStrip);
            WaitClick(getXTab(caption));
        }

        /// <summary>
        /// Open the tab of a dialog.
        /// </summary>
        /// <param name="caption">The caption of the tab.</param>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'RevenueBatchConstituentInbatchEditForm'</param>
        public static void OpenTab(string caption, string dialogId)
        {
            WaitClick(getXTab(caption, dialogId));
        }

        /// <summary>
        /// Returns an xPath for the close Window [X] on the Dialog.
        /// 
        /// </summary>
        public static string getXCloseX() { return String.Format("//*[contains(@class,\"bbui-dialog\") and contains(@style,\"visible\")]//*[contains(@class,'x-tool-close')]"); }

        /// <summary>
        /// Returns an xPath for any button that is a decendant of a visible bbui-dialog and having "buttonText" text.
        /// Uniqueness depends on the text passed.
        /// 
        /// </summary>
        /// <param name="buttonText">The text of the required button.</param>        
        public static string getXDialogButton(string buttonText) { return String.Format("//*[contains(@class,\"bbui-dialog\") and contains(@style,\"visible\")]//*[./text()=\"{0}\" and contains(@class,\"x-btn-text\")]", buttonText); }

        /// <summary>
        /// Format an xPath for finding a dialog's button including the common bottom bar that contains buttons
        /// such as Save, Close, and Cancel.
        /// </summary>
        /// <param name="buttonText">The caption of the button.</param>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'RevenueBatchConstituentInbatchEditForm'</param>
        public static string getXEntireDialogButton(string buttonText, string dialogId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'{0}')]/../../../../../../../..//*[text()=\"{1}\" and contains(@class,\"x-btn-text\")]", dialogId, buttonText);
        }

        /// <summary>
        /// Format an xPath for finding a dialog's button.
        /// </summary>
        /// <param name="buttonText">The caption of the button.</param>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'RevenueBatchConstituentInbatchEditForm'</param>
        public static string getXDialogButton(string buttonText, string dialogId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'{0}')]//button[text()=\"{1}\" and contains(@class,\"x-btn-text\")]", dialogId, buttonText);
        }

        /// <summary>
        /// Constant xPath for the searchlist icon of a grid's edit cell.
        /// </summary>
        public const string getXGridEditCellSearchlist = "//div[contains(@class,'x-grid-editor') and contains(@style,'visibility: visible')]//input[contains(@class,'x-form-focus')]/..//img[contains(@class,'search-trigger')]";

        /// <summary>
        /// Constant xPath for the dropdown arrow of a grid's edit cell.
        /// </summary>
        public const string getXGridEditCellDropdownArrow = "//div[contains(@class,'x-grid-editor') and contains(@style,'visibility: visible')]//input[contains(@class,'x-form-focus')]/..//img[contains(@class,'arrow-trigger')]";

        /// <summary>
        /// The unique xPath for the input of the currently selected cell in a Grid.
        /// 
        /// When a grid cell is selected, an HTML edit input is created where the actual value should
        /// be set.
        /// </summary>
        public const string getXGridEditCell = "//div[contains(@class,'x-grid-editor') and contains(@style,'position: absolute') and contains(@style,'visibility: visible')]//input";

        /// <summary>
        /// The unique xPath for the input of the currently selected cell in a Grid adding the 'focused' criteria
        /// 
        /// NOTE: this is risky to use because the focus attribute is unreliably set and can cause MANY race conditions.
        /// The 'focused' criteria is also not always set on the edit cell depending on the approach you went to select a grid cell.
        /// 
        /// When a grid cell is selected, an HTML edit input is created where the actual value should
        /// be set.
        /// </summary>
        public const string getXGridFocusedEditCell = "//div[contains(@class,'x-grid-editor') and contains(@style,'position: absolute') and contains(@style,'visibility: visible')]//input[contains(@class,'x-form-focus')]";

        /// <summary>
        /// Formats the unique xPath for getting a single grid TD element that belongs to a specified
        /// row and columun in a grid on a dialog.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'PlanOutlineAddForm'</param>
        /// <param name="gridId">The HTML [] element's unique id identifier - i.e. 'PLANOUTLINESTEPS'</param>
        /// <param name="row">Row number to use.  Counting starts at 1</param>
        /// <param name="columnIndex">Column index to use.  Column indexing does NOT always start at 1 or 0 as columns can be hidden.</param>
        public static string getXGridCell(string dialogId, string gridId, int row, int columnIndex)
        {
            return String.Format("//div[contains(@id,'{0}')]//div[contains(@id, '{1}')]//div[@class='x-grid3-body']/div[{2}]/table/tbody/tr/td[{3}]",
                dialogId, gridId, row, columnIndex);
        }

        /// <summary>
        /// Formats the unique xPath for getting a single grid row.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'PlanOutlineAddForm'</param>
        /// <param name="gridId">The HTML [] element's unique id identifier - i.e. 'PLANOUTLINESTEPS'</param>
        /// <param name="row">Row number to use.  Counting starts at 1</param>
        public static string getXGridRow(string dialogId, string gridId, int row)
        {
            return String.Format("//div[contains(@id,'{0}')]//div[contains(@id, '{1}')]//div[@class='x-grid3-body']/div[{2}]", dialogId, gridId, row.ToString());
        }

        /// <summary>
        /// Format the unique xPath for selecting a single grid row.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'PlanOutlineAddForm'</param>
        /// <param name="gridId">The HTML [] element's unique id identifier - i.e. 'PLANOUTLINESTEPS'</param>
        /// <param name="row">Row number to use.  Counting starts at 1</param>
        public static string getXGridRowSelector(string dialogId, string gridId, int row)
        {
            return String.Format("{0}//td[1]", getXGridRow(dialogId, gridId, row));
        }

        /// <summary>
        /// Formats the unique xPath for getting the TR row header element that belongs to
        /// a grid on a dialog.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'PlanOutlineAddForm'</param>
        /// <param name="gridId">The HTML [] element's unique id identifier - i.e. 'PLANOUTLINESTEPS'</param>
        public static string getXGridHeaders(string dialogId, string gridId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'{0}')]//div[contains(@id, '{1}')]//div[@class='x-grid3-header']//tr", dialogId, gridId);
        }

        /// <summary>
        /// Constant xPath for the 'mceContentBody' of an HTML element.  This is generally in an IFrame field.
        /// </summary>
        public const string getXIFrameHtmlBodyP = "//html/body[contains(@class, 'mceContentBody')]/p";

        /// <summary>
        /// Given the unique HTML element ids of a dialog and div, return a unique identifier xPath
        /// to find the IFRAME field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_individualSpouseBusinessAddForm'</param>
        /// <param name="divId">The DIV element's unique id identifier - i.e. 'HTMLNOTE'</param>
        public static string getXIFrame(string dialogId, string divId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]//div[contains(@id, '{1}')]//iframe", dialogId, divId);
        }

        /// <summary>
        /// Format an xPath to find a link on the dialog.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_individualSpouseBusinessAddForm'</param>
        /// <param name="linkId">The unique id associated with the link - i.e. 'SPLITSACTION'</param>
        public static string getXLinkByAction(string dialogId, string linkId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]//a[contains(@id, '{1}')]", dialogId, linkId);
        }

        /// <summary>
        /// Format an xPath to find a link on the dialog.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_individualSpouseBusinessAddForm'</param>
        /// <param name="caption">'The caption/text of the link.</param>
        public static string getXLinkByCaption(string dialogId, string caption)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]//a[contains(text(), '{1}')]", dialogId, caption);
        }

        /// <summary>
        /// Given the unique HTML element ids of a dialog and div, return a unique identifier xPath
        /// to find the DIV field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'PledgeAddForm'</param>
        /// <param name="divId">The 'a' link element's unique id identifier - i.e. 'SHOWSYSTEM'</param>
        public static string getXDiv(string dialogId, string divId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]//div[contains(@id, '{1}')]", dialogId, divId);
        }

        /// <summary>
        /// Given the unique HTML element ids of a dialog and textarea, return a unique identifier xPath
        /// to find the TEXTAREA field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_individualSpouseBusinessAddForm'</param>
        /// <param name="textAreaId">The TEXTAREA element's unique id identifier - i.e. '_DESCRIPTION_value'</param>
        public static string getXTextArea(string dialogId, string textAreaId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]//textarea[contains(@id, '{1}')]", dialogId, textAreaId);
        }

        /// <summary>
        /// Format a unique xPath identifier to find a button on a dialog.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'EventManagementTemplateAddDataForm'</param>
        /// <param name="buttonCaption">The caption of the button.</param>
        public static string getXButton(string dialogId, string buttonCaption)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]//button[./text()='{1}']", dialogId, buttonCaption);
        }

        /// <summary>
        /// Given the unique HTML element ids of a dialog and span, return a unique identifier xPath
        /// to find the SPAN field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_EventSummaryViewForm3'</param>
        /// <param name="spanId">The SPAN element's unique id identifier - i.e. '_LOCATION_value'</param>
        public static string getXSpan(string dialogId, string spanId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]//span[contains(@id, '{1}')]", dialogId, spanId);
        }

        /// <summary>
        /// Given the unique HTML element ids of a dialog and input, return a unique identifier xPath
        /// to find the INPUT field.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. '_individualSpouseBusinessAddForm'</param>
        /// <param name="inputId">The INPUT element's unique id identifier - i.e. '_LASTNAME_value'</param>
        public static string getXInput(string dialogId, string inputId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]//input[contains(@id, '{1}')]", dialogId, inputId);
        }

        /// <summary>
        /// Format an xPath to find the search trigger button associated with an input field.
        /// </summary>
        /// <param name="dialogInputXPath">The unique xPath of the dialog input.  Likely will have used Dialog.getXInput() to format
        /// the xPath.</param>
        public static string getXInputSearchTrigger(string dialogInputXPath)
        {
            return string.Format("{0}/..//span[@class='x-form-twin-triggers']/img[contains(@class,'x-form-search-trigger')]", dialogInputXPath);
        }

        /// <summary>
        /// Format an xPath to find the new-form trigger button associated with an input field.
        /// </summary>
        /// <param name="dialogInputXPath">The unique xPath of the dialog input.  Likely will have used Dialog.getXInput() to format
        /// the xPath.</param>
        public static string getXInputNewFormTrigger(string dialogInputXPath)
        {
            return string.Format("{0}/..//span[@class='x-form-twin-triggers']/img[contains(@class,'bbui-forms-new-trigger')]", dialogInputXPath);
        }

        /// <summary>
        /// Format an xPath to get a visible dialog.
        /// </summary>
        public static string getXVisibleDialog()
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]");
        }

        /// <summary>
        /// Format an xPath to get a dialog based on its unique id.
        /// </summary>
        /// <param name="dialogId">The dialog's unique id identifier - i.e. 'FundraisingSearch'</param>
        public static string getXDialogById(string dialogId)
        {
            return String.Format("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id, '{0}')]", dialogId);
        }

        /// <summary>
        /// Format an xPath to get a dialog based on an value contained in the class attribute of the root dialog DIV.
        /// </summary>
        /// <param name="classValue">The value to see if contained in the class attribute.</param>
        public static string getXDialogByClass(string classValue)
        {
            return
                String.Format(
                    "//div[contains(@class,'bbui-dialog') and contains(@style,'visible') and contains(@class,'{0}')]", classValue);
        }

        /// <summary>
        /// Constant xPath value for finding a 'Save and close' button WebElement.
        /// </summary>
        public const string getXSaveAndCloseButton = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//button[./text() = 'Save and close']";

        /// <summary>
        /// Constant xPath value for finding a 'Yes' button WebElement.
        /// </summary>
        public const string getXYesButton = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//button[./text() = 'Yes']";

        /// <summary>
        /// Constant xPath value for finding an OK button WebElement.
        /// </summary>
        public const string getXOKButton = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//button[./text() = 'OK']";

        /// <summary>
        /// Constant xPath value for finding a Cancel button WebElement.
        /// </summary>
        public const string getXCancelButton = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//button[./text() = 'Cancel']";

        /// <summary>
        /// Constant xPath value for finding a Save button WebElement.
        /// </summary>
        public const string getXSaveButton = "//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//button[./text() = 'Save']";

        /// <summary>
        /// Click the 'Yes' button
        /// </summary>
        public static void Yes()
        {
            WaitClick(getXYesButton);
        }

        /// <summary>
        /// Click the 'OK' button
        /// </summary>
        public static void OK()
        {
            WaitClick(getXOKButton);
        }

        /// <summary>
        /// Click the 'Cancel' button
        /// </summary>
        public static void Cancel()
        {
            WaitClick(getXCancelButton);
        }

        /// <summary>
        /// Click the 'Save' button
        /// </summary>
        public static void Save()
        {
            WaitClick(getXSaveButton);
        }

        /// <summary>
        /// Constant xPath for the entire visible dropdown list.
        /// </summary>
        public const string getXDropdownList = "//div[contains(@class,'x-combo-list') and contains(@style,'visibility: visible')]";

        /// <summary>
        /// Formats an xPath for a dropdown item.
        /// </summary>
        /// <param name="caption">The caption value of the dropdown list item.</param>
        public static string getXDropdownItem(string caption)
        {
            return String.Format("//div[contains(@class,'x-combo-list') and contains(@style,'visibility: visible')]//div[contains(@class,'x-combo-list-item') and @title='{0}']", caption);
        }

        /// <summary>
        /// Constant xPath for all items in the visible dropdown list.
        /// </summary>
        public const string getXDropdownListItems = "//div[contains(@class,'x-combo-list') and contains(@style,'visibility: visible')]//div[contains(@class,'x-combo-list-item')]";

        /// <summary>
        /// Formats an xPath for the arrow dropdown of a dropdown INPUT field.
        /// </summary>
        /// <param name="dropdownxPath">The xPath of the dropdown.  Advised to use Dialog.getXInput()</param>
        public static string getXDropdownArrow(string dropdownxPath)
        {
            return String.Format("{0}/..//img[contains(@class,'x-form-arrow-trigger')]", dropdownxPath);
        }

        /// <summary>
        /// Check if a value exists as a selectable item in a dropdown list.
        /// </summary>
        /// <param name="xPath">The xPath of the dropdown.  Advised to use Dialog.getXInput()</param>
        /// <param name="value">The value to check for as an option in the dropdown.</param>
        /// <returns>True if the value is an option, false otherwise.</returns>
        public static bool DropdownValueExists(string xPath, string value)
        {
            WebDriverWait waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            //click the arrow
            waiter.Until(d =>
            {
                IWebElement element = d.FindElement(By.XPath(getXDropdownArrow(xPath)));
                if (element != null && element.Displayed && element.Enabled)
                {
                    element.Click();
                    return true;
                }
                return false;
            });

            //wait for the list to appear
            waiter.Until(d =>
            {
                IWebElement dropdownList = d.FindElement(By.XPath(getXDropdownListItems));
                if (dropdownList == null || !dropdownList.Displayed) return false;
                return true;
            });

            //see if value exists in list.  instead of iterating through all items, I could format an xPath and use the driver
            //without a waiter to find the element.  if found, return true.  if nosuchelement thrown, return false.
            ICollection<IWebElement> dropdownListItems = Driver.FindElements(By.XPath(getXDropdownListItems));
            if (dropdownListItems.Count == 0) return false;
            else
            {
                foreach (IWebElement item in dropdownListItems)
                {
                    if (item.Enabled && item.Text == value) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Set an IFrame HTML field's value.
        /// </summary>
        /// <param name="xPath">The xPath of the IFrame.  Advised to use Dialog.getXIFrame()</param>
        /// <param name="value">The value to set in the HTML body of the IFrame.</param>
        public static void SetHtmlField(string xPath, string value = "")
        {
            if (value == null) return;
            CopyToClipBoard(value);

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException));
            waiter.Until(d =>
            {
                var iframeElement = d.FindElement(By.XPath(xPath));
                if (iframeElement == null
                    || !iframeElement.Displayed || !iframeElement.Enabled) return false;

                iframeElement.Click();
                iframeElement.SendKeys(Keys.Control + "a");
                iframeElement.SendKeys(Keys.Control + "v");
                iframeElement.SendKeys(Keys.Tab);
                return true;
            });

            /*
             * The iframe HTML fields do not have any children.  the next child is a '#document'
             * element that cannot be queried or discovered by xPath.  
             * 
             * The IFrame's value attribute will not be set to the desired value.  The <p> child DOM element
             * of the IFrame has its text set to the desired value, but a different selector from xPaths needs
             * to be used in order to access it.
             * 
             * To get that element via xPaths you can only start your root search with the top level HTML
             * element in the dialog.  ie '//html/body[contains(@class, 'mceContentBody')]'
             * 
             */
        }

        /// <summary>
        /// Set a Dropdown field.
        /// 
        /// WebDriverTimeoutException is thrown if no value is found.
        /// </summary>
        /// <param name="xPath">The xPath to find the dropdown INPUT element with</param>
        /// <param name="value">The value to set the dropdown to.</param>
        public static void SetDropDown(string xPath, string value = "")
        {
            WaitClick(xPath); 

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            waiter.Until(d =>
            {
                var innerWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                innerWaiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

                try
                {
                    //click the dropdown arrow
                    waiter.Until(d1 =>
                    {
                        var dropdownArrow = d.FindElement(By.XPath(getXDropdownArrow(xPath)));
                        if (dropdownArrow == null || !dropdownArrow.Displayed || !dropdownArrow.Enabled) return false;
                        dropdownArrow.Click();
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                try
                {
                    //wait for the list to appear
                    waiter.Until(d2 =>
                    {
                        var dropdownList = d.FindElement(By.XPath(getXDropdownListItems));
                        return dropdownList != null && dropdownList.Displayed;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                try
                {
                    //select the item in the list
                    waiter.Until(d3 =>
                    {
                        var dropdownItem = d.FindElement(By.XPath(getXDropdownItem(value)));
                        if (dropdownItem == null || !dropdownItem.Displayed || !dropdownItem.Enabled) return false;
                        dropdownItem.Click();
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                try
                {
                    //send enter on the field
                    waiter.Until(d4 =>
                    {
                        var field = d.FindElement(By.XPath(xPath));
                        if (field == null || !field.Displayed) return false;
                        field.SendKeys(Keys.Tab);
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                try
                {
                    //ensure the original grid value has been set before continuing
                    ElementValueIsSet(xPath, value);
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                return true;
            });
        }

        /// <summary>
        /// Set a grid cell's value using the searchlist.
        /// </summary>
        /// <param name="xPath">The xPath of the grid cell TD element.</param>
        /// <param name="searchDialogxPath">The xPath of the search dialog's field to set.</param>
        /// <param name="value">The value to set and use as search criteria in the specified search dialog field.</param>
        public static void SetGridSearchList(string xPath, string searchDialogxPath, string value){
            var clickedWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            clickedWaiter.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            clickedWaiter.Until(d1 =>
            {
                try
                {
                    //original field should be covered by the edit cell and therefore unclickable
                    d1.FindElement(By.XPath(xPath)).Click();
                    return false;
                }
                catch (InvalidOperationException)
                {
                    try
                    {
                        var innerWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                        innerWaiter.IgnoreExceptionTypes(typeof(InvalidOperationException),
                            typeof(StaleElementReferenceException));

                        //click the searchlist
                        innerWaiter.Until(d =>
                        {
                            var searchIcon = d.FindElement(By.XPath(getXGridEditCellSearchlist));
                            if (searchIcon == null || !searchIcon.Displayed || !searchIcon.Enabled) return false;
                            searchIcon.Click();
                            return true;
                        });
                    }
                    catch (WebDriverTimeoutException)
                    {
                        return false;
                    }
                    return true;
                }
            });

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

            //search and select
            SetTextField(searchDialogxPath, value);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();

            //send enter on the edit grid
            waiter.Until(d =>
            {
                //sometimes the original field is set and there is no need to send enter on the edit cell.
                //The value you search for may not be the associated display value of the selected item...
                var originalField = d.FindElement(By.XPath(xPath));
                if (originalField.Displayed &&
                    (!String.IsNullOrWhiteSpace(originalField.Text) || !String.IsNullOrWhiteSpace(originalField.GetAttribute("value")))) 
                    return true;

                try
                {
                    IWebElement editField = d.FindElement(By.XPath(getXGridEditCell));
                    if (editField == null || !editField.Displayed) return false;
                    editField.SendKeys(Keys.Return);
                    return true;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            //ensure the original grid value is not an empty string
            ElementValueIsNotNullOrEmpty(xPath);
        }

        /// <summary>
        /// Set a Grid cell's value for a dropdown field.
        /// </summary>
        /// <param name="xPath">The xPath of the grid cell TD element.</param>
        /// <param name="value">The value to set the cell to.</param>
        public static void SetGridDropDown(string xPath, string value = "")
        {
            var setWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            setWaiter.Until(d =>
            {
                try
                {
                    var clickedWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                    clickedWaiter.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                    clickedWaiter.Until(d1 =>
                    {
                        try
                        {
                            //original field should be covered by the edit cell and therefore unclickable
                            d1.FindElement(By.XPath(xPath)).Click();
                            return false;
                        }
                        catch (InvalidOperationException)
                        {
                            return true;
                        }
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

                //click the dropdown arrow
                try
                {
                    waiter.Until(d1 =>
                    {
                        var dropdownArrow = d1.FindElement(By.XPath(getXGridEditCellDropdownArrow));
                        if (dropdownArrow == null || !dropdownArrow.Displayed || !dropdownArrow.Enabled) return false;
                        dropdownArrow.Click();
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                //wait for the list to appear
                try
                {
                    waiter.Until(d2 =>
                    {
                        var dropdownList = d2.FindElement(By.XPath(getXDropdownListItems));
                        return dropdownList != null && dropdownList.Displayed;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                //select the item in the list
                try
                {
                    waiter.Until(d3 =>
                    {
                        var dropdownItem = d3.FindElement(By.XPath(getXDropdownItem(value)));
                        if (dropdownItem == null || !dropdownItem.Displayed || !dropdownItem.Enabled) return false;
                        dropdownItem.Click();
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                //wait for the editgrid to be set
                try
                {
                    ElementValueIsSet(getXGridEditCell, value);
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                //send enter on the edit grid
                try
                {
                    waiter.Until(d4 =>
                    {
                        var editField = d4.FindElement(By.XPath(getXGridEditCell));
                        if (editField == null || !editField.Displayed) return false;
                        editField.SendKeys(Keys.Return);
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                //ensure the original grid value has been set before continuing
                try
                {
                    ElementValueIsSet(xPath, value);
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                return true;
            });

        }

        /// <summary>
        /// Set a Grid cell's value for a text field.
        /// </summary>
        /// <param name="xPath">The xPath of the grid cell TD element.</param>
        /// <param name="value">The value to set the cell to.</param>
        public static void SetGridTextField(string xPath, string value = "")
        {
            if (value == null) return;

            var setWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            setWaiter.Until(d =>
            {
                //Have to click several elements potentially.  there are rare moments when the edit field is weirdly on the 2nd
                //row when trying to click the first row.
                try
                {
                    var clickedWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                    clickedWaiter.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                    clickedWaiter.Until(d1 =>
                    {
                        try
                        {
                            //original field should be covered by the edit cell and therefore unclickable
                            d1.FindElement(By.XPath(xPath)).Click();
                            return false;
                        }
                        catch (InvalidOperationException)
                        {
                            return true;
                        }
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                CopyToClipBoard(value);

                try
                {
                    var setValueWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                    setValueWaiter.IgnoreExceptionTypes(typeof(InvalidOperationException));
                    setValueWaiter.Until(d2 =>
                    {
                        var element = d2.FindElement(By.XPath(getXGridEditCell));
                        if (element == null
                            || !element.Displayed || !element.Enabled) return false;
                        element.SendKeys(Keys.Control + "a");
                        element.SendKeys(Keys.Control + "v");
                        return true;
                    });

                    //wait for value of edit cell to be set
                    setValueWaiter.Until(d3 =>
                    {
                        var element = d3.FindElement(By.XPath(getXGridEditCell));
                        if (element != null && element.Displayed && element.Enabled
                            && ((element.Text != null && element.Text.Contains(value)) || (element.GetAttribute("value") != null && element.GetAttribute("value").Contains(value)) ))
                        {
                            element.SendKeys(Keys.Return);
                            return true;
                        }
                        return false;
                    });

                    //ensure the original grid value has been set before continuing
                    ElementValueIsSet(xPath, value);
                }
                catch (WebDriverTimeoutException) { return false; }

                return true;
            });
        }

        /// <summary>
        /// Set a field's value using the associated search dialog to the first found item in the search results.
        /// </summary>
        /// <param name="xPath">The xPath of the field to set.</param>
        /// <param name="searchFieldxPath">The xPath of the search dialog's field to set.</param>
        /// <param name="value">The value to set and use as search criteria in the specified search dialog field.</param>
        public static void SetSearchList(string xPath, string searchFieldxPath, string value = "")
        {
            /*
             * A formfield with a searchlist does different actions based on the input value.
             * If an existing item is input and enter is hit, then the searchlist appears.  If
             * a partial item name is input and enter is hit, then an auto-search in the background
             * is done to try and find a suggested item. If an item is found then auto-complete changes
             * the current value to the suggest value.  If no suggested item is found, a searchlist appears.
             * 
             * This makes is difficult to come up with consistent behavior setting the value directly into the field
             * and checking for UI element changes.  Instead I am defaulting to opening the searchlist always,
             * looking for the provided value, and selecting the first found element.
             */

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException));

            waiter.Until(d =>
            {
                IWebElement searchField = GetEnabledElement(xPath);
                if (searchField == null
                    || searchField.Displayed == false || searchField.Enabled == false) return false;
                return true;
            });

            GetEnabledElement(xPath);

            //click the search button
            WaitClick(getXInputSearchTrigger(xPath));

            //enter the search criteria
            SetTextField(searchFieldxPath, value);

            //search
            WaitClick(SearchDialog.getXSearchButton);

            //select
            WaitClick(SearchDialog.getXSelectButton);

            ElementValueIsNotNullOrEmpty(xPath);
        }

        /// <summary>
        /// Click a button that has a new dialog appear.  Continuely click the button
        /// until the new dialog appears.
        /// </summary>
        /// <param name="caption">The caption of the dialog button</param>
        /// <param name="dialogId">The new dialog's unique id identifer. i.e. - 'RevenueBatchConstituentInbatchEditForm' 
        /// If null is provided, then the method waits for a message box dialog that contains no unique id.</param>
        public static void ClickButton(string caption, string dialogId)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            waiter.Until(d =>
            {
                ClickButton(caption);
                try
                {
                    GetDisplayedElement(
                        dialogId == null ? getXDialogByClass("bbui-dialog-msgbox") : getXDialogById(dialogId), 30);
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Click a dialog button.
        /// </summary>
        /// <param name="caption">The caption of the dialog button.</param>
        /// <param name="timeout">The amount of time to try and find a valid button to click before
        /// a WebDriverTimeoutException is thrown.</param>
        public static void ClickButton(string caption, double timeout)
        {
            WaitClick(getXDialogButton(caption), timeout);
        }

        /// <summary>
        /// Click a dialog button.
        /// </summary>
        /// <param name="caption">The caption of the dialog button.</param>
        public static new void ClickButton(string caption)
        {
            ClickButton(caption, TimeoutSecs);
        }

        public void CloseDialog()
        {
            WaitClick(getXCloseX());
        }

        /// <summary>
        /// Wait for a dialog to appear that has a unique is in the provided list of
        /// ids and return the matching id.
        /// </summary>
        /// <param name="supportedIds">A list of ids to wait for.</param>
        /// <returns>The first found matching id for a dialog that is visible with that id.
        /// If no id is found, a WebDriverTimeoutException is eventually thrown.</returns>
        public static string GetDialogId(IEnumerable<string> supportedIds)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

            string returnDialogId = String.Empty;
            waiter.Until(d =>
            {
                foreach (var dialogId in supportedIds)
                {
                    try
                    {
                        var dialog = d.FindElement(By.XPath(getXDialogById(dialogId)));
                        if (dialog == null || !dialog.Displayed) continue;
                        returnDialogId = dialogId;
                        return true;
                    }
                    catch (NoSuchElementException) { }
                }
                return false;
            });

            if (returnDialogId == String.Empty) throw new WebDriverTimeoutException("No supported dialogId found.");

            return returnDialogId;
        }

        /// <summary>
        /// Set any supported field's value for any supported field type.
        /// </summary>
        /// <param name="dialogId">The unique of the dialog.  i.e. 'RevenueBatchConstituentInbatchEditForm'</param>
        /// <param name="caption">The caption of the field.</param>
        /// <param name="value">The desired value of the field.</param>
        /// <param name="supportedFields">Mapping of the supported field captions to a CrmField object encapsulating all relevant variables
        /// needed to set the field's value.</param>
        public static void SetField(string dialogId, string caption, string value, IDictionary<string, CrmField> supportedFields)
        {
            if (!supportedFields.ContainsKey(caption)) throw new NotImplementedException(String.Format("Field '{0}' is not implemented.", caption));
            var crmField = supportedFields[caption];
            switch (crmField.CellType)
            {
                case FieldType.Checkbox:
                    SetCheckbox(getXInput(dialogId, crmField.Id), value);
                    break;
                case FieldType.Dropdown:
                    SetDropDown(getXInput(dialogId, crmField.Id), value);
                    break;
                case FieldType.Searchlist:
                    SetSearchList(getXInput(dialogId, crmField.Id),
                        getXInput(crmField.SearchDialogId, crmField.SearchDialogFieldId), value);
                    break;
                case FieldType.TextInput:
                    SetTextField(getXInput(dialogId, crmField.Id), value);
                    break;
                case FieldType.TextArea:
                    SetTextField(getXTextArea(dialogId, crmField.Id), value);
                    break;
                case FieldType.TextIframe:
                    SetTextField(getXIFrame(dialogId, crmField.Id), value);
                    break;
                default:
                    throw new NotImplementedException(String.Format("Field type '{0}' is not implemented.", crmField.CellType));
            }
        }

        /// <summary>
        /// Set all supported and displayed field values for any supported field type.
        /// </summary>
        /// <param name="dialogId">The unique id of the dialog.  i.e. 'RevenueBatchConstituentInbatchEditForm'</param>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        /// <param name="supportedFields">Mapping of the supported field captions to a CrmField object encapsulating all relevant variables
        /// needed to set the field's value.</param>
        public static void SetFields(string dialogId, TableRow fields, IDictionary<string, CrmField> supportedFields)
        {
            foreach (var caption in fields.Keys)
            {
                if (fields[caption] == null) continue;
                var value = fields[caption];
                SetField(dialogId, caption, value, supportedFields);
            }
        }

        /// <summary>
        /// Set all supported and displayed field values for any supported field type.
        /// </summary>
        /// <param name="dialogId">The unique id of the dialog.  i.e. 'RevenueBatchConstituentInbatchEditForm'</param>
        /// <param name="fields">Mapping of the field captions to their desired values.</param>
        /// <param name="supportedFields">Mapping of the supported field captions to a CrmField object encapsulating all relevant variables
        /// needed to set the field's value.</param>
        /// <param name="customSupportedFields">Mapping of field captions to to a CrmField oject encapsulating
        /// all relevant variables.  Field in the supportedFields mapping can be overridden by including them in this mapping.</param>
        public static void SetFields(string dialogId, TableRow fields, IDictionary<string, CrmField> supportedFields, 
            IDictionary<string, CrmField> customSupportedFields)
        {
            foreach (var caption in fields.Keys)
            {
                if (fields[caption] == null) continue;
                var value = fields[caption];

                SetField(dialogId, caption, value,
                    customSupportedFields.ContainsKey(caption) ? customSupportedFields : supportedFields);
            }
        }

        /// <summary>
        /// Set a single row's values for a grid.
        /// </summary>
        /// <param name="dialogId">The unique id of the dialog.  i.e. 'EventTaskAddForm'</param>
        /// <param name="gridId">The unique id of the grid - i.e. '_REMINDERS_'</param>
        /// <param name="row">Mapping of the column captions to a single row's desired values.</param>
        /// <param name="rowIndex">The index of the row to set.  
        /// The first row's index is represented as 1.</param>
        /// <param name="columnCaptionToIndex">Mapping of the column captions to their DOM TR index.</param>
        /// <param name="supportedFields">Mapping of the supported field captions to a CrmField object encapsulating all relevant variables
        /// needed to set the field's value.</param>
        public static void SetGridRow(string dialogId, string gridId, TableRow row, int rowIndex,
            IDictionary<string, int> columnCaptionToIndex, IDictionary<string, CrmField> supportedFields)
        {
            foreach (var caption in row.Keys)
            {
                //Add if value provided using appropriate field set.  Use rowCount and column caption mapping when making xPath
                if (row[caption] == null) continue;
                var value = row[caption];
                var cellxPath = getXGridCell(dialogId, gridId, rowIndex,
                    columnCaptionToIndex[caption]);

                if (!supportedFields.ContainsKey(caption)) throw new NotImplementedException(String.Format("Field '{0}' is not implemented.", caption));
                var crmField = supportedFields[caption];

                switch (crmField.CellType)
                {
                    case FieldType.Checkbox:
                        throw new NotImplementedException(String.Format("Field type '{0}' is not implemented.", crmField.CellType));
                    case FieldType.Dropdown:
                        SetGridDropDown(cellxPath, value);
                        break;
                    case FieldType.Searchlist:
                        SetGridSearchList(cellxPath, getXInput(crmField.SearchDialogId,crmField.SearchDialogFieldId), value);
                        break;
                    case FieldType.TextInput:
                        SetGridTextField(cellxPath, value);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Field type '{0}' is not implemented.", crmField.CellType));
                }
            }
        }

        /// <summary>
        /// Set a single row's values for a grid.
        /// </summary>
        /// <param name="dialogId">The unique id of the dialog.  i.e. 'EventTaskAddForm'</param>
        /// <param name="gridId">The unique id of the grid - i.e. '_REMINDERS_'</param>
        /// <param name="row">Mapping of the column captions to a single row's desired values.</param>
        /// <param name="rowIndex">The index of the row to set.  
        /// The first row's index is represented as 1.</param>
        /// <param name="columnCaptionToIndex">Mapping of the column captions to their DOM TR index.</param>
        /// <param name="supportedFields">Mapping of the supported field captions to a CrmField object encapsulating all relevant variables
        /// needed to set the field's value.</param>
        /// <param name="customSupportedFields">Mapping of field captions to a CrmField oject encapsulating
        /// all relevant variables.  Field in the supportedFields mapping can be overridden by including them in this mapping.</param>
        public static void SetGridRow(string dialogId, string gridId, TableRow row, int rowIndex,
            IDictionary<string, int> columnCaptionToIndex, IDictionary<string, CrmField> supportedFields, 
            IDictionary<string, CrmField> customSupportedFields)
        {
            foreach (var caption in row.Keys)
            {
                //Add if value provided using appropriate field set.  Use rowCount and column caption mapping when making xPath
                if (row[caption] == null) continue;
                var value = row[caption];
                var cellxPath = getXGridCell(dialogId, gridId, rowIndex,
                    columnCaptionToIndex[caption]);

                CrmField crmField;
                if (customSupportedFields.ContainsKey(caption)) crmField = customSupportedFields[caption];
                else if (supportedFields.ContainsKey(caption)) crmField = supportedFields[caption];
                else throw new NotImplementedException(String.Format("Field '{0}' is not implemented.", caption));

                switch (crmField.CellType)
                {
                    case FieldType.Checkbox:
                        throw new NotImplementedException(String.Format("Grid field type '{0}' is not implemented.", crmField.CellType));
                    case FieldType.Dropdown:
                        SetGridDropDown(cellxPath, value);
                        break;
                    case FieldType.Searchlist:
                        SetGridSearchList(cellxPath, getXInput(crmField.SearchDialogId, crmField.SearchDialogFieldId), value);
                        break;
                    case FieldType.TextInput:
                        SetGridTextField(cellxPath, value);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Grid field type '{0}' is not implemented.", crmField.CellType));
                }
            }
        }

        /// <summary>
        /// Set the row values for a grid.
        /// </summary>
        /// <param name="dialogId">The unique id of the dialog.  i.e. 'EventTaskAddForm'</param>
        /// <param name="gridId">The unique id of the grid - i.e. '_REMINDERS_'</param>
        /// <param name="rows">Table where each TableRow a row to set.
        /// Each TableRow is a mapping of the column captions to a single row's desired values.</param>
        /// <param name="startingRowIndex">The row index to start adding rows from.  
        /// The first row's index is represented as 1.</param>
        /// <param name="columnCaptionToIndex">Mapping of the column captions to their DOM TR index.</param>
        /// <param name="supportedFields">Mapping of the supported field captions to a CrmField object encapsulating all relevant variables
        /// needed to set the field's value.</param>
        public static void SetGridRows(string dialogId, string gridId, Table rows, int startingRowIndex,
            IDictionary<string, int> columnCaptionToIndex, IDictionary<string, CrmField> supportedFields)
        {
            if (startingRowIndex < 1) throw new ArgumentException(String.Format("Starting row index for SetGridFields must be greater then or equal to 1.  Given '{0}'", startingRowIndex));

            //add the rows
            foreach (var row in rows.Rows)
            {
                SetGridRow(dialogId, gridId, row, startingRowIndex, columnCaptionToIndex, supportedFields);
                startingRowIndex++;
            }
        }

        /// <summary>
        /// Set the row values for a grid.
        /// </summary>
        /// <param name="dialogId">The unique id of the dialog.  i.e. 'EventTaskAddForm'</param>
        /// <param name="gridId">The unique id of the grid - i.e. '_REMINDERS_'</param>
        /// <param name="rows">Table where each TableRow a row to set.
        /// Each TableRow is a mapping of the column captions to a single row's desired values.</param>
        /// <param name="startingRowIndex">The row index to start adding rows from.  
        /// The first row's index is represented as 1.</param>
        /// <param name="columnCaptionToIndex">Mapping of the column captions to their DOM TR index.</param>
        /// <param name="supportedFields">Mapping of the supported field captions to a CrmField object encapsulating all relevant variables
        /// needed to set the field's value.</param>
        /// <param name="customSupportedFields">Mapping of field captions to to a CrmField oject encapsulating
        /// all relevant variables.  Field in the supportedFields mapping can be overridden by including them in this mapping.</param>
        public static void SetGridRows(string dialogId, string gridId, Table rows, int startingRowIndex,
            IDictionary<string, int> columnCaptionToIndex, IDictionary<string, CrmField> supportedFields,
            IDictionary<string, CrmField> customSupportedFields)
        {
            if (startingRowIndex < 1) throw new ArgumentException(String.Format("Starting row index for SetGridFields must be greater then or equal to 1.  Given '{0}'", startingRowIndex));

            //add the rows
            foreach (var row in rows.Rows)
            {
                SetGridRow(dialogId, gridId, row, startingRowIndex, columnCaptionToIndex, supportedFields, customSupportedFields);
                startingRowIndex++;
            }
        }
    }
}
