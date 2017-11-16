using System;
using System.Configuration;
using System.Diagnostics;
using Blackbaud.UAT.Core.Crm;
using OpenQA.Selenium;

namespace BlackbaudDemo40
{
    /*
         A Note on the automated authentication options (or lack of them) in chrome:

         Until recently (I want to say about chrome 39 but I'm too lazy to look at the release notes so I'll stick with recently) 
         Chrome was happy with the http://user:passwd@hostname.domain/path URL format for basic authentication on BBCRM and BBIS.
         However security tightening by the Chrome team has removed this support# forcing us to search for other options. 
         (I can understand the need to improve security for the majority use cases however I cannot understand the complete lack of options for the automated test community)
         Anyway at the time the c# bindings for webdriver did not have support for SetAuthenticationCredentials and our target was restricted to test environemnts so I
         set up the suites and environments to use BBCustomAuthentication which is entirely within the webpage and is no problem to automate using standard webdriver methods.

         Now however I now require to test in environemnts where we cannot dictate the authentication method and have to deal with the native chrome authentication dialog.
         Since my first experiments, chromedriver and the bindings have moved on and apparently include support for SetAuthenticationCredentials.
         However, I cannot get it to work as advertised - the dialog is not recognised as an alert by chromedriver.

         So I am forced to go to another tool. Turns out that, unsatisfactory as this is, it is not as messy as I feared.
         I have precompiled a simple AutoIT script into a stand-alone exe basically according to this :

         http://toolsqa.com/selenium-webdriver/autoit-selenium-webdriver/

         If SetAuthenticationCredentials ever gets fixed I think it will be preferable but the AutoIT way irks me less than I tought.

         # turns out it may still be supported in a narrow sense so technically they are not breaking standard compliance, however it doesn't work for our case.

         Aside:
         There are of cousre other less well known (or attractive?) options for automated authentication
            - It should be possible to use the chrome password manager and start the chromedriver with a saved profile
            - It should also be possible to automate the gui directly from code here without AutoIT 
            - Use a proxy                       
    */

    class BBCRMHomePageBasicAuthenticate : BBCRMHomePage
    {
        /// <summary>
        /// Login as a specified user.
        /// Pass credentials in the form "username:password".  
        /// </summary>
        /// <param name="credentials"></param>
        public new static void LoginAs(string credentials = null)
        {

            string url = BaseUrl;
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

            Driver.Navigate().GoToUrl(url.TrimEnd(new char[] { '/' }) + VirtualPath);

            if (creds != null)
            {
                
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = "chromeauth.exe";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = creds[0].Trim() + " " + creds[1].Trim();

                try
                {
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
