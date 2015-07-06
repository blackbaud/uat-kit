---
layout: layout-sidebar
name: Use the Selenium WebDriver
description: Walkthrough of how the UAT Automation Kit uses Selenium to interact with the Blackbaud CRM user interface.
order: 20
---

{{ include 'includes/eapwarning/index.md' }}

# Use the Selenium WebDriver
The {{ stache.config.product_name_short }} provides an API that lets you take advantage of Selenium WebDriver to interact with common elements in Infinity-based applications such as ***Blackbaud CRM***. You can use Selenium's WebDriver and Wait pattern to drive browser interactions.

## Prerequisites
* Complete the [Get Started]({{stache.config.blue_walkthroughs_getting-started}}) walkthrough.
* A unit test project with the {{ stache.config.product_name_short }} NuGet packages.
* Access to a ***Blackbaud CRM*** instance to test against.
* Familiarity with the {{ stache.config.product_name_short }} Custom SpecFlow Plugin for ***Visual Studio***.
* Familiarity with creating feature files.
* Familiarity with generating step classes bound to feature files.
* Familiarity with accessing the {{ stache.config.product_name_short }} Core API.
 
## Objectives
This tutorial guides you through the steps to create a custom interaction with the Selenium WebDriver and to use the WebDriver and Wait pattern to drive browser interactions. In this walkthrough, you will:
* Create a behavior-driven development test with Gherkin to navigate between functional areas.
* Create a step file to  specify actions for the test.
* Create a custom class that uses WebDriver to find an element, check a condition, and execute a step if the condition is met.
* Provide an XPath within the custom class's method to parse HTML elements and find the desired element.
* Build the test project and run the test.

## Create a Custom Interaction with the WebDriver
<p class="alert alert-info">The {{ stache.config.product_name_short }} API provides browser interactions that encapsulate different WebDriver logic. Before you create new WebDriver logic, check the API to see if the functionality is already provided. Hopefully the desired WebDriver logic and XPath constructor are already available in the API.</p>

<ol>
<li>
<p>Create a Gherkin test and step method in your unit test project in ***Visual Studio***.</p>

<ol>
<li>
<p>To create a feature test that requires navigation between functional areas, right-click the project in Solution Explorer, select <strong>Add</strong>, <strong>New item</strong>, and then select the <strong>SpecFlow Feature File</strong> template and click <strong>Add</strong>.</p>
<p>Then update the behavior-driven development test to confirm that you can navigate from the <strong><em>Constituents</em></strong> functional area to the <strong><em>Revenue</em></strong> functional area.</p>
<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Unimplemented feature test</div></div>
<pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Log into BBCRM, load a functional area, and change functional area.
	Given I have logged into BBCRM and navigated to functional area "Constituents"
	When I navigate to functional area "Revenue"
	Then the panel header caption is "Revenue"
</code>
</pre>
</li>

