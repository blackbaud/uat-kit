---
layout: layout-sidebar
name: Setting Custom Fields
description: In this walkthrough you'll gain experience setting a custom field on a dialog and overriding a default implementations.
order: 40
---

# SpecFlow's Table and TableRow Guidelines

<p class="alert alert-warning">Warning: This is preliminary documentation and is subject to change</p>

<p class="alert alert-warning">Examples follow customizations that you likely will not have on your application. Either follow along against your own customizations and modify steps accordingly, or follow the screenshots below.</p>

In this walkthrough you'll gain experience setting a custom field on a dialog and overriding default implementations.

## Prerequisites

* You have completed the [Using the Selenium WebDriver]({{stache.config.blue_walkthroughs_201_selenium}}) walkthrough and have access to an Enterprise CRM application.
* You have completed the [SpecFlow's Table and TableRow Guidelines]({{stache.config.blue_walkthroughs_201_specflow}}) walkthrough.
* You are comfortable adding tests and step implementations to existing feature and step files.
* You are comfortable accessing the existing UAT SDK (Project Blue) Core API.
* You are comfortable modifying the app.config to change which application the tests run against.
* You are comfortable identifying the unique attribute values for the XPath constructors in the Core API and have completed the [XPath Guidelines]({{stache.config.blue_walkthroughs_201_xpaths}}) walkthrough.

## Adding Support For Custom Fields - Overload Approach

#### Identify Need for Custom Support

This is the Enterprise CRM standard "Add an individual" dialog.

![TODO](/assets/img/CustomFields/OriginalAddIndividual.PNG)

Here is a customized "Add an individual" dialog containing new fields.

![TODO](/assets/img/CustomFields/CustomAddIndividual.PNG)

<p class="alert alert-info">Notice that there are also custom required fields as well. We need to consider that when setting the fields for adding an individual on this application.</p>

In our test project, if we create a scenario outlined and implemented like this...

<pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Add an individual on a dialog containing a custom field.
	Given I have logged into BBCRM
	When I add constituent
	| Last name | First name | Title | Nickname  | Information source | Country of Origin | Matriculation Year (Use) |
	| Parker    | Peter      | Mr    | Spiderman | Friend             | United States     | 2014                     |
	Then a constituent is created
</code>
</pre>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Core API used to implement steps.</div></div><pre><code class="language-csharp">
using System;
using Blueshirt.Core.Base;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    [Binding]
    public class SampleTestsSteps : BaseSteps
    {

        [Given(@"I have logged into BBCRM")]
        public void GivenIHaveLoggedIntoBBCRM()
        {
            BBCRMHomePage.Logon();
        }

        [When(@"I add constituent")]
        public void WhenIAddConstituent(Table constituents)
        {
            foreach (var constituent in constituents.Rows)
            {
                BBCRMHomePage.OpenConstituentsFA();
                ConstituentsFunctionalArea.AddAnIndividual(groupCaption: "Add Records");
                IndividualDialog.SetIndividualFields(constituent);
                IndividualDialog.Save();
            }
        }

        [Then(@"a constituent is created")]
        public void ThenAConstituentIsCreated()
        {
            if (!BaseComponent.Exists(Panel.getXPanelHeader("individual"))) FailTest("A constituent panel did not load.");
        }

    }
}
</code>
</pre>

<p class="alert alert-info">The application with the custom field also had a custom group caption for the "Add an individual" task. This is why the ConstituentsFunctionalArea.AddAnIndividual() call overloads the 'groupCaption' parameter.</p>

...and run the test against the application with the new custom field, then we get an error indicating a need to add custom support for the new field.

![TODO](/assets/img/CustomFields/NotImplemetedFieldAddIndividual.PNG)

#### Create Class Inheriting Core Class

To resolve this failure, we need to add support for the additional custom fields. Create a new class in your project.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Custom Individual Dialog Class Inheriting IndividualDialog</div></div><pre><code class="language-csharp">
using Blueshirt.Core.Crm;

namespace Delving_Deeper
{
    public class CustomIndividualDialog : IndividualDialog
    {

    }
}
</code>
</pre>

#### Create Custom Supported Fields Mapping

