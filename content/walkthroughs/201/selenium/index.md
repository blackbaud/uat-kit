---
layout: layout-sidebar
name: Using the Selenium WebDriver
description: Walkthrough of how Project Blue uses Selenium to interact with the UI.
order: 10
---

# Using the Selenium WebDriver

<p class="alert alert-warning">Warning: This is preliminary documentation and is subject to change</p>

In this walkthrough you'll learn how to use Selenium's WebDriver and Wait pattern to drive browser interactions.

## Prerequisites

1. A unit test project with the UAT SDK (Project Blue) NuGet package installed.
2. An Enterprise CRM application is accessible and tests can be run against this application.
3. Familiarity with
 * Using the UAT SDK (Project Blue) Custom SpecFlow Plugin for Visual Studio.
 * Creating new feature files.
 * Generating step classes bound to feature files.
 * Accessing the UAT SDK (Blueshirt) Core API.

## Create a Custom Interaction Using the WebDriver.

#### Create the Gherkin test and Step method.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Create an unimplemented feature test requiring navigation between functional areas.  </div></div><pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Log into BBCRM, load a functional area, and change functional area.
	Given I have logged into BBCRM and navigated to functional area "Constituents"
	When I navigate to functional area "Revenue"
	Then the panel header caption is "Revenue"
</code>
</pre>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Generate the failing step class methods.</div></div><pre><code class="language-csharp">
using Blueshirt.Core.Base;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    [Binding]
    public class SampleTestsSteps : BaseSteps
    {
        [Given(@"I have logged into BBCRM and navigated to functional area ""(.*)""")]
        public void GivenIHaveLoggedIntoBbcrmAndNavigatedToFunctionalArea(string functionalArea)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I navigate to functional area ""(.*)""")]
        public void WhenINavigateToFunctionalArea(string functionalArea)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the panel header caption is ""(.*)""")]
        public void ThenThePanelHeaderCaptionIs(string headerCaption)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
</code>
</pre>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Unimplemented class and method.  Build should fail at this point.</div></div><pre><code class="language-csharp">
[Given(@"I have logged into BBCRM and navigated to functional area ""(.*)""")]
public void GivenIHaveLoggedIntoBbcrmAndNavigatedToFunctionalArea(string functionalArea)
{
    BBCRMHomePage.Logon();
    MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);
}
</code>
</pre>
    
#### Create custom class inheriting BBCRMHomePage.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Create the new custom class and method. Have the new method throw a NotImplementedException. The build should succeed now.</div></div><pre><code class="language-csharp">
using System;
using Blueshirt.Core.Crm;

namespace Delving_Deeper
{
    public class MyCustomBBCrmHomePage : BBCRMHomePage
    {
        public static void NavigateToFunctionalArea(string caption)
        {
            throw new NotImplementedException();
        }
    }
}
</code>
</pre>

#### Implement the custom method. The common pattern employed in the UAT SDK is to wait until a certain condition has been met before proceeding with the next action. The Selenium Webdriver is what allows us to interact with the browser in order to determine whether or not our desired condition has been met.
<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Be sure to add the 'OpenQA.Selenium' and 'OpenQA.Selenium.Support.UI' references.</div></div><pre><code class="language-csharp">
public static void NavigateToFunctionalArea(string caption)
{
    WebDriverWait navigateWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
    navigateWaiter.IgnoreExceptionTypes(typeof(InvalidOperationException));
    navigateWaiter.Until(driver =>
    {
        throw new NotImplementedException();
    });
}
</code>
</pre>

In the above code, we have essentially created a while loop that waits for 'True' to be returned before exiting the loop. Any time 'False' is returned, the loop starts over. We can specify an amount of time that should expire in the loop until a WebDriverTimeoutException will be thrown. Finally we can specify Exception types to ignore in the loop. If exceptions of the specified types are thrown, the resulting action is the equivalent of 'False' being returned at that moment.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Example of using the WebDriver to find a web element, check a condition on it, and execute a step if the condition has been met.</div></div><pre><code class="language-csharp">
WebDriverWait navigateWaiter = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
navigateWaiter.IgnoreExceptionTypes(typeof(InvalidOperationException));
navigateWaiter.Until(driver =>
{
    IWebElement functionalAreaElement = driver.FindElement(null);
    if (!functionalAreaElement.Displayed) return false;
    functionalAreaElement.Click();
    return true;
});
</code>
</pre>

Above we used the WebDriver (referenced as 'driver' in our lambda method) to find an element on our application and checked a condition on the element. In this instance, we want to check if the element is 'Displayed' before proceeding. If we found the element but it is not visible yet, we can immediately return false because our desired condition has not been met. This will cause the loop to start over, and the web driver will attempt to get a refreshed version of the element. If the element is visible, then we use the WebDriver to send a 'Click' action on it and return true to exit the loop so that the next step method call can begin.  
<br>
An immediate question might be "How did the WebDriver find the element we wanted?" The WebDriver has an API with different selection methods in order to find elements in your browser application. The UAT SDK (Blueshirt) relies on XPaths to parse the HTML elements and find the desired element. More details about XPaths and best practices can be found in the reference links at the end of this article. Suggested Enterprise CRM XPath patterns and examples will be discussed in a later walkthrough. For the moment, update the code driver.FindElement line to the following:

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">WebDriver FindElement using an XPath for the selector.</div></div><pre><code class="language-csharp">
IWebElement functionalAreaElement = driver.FindElement(By.XPath(String.Format("//button[text()='{0}']", caption)));
</code>
</pre>

#### Finish Implementing Step Methods

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Fully implemented step methods.</div></div><pre><code class="language-csharp">
[Given(@"I have logged into BBCRM and navigated to functional area ""(.)""")]
public void GivenIHaveLoggedIntoBbcrmAndNavigatedToFunctionalArea(string functionalArea)
{
    BBCRMHomePage.Logon();
    MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);
}

[When(@"I navigate to functional area ""(.)""")]
public void WhenINavigateToFunctionalArea(string functionalArea)
{
    MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);
}

[Then(@"the panel header caption is ""(.*)""")]
public void ThenThePanelHeaderCaptionIs(string headerCaption)
{
    if (!BaseComponent.Exists(Panel.getXPanelHeaderByText(headerCaption))) 
        FailTest(String.Format("'{0}' was not in the header caption.", headerCaption));
}
</code>
</pre>

Make sure your app.config is set to run on your accessible application, build the solution, and run the test.  
![](/assets/img/Selenium/RunSelectedTests.PNG)

The test should pass!  
![](/assets/img/Selenium/SelectedTestsPass.PNG)
    
## See Also

[XPath Guidelines]({{stache.config.blue_walkthroughs_201_xpaths}})
