---
layout: layout-sidebar
name: SpecFlow's Table and TableRow Guidelines
description: In this walkthrough you will get experience with handling SpecFlow's Table and TableRow objects with the UAT SDK (Project Blue) API.
order: 30
---

# SpecFlow's Table and TableRow Guidelines

<p class="alert alert-warning">Warning: This is preliminary documentation and is subject to change</p>

In this walkthrough you will get experience with handling SpecFlow's Table and TableRow objects with the UAT SDK (Project Blue) API.

## Prerequisites

* You have completed the [Using the Selenium WebDriver]({{stache.config.blue_walkthroughs_201_selenium}}) walkthrough and have access to an Enterprise CRM application.
* You are comfortable adding tests and step implementations to existing feature and step files.
* You are comfortable accessing the existing UAT SDK (Project Blue) Core API.
* You are comfortable modifying the app.config to change which application the tests run against.
* You are comfortable identifying the unique attribute values for the XPath constructors in the Core API and have completed the [XPath Guidelines]({{stache.config.blue_walkthroughs_201_xpaths}}) walkthrough.

## From Feature File to Step File - The Old Approach To Tables

SpecFlow feature files support Tables for passing in variables to the .NET step methods.
<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Test example for adding an address to a constituent.</div></div><pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Add an address to a constituent
	Given I have logged into BBCRM
    And a constituent exists with the last name "Enterprise"
	When I add an address to the current constituent
	| Field                            | Value                 |
	| Type                             | Business              |
	| Country                          | United States         |
	| Address                          | 2000 Daniel Island Dr |
	| City                             | Charleston            |
	| State                            | SC                    |
	| ZIP                              | 29492                 |
	| Do not send mail to this address | checked               |
	Then an address exists
	| Field               | Value                                       |
	| Contact information | 2000 Daniel Island Dr Charleston, SC  29492 |
	| Type                | Business (Current)                          |
</code>
</pre>

At some point the test attemps to set the fields on the 'Add an address' dialog.  
![Add an address dialog][AddAddressDialog]

Specflow creates bindings between the test cases and the step methods. The field variables for the address dialog are passed through the Table parameter.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Step method with a Table parameter.</div></div><pre><code class="language-csharp">
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

        [Given(@"a constituent exists with last name ""(.&#42;)""")]
        public void GivenAConstituentExistsWithLastName(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I add an address to the current constituent")]
        public void WhenIAddAnAddressToTheCurrentConstituent(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"an address exists")]
        public void ThenAnAddressExists(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
</code>
</pre>

Here is an implementation of the step methods.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Implemented steps</div></div><pre><code class="language-csharp">
[Given(@"I have logged into BBCRM")]
public void GivenIHaveLoggedIntoBBCRM()
{
    BBCRMHomePage.Logon();
}

[Given(@"a constituent exists with last name ""(.&#42;)""")]
public void GivenAConstituentExistsWithLastName(string constituent)
{
    constituent += uniqueStamp;
    BBCRMHomePage.OpenConstituentsFA();
    ConstituentsFunctionalArea.AddAnIndividual();
    IndividualDialog.SetLastName(constituent);
    IndividualDialog.Save();
}

[When(@"I add an address to the current constituent")]
public void WhenIAddAnAddressToTheCurrentConstituent(Table addressFields)
{
    ConstituentPanel.SelectTab("Contact");
    ConstituentPanel.ClickSectionAddButton("Addresses");
    AddressDialog.SetAddressFields(addressFields);
    Dialog.Save();
}

[Then(@"an address exists")]
public void ThenAnAddressExists(Table addressFields)
{
    IDictionary&lt;string, string&gt; addressRow = new Dictionary&lt;string, string&gt;();
    foreach (TableRow row in addressFields.Rows)
    {
        addressRow.Add(row["Field"], row["Value"]);
    }
    ConstituentPanel.SelectTab("Contact");
    if (!ConstituentPanel.SectionDatalistRowExists(addressRow, "Addresses")) 
        FailTest(String.Format("Address '{0}' not found.", addressRow.Values));
}
</code>
</pre>

AddressDialog is not a class in the UAT SDK (Project Blue). At this point your build should be failing. Let's create an AddressDialog class and implement the SetAddressFields() method.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">For each TableRow in Table</div></div><pre><code class="language-csharp">
using System;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    public class AddressDialog : Dialog
    {
        public static void SetAddressFields(Table addressFields)
        {
            OpenTab("Address");
            foreach (TableRow row in addressFields.Rows)
            {

            }
        }
    }
}
</code>
</pre>

