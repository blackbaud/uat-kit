using System;
using Blackbaud.UAT.Core.Bbis.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Blackbaud.UAT.Base;

namespace SystemTest.Common.Bbis
{

    // TODO - merge these into Core BBISHomePage proper when development is complete in this project.

    public class BBISHomePageCustom : BBISHomePage
    {
        /// <summary>
        /// Open the Gallery by the name used in the final part of the Url. 
        /// e.g. use 'file' for the Files page at s01t2iseweb001.blackbaudqa.net/cms/filegallery 
        /// </summary>
        /// <param name="name">The final part of the Url minus 'gallery'.</param>
        public static void OpenGallery(string name)
        {
            string url = BaseUrl;
            Driver.Navigate().GoToUrl(url.TrimEnd('/') + "/cms/" + name + "gallery");
        }

        public static new void SetTextField(string xpath, string text)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

            wait.Until(d =>
            {
                var element = Driver.FindElement(By.XPath(xpath));
                if (element == null || !element.Displayed || !element.Enabled) return false;
                element.Clear();
                element.SendKeys(text);
                return true;
            });
        }

        public static void SetDropDownField(string xpath, string text)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(StaleElementReferenceException));

            wait.Until(d =>
            {
                var element = Driver.FindElement(By.XPath(xpath));
                if (element == null || !element.Displayed || !element.Enabled) return false;
                element.SendKeys(text);
                return true;
            });
        }

        public static string SwitchToNewestChildWindow(int wait = 10)
        {
            var waiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(wait));
            string handle = Driver.CurrentWindowHandle;

            waiter.Until(d => d.WindowHandles.Count == 2);

            return handle;
        }

        public static void SwitchBackWindow(string handle)
        {
            Driver.SwitchTo().Window(handle);
        }

    }
}