<li>
<p>To generate the failing step class methods, right-click within the feature file and select <strong>Generate Step Definitions</strong>, then click <strong>Generate</strong>, make sure the path points to your test project, and click <strong>Save</strong>.</p>
<p>At the beginning of the step file, insert <code>using Blueshirt.Core.Base</code> and <code>using Blueshirt.Core.Crm</code> to import the <strong><em>Blackbaud CRM</em></strong> types defined by those namespaces.</p>
<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Failing step class methods</div></div>
<pre><code class="language-csharp">
using Blueshirt.Core.Base;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    [Binding]
    public class SampleTestsSteps : BaseSteps
    {
        [Given(@"I have logged into BBCRM and navigated to functional area ""(.&#42;)""")]
        public void GivenIHaveLoggedIntoBbcrmAndNavigatedToFunctionalArea(string functionalArea)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I navigate to functional area ""(.&#42;)""")]
        public void WhenINavigateToFunctionalArea(string functionalArea)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the panel header caption is ""(.&#42;)""")]
        public void ThenThePanelHeaderCaptionIs(string headerCaption)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
</code>
</pre>
</li>

<li>
<p>Update the first <code>ScenarioContext.Current.Pending();</code> placeholder with an unimplemented class and method.</p>
<p>Delete <code>ScenarioContext.Current.Pending();</code> and replace it with <code>BBCRMHomePage.Login(); MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);</code> to specify logging in to <strong><em>Blackbuad CRM</em></strong> and navigating to the <strong><em>Constituents</em></strong> functional area.</p>
<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Unimplemented class and method</div></div>
<pre><code class="language-csharp">
[Given(@"I have logged into BBCRM and navigated to functional area ""(.&#42;)""")]
public void GivenIHaveLoggedIntoBbcrmAndNavigatedToFunctionalArea(string functionalArea)
{
    BBCRMHomePage.Login();
    MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);
}
</code>
</pre>
<p>If you build the project at this point with the unimplemented class and method in place, the build should fail.</p>
</li>
</ol>
</li>

<li>
<p>To create a custom class that inherits <code>BBCRMHomePage</code>, right-click the project in <strong>Solution Explorer</strong> and select <strong>Add</strong>, <strong>New item</strong>. On the Add New Item screen, select the <strong>Visual C# Items</strong> category and the <strong>Class</strong> template, enter "MyCustomBBCrmHomePage.cs" for the file name, and click <strong>Add</strong>.</p>

<p>At the beginning of the class file, insert <code>using Blueshirt.Core.Crm</code> to import the <strong><em>Blackbaud CRM</em></strong> types defined by that namespace, and specify that the <code>MyCustomBBCrmHomePage</code> class inherits from <code>BBCRMHomePage</code>. Then add the <code>NavigateToFunctionalArea</code> method to the class and specify that the method throws a <code>NotImplementedException</code>.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Custom class and method</div></div>
<pre><code class="language-csharp">
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
<p>If you attempt to build the project at this point with the exception specified for the method, the build should now succeed.</p>
</li>

<li>
<p>Implement the custom method.</p>

<p class="alert alert-info">The common pattern that the {{ stache.config.product_name_short }} employs is to wait until a condition is met and then proceed with the next action. It uses the Selenium Webdriver to interact with the browser and determine whether the desired condition is met.</p>

<ol>
<li>
<p>At the beginning of the new class file, insert references to the "OpenQA.Selenium" and "OpenQA.Selenium.Support.UI" namespaces to import the types defined by the namespaces.</p>
</li>

<li>
<p>Create a while loop that waits for "True" to be returned and starts over if "False" is returned. Specify the amount of time to expire before the loop throws a <code>WebDriverTimeoutException</code>, and specify the exception types to ignore in the loop. If exceptions of the specified types are thrown, the resulting action is the equivalent to "False" being returned at that moment.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Loop</div></div>
<pre><code class="language-csharp">
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
</li>

<li>
<p>Replace <code>NotImplementedException</code>. Use the WebDriver (referenced as <code>driver</code> in the lambda method) to find an element in <strong><em>Blackbaud CRM</em></strong> and check a condition on the element. In this example, check whether the element is <code>Displayed</code>. If the element is found but is not visible yet, return "False" because the desired condition is not met. This causes the loop to start over, and the WebDriver attempts to get a refreshed version of the element. When the element is visible, the WebDriver can send a <code>Click</code> action on the element and return "True" to exit the loop so that the next step method call can begin.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Example of using WebDriver to find an element, check a condition, and execute a step if the condition is met</div></div>
<pre><code class="language-csharp">
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
</li>

<li>
<p>To find elements the browser application, the WebDriver has an API with different selection methods. The {{ stache.config.product_name_short }} relies on XPaths to parse HTML elements and find desired elements. To learn about XPath patterns and best practices for <strong><em>Blackbaud CRM</em></strong>, see [XPath Guidelines]({{stache.config.blue_walkthroughs_xpaths}}).</p>

<p>For now, update the code driver <code>.FindElement</code> line with the following XPath information:</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">WebDriver FindElement using an XPath for the selector.</div></div><pre><code class="language-csharp">
IWebElement functionalAreaElement = driver.FindElement(By.XPath(String.Format("//button[text()='{0}']", caption)));
</code>
</pre>
</li>
</ol>
</li>

<li>
<p>Return to the step file and update the remaining <code>ScenarioContext.Current.Pending();</code> placeholders to implement the remaining step methods.</p>
<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Fully implemented step methods.</div></div>
<pre><code class="language-csharp">
[Given(@"I have logged into BBCRM and navigated to functional area ""(.&#42;)""")]
public void GivenIHaveLoggedIntoBbcrmAndNavigatedToFunctionalArea(string functionalArea)
{
    BBCRMHomePage.Login();
    MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);
}

[When(@"I navigate to functional area ""(.&#42;)""")]
public void WhenINavigateToFunctionalArea(string functionalArea)
{
    MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);
}

[Then(@"the panel header caption is ""(.&#42;)""")]
public void ThenThePanelHeaderCaptionIs(string headerCaption)
{
    if (!BaseComponent.Exists(Panel.getXPanelHeaderByText(headerCaption))) 
        FailTest(String.Format("'{0}' was not in the header caption.", headerCaption));
}
</code>
</pre>
</li>

<li>
<p>Build the project and run your test.</p>
<p class="alert alert-info"><strong><em>Reminder</em></strong>: If you did not already set your App.config to run on your accessible application and place ChromeDriver on your path, do that before you build the project and run your test. For more information, see <a href="{{stache.config.blue_walkthroughs_getting-started}}">Get Started</a>.</p>