Each iteration through the loop gives us a new row from the Table. We need to use the TableRow object to find a field with an XPath selector and set the field's value. How we construct the XPath, what variables we pass to the XPath constructor, and what type of field setter we use are all determined by the specific field represented as the TableRow object. This logic must be defined for each possible value of row["Field"].  

To handle this, we create a switch on the caption value. The caption dictates what type of field we want to set and how to set its value.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Implemented AddressDialog</div></div><pre><code class="language-csharp">
using System;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    public class AddressDialog : Dialog
    {
        public static void SetAddressFields(Table addressFields)
        {
            OpenTab("Address");
            foreach (TableRow row in addressFields.Rows)
            {
                string caption = row["Field"];
                string value = row["Value"];
                switch (caption)
                {
                    case "Type":
                        SetDropDown(getXInput("AddressAddForm2", "_ADDRESSTYPECODEID_value"), value);
                        break;
                    case "Country":
                        SetDropDown(getXInput("AddressAddForm2", "_COUNTRYID_value"), value);
                        break;
                    case "Address":
                        SetTextField(getXTextArea("AddressAddForm2", "_ADDRESSBLOCK_value"), value);
                        break;
                    case "City":
                        SetTextField(getXInput("AddressAddForm2", "_CITY_value"), value);
                        break;
                    case "State":
                        SetDropDown(getXInput("AddressAddForm2", "_STATEID_value"), value);
                        break;
                    case "ZIP":
                        SetTextField(getXInput("AddressAddForm2", "_POSTCODE_value"), value);
                        break;
                    case "Do not send mail to this address":
                        SetCheckbox(getXInput("AddressAddForm2", "_DONOTMAIL_value"), value);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Field '{0}' is not implemented.", caption));
                }
            }
        }
    }
}
</code>
</pre>

<p class="alert alert-info">If you do not understand where the variables for the XPath constructors come from, please review the [XPath Guidelines]({{stache.config.blue_walkthroughs_201_xpaths}}) walkthrough.</p>

This approach will handle the desired logic and UI interactions, but the code itself is bulky and unpleasant. The next section shows how manipulating the format of your table can lead to cleaner, more adaptable code.

## Table Guidelines

####["Table headers are no longer required to be 'Field' and 'Value'"](https://github.com/techtalk/SpecFlow/wiki/SpecFlow-Assist-Helpers)

By changing the format of our feature file tables and how we pass variables to a step method, we can take advantage of more functionality in the UAT SDK.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">The same test from the previous section with a different format for the Tables</div></div><pre>.<code class="language-gherkin">
@DelvingDeeper
Scenario: Add an address to a constituent
	Given I have logged into BBCRM
	And a constituent exists with last name "Enterprise"
	When I add an address to the current constituent
	| Type     | Country       | Address               | City       | State | ZIP   | Do not send mail to this address |
	| Business | United States | 2000 Daniel Island Dr | Charleston | SC    | 29492 | checked                          |
	Then an address exists
	| Contact information                         | Type               |
	| 2000 Daniel Island Dr Charleston, SC  29492 | Business (Current) |
</code>
</pre>

Changing the table's headers from "Field" and "Value" to the dialog's field captions forces a change to the code and how it handles the Table object.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited step definitions.</div></div><pre><code class="language-csharp">
[When(@"I add an address to the current constituent")]
public void WhenIAddAnAddressToTheCurrentConstituent(Table addressTable)
{
    foreach (TableRow row in addressTable.Rows)
    {
        ConstituentPanel.SelectTab("Contact");
        ConstituentPanel.ClickSectionAddButton("Addresses");
        AddressDialog.SetAddressFields(row);
        Dialog.Save();
    }
}
</code>
</pre>

