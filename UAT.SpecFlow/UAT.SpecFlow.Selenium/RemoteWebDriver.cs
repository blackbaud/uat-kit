using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Blackbaud.UAT.SpecFlow.Selenium
{
    /// <summary>
    /// Extension to <see cref="OpenQA.Selenium.Remote.RemoteWebDriver"/> allowing access to Performance Logging capabilites
    /// </summary>
    public class RemoteWebDriver : OpenQA.Selenium.Remote.RemoteWebDriver
    {
        /// <summary>
        /// Initializes a new instance of the RemoteWebDriver class - this interface is here for backwards compatibility only
        /// </summary>
        /// <param name="remoteAddress">URI containing the address of the WebDriver remote server (e.g. http://127.0.0.1:4444/wd/hub).</param>
        /// <param name="desiredCapabilities">An <see cref="ICapabilities"/> object containing the desired capabilities of the browser.</param>
        public RemoteWebDriver(Uri remoteAddress, ICapabilities desiredCapabilities, String stoopid = "WTF")
            : this(remoteAddress, desiredCapabilities)
        {
        }


        private RemoteWebDriver(Uri remoteAddress, ICapabilities desiredCapabilities)
            : base(remoteAddress, desiredCapabilities)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RemoteWebDriver class - these interfaces are here for backwards compatibility only
        /// </summary>
        /// <param name="remoteAddress">URI containing the address of the WebDriver remote server (e.g. http://127.0.0.1:4444/wd/hub).</param>
        /// <param name="options">An <see cref="DriverOptions"/> object containing the desired capabilities of the browser.</param>
        public RemoteWebDriver(Uri remoteAddress, DriverOptions options, String stoopid = "WTF")
            : this(remoteAddress, options)
        {
        }


        private RemoteWebDriver(Uri remoteAddress, DriverOptions options)
            : base(remoteAddress, options)
        {
        }

        /// <summary>
        /// Gets the available Logs Types
        /// </summary>
        public Response GetAvailableLogTypes()
        {
            return this.Execute(DriverCommand.GetAvailableLogTypes, null);
        }

        /// <summary>        
        ///  Gets the specified Log
        /// </summary>
        /// <param name="type"> The type of log to retrieve e.g. "performance","browser","server",etc.</param>
        public Response GetLog(string type)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "type", type } };
            return this.Execute(DriverCommand.GetLog, parameters);
        }
    }
}
