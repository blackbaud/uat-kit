---
layout: layout-sidebar
name: Quick Start
description: Learn to quickly create a Blackbaud CRM GUI test suite using the Blackbaud UAT SDK.
order: 10
---

# Quick Start

<p class="alert alert-warning">Warning: This is preliminary documentation and is subject to change</p>

Learn to quickly create a Blackbaud CRM GUI test suite using the Blackbaud UAT SDK.

## Prerequisites

* Visual Studio 2013 Community
* NuGet Plugin
* SpecFlow Pugin

<p class="alert alert-info">You can find details on how to install these at [Visual Studio](https://www.visualstudio.com/en-us/products/free-developer-offers-vs.aspx) and at [NuGet](https://www.nuget.org/) and at [SpecFlow](http://www.specflow.org/)</p>

## Creating a Blue Test Project.

#### 1. Create a new Unit Test Project.

**File | New | Project | Unit Test Project**

New Project Wizard  
![New Project wizard](/assets/img/FirstProject/NewBSProject.PNG)

#### 2. Add the *Blue SpecFlow Plugin* and *Blue Core Classes* packages to your project.

Right-click on your solution in the solution explorer and choose **Manage NuGet Packages for solution**

Package manager wizard  
![Package manager wizard](/assets/img/FirstProject/AddBSNuGetPackages.PNG)

<p class="alert alert-info">The Blue packages will normally be available from the public nuget package repository. If you have stand alone nupkg files you can add their location as a package source in the nuget settings.</p>

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

Right-Click on the feature file and choose **Generate Step Definitions**

Step File wizard  
![Step File wizard](/assets/img/FirstProject/GenerateSteps.PNG)

#### 7. Populate the Steps.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Example Steps File</div></div><pre><code class="language-csharp">
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

<p class="alert alert-info">Adding it to your project and setting its **Copy to Output Directory** property to **Copy always** is one way to ensure this.</p>

<p class="alert alert-danger">At the moment you will need to manually download the latest [ChromeDriver](https://sites.google.com/a/chromium.org/chromedriver/downloads), copy it to your project's directory, and add it as an existing item to your project.  Future versions of Specflow.Blue intend to have ChromeDriver included in the NuGet package.</p>

<p class="alert alert-danger">If you are getting a Dictionary Key error for the Driver, then you may need to delete your WebDriver project reference.  You will then need to create a new WebDriver reference by browsing to "packages\Blackbaud.SpecFlow.Selenium.Blue\[version here\]\lib\net40\WebDriver.dll" and adding this reference.  This additional step should be resolved with future versions of ChromeDriver.</p>

#### 9. Build and Run your tests.

For example using the Visual Studio Test Explorer  
![For example using the Visual Studio Test Explorer](/assets/img/FirstProject/ConstituentSearchResults.PNG)

## See Also

[NuGet](https://www.nuget.org/)  
[SpecFlow](http://www.specflow.org/)

