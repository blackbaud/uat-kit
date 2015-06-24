---
layout: layout-sidebar
name: Set Custom Fields
description: In this walkthrough you'll gain experience setting a custom field on a dialog and overriding default implementations.
order: 50
---

<p class="alert alert-warning"><strong><em>Warning:</em></strong> This is preliminary documentation and is subject to change.</p>

# Set Custom Fields
In this walkthrough, you'll gain experience setting a custom field on a dialog and overriding default implementations.

<p class="alert alert-warning">The examples in this walkthrough follow customizations that you likely do not have in your ***Blackbaud CRM*** instance. You can follow along with your own customizations and modify the steps accordingly.</p>

## Prerequisites

* Completion of the [Using the Selenium WebDriver]({{stache.config.blue_walkthroughs_selenium}}) walkthrough.
* Access to a ***Blackbaud CRM*** instance to test against.
* Completion of the [SpecFlow's Table and TableRow Guidelines]({{stache.config.blue_walkthroughs_specflow}}) walkthrough.
* Familiarity with adding tests and step implementations to existing feature and step files.
* Familiarity with accessing the existing {{ stache.config.product_name_short }} Core API.
* Familiarity with modifying the App.config to change which application the tests run against.
* Familiarity with  identifying the unique attribute values for the XPath constructors in the Core API and completion of the [XPath Guidelines]({{stache.config.blue_walkthroughs_xpaths}}) walkthrough.

## Add Support For Custom Fields - Overload Approach

<ol>
<li>
<p>Identify the need for custom support. For example, this is the standard ***Blackbaud CRM*** Add an individual screen.</p>

<p>![TODO](/assets/img/CustomFields/OriginalAddIndividual.PNG)</p>

<p>And this is a customized version of the Add an individual screen that includes additional fields.</p>

<p>![TODO](/assets/img/CustomFields/CustomAddIndividual.PNG)</p>

<p class="alert alert-info">The custom fields are required. We need to keep this in mind when we add individuals in ***Blackbaud CRM***.</p>

<p>In our test project, we can create a feature file to create a behavior-driven development test with Gherkin.</p>

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

<p>Then we can create a step file and populate its steps.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Core API to implement steps</div></div><pre><code class="language-csharp">
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

<p class="alert alert-info">In addition to the custom field, the customization also includes a custom group caption for the Add an individual task. This is why the <code>ConstituentsFunctionalArea.AddAnIndividual()</code> call overloads the <code>groupCaption</code> parameter.</p>

<p>When we run the test against the customization and its custom field, an error indicates that we need custom support for the new field.</p>

<p>![TODO](/assets/img/CustomFields/NotImplemetedFieldAddIndividual.PNG)</p>
</li>

<li>
<p>Create a class that inherits a core class. To resolve the error in the previous step, we create a class in the project to add support for the additional custom fields.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Custom Individual Dialog Class That Inherits IndividualDialog</div></div>
<pre><code class="language-csharp">
using Blueshirt.Core.Crm;

namespace Delving_Deeper
{
    public class CustomIndividualDialog : IndividualDialog
    {

    }
}
</code>
</pre>
</li>

<li>
<p>Create custom-supported field mapping. Map the custom field captions to their relevant XPath and Field Setter values.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Custom Individual Dialog Class with mapped custom fields</div></div
><pre><code class="language-csharp">
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

<p class="alert alert-warning">You should understand how the unique <code>id</code> attributes for the fields were gathered from the UI. These values are used for the XPath constructors that locate and interact with the fields. If you do not understand where the values <code>_ATTRIBUTECATEGORYVALUE0_value</code> and <code>_ATTRIBUTECATEGORYVALUE1_value</code> come from, review the [XPath Guidelines]({{stache.config.blue_walkthroughs_xpaths}}).</p>
</li>

<li>
<p>Pass custom-supported fields To the base. The custom class uses its inherited <code>IndividualDialog</code> values to pass the required values to the dialog's <code>SetFields()</code> method. <code>SetFields</code> has an overload that takes in a second <code>IDictionary</code> mapping of field captions to <code>CrmFields</code>. We can pass our dictionary of custom fields to add additional support for custom fields.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Custom SetIndividualFields()</div></div>
<pre><code class="language-csharp">
public new static void SetIndividualFields(TableRow fields)
{
    SetFields(GetDialogId(DialogIds), fields, SupportedFields, CustomSupportedFields);
}
</code>
</pre>

<p class="alert alert-info">If a mapping exists in our <code>CustomSupportedFields</code> where the string key is an already existing key for <code>SupportedFields</code>, the mapped values for <code>CustomSupportedFields</code> are used.</p>
</li>

<li>
<p>Modify the step implementation.  Update the step definition to use the <code>CustomIndividualDIalog</code>'s <code>SetIndividualFields</code>() method.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Modified step implementation</div></div>
<pre><code class="language-csharp">
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

<p class="alert alert-warning">The <code>Save</code> step can cause a duplicate entry or error dialog. If this occurs, we advise adding a unique stamp to the last name of your constituent's values as shown above.</p>

<p>The test should pass now.</p>

<p>![TODO](/assets/img/CustomFields/PassingTest.PNG)</p>
</li>

</ol>

## Add Support For Custom Fields - Custom Method Approach

<ol>
<li>
<p>You can also use an alternative Gherkin approach that drives a need for an entirely custom method.</p>

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
</li>

<li>
<p>Add a method to a custom class. In this approach, we describe setting a single field's value for a step. Add the following method to your <code>CustomIndividualDialog</code> class.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Custom method</div></div>
<pre><code class="language-csharp">
public static void SetCustomField(string fieldCaption, string value)
{
    //Use the same IDictionary&lt;string, CrmField&gt; CustomSupportedFields from the Overload Approach
    SetField(GetDialogId(DialogIds), fieldCaption, value, CustomSupportedFields);
}
</code>
</pre>

<p class="alert alert-info">The custom method does not need the <code>new</code> attribute in the method declaration because <code>new</code> is only needed when overriding an inherited method.</>
</li>

<li>
<p>Implement the new step methods.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Implemetation of new steps</div></div>
<pre><code class="language-csharp">
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

<p>The test should pass now.</p>

<p>![TODO](/assets/img/CustomFields/2ndPassingTest.PNG)</p>

<p class="alert alert-info">The {{ stache.config.product_name_short }} provides multiple ways to achieve a result. This walkthrough describes two potential implementations to handle a dialog with a custom field, but these are not the only approaches. The methods and their underlying logic are defined by the user. You are free to create whatever helper methods you see fit. Look into the API documentation and see if you can come up with a different solution.</p>
</li>

</ol>

## Overload an Implementation

<ol>
<li>
<p>Identify the need to overload an implementation. We start with the following test case and step file.</p>

<pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Set the Last/Org/Group name field in a constituent search dialog
	Given I have logged into BBCRM
	When I open the constituent search dialog
	And set the Last/Org/Group name field value to "Smith"
	Then the Last/Org/Group name field is "Smith"
</code>
</pre>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Implemetation of steps</div></div>
<pre><code class="language-csharp">
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

<p>When run against a standard ***Blackbaud CRM*** application, the test passes.</p>

<p>![TODO](/assets/img/CustomFields/PassingDefaultConstituentSearch.PNG)</p>

<p>The steps navigate to the ***Constituents*** functional area and click the **Constituent search** task.</p>

<p>![TODO](/assets/img/CustomFields/DefaultConstituentSearchTask.PNG)</p>

<p>The **Last/Org/Group name** field is set and validated as containing the desired value.</p>

<p>![TODO](/assets/img/CustomFields/DefaultConstituentSearchDialog.PNG)</p>

<p>If we run the test against a custom application whose ***Constituent*** functional area looks like this...</p>

<p>![TODO](/assets/img/CustomFields/CustomConstituentSearchTask.PNG)</p>

<p>... then we get the following error:</p>

<p>![TODO](/assets/img/CustomFields/DefaultOnCustomSearchDialogError.PNG)</p>

<p>We can resolve this with the following code edit.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited step</div></div>
<pre><code class="language-csharp">
[When(@"I open the constituent search dialog")]
public void WhenIOpenTheConstituentSearchDialog()
{
    BBCRMHomePage.OpenConstituentsFA();
    FunctionalArea.OpenLink("Searching", "Constituent search");
}
</code>
</pre>

<p>If we run the test now, we get a new error.</p>

<p>![TODO](/assets/img/CustomFields/DefaultOnCustomSearchDialogFieldError.PNG)</p>

<p>Another customization must exist. The error stack trace indicates that the XPath constructor for the **Last/Org/Group name** field is not compatible with this application. <code>NoSuchElementExceptions</code> are thrown when Selenium's WebDriver times out looking for a web element using the XPath.</p>

<p>![TODO](/assets/img/CustomFields/CustomConstituentSearchDialog.PNG)</p>
</li>

<li>
<p>Identify the customization. Let's look at the search dialogs between the default and custom applications. Comparing the dialogs, clearly the dialog on the right is customized. Inspecting the **Last/Org/Group name** field between the two applications, we can see they share the same unique field ID.</p>

<p>![TODO](/assets/img/CustomFields/ComparingFieldIdSearchDialog.PNG)</p>

<p class="alert alert-info">If you do not know how to identify the field's unique ID, please review the [XPath Guidelines]({{stache.config.blue_walkthroughs_xpaths}})</p>

<p>Inspecting the unique dialog IDs, we can see that they are different. The supported XPath constructs an XPath using the dialog ID <code>ConstituentSearchbyNameorLookupID</code>. We need to modify the dialog ID to use the custom dialog ID.</p>

<p>![TODO](/assets/img/CustomFields/ComparingDialogIdsSearchDialog.PNG)</p>
</li>

<li>
<p>Edit the steps. Update the step code so the XPath constructors use the custom dialog ID.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited steps for custom dialog ID</div></div>
<pre><code class="language-csharp">
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

<p>The test should pass now.</p>

<p>![TODO](/assets/img/CustomFields/PassingDefaultConstituentSearch.PNG)</p>
</li>
</ol>

## Override an Implementation

<ol>
<li>
<p>Identify the need to override an implementation. Start with the following test case that works against a standard ***Blackaud CRM*** instance.</p>

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

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Implemetation of steps</div></div>
<pre><code class="language-csharp">
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

<p>At some point in the test, the **Related individual** field on the Add an individual dialog is set by using the associated searchlist.</p>

<p>![TODO](/assets/img/CustomFields/OverrideProcedure/RecordSearch.PNG)</p>

<p>What if we want to set the field through the **Add** button? This would require us to override the default implementation for how the **Related individual** field is set.</p>

<p>![TODO](/assets/img/CustomFields/OverrideProcedure/AddIcon.PNG)</p>
</li>

<li>
<p>Create a custom method.</p>

<p>If you have not created the <code>CustomIndividualDialog</code>, add it to your project and implement it as follows. First, we make sure to select the Household tab.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Selecting the right tab</div></div>
<pre><code class="language-csharp">
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

<p>Next we specify custom logic if a value for the **Related individual** field is provided. If a value is provided for this field, we click the button that brings up the add dialog. Be sure to read the API documentation for the XPath constructors.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Click the fields's Add button</div></div>
<pre><code class="language-csharp">
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

<p>This is the resulting dialog from clicking the add button on the **Related individual** field.</p>

<p>![TODO](/assets/img/CustomFields/OverrideProcedure/TriggerDialog.PNG)</p>

<p>We then set the **Last name** field value to the value provided for **Related individual** before clicking **OK**. We could have defined any logic and interactions involving this dialog, but let's keep it simple.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Set the Last name field</div></div>
<pre><code class="language-csharp">
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

<p>Before we call the base implementation to set the rest of the fields, we set fields["Related individual"] to equal null. We do this because we want the base <code>SetHouseholdFields</code> to skip its handling of the **Related individual** field.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Set Related individual to null and call the base method</div></div>
<pre><code class="language-csharp">
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

Another solution would be to remove the **Related individual** key from the fields object.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Remove the Related individual key</div></div><pre><code class="language-csharp">
fields.Keys.Remove("Related individual");
</code>
</pre>
</li>

<li>
<p>Update the Steps</p>

<p>Change the step setting the household tab fields.</p>

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Updated step</div></div>
<pre><code class="language-csharp">
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

<p>The test now sets the **Related individual** field through the add button and not the search dialog.</p>
</li>

</ol>

### See Also

[SpecFlow's Table and TableRow Guidelines]({{stache.config.blue_walkthroughs_specflow}})
