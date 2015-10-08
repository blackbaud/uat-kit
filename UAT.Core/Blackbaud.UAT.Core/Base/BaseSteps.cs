using System;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Base
{
    /// <summary>
    /// Base class for Specflow Step classes.  Provides access to common functionality in step classes.
    /// </summary>
    public class BaseSteps
    {        
        /// <summary>
        /// Time stamp that should be static for all test step definitions in a single test
        /// </summary>
        // One for the kids - with a twist
        protected static string uniqueStamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 7, 4)).TotalSeconds).ToString();

        /// <summary>
        /// Fail a test by throwing a SpecFlowException with the provided message
        /// </summary>
        /// <param name="errorMessage">The message to output with the SpecFlowException</param>
        public static void FailTest(string errorMessage)
        {
            throw new SpecFlowException(errorMessage);
        }

    }
}