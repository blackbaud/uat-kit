---
layout: layout-sidebar
name: Get Started
description: Learn how to create a Blackbaud CRM GUI test suite using the UAT Automation Kit.
order: 10
---

<p class="alert alert-warning"><strong><em>Warning:</em></strong> This is preliminary documentation and is subject to change.</p>

# Get Started with {{ stache.config.product_name_short }}
This tutorial walks you through the process of creating a test project with the {{ stache.config.product_name_short }}. After you master the basics, you can use the {{ stache.config.product_name_short }} to start creating your a suite of automated tests to facilitate your User Acceptance Testing for ***Blackbaud CRM*** upgrades.

<p class="alert alert-info">This tutorial assumes familiarity with ***Visual Studio***, Gherkin, SpecFlow, WebDriver, and XPath.</p>

## Prerequisites
Before you create your first test project and install the NuGet packages for the {{ stache.config.product_name_short }}, you must have a hanful of prerequisites in place:

* Install ***Visual Studio 2013*** or later. You can install the free ***Visual Studio 2013 Community*** edition, or you can use a more advanced edition. See [Microsoft's installation instructions](https://www.visualstudio.com/en-us/products/free-developer-offers-vs.aspx) to install the free version.
* Make sure the ***Visual Studio*** NuGet extension is in place. Starting with ***Visual Studio 2012***, NuGet is included in every edition (except Team Foundation Server) by default, and updates to NuGet can be found through the Extension Manager. For more information about NuGet, see [https://www.nuget.org/](https://www.nuget.org/).
* Install the IDE integration for the ***Visual Studio*** SpecFlow extension. For more information about SpecFlow, see [SpecFlow](http://www.specflow.org/).
* Install the Chromedriver standalone server that implements WebDriver's wire protocol for Chromium. For more information about ChromeDriver, see [https://sites.google.com/a/chromium.org/chromedriver/downloads](https://sites.google.com/a/chromium.org/chromedriver/downloads).

## Create a Test Project
<ol>
<li>
<p>In ***Visual Studio***, select <strong>File</strong>, <strong>New</strong>, <strong>Project</strong>, and then select the <strong>Test</strong> category and the <strong>Unit Test Project</strong> template. Edit the project name, location, and solution name as necessary.</p>
<p>![New Project wizard](/assets/img/FirstProject/NewBSProject.PNG)</p>
<p class="alert alert-info">The {{ stache.config.product_name_short }} takes advantage of the <strong>Unit Test Project</strong> template to create a test project, but it is a system test, not a unit test.</p>
</li>

<li>
<p>Add the <strong>Blueshirt SpecFlow Plugin</strong> and <strong>Blueshirt Core Classes</strong> packages to your project.</p>
<p>![Package manager wizard](/assets/img/FirstProject/AddBSNuGetPackages.PNG)</p>

<ol>
<li>
<p>Right-click the solution in <strong>Solution Explorer</strong> and select <strong>Manage NuGet Packages for Solution.</strong></p>
</li>
<li>
<p>On the Manage NuGet Packages screen, select the <strong>BB NuGet</strong> category and search for "Blueshirt."</p>
<p class="alert alert-info">For release, {{ stache.config.product_name_short }} packages will be available from the public NuGet package repository in the nuget.org category. ... If Blackbaud provides standalone nupkg files instead, then users can add their location as a package source in the NuGet settings.</p>
</li>
<li>
<p>Select <strong>Blueshirt SpecFlow Plugin</strong> and click <strong>Install</strong>. Then with your unit test project and solution selected, click <strong>OK</strong> on the Select projects screen.</p>
</li>
<li>
<p>Select <strong>Blueshirt Core Classes</strong> and click <strong>Install</strong>. Then with your unit test project and solution selected, click <strong>OK</strong> on the Select projects screen.</p>
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

<p>From <strong>Solution Explorer</strong>, open the App.config file and scroll down to the appSettings elemnt.</p>

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
<p>You must use <strong><em>Blackbaud CRM</em></strong> credentials, not Windows authentication credentials. If you do not want to enter credentials here for a test, you can exclude the third add element and manually log in to <strong><em>Blackbaud CRM</em></strong> before you run the test.</p>
<p class="alert alert-warning"><strong><em>Warning:</em></strong> For security reasons, you should not use credentials for your production environment. The {{ stache.config.product_name_short }} is intended for test or staging environments.</p>
</li>
</ul>
</li>

<li>
<p>Add a new feature file.</p>

<p>![New Item wizard](/assets/img/FirstProject/AddAFeatureFile.PNG)</p>

<ol>
<li>
<p>Right-click the project in <strong>Solution Explorer</strong> and select <strong>Add</strong>, <strong>New item</strong>.</p>
</li>
<li>
<p>Select the <strong>SpecFlow Feature File</strong> template and edit the file name as necessary, then click <strong>Add</strong>.</p> 
</li>
</ol>
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

<ol>
<li>
<p>Right-click within the feature file and select <strong>Generate Step Definitions</strong>. The Generate Step Definition Skeleton screen appears.</p>
</li>
<li>
<p>Click <strong>Generate</strong>, and then on the Select target step definition class file screen, make sure the path points to your test project and click <strong>Save</strong>. The step file appears in <strong>Solution Explorer</strong>.</p>
</li>
</ol>
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
<p>Within the class, update placeholders within the brackets to specify the actions to take.</p>
<p>For example, in the first set of brackets, replace <code>ScenarioContext.Current.Pending();</code> with <code>BBCRMHomePage.Login();</code> to specify logging in to the <strong><em>Blackbuad CRM</em></strong> home page.</p>
</li>
</ol>
</li>

<li>
<p>Ensure ChromeDriver is on your path.</p>

<ol>
<li>
<p>Right-click the project in <strong>Solution Explorer</strong> and select <strong>Add</strong>, <strong>Existing Item.</strong></p>
</l>
<li>
<p>On the Add existing item screen, navigate to ChromeDriver download and click <strong>Add</strong>. The .exe file appears in <strong>Solution Explorer</strong>.</p>
<p class="alert alert-info">To ensure that ChromeDriver is on your path, set its <strong>Copy to Output Directory</strong> property to "Copy always."</p>
</l>
</ol>

<p class="alert alert-danger">While you currently must manually download the latest [ChromeDriver](https://sites.google.com/a/chromium.org/chromedriver/downloads), copy it to your project's directory, and add it as an existing item to your project, we plan to include ChromeDriver in the <em>Blueshirt SpecFlow Plugin</em> NuGet ackage in a future version.</p>
</li>

<li>
<p>Build and run your tests.</p>

<p>![For example using the Visual Studio Test Explorer](/assets/img/FirstProject/ConstituentSearchResults.PNG)</p>

<ol>
<li>
<p>Right-click the project in <strong>Solution Explorer</strong> and select <strong>Build</strong>.</p>
</l>
<li>
<p>Open Test Explorer (<strong>Test</strong>, <strong>Windows</strong>, <strong>Test Explorer</strong>), and select <strong>Run, Not Run Tests</strong>.</p>
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
