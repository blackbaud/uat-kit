using System;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Blackbaud.UAT.Core.Crm.Panels;

namespace Blackbaud.UAT.Core.Crm
{    
    /// <summary>
    /// Utility methods for operations on a functional area.
    /// </summary>
    public class FunctionalArea : BaseComponent
    {

        /// <summary>
        /// Returns an Xpath for a Functional Area link.
        /// </summary>
        /// <param name="linkText">The Link text to use.</param>
        /// <returns></returns>
        public static string getXFaLink(string linkText)
        {
            //not(contains(@class,'bbui-pages-actiongroup-tooltip-header')) - don't select popup button
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//button[contains(@class,'linkbutton') and not(contains(@class,'actiongroup-tooltip-header'))]//div[text()='{0}']", linkText);
        }


        /// <summary>
        /// Formats an XPath to handle an Add menu.  
        /// </summary>
        /// <param name="addId">The unique ID in the HTML Table element's id. i.e. 'ADDNEWGROUP'</param>
        /// <param name="caption">The caption of the add menu.  Defaults to 'Add new'</param>
        public static string getXAddMenu(string addId, string caption = "Add new")
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//table[contains(@id,'{0}')]//button[./text()='{1}']", addId, caption);
        }

        /// <summary>
        /// Formats the unique XPath for finding a task on a functional area page
        /// using the grouping caption and task caption.
        /// </summary>
        /// <param name="groupCaption">The caption of the Grouping header that the task belongs to.</param>
        /// <param name="taskName">The name of the task to interact with.</param>
        public static string getXFunctionalAreaTask(string groupCaption, string taskName)
        {
            return String.Format("//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'hide'))]//*[@title='{0}']/..//button[contains(@class,'linkbutton')]//div[text()='{1}']", groupCaption, taskName);
        }

        /// <summary>
        /// Formats an XPath for finding a task on a functional area page
        /// using the task link's caption.
        /// </summary>
        /// <param name="taskName">The name/caption of the task.</param>
        public static string getXFunctionalAreaTask(string taskName)
        {
            return String.Format("//button/div[text='{0}']", taskName);
        }

        /// <summary>
        /// Click on the functional area link.
        /// </summary>
        /// <param name="caption">The caption of the task link.</param>
        public static void OpenLink(string caption)
        {
            WaitClick(getXFaLink(caption));    
        }

        /// <summary>
        /// Click on the functional area link.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.</param>
        /// <param name="linkCaption">The caption of the task link.</param>
        public static void OpenLink(string groupCaption, string linkCaption)
        {
            //TODO add retry logic.  Wait for a visible dialog to appear or a different panel header.
            WaitClick(getXFunctionalAreaTask(groupCaption, linkCaption));
        }

        /// <summary>
        /// Click on the functional area link that navigates to a panel.
        /// </summary>
        /// <param name="groupCaption">The group header caption of the task.</param>
        /// <param name="linkCaption">The caption of the task link.</param>
        /// <param name="headerText">The header caption of the desired panel to navigate to.</param>
        public static void OpenLinkToPanel(string groupCaption, string linkCaption, string headerText)
        {
            var navigateWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            navigateWaiter.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            navigateWaiter.Until(d =>
            {
                var panelHeader = d.FindElement(By.XPath(Panel.getXPanelHeader()));
                if (panelHeader.Text != headerText)
                {
                    WaitClick(getXFunctionalAreaTask(groupCaption, linkCaption));
                    return false;
                }
                return true;
            });
        }
    }
}