We need to map the custom field captions to their relevant XPath and Field Setter values.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Custom Individual Dialog Class with mapped custom fields.</div></div><pre><code class="language-csharp">
using System.Collections.Generic;
using Blueshirt.Core.Base;
using Blueshirt.Core.Crm;

namespace Delving_Deeper
{
    public class CustomIndividualDialog : IndividualDialog
    {
        private static readonly IDictionary&lt;string, CrmField&gt; CustomSupportedFields = new Dictionary&lt;string, CrmField&gt;
        {
            {"Country of Origin", new CrmField("_ATTRIBUTECATEGORYVALUE0_value", FieldType.Dropdown)},
            {"Matriculation Year (Use)", new CrmField("_ATTRIBUTECATEGORYVALUE1_value", FieldType.Dropdown)}
        };
    }
}
</code>
</pre>

<p class="alert alert-warning">You should be comfortable understanding how the unique id attributes for the fields were gathered from the UI. These values are used for the XPath constructors that locate and interact with the fields. Review the [XPath Guidelines]({{stache.config.blue_walkthroughs_201_xpaths}}) if you do not follow where the values "_ATTRIBUTECATEGORYVALUE0_value" and "_ATTRIBUTECATEGORYVALUE1_value" come from.</p>

#### Pass Custom Supported Fields To Base.

The custom class uses its inherited IndividualDialog values to pass the required values to the Dialog's SetFields() method. SetFields has an overload that takes in a second IDictionary mapping of field captions to CrmFields. We can pass our dictionary of custom fields to add additional support for custom fields.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Custom SetIndividualFields()</div></div><pre><code class="language-csharp">
public new static void SetIndividualFields(TableRow fields)
{
    SetFields(GetDialogId(DialogIds), fields, SupportedFields, CustomSupportedFields);
}
</code>
</pre>

<p class="alert alert-info">If a mapping exists in our CustomSupportedFields where the string key is an already existing key for SupportedFields, the mapped values for CustomSupportedFields is used.</p>

#### Modify Your Step Implementation.

Modify your step definition to use the new CustomIndividualDIalog's SetIndividualFields() method.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Modified Step Implementation</div></div><pre><code class="language-csharp">
[When(@"I add constituent")]
public void WhenIAddConstituent(Table constituents)
{
    foreach (var constituent in constituents.Rows)
    {
        BBCRMHomePage.OpenConstituentsFA();
        constituent["Last name"] += uniqueStamp;
        ConstituentsFunctionalArea.AddAnIndividual(groupCaption: "Add Records");
        CustomIndividualDialog.SetIndividualFields(constituent);
        IndividualDialog.Save();
    }
}
</code>
</pre>

<p class="alert alert-warning">Depending on your application, the Save step may cause a duplicate entry or error dialog to appear in the application. If this occurs, we advise adding a unique stamp to the last name of your constituent's values as shown above.</p>

The test passes now!

![TODO](/assets/img/CustomFields/PassingTest.PNG)

## Adding Support For Custom Fields - Custom Method Approach

#### Alternative Gherkin Syntax

An alternative Gherkin approach that drives a need for an entirely custom method.

<pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Add an individual on a dialog containing a custom field using a custom method
	Given I have logged into BBCRM
	When I start to add a constituent
	| Last name | First name | Title | Nickname  | Information source |
	| Parker    | Peter      | Mr    | Spiderman | Friend             |
	And I set the custom constituent field "Country of Origin" to "Argentina"
	And I set the custom constituent field "Matriculation Year (Use)" to "2012"
	And I save the add an individual dialog
	Then a constituent is created
</code>
</pre>

#### Add Method to Custom Class

In this approach we describe setting a single field's value for a step. Add the following method to your CustomIndividualDialog class.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Custom method.</div></div><pre><code class="language-csharp">
public static void SetCustomField(string fieldCaption, string value)
{
    //Use the same IDictionary&lt;string, CrmField&gt; CustomSupportedFields from the Overload Approach
    SetField(GetDialogId(DialogIds), fieldCaption, value, CustomSupportedFields);
}
</code>
</pre>

<p class="alert alert-info">Notice how the custom method did not need the "new" attribute in the method declaration. "new" is only needed when overriding an inherited method.</p>