<p class="alert alert-info">Instead of passing the whole table to the SetMethod, we loop through the rows in the Table and pass in a single TableRow.
<br>
<br>
We only want to pass to the SetAddressFields() method an object that contains the relevant address dialog values.  In the previous method, the entire Table object contained these values.  In this situation, only a TableRow is needed to gather the necessary values.
</p>

Let's implement the method for handling a single TableRow.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited AddressDialog class.</div></div><pre><code class="language-csharp">
using System;
using System.Collections.Generic;
using Blueshirt.Core.Base;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    public class AddressDialog : Dialog
    {
        protected static readonly IDictionary&lt;string, CrmField&gt; SupportedFields = new Dictionary&lt;string, CrmField&gt;
        {
            {"Type", new CrmField("_ADDRESSTYPECODEID_value", FieldType.Dropdown)},
            {"Country", new CrmField("_COUNTRYID_value", FieldType.Dropdown)},
            {"Address", new CrmField("_ADDRESSBLOCK_value", FieldType.TexArea)},
            {"City", new CrmField("_CITY_value", FieldType.TextInput)},
            {"State", new CrmField("_STATEID_value", FieldType.Dropdown)},
            {"ZIP", new CrmField("_POSTCODE_value", FieldType.TextInput)},
            {"Do not send mail to this address", new CrmField("_DONOTMAIL_value", FieldType.Checkbox)}
        };

        public static void SetAddressFields(TableRow addressFields)
        {
            OpenTab("Address");
            SetFields("AddressAddForm2", addressFields, SupportedFields);
        }
    }
}
</code>
</pre>

<p class="alert alert-info">Note there is also support in CrmFields for setting fields through a search dialog.  Refer to the [CrmField]() and [FieldType]() API documentation to get a better understanding of the CrmField constructors.
</p>

With a TableRow whose Keys represent the dialog's field captions, we can now utilize the API's Dialog.SetFields() method. Instead of creating a switch on the field caption value, we can create a dictionary mapping the supported field captions to the relevant variables needed to set the field's value. These variables are encapsulated in the CrmField class.  

Now when we want to add support for a new field, we define the logic in a single line for the SupportedFields dictionary instead of a switch-case handler.  

Let's examine the 'Then' step again. By changing the table format here, we no longer need to convert the Table to a Dictionary. Instead we can directly pass the TableRows of the Table to Panel.SectionDatalistRowExists().

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited 'Then' Step.</div></div><pre><code class="language-csharp">
[Then(@"an address exists")]
public void ThenAnAddressExists(Table addressTable)
{
    ConstituentPanel.SelectTab("Contact");
    foreach (TableRow row in addressTable.Rows)
    {
        if (!ConstituentPanel.SectionDatalistRowExists(row, "Addresses"))
            FailTest(String.Format("Address '{0}' not found.", row.Values));
    }
}
</code>
</pre>

**BUT WAIT THERE'S MORE!!!!!**  

With this format, we now also have the ability to add multiple addresses and validate multiple addresses simply by adding rows to the table. No additional code required.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Modify your test case to contain multiple rows.</div></div><pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Add an address to a constituent
	Given I have logged into BBCRM
	And a constituent exists with last name "Enterprise"
	When I add an address to the current constituent
	| Type     | Country       | Address               | City       | State | ZIP   | Do not send mail to this address |
	| Business | United States | 2000 Daniel Island Dr | Charleston | SC    | 29492 | checked                          |
	|          | United States | 1990 Daniel Island Dr | Charleston | SC    | 29492 | checked                          |
	Then an address exists
	| Contact information                         | Type               |
	| 2000 Daniel Island Dr Charleston, SC  29492 | Business (Current) |
	| 1990 Daniel Island Dr Charleston, SC  29492 |                    |
</code>
</pre>

The foreach loop in the step methods breaks down the Table to TableRows allowing us to reliably add and validate each address.

<p class="alert alert-info">Empty table cells are treated as empty strings.
<br>
<br>
Leaving a cell as empty will result in the an attempt to set the field's value to an empty string.  If you wish to skip setting the field, you must remove the key from the TableRow or set the value to null.
<br>
<br>
<code>if (row.ContainsKey("Country") && row["Country"] == String.Empty) row["Country"] = null;</code>
<br>
<br>
Empty table cells for a datalist select or validation are skipped and no code edits are necessary.
</p>

