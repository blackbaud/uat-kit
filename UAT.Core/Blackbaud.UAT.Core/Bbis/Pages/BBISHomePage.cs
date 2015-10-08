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

        public static string BaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BBISBaseUrl"];
            }
        }

        public static string VirtualPath
        {
            get
            {
                return ConfigurationManager.AppSettings["BBISHomeUrl"];
            }
        }
        public override string Title { get { return "Login"; } }

        public const string getXUserNameField = "//*[@id=\"txtUsername\"]";

        public const string getXPasswordField = "//*[@id=\"txtPassword\"]";

        public const string getXLoginButton = "//*[@id=\"btnLogin\"]";

        public static void OpenLayoutGallery()
        {
            string url = BaseUrl;
            Driver.Navigate().GoToUrl(url.TrimEnd(new char[] { '/' }) + "/cms/layoutgallery");
        }

        public static void OpenImageLibrary()
        {
            string url = BaseUrl;
            Driver.Navigate().GoToUrl(url.TrimEnd(new char[] { '/' }) + "/cms/imagegallery");
        }

        public const string getXNewLayoutButton = "//*[contains(@class,'BBAdminButtonLabel') and .//text()='New layout']";

        // Popup elements
        public const string getXNewLayoutLayoutName = "//*[@id=\"pagecntnt_tmpltcntnt_TemplateName\"]";

        public const string getXNewLayoutLayoutDescription = "//*[@id=\"pagecntnt_tmpltcntnt_Description\"]";

        public const string getXNewLayoutNextButton = "//*[@id=\"pagecntnt_DialogButtons6_btn0txt\"]";

        public const string getXNewLayoutSaveButton = "//*[@id=\"pagecntnt_DialogButtons6_btn0txt\"]";

        public const string getXFilterByNameField = "//*[@id=\"pagecntnt_tmpltcntnt_searchTitle\"]";        

        public const string getXFilterByNameButton = "//*[@id=\"pagecntnt_tmpltcntnt_btnGo6_Toolbar1_btn0txt\"]";

        public static void FilterLayoutByName(string layoutName)
        {
            //Assumes we're on the Layout Gallery

            //SetTextField(getXFilterByNameField, layoutName);

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

        public const string getXLayoutGridNameField = "//*[@id='pagecntnt_tmpltcntnt_Grid1_Grid1_DataGrid1']/tbody/tr/td[2]/span/span/span";
        public const string getXLayoutSearchResultsLine = "//*[@id='pagecntnt_tmpltcntnt_Grid1_Grid1_lblTotalRecs']";

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


        public static void NewLayout(string Layoutname, String Description)
        {

            WaitClick(getXNewLayoutButton);

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException));

            //wait.Until(d =>
            //{
            //    if (NewLayoutButtonElement == null) return false;
            //    NewLayoutButtonElement.Click();
            //    return true;
            //});

            // Do the popup thing like it's 1995 ....

            // This assumes the newly opened window will be the last handle on the list and that the parent is first on the list.
            // Its worked fine so far so its KISS for me but if it starts mis-identifying windows we could select by Driver.Title
            Driver.SwitchTo().Window(Driver.WindowHandles[Driver.WindowHandles.Count - 1]);

            // SetTextfield seems to have problems in BBIS :)

            //SetTextField(getXNewLayoutLayoutName, Layoutname);
            //SetTextField(getXNewLayoutLayoutDescription, Description);

            IWebElement NewLayoutLayoutNameElement = Driver.FindElement(By.XPath(getXNewLayoutLayoutName));
            IWebElement NewLayoutLayoutDescriptionElement = Driver.FindElement(By.XPath(getXNewLayoutLayoutDescription));

            wait.Until(d =>
            {
                if (NewLayoutLayoutNameElement == null || NewLayoutLayoutDescriptionElement == null) return false;
                NewLayoutLayoutNameElement.Clear();
                NewLayoutLayoutDescriptionElement.Clear();
                return true;
            });

            //try
            //{
            NewLayoutLayoutNameElement.SendKeys(Layoutname);
            NewLayoutLayoutDescriptionElement.SendKeys(Description);
            //} catch (RuntimeBinderException) { }

            WaitClick(getXNewLayoutNextButton);
            //wait.Until(d =>
            //{
            //    if (NewLayoutNextButton == null) return false;
            //    NewLayoutNextButton.Click();
            //    return true;
            //});
            WaitClick(getXNewLayoutSaveButton);
            //wait.Until(d =>
            //{
            //    if (NewLayoutSaveButton == null) return false;
            //    NewLayoutSaveButton.Click();
            //    return true;
            //});

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
        ///// What it says - takes Credentials of the form "username:password".
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
    }
}
