using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Blackbaud.UAT.Core.Bbis.Pages
{
    /// <summary>
    /// BBISHomePage
    /// </summary>
    public class BBISHomePage : BaseComponent
    {
        /// <summary>
        /// Return the config value for BBIS BaseUrl (BBISBaseUrl)
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BBISBaseUrl"];
            }
        }

        /// <summary>
        /// Return URL config postfix for BBIS (BBISHomeUrl)
        /// </summary>
        public static string VirtualPath
        {
            get
            {
                return ConfigurationManager.AppSettings["BBISHomeUrl"];
            }
        }

        /// <summary>
        /// Return the default page title
        /// </summary>
        public override string Title { get { return "Login"; } }

        /// <summary>
        /// Xpath for User Name field
        /// </summary>
        public const string getXUserNameField = "//*[@id=\"txtUsername\"]";

        /// <summary>
        /// XPath for Password field
        /// </summary>
        public const string getXPasswordField = "//*[@id=\"txtPassword\"]";

        /// <summary>
        /// XPath for the Loging button
        /// </summary>
        public const string getXLoginButton = "//*[@id=\"btnLogin\"]";

        /// <summary>
        /// Open the BBIS Layout Gallery
        /// </summary>
        public static void OpenLayoutGallery()
        {
            string url = BaseUrl;
            Driver.Navigate().GoToUrl(url.TrimEnd(new char[] { '/' }) + "/cms/layoutgallery");
        }

        /// <summary>
        /// Open the BBIS Image Library
        /// </summary>
        public static void OpenImageLibrary()
        {
            string url = BaseUrl;
            Driver.Navigate().GoToUrl(url.TrimEnd(new char[] { '/' }) + "/cms/imagegallery");
        }

        /// <summary>
        /// XPath for the New Layout Button
        /// </summary>
        public const string getXNewLayoutButton = "//*[contains(@class,'BBAdminButtonLabel') and .//text()='New layout']";

        /// <summary>
        /// XPath for the New Layout Layout Name
        /// </summary>
        public const string getXNewLayoutLayoutName = "//*[@id=\"pagecntnt_tmpltcntnt_TemplateName\"]";

        /// <summary>
        /// XPath for the New Layout Layout Description
        /// </summary>
        public const string getXNewLayoutLayoutDescription = "//*[@id=\"pagecntnt_tmpltcntnt_Description\"]";

        /// <summary>
        /// XPath for the New Layout Next Button
        /// </summary>
        public const string getXNewLayoutNextButton = "//*[@id=\"pagecntnt_DialogButtons6_btn0txt\"]";

        /// <summary>
        /// XPath for the New Layout Save Button
        /// </summary>
        public const string getXNewLayoutSaveButton = "//*[@id=\"pagecntnt_DialogButtons6_btn0txt\"]";

        /// <summary>
        /// XPath for the search Filter by Name Field
        /// </summary>
        public const string getXFilterByNameField = "//*[@id=\"pagecntnt_tmpltcntnt_searchTitle\"]";        

        /// <summary>
        /// XPath for the search Filter by Button
        /// </summary>
        public const string getXFilterByNameButton = "//*[@id=\"pagecntnt_tmpltcntnt_btnGo6_Toolbar1_btn0txt\"]";

        /// <summary>
        /// Filters Layout by the Name provided
        /// </summary>
        /// <param name="layoutName">The name of the layout to be filtered on</param>
        public static void FilterLayoutByName(string layoutName)
        {
            //Assumes we're on the Layout Gallery
            IWebElement filterByNameFieldElement = Driver.FindElement(By.XPath(getXFilterByNameField));

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            wait.Until(d =>
            {
                if (filterByNameFieldElement == null) return false;
                filterByNameFieldElement.Clear();
                return true;
            });
            filterByNameFieldElement.SendKeys(layoutName);
            filterByNameFieldElement.SendKeys(Keys.Return);
        }

        /// <summary>
        /// XPath for the Layout Grid Name Field
        /// </summary>
        public const string getXLayoutGridNameField = "//*[@id='pagecntnt_tmpltcntnt_Grid1_Grid1_DataGrid1']/tbody/tr/td[2]/span/span/span";

        /// <summary>
        /// XPath for the Layout Search Results Line
        /// </summary>
        public const string getXLayoutSearchResultsLine = "//*[@id='pagecntnt_tmpltcntnt_Grid1_Grid1_lblTotalRecs']";


        /// <summary>
        /// Check layout search results contain for passed string
        /// </summary>
        /// <param name="expected">The string excepted in the results</param>
        public static void CheckLayoutSearchResultsContain(string expected)
        {
            IList<IWebElement> layoutGridNameFieldElements = Driver.FindElements(By.XPath(getXLayoutGridNameField));
            IWebElement layoutSearchResultsLineElement = Driver.FindElement(By.XPath(getXLayoutSearchResultsLine));

            new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs))
                .Until(d => (layoutGridNameFieldElements != null &&
                             layoutSearchResultsLineElement.Displayed != false &&
                             !layoutSearchResultsLineElement.Text.Contains("No")));

            var names = from element in layoutGridNameFieldElements select element.Text;

            if (!names.Contains(expected))
            {
                throw new Exception(String.Format("CheckLayoutSearchResultsContain Failed: Expected Layout Name '{0}' was not found in the Results.", expected));
            }

        }

        // BBIS already has made some stab at meaningful id's in the HTML (<add key="AutomationHooksEnabled" value="true" /> or not)
        // However they're not useful in non-trivial situations, e.g. finding results in a grid. 
        // The grid elements id's are *unique* they're just not very helpful in idenitifying contents.
        /// <summary>
        /// Delete a layout
        /// </summary>
        /// <param name="layoutname">The layout name to remove</param>
        public static void DeleteLayout(string layoutname)
        {
            FilterLayoutByName(layoutname);

            // Select all 'Delete' grid elements that have a sibling with 'EVENTCALENDAR' text.        
            IList<IWebElement> layoutDeleteElements = Driver.FindElements(By.XPath("//*[@id=\"pagecntnt_tmpltcntnt_Grid1_Grid1_DataGrid1\"]/tbody/tr[td[2]/span/span/span/text()='" + layoutname + "']/td[1]/img[5]"));            
            IList<IWebElement> layoutGridNameFieldElements = Driver.FindElements(By.XPath(getXLayoutGridNameField));
            IWebElement layoutSearchResultsLineElement = Driver.FindElement(By.XPath(getXLayoutSearchResultsLine));


            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

            wait.Until(d => (layoutGridNameFieldElements != null &&
                 layoutSearchResultsLineElement.Displayed != false &&
                 !layoutSearchResultsLineElement.Text.Contains("No")));

            foreach (IWebElement element in layoutDeleteElements)
            {
                wait.Until(d =>
                {
                    if (element == null) return false;
                    element.Click();
                    return true;
                });

                IAlert alert = Driver.SwitchTo().Alert();
                alert.Accept();
            }
        }

        /// <summary>
        /// Create a new layout
        /// </summary>
        /// <param name="Layoutname">The name fo the new layout</param>
        /// <param name="Description">Description of the new layout</param>
        public static void NewLayout(string Layoutname, String Description)
        {

            WaitClick(getXNewLayoutButton);

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException));

            // This assumes the newly opened window will be the last handle on the list and that the parent is first on the list.
            // Its worked fine so far so its KISS for me but if it starts mis-identifying windows we could select by Driver.Title
            Driver.SwitchTo().Window(Driver.WindowHandles[Driver.WindowHandles.Count - 1]);

            IWebElement NewLayoutLayoutNameElement = Driver.FindElement(By.XPath(getXNewLayoutLayoutName));
            IWebElement NewLayoutLayoutDescriptionElement = Driver.FindElement(By.XPath(getXNewLayoutLayoutDescription));

            wait.Until(d =>
            {
                if (NewLayoutLayoutNameElement == null || NewLayoutLayoutDescriptionElement == null) return false;
                NewLayoutLayoutNameElement.Clear();
                NewLayoutLayoutDescriptionElement.Clear();
                return true;
            });

            NewLayoutLayoutNameElement.SendKeys(Layoutname);
            NewLayoutLayoutDescriptionElement.SendKeys(Description);

            WaitClick(getXNewLayoutNextButton);
            WaitClick(getXNewLayoutSaveButton);

            Driver.SwitchTo().Window(Driver.WindowHandles[0]);

            // This section is here to make sure we don't move on before the driver has refereshed itself
            // after the window switch - wait until we stop getting 'Stale Handles' for the search field 
            // back on the main Layout page.

            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));
            wait.Until(d =>
            {
                IWebElement FilterByNameFieldElement = d.FindElement(By.XPath(getXFilterByNameField));

                if (FilterByNameFieldElement == null) return false;
                FilterByNameFieldElement.Clear();
                return true;
            });

        }

        /// <summary>
        /// Logon with the credentials specified in the config file.
        /// </summary>
        public static void Login()
        {
            LoginAs();
        }

        /// <summary>
        /// What it says - takes Credentials of the form "username:password".
        /// </summary>
        /// <param name="credentials"></param>
        public static void LoginAs(string credentials = null)
        {
            string url = BaseUrl;
            string[] creds = null;

            if (null == credentials)
            {
                try
                {
                    creds = ConfigurationManager.AppSettings["BBISCredentials"].Split(':');
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                creds = credentials.Split(':');
            }

            Driver.Navigate().GoToUrl(url.TrimEnd(new char[] { '/' }) + VirtualPath);

            if (creds != null)
            {
                SetCredentailsField("//*[@id=\"pnl_login\"]//*[@id=\"txtUsername\"]", creds[0].Trim());
                SetCredentailsField("//*[@id=\"pnl_login\"]//*[@id=\"txtPassword\"]",creds[1].Trim());
                WaitClick("//*[@id=\"pnl_login\"]//*[@id=\"btnLogin\"]");                
            }
        }

        /// <summary>
        /// Set the value of a field by copy pasting the value into the field and sending a Tab keystroke.
        /// </summary>
        /// <param name="xPath">The xPath to find an element for setting the value to.</param>
        /// <param name="value">The desired value of the element.</param>
        public static new void SetTextField(string xPath, string value)
        {
            if (value == null) return;

            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            waiter.IgnoreExceptionTypes(typeof(InvalidOperationException));
            //removed element from being a public field and moved to scope of method
            waiter.Until(d =>
            {
                CopyToClipBoard(value);

                var fieldElement = d.FindElement(By.XPath(xPath));
                if (fieldElement == null
                    || !fieldElement.Displayed || !fieldElement.Enabled) return false;

                fieldElement.Click();

                fieldElement.SendKeys(Keys.Control + "a");

                fieldElement.SendKeys(Keys.Control + "v");

                var innerwaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                innerwaiter.IgnoreExceptionTypes(typeof(InvalidOperationException));

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
    }
}