## Supporting Multiple Dialog Ids

Continuing from the previous section, let's create a test that edits an existing address.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Test case adding and editing an address.</div></div><pre><code class="language-gherkin">
@DelvingDeeper
Scenario: Edit an address on a constituent
	Given I have logged into BBCRM
	And a constituent exists with last name "Enterprise"
	And I add an address to the current constiteunt
	| Address               | City       | State | ZIP   |
	| 2000 Daniel Island Dr | Charleston | SC    | 29492 |
	When I start to edit an address on the current constituent
	| Contact information                         | Type      |
	| 2000 Daniel Island Dr Charleston, SC  29492 | (Current) |
	And set the address fields and save the dialog
	| Type     | Address            | City       | ZIP   |
	| Business | 100 Aquarium Wharf | Charleston | 29401 |
	Then an address exists
	| Contact information                      | Type               |
	| 100 Aquarium Wharf Charleston, SC  29401 | Business (Current) |
</code>
</pre>

Here are implementations for the new step methods. Because of our table format, we can use TableRows to find and select our desired address row before clicking Edit.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">New step methods for editing an address.</div></div><pre><code class="language-csharp">
[Given(@"I add an address to the current constiteunt")]
public void GivenIAddAnAddressToTheCurrentConstiteunt(Table addressTable)
{
    WhenIAddAnAddressToTheCurrentConstituent(addressTable);
}

[When(@"I start to edit an address on the current constituent")]
public void WhenIStartToEditAnAddressOnTheCurrentConstituent(Table addressTable)
{
    ConstituentPanel.SelectTab("Contact");
    ConstituentPanel.SelectSectionDatalistRow(addressTable.Rows[0], "Addresses");
    ConstituentPanel.WaitClick(ConstituentPanel.getXSelectedDatalistRowButton("Edit"));
}

[When(@"set the address fields and save the dialog")]
public void WhenSetTheAddressFields(Table addressTable)
{
    AddressDialog.SetAddressFields(addressTable.Rows[0]);
    Dialog.Save();
}
</code>
</pre>

<p class="alert alert-info">Notice that you can call step methods from within step methods as done in GivenIAddAnAddressToTheCurrentConstiteunt().
</p>

The above code will compile but fail against the application. The implementation of SetAddressFields(TableRow addressFields) statically enters "AddressAddForm2" as the dialog's unique if for the XPath constructors.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Static dialog id.</div></div><pre><code class="language-csharp">
public static void SetAddressFields(TableRow addressFields)
{
    OpenTab("Address");
    SetFields("AddressAddForm2", addressFields, SupportedFields);
}
</code>
</pre>

Instead of creating a separate method or class, we can create a list of supported dialog ids.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">AddressDialog with supported dialog ids.</div></div><pre><code class="language-csharp">
public class AddressDialog : Dialog
{
    protected static readonly string[] DialogIds = { "AddressAddForm2", "AddressEditForm" };

    protected static readonly IDictionary&lt;string, CrmField&gt; SupportedFields = new Dictionary&lt;string, CrmField&gt;
    {
        {"Type", new CrmField("_ADDRESSTYPECODEID_value", FieldType.Dropdown)},
        {"Country", new CrmField("_COUNTRYID_value", FieldType.Dropdown)},
        {"Address", new CrmField("_ADDRESSBLOCK_value", FieldType.TextArea)},
        {"City", new CrmField("_CITY_value", FieldType.TextInput)},
        {"State", new CrmField("_STATEID_value", FieldType.Dropdown)},
        {"ZIP", new CrmField("_POSTCODE_value", FieldType.TextInput)},
        {"Do not send mail to this address", new CrmField("_DONOTMAIL_value", FieldType.Checkbox)}
    };

    public static void SetAddressFields(TableRow addressFields)
    {
        OpenTab("Address");
        SetFields(GetDialogId(DialogIds), addressFields, SupportedFields);
    }
}
</code>
</pre>

## See Also

[SpecFlow Tables and TableRows](https://github.com/techtalk/SpecFlow/wiki/SpecFlow-Assist-Helpers)  



[AddAddressDialog]: /assets/img/AddAddressDialog.PNG