#### Implement The New Step Methods

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Implemetation of new steps.</div></div><pre><code class="language-csharp">
[When(@"I start to add a constituent")]
public void WhenIStartToAddAConstituent(Table constituents)
{
    foreach (var constituent in constituents.Rows)
    {
        BBCRMHomePage.OpenConstituentsFA();
        constituent["Last name"] += uniqueStamp;
        ConstituentsFunctionalArea.AddAnIndividual(groupCaption: "Add Records");
        IndividualDialog.SetIndividualFields(constituent);
    }
}

[When(@"I set the custom constituent field ""(.&#42;)"" to ""(.&#42;)""")]
public void WhenISetTheCustomConstituentFieldTo(string fieldCaption, string value)
{
    CustomIndividualDialog.SetCustomField(fieldCaption, value);
}

[When(@"I save the add an individual dialog")]
public void WhenISaveTheAddAnIndividualDialog()
{
    IndividualDialog.Save();
}
</code>
</pre>

The test passes now!

![TODO](/assets/img/CustomFields/2ndPassingTest.PNG)

<p class="alert alert-info">There are many ways to use the UAT SDK (Project Blue) API in order to achieve the same result. Above are two potential implementations to handle a dialog with a custom field, but these are not the only approaches. The methods and their underlying logic are totally defined by the user. You are free to create whatever helper methods you see fit. Look into the API documentation and see if you can come up with a different solution.</p>

## Overloading An Implementation

#### Identify Need To Overload Implementation

Let's start with the following test case.

<pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Set the Last/Org/Group name field in a constituent search dialog
	Given I have logged into BBCRM
	When I open the constituent search dialog
	And set the Last/Org/Group name field value to "Smith"
	Then the Last/Org/Group name field is "Smith"
</code>
</pre>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Implemetation of steps.</div></div><pre><code class="language-csharp">
[When(@"I open the constituent search dialog")]
public void WhenIOpenTheConstituentSearchDialog()
{
    BBCRMHomePage.OpenConstituentsFA();
    FunctionalArea.OpenLink("Constituents", "Constituent search");
}

[When(@"set the Last/Org/Group name field value to ""(.&#42;)""")]
public void WhenSetTheLastOrgGroupNameFieldValueTo(string fieldValue)
{
    SearchDialog.SetTextField(Dialog.getXInput("ConstituentSearchbyNameorLookupID", "KEYNAME"), fieldValue);
}

[Then(@"the Last/Org/Group name field is ""(.&#42;)""")]
public void ThenTheLastOrgGroupNameFieldIs(string expectedValue)
{
    SearchDialog.ElementValueIsSet(Dialog.getXInput("ConstituentSearchbyNameorLookupID", "KEYNAME"), expectedValue);
}
</code>
</pre>

When run against the standard CRM application, the test passes.

![TODO](/assets/img/CustomFields/PassingDefaultConstituentSearch.PNG)

The steps navigate to the Constituents functional area and click the "Constituent search" task.

![TODO](/assets/img/CustomFields/DefaultConstituentSearchTask.PNG)

The "Last/Org/Group name" field is set and validated as containing the desired value.

![TODO](/assets/img/CustomFields/DefaultConstituentSearchDialog.PNG)

If we run the test against a custom application whose Constituent functional area looks like this...

![TODO](/assets/img/CustomFields/CustomConstituentSearchTask.PNG)

...we get the following error.

![TODO](/assets/img/CustomFields/DefaultOnCustomSearchDialogError.PNG)

This is resolved with the following code edit.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited step</div></div><pre><code class="language-csharp">
[When(@"I open the constituent search dialog")]
public void WhenIOpenTheConstituentSearchDialog()
{
    BBCRMHomePage.OpenConstituentsFA();
    FunctionalArea.OpenLink("Searching", "Constituent search");
}
</code>
</pre>

Running the test now we get a new error.

![TODO](/assets/img/CustomFields/DefaultOnCustomSearchDialogFieldError.PNG)

Another must customization exist. The error stack trace indicates that the XPath constructor for the "Last/Org/Group name" field is not compatible with this application. NoSuchElementExceptions are thrown when Selenium's WebDriver times out looking for a web element using the XPath.

