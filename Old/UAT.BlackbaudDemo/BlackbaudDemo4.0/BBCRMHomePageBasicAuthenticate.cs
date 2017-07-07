using System;
using System.Configuration;
using System.Diagnostics;
using Blackbaud.UAT.Core.Crm;
using OpenQA.Selenium;

namespace BlackbaudDemo40
{
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
