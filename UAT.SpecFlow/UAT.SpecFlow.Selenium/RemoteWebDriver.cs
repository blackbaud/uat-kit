using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Blackbaud.UAT.SpecFlow.Selenium
{
    /// <summary>
    /// Extension to <see cref="OpenQA.Selenium.Remote.RemoteWebDriver"/> allowing access to Performance Logging capabilites
    /// 
    /// Note: 
    /// This currently relies on the inbuilt pre release Webdriver verison which I built from source.
    /// Specifically the CommandInfoRepository.TryAddCommand method to access the JSONWireProtocol directly.
    /// As of version 2.45 this is not in the release.
    /// We should switch to 2.46 when the nuget is available.
    /// 
    /// https://code.google.com/p/selenium/wiki/JsonWireProtocol#/session/:sessionId/log/types
    ///
    /// </summary>
    public class RemoteWebDriver : OpenQA.Selenium.Remote.RemoteWebDriver
    {
        /// <summary>
        /// Initializes a new instance of the RemoteWebDriver class
        /// 
        /// <param name="remoteAddress">URI containing the address of the WebDriver remote server (e.g. http://127.0.0.1:4444/wd/hub).</param>
        /// <param name="desiredCapabilities">An <see cref="ICapabilities"/> object containing the desired capabilities of the browser.</param>
        public RemoteWebDriver(Uri remoteAddress, ICapabilities desiredCapabilities)
            : base(remoteAddress, desiredCapabilities)
        {
            CommandInfoRepository.Instance.TryAddCommand("getAvailableLogsTypes",new CommandInfo("GET","/session/{sessionId}/log/types"));
            CommandInfoRepository.Instance.TryAddCommand("getLog", new CommandInfo("POST", "/session/{sessionId}/log"));
        }

        /// <summary>
        /// Gets the available Logs Types
        /// </summary>
        public Response getAvailableLogTypes()
        {
            return base.Execute("getAvailableLogsTypes", null);
        }

        /// <summary>        
        ///  Gets the specified Log
        /// </summary>
        /// <param name="type"> The type of log to retrieve e.g. "performance","browser","server",etc.</param>
        public Response getLog(string type)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "type", type } };
            return base.Execute("getLog", parameters);
        }
    }
}