![TODO](/assets/img/CustomFields/CustomConstituentSearchDialog.PNG)

#### Identify The Customization

Let's take a look at the search dialogs between the default and custom applications. Comparing the dialogs, clearly the dialog on the right has been customized. Inspecting the "Last/Org/Group name" field between the two applications, we can see they share the same unique field id.

![TODO](/assets/img/CustomFields/ComparingFieldIdSearchDialog.PNG)

<p class="alert alert-info">If you do not know how to identify the field's unique id, please review the [XPath Guidelines]({{stache.config.blue_walkthroughs_201_xpaths}})</p>

Inspecting the unique dialog ids, we can see that they are different. The supported XPath constructs an XPath using the dialog id "ConstituentSearchbyNameorLookupID". We need to modify the dialog id to use the custom dialog id.

![TODO](/assets/img/CustomFields/ComparingDialogIdsSearchDialog.PNG)

#### Edit Steps

Update the step code so the XPath constructors use the custom dialog id.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited steps for custom dialog id</div></div><pre><code class="language-csharp">
[When(@"set the Last/Org/Group name field value to ""(.&#42;)""")]
public void WhenSetTheLastOrgGroupNameFieldValueTo(string fieldValue)
{
    SearchDialog.SetTextField(Dialog.getXInput("UniversityofOxfordConstituentSearch", "KEYNAME"), fieldValue);
}

[Then(@"the Last/Org/Group name field is ""(.&#42;)""")]
public void ThenTheLastOrgGroupNameFieldIs(string expectedValue)
{
    SearchDialog.ElementValueIsSet(Dialog.getXInput("UniversityofOxfordConstituentSearch", "KEYNAME"), expectedValue);
}
</code>
</pre>

The test passes now on the custom application.

![TODO](/assets/img/CustomFields/PassingDefaultConstituentSearch.PNG)

## Overriding An Implementation

#### Identify Need For Overriding Implementation

Let's start with the following test case that works against the standard CRM application.

<pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Add an individual and set the related individual through the constituent search list
	Given I have logged into BBCRM
	And a constituent exists
	| Last name | First name |
	| LeBouf    | Shia       |
	When I start to add a constituent
    | Last name | First name |
    | Prime     | Optimus    |
	And set the household fields
	| Related individual | Individual is the | Related individual is the |
	| LeBouf             | Co-worker         | Co-worker                 |
	And I save the add an individual dialog
	Then a constituent is created
</code>
</pre>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Implemetation of steps.</div></div><pre><code class="language-csharp">
using System;
using Blueshirt.Core.Base;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    [Binding]
    public class SampleTestsSteps : BaseSteps
    {
        [Given(@"I have logged into BBCRM")]
        public void GivenIHaveLoggedIntoBBCRM()
        {
            BBCRMHomePage.Logon();
        }

        [Then(@"a constituent is created")]
        public void ThenAConstituentIsCreated()
        {
            if (!BaseComponent.Exists(Panel.getXPanelHeader("individual"))) FailTest("A constituent panel did not load.");
        }

        [When(@"I start to add a constituent")]
        public void WhenIStartToAddAConstituent(Table constituents)
        {
            foreach (var constituent in constituents.Rows)
            {
                BBCRMHomePage.OpenConstituentsFA();
                constituent["Last name"] += uniqueStamp;
                ConstituentsFunctionalArea.AddAnIndividual();
                IndividualDialog.SetIndividualFields(constituent);
            }
        }

        [When(@"I save the add an individual dialog")]
        public void WhenISaveTheAddAnIndividualDialog()
        {
            IndividualDialog.Save();
        }

        [Given(@"a constituent exists")]
        public void GivenAConstituentExists(Table constituents)
        {
            foreach (var constituent in constituents.Rows)
            {
                BBCRMHomePage.OpenConstituentsFA();
                constituent["Last name"] += uniqueStamp;
                ConstituentsFunctionalArea.AddAnIndividual(constituent);
            }
        }

        [When(@"set the household fields")]
        public void WhenSetTheHouseholdFields(Table fieldsTable)
        {
            foreach (var fieldValues in fieldsTable.Rows)
            {
                IndividualDialog.SetHouseholdFields(fieldValues);
            }
        }
    }
}
</code>
</pre>

