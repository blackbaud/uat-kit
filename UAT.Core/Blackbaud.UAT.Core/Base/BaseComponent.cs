using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Keys = OpenQA.Selenium.Keys;
using OpenQA.Selenium.Interactions;

namespace Blackbaud.UAT.Base
{
    /// <summary>
    /// Base functionality class common to all controls.
    /// </summary>
    public class BaseComponent
    {
        /// <summary>
        /// The default WebDriver timeout value represented in seconds.
        /// </summary>
        public static int TimeoutSecs { get { return 120; } }

        /// <summary>
        /// Return the default page title
        /// </summary>
        public virtual string Title { get { return "Blackbaud"; } }

        /// <summary>
        /// Selenium WebDriver used to interact with the web browser.
        /// </summary>
        public static IWebDriver Driver
        { get { return (IWebDriver)ScenarioContext.Current["Driver"]; } }

        /// <summary>
        /// Format an XPath to find a button.  Adds no additional constraints other than
        /// the caption of the button equalling the provided value.
        /// </summary>
        /// <param name="caption">The caption of the button.</param>
        protected static string getXButton(string caption)
        {
            return String.Format("//*[./text()=\"{0}\" and contains(@class,\"x-btn-text\")]", caption);
        }

        /// <summary>
        /// Given the caption name of a menu item, return a unique identifier xPath
        /// to find a menu item.
        /// </summary>
        /// <param name="caption">The caption of the functional area.</param>
        protected static string getXMenuItem(string caption)
        {
            return String.Format("//div[contains(@class,'x-menu') and contains(@style,'visibility: visible')]//span[./text()='{0}' and @class='x-menu-item-text']", caption);
        }

        /// <summary>
        /// Convert the string to a bool value.
        /// </summary>
        /// <param name="value">The string to try and convert to a bool.</param>
        /// <returns>A bool if successfully converted.  Unrecognized values throw a NotSupportedException</returns>
        protected static bool ConvertToBool(string value)
        {
            value = value.ToLower();
            switch (value)
            {
                case "selected":
                case "on":
                case "1":
                case "true":
                case "checked":
                    return true;
                case "unselected":
                case "off":
                case "0":
                case "false":
                case "unchecked":
                    return false;
                default:
                    throw new NotSupportedException(String.Format("'{0}' is not a supported checkbox or radio button value.", value));
            }
        }

        /// <summary>
        /// Click a checkbox if it is not the desired value.
        /// </summary>
        /// <param name="xPath">The xPath to use for finding the checkbox INPUT.</param>
        /// <param name="value">The value to set the checkbox to.</param>
        public static void SetCheckbox(string xPath, string value)
        {
            if (ConvertToBool(value) != GetEnabledElement(xPath).Selected) WaitClick(xPath);
        }

        /// <summary>
        /// Click a checkbox if it is not the desired value.
        /// </summary>
        /// <param name="xPath">The xPath to use for finding the checkbox INPUT.</param>
        /// <param name="value">True if the checkbox should be checked, false otherwise.</param>
        public static void SetCheckbox(string xPath, bool value)
        {
            if (value != GetEnabledElement(xPath).Selected) WaitClick(xPath);
        }

        /// <summary>
        /// Wait until a displayed element found with the provided xPath does not have a null or empty value
        /// If no matching element is found, a WebDriverTimeoutException is thrown.
        /// </summary>
        /// <param name="xPath">The xPath to use for finding an element.</param>
        /// <param name="secondsToWait">Timeout allowed in seconds.</param>
        public static void ElementValueIsNotNullOrEmpty(string xPath, double secondsToWait = -1)
        {
            if ((int)secondsToWait == -1) secondsToWait = TimeoutSecs;
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(secondsToWait));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            waiter.Until(d =>
            {
                var element = d.FindElement(By.XPath(xPath));
                return element != null && element.Displayed && (!String.IsNullOrEmpty(element.Text) || !String.IsNullOrEmpty(element.GetAttribute("value")))
                       && !element.GetAttribute("class").Contains("required");
            });
        }

