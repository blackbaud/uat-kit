using System;
using System.Configuration;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// A class for handling interactions and actions on the main BBCRM page.
    /// 
    /// </summary>
    public class BBCRMHomePage : BaseComponent
    {

        /// <summary>
        /// Format an xPath to find a task in the left explorer bar.
        /// </summary>
        /// <param name="caption">The caption of the task.</param>
        public static string getXTask(string caption)
        {
            return
                String.Format(
                    "//div[contains(@id, 'actionpanel')]//ul[contains(@class,'actionpanel-items')]//div[text()='{0}']",
                    caption);
        }

        /// <summary>
        /// Returns an Xpath for the more button icon on the toolbar.
        /// </summary>
        public const string getXMenuMore = "//button[@class=' x-btn-text x-toolbar-more-icon']";

        /// <summary>
        /// Returns an Xpath that will match a Tab either on the main tool bar or the expanded one.
        /// 
        /// Any Functional Area link can be in two places; the main 'Tabs' menu and in the expanded menu. 
        /// This Xpath matches both elements.
        /// It relies on only one side matching at a time, which is true for this method but might not be for all possible permutaions
        /// of GUI manipulation, particularly changing window size. So the uniqueness of this Xpath depends on the sequence of GUI actions
        /// that lead up to its use.
        /// </summary>
        /// <param name="caption">The caption of the tab.</param>
        //public static string getXExtTab(string caption){ return String.Format("//*[contains(@class,\"x-toolbar-cell\") and not(contains(@class,\"x-hide-display\"))]//button[./text()=\"{0}\"] | //span[./text()=\"{0}\" and contains(@class,\"x-menu-item-text\")]",caption);}

        public static string getXExtTab(string caption)
        {
            return
                String.Format(
                    "//*[contains(@class,'x-toolbar-cell') and not(contains(@class,'x-hide-display'))]//button[text()='{0}']",
                    caption);
        }

        public static string getXExtMenuTab(string caption)
        {
            return String.Format("//span[text()='{0}' and contains(@class,'x-menu-item-text')]", caption);
        }

        /// <summary>
        /// Returns an Xpath for the WaitBar
        /// </summary>
        public static string getXWaitBar = "//*[@class='bbui-wait-bar']";

        public static string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["BBCRMBaseUrl"] ?? "http://localhost:80/bbappfx_40"; }
        }

        public static string VirtualPath
        {
            get
            {
                return ConfigurationManager.AppSettings["BBCRMHomeUrl"] ??
                       "/webui/webshellpage.aspx?databasename=BBInfinity";
            }
        }

        /// <summary>
        /// Navigate to the 'Constituents' functional area.
        /// </summary>
        public static void OpenConstituentsFA()
        {
            OpenFunctionalArea("Constituents", "fa_crm");
        }

        /// <summary>
        /// Navigate to the 'Fundraising' functional area.
        /// </summary>
        public static void OpenFundraisingFA()
        {
            OpenFunctionalArea("Fundraising", "fa_fundraisinginitiatives");
        }

        /// <summary>
        /// Navigate to the 'Revenue' functional area.
        /// </summary>
        public static void OpenRevenueFA()
        {
            OpenFunctionalArea("Revenue", "fa_revenue");
        }

        /// <summary>
        /// Navigate to the 'Events' functional area.
        /// </summary>
        public static void OpenEventsFA()
        {
            OpenFunctionalArea("Events");
        }

        /// <summary>
        /// Navigate to the 'Prospects' functional area.
        /// </summary>
        public static void OpenProspectsFA()
        {
            OpenFunctionalArea("Prospects", "fa_prospectresearch");
        }

        /// <summary>
        /// Navigate to the 'Analysis' functional area.
        /// </summary>
        public static void OpenAnalysisFA()
        {
            OpenFunctionalArea("Analysis", "query_large");
        }

        /// <summary>
        /// Navigate to the 'Memberships' functional area.
        /// </summary>
        public static void OpenMembershipsFA()
        {
            OpenFunctionalArea("Memberships", "memberships_32");
        }

        /// <summary>
        /// Navigate to the 'Marketing and Communications' functional area.
        /// </summary>
        public static void OpenMarketingAndCommunicationsFA()
        {
            OpenFunctionalArea("Marketing and Communications", "communication_32");
        }

        /// <summary>
        /// Open a Functional Area by caption and wait for its header image to load.
        /// </summary>
        /// <param name="caption">The caption(name) of the Functional Area to open.</param>
        /// <param name="imageName">All or part of the image name that the desired functional area displays in the header.</param>
        public static void OpenFunctionalArea(string caption, string imageName)
        {
            WebDriverWait navigateWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            navigateWaiter.IgnoreExceptionTypes(typeof (InvalidOperationException));
            navigateWaiter.Until(d =>
            {
                try
                {
                    OpenFunctionalArea(caption);
                    GetDisplayedElement(Panel.getXPanelHeader(imageName));
                    return true;
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Open a Functional Area by caption.
        /// </summary>
        /// <param name="caption">The caption(name) of the Functional Area to open.</param>
        public static void OpenFunctionalArea(string caption)
        {
            //ensure the basic components of the current panel have loaded.  Clicking too soon can lead to no navigation.
            GetDisplayedElement(Panel.getXPanelHeader());

            /*
             * The element to click can reside in the top menu bar, or be hidden in the additional
             * functional areas expander.
             */
            if (ExistsNow(getXExtTab(caption))) WaitClick(getXExtTab(caption));
            else
            {
                WebDriverWait navigateWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
                navigateWaiter.IgnoreExceptionTypes(typeof (InvalidOperationException));
                //retry logic
                navigateWaiter.Until(d1 =>
                {
                    WebDriverWait clickWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                    clickWaiter.IgnoreExceptionTypes(typeof (InvalidOperationException));
                    try
                    {
                        clickWaiter.Until(d2 =>
                        {
                            IWebElement additionalMenuItems = d2.FindElement(By.XPath(getXMenuMore));
                            if (!additionalMenuItems.Displayed) return false;
                            additionalMenuItems.Click();
                            return true;
                        });
                    }
                    catch (WebDriverTimeoutException)
                    {
                        return false;
                    }

                    try
                    {
                        clickWaiter.Until(d2 =>
                        {
                            IWebElement additionalMenuItems = d2.FindElement(By.XPath(getXExtMenuTab(caption)));
                            if (!additionalMenuItems.Displayed) return false;
                            additionalMenuItems.Click();
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
        /// Logon with the credentials specified in the config file.
        /// </summary>
        public static void Login()
        {
            LoginAs();
        }

        /// <summary>
        /// Login as a specified user.
        /// Pass credentials in the form "username:password".
        /// 
        /// Authenticaiton requires that the target environment uses Blackbaud
        /// CustomBasicAuthentication.
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        public static void LoginAs(string credentials=null)
        {
            string url = BaseUrl;
            //string ptcl = BaseUrl.StartsWith("https://") ? "https://" : "http://";
            string[] creds = null;

            if (null == credentials)
            {
                try
                {
                    creds = ConfigurationManager.AppSettings["Credentials"].Split(':');
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

            //if (null != credentials)
            //{
            //    url = BaseUrl.Replace(ptcl, ptcl + credentials.Trim() + "@");
            //}

            Driver.Navigate().GoToUrl(url.TrimEnd(new char[] { '/' }) + VirtualPath);

            if (creds != null)
            {                
                SetCredentailsField("//*[contains(@type,\"text\") and contains(@id,\"splash-login-username\")]", creds[0].Trim());
                SetCredentailsField("//*[contains(@type,\"password\") and contains(@id,\"splash-login-password\")]",
                    creds[1].Trim());
                
                ClickButton("Login");
            }
        }   
    }
}
