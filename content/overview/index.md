---
layout: layout-showcase
name: Overview
order: 20
---

{{ include 'includes/eapwarning/index.md' }}

# Welcome to the {{ stache.config.product_name_short }} Website
The {{ stache.config.product_name_short }} is Blackbaud's solution to enhance and accelerate the automation of User Acceptance Testing (UAT) for Infinity applications such as ***Blackbaud CRM***. The {{ stache.config.product_name_short }} is a set of tools, templates, samples, and documentation that serves as the basis for developing a suite of automated tests, and this website features the documentation and examples thta you need to get you up and running.

## What Is the {{ stache.config.product_name_short }}?
***Blackbaud CRM*** is a powerful tool to manage your constituent relationships and includes a diverse portfolio of features to manage everything from fundraising and marketing to volunteers and memberships. This extensive functionality provides you with building blocks that are critical to achieve your organization's mission, but all that functionality can also make User Acceptance Testing (UAT) for upgrades very complicated. ***Blackbaud CRM*** customers manually run UAT tests as part of new implementations, service pack releases, and version upgrades, and they can spend a great deal of time testing because of the manual effort involved.

To address this issue, Blackbaud created the {{ stache.config.product_name_short }} to assist with the automation of UAT tests. The automation tool is our response to requests for assistance with UAT testing, and it facilitates the automation of UAT tests for both out-of-the-box features and customizations. You can use the {{ stache.config.product_name_short }} to help reduce your testing time for upgrades, gain confidence in those upgrades, and save time with initial pilot tests. The documentation and examples on this website will get you up and running.

<p class="alert alert-info">The {{ stache.config.product_name_short }} was created with open source technologies that are part of the public domain. While you can use it to create automated test suites for ***Blackbaud CRM***, it does not include any Blackbaud-maintained tools and is not actively supported by Blackbaud.</p>

## What Does the {{ stache.config.product_name_short }} Include?
The {{ stache.config.product_name_short }} provides an API that lets users take advantage of Selenium WebDriver to interact with common elements in Infinity-based applications such as ***Blackbaud CRM***. Tests can be run against the NUnit open-source test runner or within ***Visual Studio*** and integrated with TFS.

Blackbaud provides NuGet packages for a Custom Specflow Generator and core API, and the automation tool's features include:
* A SpecFlow NuGet package that generates the code to drive automated tests with Selenium WebDriver SpecFlow 
* A core API library NuGet package to provide access to methods that can be used to test both out-of-the-box and custom features in ***Blackbaud CRM*** and other Infinity applications
* Documentation on how to get started and how to write customized tests
* Example projects
* API documentation

## What Tools Does the {{ stache.config.product_name_short }} Use?
The {{ stache.config.product_name_short }} requires minimal setup by configuring some settings and installing NuGet packages for a Custom Specflow Generator and core API so that you can create customer-developed and customer-maintained test suites. It relies on open standards and free tools, and it provides technical solutions and examples to help automate testing. It does not dictate practices on the management of data, tests, or environments.

The {{ stache.config.product_name_short }} uses:
* ChromeDriver and Xpath
* Gherkin
* NUnit and MSTest
* NuGet packages

In addition, since the {{ stache.config.product_name_short }} requires a fair amount of technical knowledge, Blackbaud assumes that developers have the following skills before they use it:
* Familiarity with ***Visual Studio*** to create, manage, and build test projects.
* Familiarity with C#. The out-of-the-box features with the {{ stache.config.product_name_short }} set developers up to work in C# in their ***Visual Studio*** projects.
* Familiarity with Gherkin to author feature files for behavior-driven development (BDD) testing.
* Familiarity with SpecFlow to generate and populate step files and to pass variables to .NET step methods.
* Familiarity with XPath to select elements in the browser. XPath is especially important to create tests for customizations because the {{ stache.config.product_name_short }} relies on XPaths to parse HTML elements and find desired elements, so users who plan to create UAT suites for customizations will need to be particularly familiar with XPath.
