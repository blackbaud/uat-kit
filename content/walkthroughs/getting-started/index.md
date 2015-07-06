---
layout: layout-sidebar
name: Get Started
description: Learn how to create a Blackbaud CRM GUI test suite using the UAT Automation Kit.
order: 10
---

{{ include 'includes/eapwarning/index.md' }}

# Get Started with {{ stache.config.product_name_short }}
This tutorial walks you through the process of creating a test project with the {{ stache.config.product_name_short }}. After you master the basics, you can use the {{ stache.config.product_name_short }} to create a suite of automated tests that facilitate your User Acceptance Testing for ***Blackbaud CRM*** upgrades.

<p class="alert alert-info">This tutorial assumes familiarity with ***Visual Studio***, Gherkin, SpecFlow, WebDriver, and XPath.</p>

## Prerequisites
Before you create a test project and install the NuGet packages for the {{ stache.config.product_name_short }}, you must have a handful of prerequisites in place:
* Install ***Visual Studio 2013*** or later. You can install the free ***Visual Studio 2013 Community*** edition, or you can use a more advanced edition. To install the free Community edition, see [Microsoft's installation instructions](https://www.visualstudio.com/en-us/products/free-developer-offers-vs.aspx).
* Make sure the ***Visual Studio*** NuGet extension is in place. Starting with ***Visual Studio 2012***, NuGet is included by default in every edition except Team Foundation Server, and you can find updates to NuGet through the Extension Manager. For more information about NuGet, see [www.nuget.org](https://www.nuget.org/).
* Install the IDE integration for the ***Visual Studio*** SpecFlow extension. For information about SpecFlow, see [SpecFlow](http://www.specflow.org/).
* Install the Chromedriver standalone server that implements WebDriver's wire protocol for Chromium. For information about ChromeDriver, see [sites.google.com/a/chromium.org/chromedriver/downloads](https://sites.google.com/a/chromium.org/chromedriver/downloads).
* Install or get access to a ***Blackbaud CRM*** instance to test against. To run tests that you create with the {{ stache.config.product_name_short }}, your target ***Blackbaud CRM*** environment must either use customBasicAuthentication or the tests must not use authentication. You cannot run {{ stache.config.product_name_short }} tests with Windows authentication. 


## Objectives
This tutorial guides you through the steps to create a simple test that confirms that the constituent search screen returns a specific result. In this walkthrough, you will:
* Create a test project in ***Visual Studio*** and install NuGet packages for the {{ stache.config.product_name_short }}.
* Update the test project's App.config to include URL and login information for your test environment.
* Create a behavior-driven development test with Gherkin in a SpecFlow feature file, and create a step file to  specify actions for the test.
* Build the test project and run the test.

## Create a Test Project
<ol>
<li>
<p>Set up authentication for the <strong><em>Blackbaud CRM</em></strong> instance that you will test against.</p>
<p class="alert alert-info">To run the tests that you create with the {{ stache.config.product_name_short }}, your target ***Blackbaud CRM*** environment must either use customBasicAuthentication or the tests must not use authentication. If you prefer to run tests without authentication, skip this step and in the step below about updating the App.config file, exclude the <code>add</code> element for credentials.</p>
<ol>
<li>
<p>Make sure that customBasicAuthentication is enabled in the Web.config file for your ***Blackbaud CRM*** instance.</p>
<p>In the installation directory, open the bbappfx\vroot folder, then open the Web.config file and locate the <code>customBasicAuthentication</code> element. If the <code>enabled</code> attribute is not already set to "true," then update the attribute.</p>
<pre><code>
&#60;customBasicAuthentication enabled="true" requireSSL="False" cachingEnabled="True" cachingDuration="600" &#47;&#62;
</code>
</pre>

</li>
<li>
<p>In <strong><em>Internet Information Services</em></strong> (<strong><em>IIS</em></strong>), disable all authentication except for Anonymous Authentication at the vroot level for the <strong><em>Blackbaud CRM</em></strong> instance.</p>
<p>![Enable IIS Anonymous Authentication](/assets/img/IISAuthentication.PNG)</p>
</li>

<li>
<p>In <strong><em>IIS</em></strong>, disable all authentication for the uimodel and webui folders under the vroot level.</p>
<p>![Disable IIS Authentication](/assets/img/IISAuthentication2.PNG)</p>
</li>
</ol>

<p class="alert alert-info">If you set up customBasicAuthentication for your target ***Blackbaud CRM*** environment, then make sure to enter your <strong><em>Blackbaud CRM</em></strong> credentials in the third <code>add</code> element in the step below about updating the App.config file.</p>
</li>
<li>
<p>In ***Visual Studio***, select <strong>File</strong>, <strong>New</strong>, <strong>Project</strong>, and then select the <strong>Test</strong> category under <strong>Visual C#</strong> and the <strong>Unit Test Project</strong> template. Edit the project name, location, and solution name as necessary.</p>
<p>![New Project wizard](/assets/img/FirstProject/NewBSProject.PNG)</p>
<p class="alert alert-info">The {{ stache.config.product_name_short }} takes advantage of the <strong>Unit Test Project</strong> template to create a test project for system tests. It does not create unit tests.</p>
</li>

<li>
<p>Add the <strong>Blueshirt SpecFlow Plugin</strong> and <strong>Blueshirt Core Classes</strong> packages to your project.</p>
<p>![Package manager wizard](/assets/img/FirstProject/AddBSNuGetPackages.PNG)</p>

<ol>
<li>
<p>To access the NuGet package source, right-click the solution in <strong>Solution Explorer</strong> and select <strong>Manage NuGet Packages for Solution</strong>. Then on the Manage NuGet Packages screen, click <strong>Settings</strong>. And on the Options screen, click the add button on enter "BB NuGet" in the <strong>Name</strong> field and "http://tfs-sym.blackbaud.com:81/nuget/" in the <strong>Source</strong> field.</p>
<p class="alert alert-info"><strong><em>NEED TO UPDATE THIS SECTION FOR EAP. AND MAYBE AGAIN WHEN WE RELEASE.</em></strong> ... The {{ stache.config.product_name_short }} packages may be available from the public NuGet package repository in the nuget.org category. ... Or we may provide standalone nupkg files instead and have users add the location of htose files as a package source in the NuGet settings.</p>
</li>
<li>
<p>Back on the Manage NuGet Packages screen, select the <strong>BB NuGet</strong> category and search for "Blueshirt."</p>
</li>
<li>
<p>Select <strong>Blueshirt SpecFlow Plugin</strong> and click <strong>Install</strong>. Then with your unit test project and solution selected on the Select projects screen, click <strong>OK</strong>.</p>
</li>
<li>
<p>Select <strong>Blueshirt Core Classes</strong> and click <strong>Install</strong>. Then with your unit test project and solution selected on the Select projects screen, click <strong>OK</strong>.</p>
</li>
<li>
<p>After you install both packages, click <strong>Close</strong>.</p>
</li>
</ol>
</li>

<li>
<p>Add target environment URLs to the project's App.config file.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Example appSettings section</div></div>
<pre><code class="language-csharp">
&lt;appSettings&gt;

  &lt;add key="BBCRMBaseUrl" value="https://blackbaudDemo.com/bbappfx" /&gt;
  &lt;add key="BBCRMHomeUrl" value="/webui/webshellpage.aspx?databasename=BBInfinity" /&gt;
  &lt;add key="Credentials" value="user:password" /&gt;

  ....

&lt;/appSettings&gt;
</code>
</pre>

<p>From <strong>Solution Explorer</strong>, open the App.config file and scroll down to the appSettings element.</p>

<ul>
<li>
<p>Insert an <code>add</code> element and set its <code>key</code> attribute to "BBCRMBaseURL" and its <code>value</code> attribute to the portion of your <strong><em>Blackbaud CRM</em></strong> URL that includes the server and virtual directory.</p>
<p class="alert alert-info">For <strong><em>Blackbaud Internet Solutions</em></strong>, set the <code>key</code> attribute to "BBISBaseURL."</p>
</li>
<li>
<p>Insert an <code>add</code> element and set its <code>key</code> attribute to "BBCRMHomeURL" and its <code>value</code> attribute to the to the portion of your <strong><em>Blackbaud CRM</em></strong> URL after the virtual directory up to the database name.</p>
<p class="alert alert-info">For <strong><em>Blackbaud Internet Solutions</em></strong>, set the <code>key</code> attribute to "BBISHomeURL."</p>
</li>
<li>
<p>Insert an <code>add</code> element and set its <code>key</code> attribute to "Credentials" and its <code>value</code> attribute to the <strong><em>Blackbaud CRM</em></strong> user name and password to use for testing.</p>
<p>Keep in mind that your tests must use <strong><em>Blackbaud CRM</em></strong> credentials, not Windows authentication credentials. If you do not want to set up customBasicAuthentication for your target ***Blackbaud CRM*** environment and enter credentials here, you must exclude the third <code>add</code> element and manually log in to <strong><em>Blackbaud CRM</em></strong> before you run the test.</p>
<p class="alert alert-warning"><strong><em>Warning:</em></strong> For security reasons, you should not use credentials for your production environment. The {{ stache.config.product_name_short }} is intended for test or staging environments.</p>
</li>
</ul>
</li>

<li>
<p>Create a feature file.</p>
<p>![New Item wizard](/assets/img/FirstProject/AddAFeatureFile.PNG)</p>
<p>Right-click the project in <strong>Solution Explorer</strong> and select <strong>Add</strong>, <strong>New item</strong>. Then select the <strong>Visual C# Items</strong> category and <strong>SpecFlow Feature File</strong> template, edit the file name as necessary, and click <strong>Add</strong>.</p> 
</li>

<li>
<p>In the new feature file, create your behavior-driven development test with Gherkin.</p>
<p>For example, you can create a test to confirm that the constituent search screen returns a specific result for search criteria.</p>
<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Example Feature File</div></div>
<pre><code class="language-gherkin">
Feature: Constituent Search
  In order to manage Constituent Records
  As a Blackbaud CRM user
  I want to search existing Constituent records

Scenario: Quick Constituent Search
  Given I have logged into the BBCRM home page
  And I have opened the constituent search dialog
  When I search for "Hampton"
  Then The results should contain "Hampton Street Elementary School"
</code>
</pre>
</li>

<li>
<p>Generate a step file.</p>
<p></p>
<p>![Step File wizard](/assets/img/FirstProject/GenerateSteps.PNG)</p>
<p>Right-click within the feature file and select <strong>Generate Step Definitions</strong>. On the Generate Step Definition Skeleton screen, click <strong>Generate</strong>. Then on the Select target step definition class file screen, make sure the path points to your test project and click <strong>Save</strong>.</p>
</li>

<li>
<p>Populate the steps.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Example Steps File</div></div>
<pre><code class="language-csharp">
using System;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Blue&#95;101
{
    [Binding]
    public class ConstituentSearchSteps
    {
        [Given(@"I have logged into the BBCRM home page")]
        public void GivenIHaveLoggedIntoTheBBCRMHomePage()
        {
            BBCRMHomePage.Login();
        }

        [Given(@"I have opened the constituent search dialog")]
        public void GivenIHaveOpenedTheConstituentSearchDialog()
        {
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.OpenConstituentSearchDialog();
        }

        [When(@"I search for ""(.&#42;)""")]
        public void WhenISearchFor(string name)
        {
            SearchDialog.SetLastNameToSearch(name);
            SearchDialog.Search();
        }

        [Then(@"The results should contain ""(.&#42;)""")]
        public void ThenTheResultsShouldContain(string result)
        {
            SearchDialog.CheckConstituentSearchResultsContain(result);
        }
    }
}
</code>
</pre>

<ol>
<li>
<p>At the beginning of the file, insert <code>using Blueshirt.Core.Crm</code> to import the <strong><em>Blackbaud CRM</em></strong> types defined by that namespace.</p>
</li>
<li>
<p>Within the class, update the placeholders within the brackets to specify the actions to take. For example, in the first set of brackets, replace <code>ScenarioContext.Current.Pending();</code> with <code>BBCRMHomePage.Login();</code> to specify logging in to the <strong><em>Blackbuad CRM</em></strong> home page.</p>
</li>
</ol>
</li>

<li>
<p>Ensure ChromeDriver is on your path.</p>

<ol>
<li>
<p>Right-click the project in <strong>Solution Explorer</strong> and select <strong>Add</strong>, <strong>Existing Item.</strong></p>
</li>
<li>
<p>On the Add existing item screen, navigate to the ChromeDriver download and click <strong>Add</strong>. The .exe file appears in <strong>Solution Explorer</strong>.</p>
</li>
<li>
<p>To ensure that ChromeDriver is on your path, set its <strong>Copy to Output Directory</strong> property to "Copy always."</p>
</li>
</ol>

<p class="alert alert-danger">While you currently must manually download the latest [ChromeDriver](https://sites.google.com/a/chromium.org/chromedriver/downloads), copy it to your project's directory, and add it as an existing item to your project, we plan to include ChromeDriver in the <em>Blueshirt SpecFlow Plugin</em> NuGet ackage in a future version.</p>
</li>

<li>
<p>Build the project and run your test.</p>

<p>![For example using the Visual Studio Test Explorer](/assets/img/FirstProject/ConstituentSearchResults.PNG)</p>

<ol>
<li>
<p>Right-click the project in <strong>Solution Explorer</strong> and select <strong>Build</strong>.</p>
</l>
<li>
<p>In Test Explorer (<strong>Test</strong>, <strong>Windows</strong>, <strong>Test Explorer</strong>), right-click the test and select <strong>Run Selected Tests</strong>.</p>
</l>

</ol>
</li>

</ol>

<!---
Commenting out the original procedure just in case I need it again. The ordered list replacement seems to work fine, but keeping the old version just to be safe
#### 1. Create a new Unit Test Project.

**File | New | Project | Unit Test Project**

New Project Wizard  
![New Project wizard](/assets/img/FirstProject/NewBSProject.PNG)

#### 2. Add the *Blue SpecFlow Plugin* and *Blue Core Classes* packages to your project.

Right-click your solution in the **Solution Explorer** and select **Manage NuGet Packages for solution.**

Package manager wizard  
![Package manager wizard](/assets/img/FirstProject/AddBSNuGetPackages.PNG)

<p class="alert alert-info">The Blue packages are normally available from the public nuget package repository. If you have standalone nupkg files, you can add their location as a package source in the nuget settings.</p>

#### 3. Add target Environment Urls to your "_app.config_".

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Example appSettings section</div></div><pre><code class="language-csharp">
&lt;appSettings&gt;

  &lt;add key="BBCRMBaseUrl" value="https://blackbaudDemo.com/bbappfx" /&gt;
  &lt;add key="BBCRMHomeUrl" value="/webui/webshellpage.aspx?databasename=BBInfinity" /&gt;
  &lt;add key="Credentials" value="user:password" /&gt;

  ....

&lt;/appSettings&gt;
</code>
</pre>

#### 4. Add a new feature file.

New Item wizard  
![New Item wizard](/assets/img/FirstProject/AddAFeatureFile.PNG)

#### 5. Populate your feature file.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Example Feature File</div></div><pre><code class="language-gherkin">
Feature: Constituent Search
  In order to manage Constituent Records
  As a Blackbaud CRM user
  I want to search existing Constituent records

Scenario: Quick Constituent Search
  Given I have logged into the BBCRM home page
  And I have opened the constituent search dialog
  When I search for "Hampton"
  Then The results should contain "Hampton Street Elementary School"
</code>
</pre>

#### 6. Generate a step file.

Right-click the feature file and select **Generate Step Definitions.**

Step File wizard  
![Step File wizard](/assets/img/FirstProject/GenerateSteps.PNG)

#### 7. Populate the Steps.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Example Steps File</div></div>

<pre><code class="language-csharp">
using System;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Blue_101
{
    [Binding]
    public class ConstituentSearchSteps
    {
        [Given(@"I have logged into the BBCRM home page")]
        public void GivenIHaveLoggedIntoTheBBCRMHomePage()
        {
            BBCRMHomePage.Login();
        }

        [Given(@"I have opened the constituent search dialog")]
        public void GivenIHaveOpenedTheConstituentSearchDialog()
        {
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.OpenConstituentSearchDialog();
        }

        [When(@"I search for ""(.&#42;)""")]
        public void WhenISearchFor(string name)
        {
            SearchDialog.SetLastNameToSearch(name);
            SearchDialog.Search();
        }

        [Then(@"The results should contain ""(.&#42;)""")]
        public void ThenTheResultsShouldContain(string result)
        {
            SearchDialog.CheckConstituentSearchResultsContain(result);
        }
    }
}
</code>
</pre>

#### 8. Ensure ChromeDriver is on your path.

<p class="alert alert-info">One way to ensure this is to add it to your project and set its **Copy to Output Directory** property to **Copy always.**</p>

<p class="alert alert-danger">At the moment, you need to manually download the latest [ChromeDriver](https://sites.google.com/a/chromium.org/chromedriver/downloads), copy it to your project's directory, and add it as an existing item to your project. Future versions of Specflow.Blue intend to have ChromeDriver included in the NuGet package.</p>

<p class="alert alert-danger">If you get a Dictionary Key error for the Driver, then you may need to delete your WebDriver project reference. You then need to create a new WebDriver reference by browsing to "packages\Blackbaud.SpecFlow.Selenium.Blue\[version here\]\lib\net40\WebDriver.dll" and adding this reference. This additional step should be resolved with future versions of ChromeDriver.</p>

#### 9. Build and Run your tests.

For example, use the Visual Studio Test Explorer.  
![For example using the Visual Studio Test Explorer](/assets/img/FirstProject/ConstituentSearchResults.PNG)
-->

### See Also

[NuGet](https://www.nuget.org/)  
[SpecFlow](http://www.specflow.org/)