<ol>
<li>
<p>Right-click the project in <strong>Solution Explorer</strong> and select <strong>Build</strong>.</p>
</l>
<li>
<p>In Test Explorer (<strong>Test</strong>, <strong>Windows</strong>, <strong>Test Explorer</strong>), right-click the test and select <strong>Run Selected Tests</strong>.</p>
<p>![](/assets/img/Selenium/RunSelectedTests.PNG)</p>
<p>![](/assets/img/Selenium/SelectedTestsPass.PNG)</p>
</l>
</ol>

</li>
</ol>

<!---
Commenting out the original procedure just in case I need it again. The ordered list replacement seems to work fine, but keeping the old version just to be safe
#### 1. Create the Gherkin test and Step method.

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
        [Given(@"I have logged into BBCRM and navigated to functional area ""(.&#42;)""")]
        public void GivenIHaveLoggedIntoBbcrmAndNavigatedToFunctionalArea(string functionalArea)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I navigate to functional area ""(.&#42;)""")]
        public void WhenINavigateToFunctionalArea(string functionalArea)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the panel header caption is ""(.&#42;)""")]
        public void ThenThePanelHeaderCaptionIs(string headerCaption)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
</code>
</pre>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Unimplemented class and method.  Build should fail at this point.</div></div><pre><code class="language-csharp">
[Given(@"I have logged into BBCRM and navigated to functional area ""(.&#42;)""")]
public void GivenIHaveLoggedIntoBbcrmAndNavigatedToFunctionalArea(string functionalArea)
{
    BBCRMHomePage.Login();
    MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);
}
</code>
</pre>
    
#### 2. Create custom class inheriting BBCRMHomePage

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

#### Implement the custom method. The common pattern employed in the {{ stache.config.product_name_short }} is to wait until a certain condition has been met before proceeding with the next action. The Selenium Webdriver is what allows us to interact with the browser in order to determine whether or not our desired condition has been met.
<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Be sure to add the 'OpenQA.Selenium' and 'OpenQA.Selenium.Support.UI' namespace references at the top of your file.</div></div><pre><code class="language-csharp">
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

In the above code, we essentially create a while loop that waits for "True" to be returned before exiting the loop. Any time "False" is returned, the loop starts over. We can specify an amount of time that should expire in the loop until a WebDriverTimeoutException is thrown. Finally we can specify Exception types to ignore in the loop. If exceptions of the specified types are thrown, the resulting action is the equivalent of "False" being returned at that moment.

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

Above we used the WebDriver (referenced as "driver" in our lambda method) to find an element on our application and checked a condition on the element. In this instance, we want to check if the element is "Displayed" before proceeding. If we found the element but it is not visible yet, we can immediately return "False" because our desired condition has not been met. This will cause the loop to start over, and the web driver will attempt to get a refreshed version of the element. If the element is visible, then we use the WebDriver to send a "Click" action on it and return true to exit the loop so that the next step method call can begin.  
<br>
An immediate question might be "How did the WebDriver find the element we wanted?" The WebDriver has an API with different selection methods in order to find elements in your browser application. The {{ stache.config.product_name_short }} relies on XPaths to parse the HTML elements and find the desired element. More details about XPaths and best practices can be found in the reference links at the end of this article. Suggested Enterprise CRM XPath patterns and examples will be discussed in a later walkthrough. For the moment, update the code driver.FindElement line to the following:

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">WebDriver FindElement using an XPath for the selector.</div></div><pre><code class="language-csharp">
IWebElement functionalAreaElement = driver.FindElement(By.XPath(String.Format("//button[text()='{0}']", caption)));
</code>
</pre>

#### 3. Finish Implementing Step Methods

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Fully implemented step methods.</div></div><pre><code class="language-csharp">
[Given(@"I have logged into BBCRM and navigated to functional area ""(.&#42;)""")]
public void GivenIHaveLoggedIntoBbcrmAndNavigatedToFunctionalArea(string functionalArea)
{
    BBCRMHomePage.Login();
    MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);
}

[When(@"I navigate to functional area ""(.&#42;)""")]
public void WhenINavigateToFunctionalArea(string functionalArea)
{
    MyCustomBBCrmHomePage.NavigateToFunctionalArea(functionalArea);
}

[Then(@"the panel header caption is ""(.&#42;)""")]
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

<p class="alert alert-info">The {{ stache.config.product_name_short }} API provides lots browser interactions that encapsulate different WebDriver logic. Before creating new WebDriver logic, first look into the API to see what functionality is already provided. Hopefully the desired WebDriver logic and XPath constructor are already available from the API.</p>
-->

### See Also

[XPath Guidelines]({{stache.config.blue_walkthroughs_xpaths}})