        /// <summary>
        /// Wait until a displayed element found with the provided xPath has the expected value as its text.
        /// If no matching element is found with the expected value, a WebDriverTimeoutException is thrown.
        /// </summary>
        /// <param name="xPath">The xPath to use for finding an element.</param>
        /// <param name="expectedvalue">The expected text value of the element.</param>
        /// <param name="secondsToWait">Timeout allowed in seconds.</param>
        public static void ElementValueIsSet(string xPath, string expectedvalue, double secondsToWait = -1)
        {
            if ((int)secondsToWait == -1) secondsToWait = TimeoutSecs;

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(secondsToWait));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            waiter.Until(d =>
            {
                var element = d.FindElement(By.XPath(xPath));

                if (element != null)
                {
                    if (!element.GetAttribute("class").Contains("required"))
                    {
                        if ((element.Text != null && element.Text.Contains(expectedvalue))
                           || (element.GetAttribute("value") != null && element.GetAttribute("value").Contains(expectedvalue)))
                        {
                            return true;
                        }
                    }
                }
                return false;
            });
        }

        //FIX ME - do we really need this
        /// <summary>
        /// Check if a displayed element exists immediately using the provided xPath selector.
        /// </summary>
        /// <param name="xPath">The xPath selector to find an element with.</param>
        /// <returns>True if a valid element is found immediately, false otherwise.</returns>
        public static bool ExistsNow(string xPath)
        {
            return Exists(xPath, 0.01);
        }

        /// <summary>
        /// Checks if a displayed element exists using the provided xPath selector.
        /// </summary>
        /// <param name="xPath">The xPath selector to find an element with.</param>
        /// <param name="secondsToWait">The time to spend looking for the element.  Defaults to TimeoutSecs.</param>
        /// <returns>True if a valid element is found within the time to wait, false otherwise.</returns>
        public static bool Exists(string xPath, double secondsToWait = -1)
        {
            if ((int)secondsToWait == -1) secondsToWait = TimeoutSecs;
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(secondsToWait));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            try
            {
                return waiter.Until(d =>
                {
                    var element = GetDisplayedElement(xPath, secondsToWait);
                    return element != null && element.Displayed;
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the displayed IWebElement using the provided xPath.
        /// </summary>
        /// <param name="xPath">The xPath selector to find an element with.</param>
        /// <param name="waitTime">The amount of time to search for the a valid element.  Default time is TimeoutSecs.</param>
        /// <returns>An IWebElement if a valid element is found. A WebDriverTimeoutException is thrown is no element is found.</returns>
        public static IWebElement GetDisplayedElement(string xPath, double waitTime = -1)
        {
            if ((int)waitTime == -1) waitTime = TimeoutSecs;
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitTime));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            IWebElement currentElement = null;
            try
            {
                waiter.Until(d =>
                {
                    var element = d.FindElement(By.XPath(xPath));
                    if (element != null && element.Displayed)
                    {
                        currentElement = element;
                        return true;
                    }
                    return false;
                });
                return currentElement;
            }
            catch (WebDriverTimeoutException)
            {
                throw new WebDriverTimeoutException(String.Format("Timed out trying to find displayed element using xPath '{0}'.  " +
                                                                  "Waited {1} seconds.", xPath, waitTime));
            }
        }

        /// <summary>
        /// Create a mapping of the column captions as keys to their DOM index values.
        /// </summary>
        /// <param name="captions">The list of captions to map.</param>
        /// <param name="headersxPath">An xPath to get a parent element of the desired TR header columns.</param>
        /// <returns>Mapping of column captions to an index.</returns>
        public static IDictionary<string, int> MapColumnCaptionsToIndex(ICollection<string> captions, string headersxPath)
        {
            IDictionary<string, int> columnCaptionToIndex = new Dictionary<string, int>();
            foreach (var caption in captions)
            {
                columnCaptionToIndex.Add(caption,
                    GetDatalistColumnIndex(headersxPath, caption));
            }
            return columnCaptionToIndex;
        }

        /// <summary>
        /// Given the xPath for a datalist/grid's headers and the caption of a datalist column, find the index of the column
        /// as it relates to the DOM.
        /// </summary>
        /// <param name="headersxPath">Unique xPath for getting the TR column header row.</param>
        /// <param name="columnCaption">The caption of the section's datalist column whose index to find.</param>
        public static int GetDatalistColumnIndex(string headersxPath, string columnCaption)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            var columnIndex = -1;

            waiter.Until(d =>
            {
                //get the displayed row of column headers
                var headersRow = GetDisplayedElement(headersxPath, TimeoutSecs);
                if (headersRow.Displayed != true) return false;
                IList<IWebElement> columns = headersRow.FindElements(By.XPath(".//td/div")).ToList();

                //find the matching column
                foreach (var column in columns.Where(column => column != null && column.Text == columnCaption))
                {
                    //columns not displayed and requiring a scroll over to be visible will have an empty text value
                    //despite having a text value in the DOM.  Makes this method not usable for that situation.
                    columnIndex = columns.IndexOf(column) + 1;
                    return true;
                }
                return false;
            });

            if (columnIndex == -1) throw new NoSuchElementException(String.Format("No column found with the caption '{0}'.  " +
                              "Try using overload implementation of method that invokes GetDatalistColumnIndex.", columnCaption));
            return columnIndex;
        }

        /// <summary>
        /// Get the first found IWebElement based on an xPath that is Displayed AND Enabled.
        /// If no element is found, a WebDriverTimeoutException is thrown.
        /// </summary>
        /// <param name="xPath">The xPath selector to find an element with.</param>
        /// <param name="waitTime">The amount of time to search for the a valid element.  Default time is TimeoutSecs.</param>
        public static IWebElement GetEnabledElement(string xPath, double waitTime = -1)
        {
            if ((int)waitTime == -1) waitTime = TimeoutSecs;
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitTime));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            IWebElement currentElement = null;
            try
            {
                waiter.Until(d =>
                {
                    var element = d.FindElement(By.XPath(xPath));
                    if (element != null && element.Displayed && element.Enabled)
                    {
                        //ensure element is not stale by getting an attribute value from it.
                        element.GetAttribute("id");
                        currentElement = element;
                        return true;
                    }
                    return false;
                });
                return currentElement;
            }
            catch (WebDriverTimeoutException)
            {
                throw new WebDriverTimeoutException(String.Format("Timed out trying to get enabled and displayed element using xPath '{0}'.  " +
                                                                  "Waited {1} seconds.", xPath, waitTime));
            }
        }

        /// <summary>
        /// Clicks a basic button.
        /// Uniqueness is dependent purely on the caption!
        /// </summary>
        /// <param name="caption"></param>
        public static void ClickButton(string caption)
        {
            WaitClick(getXButton(caption));
        }

        /// <summary>
        /// Find an element using a provided xPath, wait for it to be displayed and enabled then Click.
        /// </summary>
        /// <param name="xPath">The xPath to find the element by.</param>
        /// <param name="timeout">The amount of time to try and find a displayed and enabled element to click
        /// before throwing a WebDriverTimeoutException.</param>
        public static void WaitClick(string xPath, double timeout)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            waiter.Until(d =>
            {
                var element = Driver.FindElement(By.XPath(xPath));
                //adding check for enabled.  Not checking for this was causing race conditions for clicking grid cells.
                if (element == null
                    || element.Displayed == false || !element.Enabled) return false;
                element.Click();
                return true;
            });
        }

        /// <summary>
        /// Find an element using a provided xPath, wait for it to be displayed and enabled then DoubleClick.
        /// </summary>
        /// <param name="xPath">The xPath to find the element by.</param>
        /// <param name="timeout">The amount of time to try and find a displayed and enabled element to DoubleClick
        /// before throwing a WebDriverTimeoutException.</param>
        public static void WaitDoubleClick(string xPath, double timeout)
        {
            Actions act = new Actions(Driver);
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            waiter.Until(d =>
            {
                var element = Driver.FindElement(By.XPath(xPath));
                //adding check for enabled.  Not checking for this was causing race conditions for clicking grid cells.
                if (element == null
                    || element.Displayed == false || !element.Enabled)
                    return false;
                act.DoubleClick(element).Build().Perform();
                return true;
            });
        }

        /// <summary>
        /// Find an element using a provided xPath, wait for it to be displayed and enabled then SendKeys directly to it.
        /// This is intended for sending directly to input elements e.g. to bypass a native file chooser.
        /// </summary>
        /// <param name="xPath">The xPath to find the element by.</param>
        /// <param name="keysToSend">The String of Keys to send to the input.</param>
        /// <param name="timeout">The amount of time to try and find a displayed and enabled element to DoubleClick
        /// before throwing a WebDriverTimeoutException.</param>
        public static void WaitSendKeys(string xPath, string keysToSend, double timeout)
        {
            Actions act = new Actions(Driver);
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            waiter.Until(d =>
            {
                var element = Driver.FindElement(By.XPath(xPath));
                //adding check for enabled.  Not checking for this was causing race conditions for clicking grid cells.
                if (element == null
                    || element.Displayed == false || !element.Enabled)
                    return false;
                act.SendKeys(element, keysToSend).Build().Perform();
                return true;
            });
        }

        /// <summary>
        /// Find an element using a provided xPath, wait for it to be displayed and enabled then SendKeys directly to it.
        /// This is intended for sending directly to input elements e.g. to bypass a native file chooser.
        /// </summary>
        /// <param name="xPath">The xPath to find the element by.</param>
        /// <param name="keysToSend">The String of Keys to send to the input.</param>
        public static void WaitSendKeys(string xPath, String keysToSend)
        {
            WaitSendKeys(xPath, keysToSend, TimeoutSecs);
        }

        /// <summary>
        /// Find an element using a provided xPath, wait for it to be displayed and enabled then DoubleClick.        /// </summary>
        /// <param name="xPath">The xPath to find the element by.</param>
        public static void WaitDoubleClick(string xPath)
        {
            WaitDoubleClick(xPath, TimeoutSecs);
        }

        /// <summary>
        /// Find an element using a provided xPath and click it once it is displayed and enabled.
        /// </summary>
        /// <param name="xPath">The xPath to find the element by.</param>
        public static void WaitClick(string xPath)
        {
            WaitClick(xPath, TimeoutSecs);
        }

        /// <summary>
        /// Start a new thread that copies a value to the System's Clipboard and waits for that
        /// thread to return.
        /// </summary>
        /// <param name="value">The value to copy to the clipboard.</param>
        protected static void CopyToClipBoard(string value)
        {
            // ArgumentNullException thrown when text is "null or Empty"
            var thread = string.IsNullOrEmpty(value) ? new Thread(Clipboard.Clear) : new Thread(() => Clipboard.SetText(value));

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        /// <summary>
        /// Set the value of a field by copy pasting the value into the field and sending a Tab keystroke.
        /// </summary>
        /// <param name="xPath">The xPath to find an element for setting the value to.</param>
        /// <param name="value">The desired value of the element.</param>
        public static void SetTextField(string xPath, string value)
        {
            if (value == null) return;

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(ElementClickInterceptedException));
            //removed element from being a public field and moved to scope of method
            waiter.Until(d =>
            {
                CopyToClipBoard(value);

                var fieldElement = d.FindElement(By.XPath(xPath));
                if (fieldElement == null
                    || !fieldElement.Displayed || !fieldElement.Enabled) return false;

                fieldElement.Click();

                var innerwaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                innerwaiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(ElementClickInterceptedException));
                try
                {
                    innerwaiter.Until(dd =>
                    {
                        if (!fieldElement.GetAttribute("class").Contains("focus")) return false;
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                fieldElement.SendKeys(Keys.Control + "a");

                fieldElement.SendKeys(Keys.Control + "v");

                try
                {
                    innerwaiter.Until(dd =>
                    {
                        //Sending a [Tab] triggers a submit on the field.  This is needed for a dropdown and speeds up a required text field being set.                
                        fieldElement.SendKeys(Keys.Tab);

                        if (fieldElement.GetAttribute("value") != value) return false;
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }

                return true;
            });
        }

        /// <summary>
        /// Specialist version of setTextField to deal with the lack of focus in a login splash screen.
        /// </summary>
        /// <param name="xPath">The xPath to find an element for setting the value to.</param>
        /// <param name="value">The desired value of the element.</param>
        public static void SetCredentailsField(string xPath, string value)
        {
            if (value == null) return;

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(ElementClickInterceptedException));

            waiter.Until(d =>
            {
                CopyToClipBoard(value);

                var fieldElement = d.FindElement(By.XPath(xPath));
                if (fieldElement == null
                    || !fieldElement.Displayed || !fieldElement.Enabled) return false;

                fieldElement.Click();

                var innerwaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                innerwaiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(ElementClickInterceptedException));

                fieldElement.SendKeys(Keys.Control + "a");

                fieldElement.SendKeys(Keys.Control + "v");

                fieldElement.SendKeys(Keys.Tab);

                try
                {
                    innerwaiter.Until(dd =>
                    {
                        if (fieldElement.GetAttribute("value") != value //) return false;
                            || fieldElement.GetAttribute("class").Contains("required")) return false;
                        return true;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }

                return true;
            });
        }
    }

    /// <summary>
    /// Represents the various type of fields with supported setter utility methods.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// An input text field.  This should also be used for grid cells that are text fields.
        /// </summary>
        TextInput,
        /// <summary>
        /// A textarea text field.
        /// </summary>
        TextArea,
        /// <summary>
        /// A dropdown field.
        /// </summary>
        Dropdown,
        /// <summary>
        /// A searchlist field.
        /// </summary>
        Searchlist,
        /// <summary>
        /// A checkbox or radiobox field.
        /// </summary>
        Checkbox,
        /// <summary>
        /// An IFrame text field.
        /// </summary>
        TextIframe
    };

    /// <summary>
    /// Wrapper class to encapsulate a field caption with a field type, unique element id, and search dialog ids when required.
    /// </summary>
    public class CrmField
    {
        /// <summary>
        /// The unique Id contained in the id attribute.
        /// i.e. '_CONSTITUENTID_value'
        /// </summary>
        public readonly string Id;

        /// <summary>
        /// The type of field.  Used to associate the field with a setter utility method.
        /// </summary>
        public readonly FieldType CellType;

        /// <summary>
        /// The unique id of the search dialog that would appear upon clicking the search icon of
        /// the associated grid cell.
        /// i.e. 'DesignationSearch'
        /// Used for fields associated with 'BatchGridCellType.Searchlist'.
        /// </summary>
        public readonly string SearchDialogId;

        /// <summary>
        /// The unique id of the search dialog field that would appear upon clicking the search icon of
        /// the associated grid cell.
        /// i.e. 'COMBINEDSEARCH'
        /// Used for fields associated with 'BatchGridCellType.Searchlist'.
        /// </summary>
        public readonly string SearchDialogFieldId;

        /// <summary>
        /// Set the field id and associated setter utility method.
        /// </summary>
        /// <param name="id">The unique Id contained in the id attribute.  i.e. '_CONSTITUENTID_value'</param>
        /// <param name="cellType">The type of grid cell.  Used to associate the cell with a setter utility method.</param>
        public CrmField(string id, FieldType cellType)
        {
            Id = id;
            if (cellType == FieldType.Searchlist) throw new ArgumentException("Please use Field constructor that takes in a searchDialogId and searchDialogFieldId for fields using the SetGridSearchlist utility method.");
            CellType = cellType;
        }

        /// <summary>
        /// Set the batch column id and associated setter utility method.  To only be used
        /// with grid cells where the value should be set using a searchlist.
        /// </summary>
        /// <param name="id">The unique Id contained in the class attribute.  i.e. '_CONSTITUENTID_value'</param>
        /// <param name="cellType">The type of grid cell.  Used to associate the cell with a setter utility method.</param>
        /// <param name="searchDialogId">The unique id of the search dialog that would appear upon clicking the search icon of
        /// the associated grid cell.  i.e. 'DesignationSearch'</param>
        /// <param name="searchDialogFieldId">The unique id of the search dialog field that would appear upon clicking the search icon of
        /// the associated grid cell.  i.e. 'COMBINEDSEARCH'</param>
        public CrmField(string id, FieldType cellType, string searchDialogId, string searchDialogFieldId)
        {
            Id = id;
            if (cellType != FieldType.Searchlist) throw new ArgumentException("If you do not wish to use the searchlist setter utility, use BatchColumn constructor that does not take in a searchDialogId and searchDialogFieldId parameters.");
            CellType = cellType;
            SearchDialogId = searchDialogId;
            SearchDialogFieldId = searchDialogFieldId;
        }
    }
}
