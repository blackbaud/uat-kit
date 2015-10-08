using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.SpecFlow.Selenium
{
    public class BaseTest
    {
        public RemoteWebDriver driver;
        public ChromeDriverService service;

        /// <summary>
        /// The Test object.
        /// </summary>
        protected static BaseTest Test
        {
            get { return (BaseTest)ScenarioContext.Current["Test"]; }
            set
            {
                ScenarioContext.Current.Remove("Test");
                ScenarioContext.Current.Add("Test", Test);
            }
        }

        protected static void SaveChromeArtifacts(Boolean pass = false)
        {
            try
            {
                string grab = ConfigurationManager.AppSettings["ChromeDriver.Screenshot"];
                if (Test.driver is ITakesScreenshot)
                {
                    if ("true" == grab.ToLower() || ("fail" == grab.ToLower() && !pass))
                    {
                        ((ITakesScreenshot)Test.driver).GetScreenshot()
                            .SaveAsFile(
                                TechTalk.SpecFlow.ScenarioContext.Current.ScenarioInfo.Title.Replace(" ", "_") + ".png",
                                ImageFormat.Png);
                    }
                }
            }
            catch
            {
                // Do nothing if configuration is missing
            }

            try
            {
                string perf = ConfigurationManager.AppSettings["ChromeDriver.PerfLogging"];
                if ("false" != perf.ToLower())
                {
                    Response response = Test.driver.getLog("performance");
                    object[] values = response.Value as object[];
                    List<object> jsonList = new List<object>();
                    foreach (Dictionary<string, object> v in values)
                    {
                        foreach (string k in v.Keys)
                        {
                            if ("message" == k)
                            {
                                try
                                {
                                    string m = v[k] as string;
                                    Dictionary<string, object> message =
                                        JsonConvert.DeserializeObject<Dictionary<string, object>>(m);
                                    jsonList.Add(message["message"]);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                    string json = JsonConvert.SerializeObject(jsonList.ToArray()); //, Formatting.Indented);
                    System.IO.File.WriteAllText(
                        TechTalk.SpecFlow.ScenarioContext.Current.ScenarioInfo.Title.Replace(" ", "_") + "_perf.log",
                        json);
                    string webPageTestUrl = ConfigurationManager.AppSettings["ChromeDriver.WebPageTestUrl"];
                    if ("false" != webPageTestUrl.ToLower())
                    {
                        string wptCredentials = ConfigurationManager.AppSettings["ChromeDriver.WebPageTestCredentials"];
                        if (wptCredentials == "") wptCredentials = null;
                        string BBCRMBaseUrl = ConfigurationManager.AppSettings["BBCRMBaseUrl"];
                        string resp =
                            WebPageTest.CreateTest(
                                webPageTestUrl + "/runtest.php?location=Test&url=" + BBCRMBaseUrl + "&label=" +
                                TechTalk.SpecFlow.ScenarioContext.Current.ScenarioInfo.Title.Replace(" ", "") +
                                "&fvonly=1&f=json", wptCredentials);
                        dynamic respo = JObject.Parse(resp);
                        string testId = respo.data.testId;
                        var respp = WebPageTest.PostResults(webPageTestUrl + "/work/workdone.php", testId,
                            wptCredentials);
                        Console.WriteLine("Performance log at: {0}/result/{1}/", webPageTestUrl, testId);
                    }
                }
            }
            catch (Exception e)
            {
                // Do nothing if configuration is missing
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        protected static void StartDriver()
        {
            try
            {
                ChromeDriverService service = null;
                Blackbaud.UAT.SpecFlow.Selenium.RemoteWebDriver driver = null;

                string remoteUrl = ConfigurationManager.AppSettings["RemoteDriver"];
                if ("false" == remoteUrl.ToLower())
                {
                    service = BaseTest.InitializeChromeService();
                    Test.service = service;
                    driver = BaseTest.InitializeChromeDriver(service.ServiceUrl);
                }
                else
                {
                    driver = new Blackbaud.UAT.SpecFlow.Selenium.RemoteWebDriver(new Uri(remoteUrl), DesiredCapabilities.Chrome());
                }

                Test.driver = driver;

                if (Test.service != null)
                    ScenarioContext.Current.Add("Service", service);
                if (Test.driver != null)
                    ScenarioContext.Current.Add("Driver", driver);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Do nothing if context is unavailable                
            }
        }

        protected static void StopDriver()
        {
            try
            {
                Test.driver.Quit();
                Test.service.Dispose();
            }
            catch (Exception)
            {
                //Do nothing if context is unavailable                
            }

            Test.driver = null;
            Test.service = null;

            ScenarioContext.Current.Remove("Driver");
            ScenarioContext.Current.Remove("Service");
        }

        public static void NewSession(Boolean force = false)
        {
            //Skip this if the existing sesison hasn't been used unless forced
            if (force || (false == Test.driver.Url.StartsWith("data")))
            {
                StopDriver();
                StartDriver();
            }
        }

        public static OpenQA.Selenium.Chrome.ChromeDriverService InitializeChromeService()
        {
            OpenQA.Selenium.Chrome.ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            try
            {
                string logging = ConfigurationManager.AppSettings["ChromeDriver.Logging"];
                if ("false" != logging.ToLower())
                {
                    service.LogPath = TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title.Replace(" ", "_") +
                                      "_feature.log";
                    if ("verbose" == logging.ToLower())
                    {
                        service.EnableVerboseLogging = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // Do nothing if configuration is missing
            }
            service.Start();
            return service;
        }

        public static RemoteWebDriver InitializeChromeDriver(Uri serviceUrl)
        {

            ChromeOptions options = new ChromeOptions();
            var perfLoggingPrefs = new Dictionary<string, object> { { "enableNetwork", true }, { "enablePage", true }, { "enableTimeline", false } };
            try
            {                
                string perf = ConfigurationManager.AppSettings["ChromeDriver.PerfLogging"];
                if ("false" != perf.ToLower())
                {
                    options.AddAdditionalCapability("perfLoggingPrefs", perfLoggingPrefs);
                }
                            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // Swallow non-existent Configurations
            }

            try
            {
                string mobile = ConfigurationManager.AppSettings["ChromeDriver.MobileEmulationDevice"];
                if ("false" != mobile.ToLower())
                {
                    options.AddAdditionalCapability("mobileEmulation",
                        new Dictionary<string, object> {{"deviceName", mobile}});
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // Swallow non-existent Configurations
            }

            try
            {
                options.AddArgument("--lang=" + ConfigurationManager.AppSettings["ChromeDriver.language"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // Swallow non-existent Configurations
            }

            DesiredCapabilities capabilities = options.ToCapabilities() as DesiredCapabilities;
            var loggingPrefs = new Dictionary<string, object> { { "performance", "ALL" }, { "browser", "ALL" } };
            try
            {
                string perf = ConfigurationManager.AppSettings["ChromeDriver.PerfLogging"];
                if ("false" != perf.ToLower())
                {
                    capabilities.SetCapability("loggingPrefs", loggingPrefs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // Swallow non-existent Configurations
            }

            return new RemoteWebDriver(serviceUrl, capabilities);
        }
    }
}
