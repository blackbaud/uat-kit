##UAT Automation Kit##
The UAT Automation Kit is a set of tools, templates, samples, and documentation to assist with the automation of User Acceptance Testing for Blackbaud CRM and other Infinity applications.
 
The UAT Kit was developed in response to requests for assistance with user acceptance testing for Blackbaud CRM, and you can use it to develop a suite of automated tests for new implementations, version upgrades, and service packs. It facilitates the automation of UAT tests for both out-of-the-box features and customizations.
 
For documentation and examples to get up and running with the UAT Kit, see the [UAT Kit Automation website](http://uat-kit-docs.blackbaudhosting.com).
 
##Open Source##
We have exposed the source code for the UAT Automation Kit in this GitHub repo. This allows end users to see our patterns and examine how the UAT Kit interact with Selenium WebDriver and uses it to interact with common elements in Infinity-based applications.
 
By making the UAT Kit open source, Blackbaud allows you to extend it as necessary to meet your particular needs for UAT testing.
 
##Contributing##
We are not accepting third-party contributions to this project at this time.

If we change this policy in the future, there are a few guidelines that we need contributors to follow. For more information, see our [canonical contributing guide](https://github.com/blackbaud-community/Blackbaud-CRM/blob/master/CONTRIBUTING.md) in the Blackbaud CRM repo which provided detailed instructions, including signing the [Contributor License Agreement](http://developer.blackbaud.com/cla/).

Version 1.0.1304.1 introduces some breaking namespace changes. Where previously this would suffice:

using Blackbaud.UAT.Core.Crm;

You will now have to optionally included:

using Blackbaud.UAT.Core.Crm.Panels;
using Blackbaud.UAT.Core.Crm.Dialogs;