At some point in the test, the 'Related individual' field on the Add an individual dialog is set by using the associated searchlist.

![TODO](/assets/img/CustomFields/OverrideProcedure/RecordSearch.PNG)

What if we wanted to set the field through the add button? This would require us to override the default implementation for how the 'Related individual' field is set.

![TODO](/assets/img/CustomFields/OverrideProcedure/AddIcon.PNG)

#### Create A Custom Method

If you do not have a CustomIndividualDialog class created yet, add a new class to your project and implement it as follows.  First we make sure to select the 'Household' tab.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Selecting the right tab</div></div><pre><code class="language-csharp">
using System.Collections.Generic;
using Blueshirt.Core.Base;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    public class CustomIndividualDialog : IndividualDialog
    {
        public new static void SetHouseholdFields(TableRow fields)
        {
            OpenTab("Household");
        }
    }
}
</code>
</pre>

Next we do specify custom logic if a value for the 'Related individual' field has been provided. If a value has been provided for this field, we click the button that brings up the add dialog. Be sure to read the API documentation for the XPath constructors.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Click the add button for the field</div></div><pre><code class="language-csharp">
public new static void SetHouseholdFields(TableRow fields)
{
    OpenTab("Household");
    if (fields.ContainsKey("Related individual"))
    {
        WaitClick(getXInputNewFormTrigger(getXInput(GetDialogId(DialogIds), "_SPOUSEID_value")));
    }
}
</code>
</pre>

The resuling dialog from clicking the add button on the 'Related individual' field.

![TODO](/assets/img/CustomFields/OverrideProcedure/TriggerDialog.PNG)

We then set the 'Last name' field value to the value provided for 'Related individual' before hitting Ok. We could have defined any logic and interactions involving this dialog, but let's keep it simple.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Set the 'Last name' field</div></div><pre><code class="language-csharp">
public new static void SetHouseholdFields(TableRow fields)
{
    OpenTab("Household");
    if (fields.ContainsKey("Related individual"))
    {
        WaitClick(getXInputNewFormTrigger(getXInput(GetDialogId(DialogIds), "_SPOUSEID_value")));
        SetTextField(getXInput("IndividualSpouseBusinessSpouseForm", "_SPOUSE_LASTNAME_value"), fields["Related individual"]);
        OK();
    }
}
</code>
</pre>

Before we call the base implementation to handle setting the rest of the fields, we set fields["Related individual"] to equal null. We do this because we want the base SetHouseholdFields to skip it's handling of the 'Related individual' field.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Set 'Related individual' to null and çall the base method.</div></div><pre><code class="language-csharp">
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    public class CustomIndividualDialog : IndividualDialog
    {
        public new static void SetHouseholdFields(TableRow fields)
        {
            OpenTab("Household");
            if (fields.ContainsKey("Related individual"))
            {
                WaitClick(getXInputNewFormTrigger(getXInput(dialogId, "_SPOUSEID_value")));
                SetTextField(getXInput("IndividualSpouseBusinessSpouseForm", "_SPOUSE_LASTNAME_value"), fields["Related individual"]);
                OK();
                fields["Related individual"] = null;
            }
            IndividualDialog.SetHouseholdFields(fields);
        }
    }
}
</code>
</pre>

Another solution would have been to remove the 'Related individual' key from the fields object.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Remove the 'Related individual' key</div></div><pre><code class="language-csharp">
fields.Keys.Remove("Related individual");
</code>
</pre>

#### Update The Steps

Change the step setting the household tab fields.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Updated step</div></div><pre><code class="language-csharp">
[When(@"set the household fields")]
public void WhenSetTheHouseholdFields(Table fieldsTable)
{
    foreach (var fieldValues in fieldsTable.Rows)
    {
        CustomIndividualDialog.SetHouseholdFields(fieldValues);
    }
}
</code>
</pre>

The test now sets the 'Related individual' field through the add button and not the search dialog.

## See Also

[SpecFlow's Table and TableRow Guidelines]({{stache.config.blue_walkthroughs_201_specflow}})
