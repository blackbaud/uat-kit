---
layout: layout-sidebar
name: SpecFlow Table and TableRow Guidelines
description: In this walkthrough, you will get experience with handling SpecFlow's Table and TableRow objects with the UAT Automation Kit.
order: 40
---

<p class="alert alert-warning"><strong><em>Warning:</em></strong> This is preliminary documentation and is subject to change.</p>

# SpecFlow's Table and TableRow Guidelines
In this walkthrough, you will get experience with handling SpecFlow's Table and TableRow objects with the {{ stache.config.product_name_short }}.

## Prerequisites

* Completion of the [Using the Selenium WebDriver]({{stache.config.blue_walkthroughs_selenium}}) walkthrough.
* Access to a ***Blackbaud CRM*** instance to test against.
* Familiarity with adding tests and step implementations to existing feature and step files.
* Familiarity with accessing the {{ stache.config.product_name_short }} Core API.
* Familiarity with modifying the App.config to change which application the tests run against.
* Familiarity with identifying the unique attribute values for the XPath constructors in the Core API and completion of the [XPath Guidelines]({{stache.config.blue_walkthroughs_xpaths}}) walkthrough.

## From Feature File to Step File - The Old Approach to Tables

SpecFlow feature files allow you to use tables to pass variables to .NET step methods.
<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Test example to add address to constituent</div></div><pre><code class="language-gherkin">
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

At some point, the test example attempts to set the fields on the Add an address dialog.

![Add an address dialog][AddAddressDialog]

Specflow creates bindings between test cases and step methods. Field variables for the Add an address dialog are passed through the <code>Table</code> parameter.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Step method with Table parameter</div></div><pre><code class="language-csharp">
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

<code>AddressDialog</code> is not a class in the {{ stache.config.product_name_short }}, so your build should fail at this point. We need to create the <code>AddressDialog</code> class and implement a <code>SetAddressFields()</code> method.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">AddressDialog Class with empty method</div></div><pre><code class="language-csharp">
using System;
using Blueshirt.Core.Crm;
using TechTalk.SpecFlow;

namespace Delving_Deeper
{
    public class AddressDialog : Dialog
    {
        public static void SetAddressFields(Table addressFields)
        {
            throw new NotImplementedException();
        }
    }
}
</code>
</pre>

We ensure that we are on the Address tab, and then we parse every row in the table.

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

Each iteration through the loop gives us a new row from the table. We need to use the <code>TableRow</code> object to find a field with an XPath selector and set the field's value. The specific field represented as the <code>TableRow</code> object determines how to construct the XPath, what variables to pass to the XPath constructor, and what type of field setter to use. This logic must be defined for each possible value of <code>row["Field"]</code>.  

To handle this, we create a switch on the caption value. The caption dictates the type of field to set and how to set its value.

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

<p class="alert alert-info">For information about where the variables for the XPath constructors come from, see the [XPath Guidelines]({{stache.config.blue_walkthroughs_xpaths}}) walkthrough.</p>

This approach handles the desired logic and UI interactions, but the code itself is bulky and unpleasant. The next section demonstrates how to manipulate the format of your table to get cleaner, more adaptable code.

## Table Guidelines
The table headers of "Field" and "Value" from the SpecFlow feature file example in the previous section [are not required to pass variables to .NET step methods.](https://github.com/techtalk/SpecFlow/wiki/SpecFlow-Assist-Helpers) To take advantage of more functionality in the {{ stache.config.product_name_short }}, we can change the table format and how we pass variables to a step method.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Modified test example to add address to constituent</div></div><pre>.<code class="language-gherkin">
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

After we change the table headers from "Field" and "Value" to the field captions in the dialog, we must then change how the code handles the <code>Table</code> object.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited step definitions</div></div><pre><code class="language-csharp">
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

<p class="alert alert-info">Instead of passing the whole table to the <code>SetMethod</code>, we loop through the rows in the table and pass in a single <code>TableRow</code>.
<br>
We only want to pass an object with  the relevant address dialog values to the <code>SetAddressFields()</code> method. In the previous method, the entire <code>Table</code> object contained these values. In this situation, only a <code>TableRow</code> is necessary to gather the necessary values.</p>

Let's implement the method for handling a single <code>TableRow</code>.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited AddressDialog class</div></div><pre><code class="language-csharp">
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

<p class="alert alert-info"><code>CrmField</code> also supports setting fields through a search dialog. Refer to the <code>[CrmField]()</code> and <code>[FieldType]()</code> API documentation to get a better understanding of <code>CrmField</code> constructors.</p>

With a <code>TableRow</code> whose <code>Keys</code> represent the dialog's field captions, we can now utilize the API's <code>Dialog.SetFields()</code> method. Instead of creating a switch on the field caption value, we can create a dictionary that maps the supported field captions to the relevant variables that are needed to set the field's value. These variables are encapsulated in the <code>CrmField</code> class.  

To add support for a new field, we define the logic in a single line for the <code>SupportedFields</code> dictionary instead of a switch-case handler.  

Let's examine the <code>Then</code> step again. Since we changed the table format, we no longer need to convert the table to a dictionary. Instead we can directly pass the <code>TableRows</code> of the <code>Table</code> to <code>Panel.SectionDatalistRowExists()</code>.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Edited 'Then' Step</div></div><pre><code class="language-csharp">
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

With this format, we can also add multiple addresses and validate multiple addresses simply by adding rows to the table. No additional code is required.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Modified test case to contain multiple rows</div></div><pre><code class="language-gherkin">
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

The <code>foreach</code> loop in the step methods breaks down the <code>Table</code> to <code>TableRows</code>, which allows us to reliably add and validate each address.

<p class="alert alert-info">Empty table cells are treated as empty strings.
<br>
Leaving a cell empty results in an attempt to set the field's value to an empty string. To skip setting the field, you can remove the key from the <code>TableRow</code> or set the value to null.
<br>
<code>if (row.ContainsKey("Country") && row["Country"] == String.Empty) row["Country"] = null;</code>
<br>
Empty table cells for a data list select or validation are skipped and no code edits are necessary.
</p>

## Support Multiple Dialog IDs

Let's create a test that edits an existing address.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Test case to add and edit an address.</div></div><pre><code class="language-gherkin">
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

Here are implementations for the new step methods. Because of our table format, we can use <code>TableRows</code> to find and select our desired address row before clicking **Edit**.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">New step methods to edit an address</div></div><pre><code class="language-csharp">
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

<p class="alert alert-info">Notice that you can call step methods from within step methods as done in <code>GivenIAddAnAddressToTheCurrentConstiteunt()</code>.</p>

The above code compiles but fails against the application. The implementation of <code>SetAddressFields(TableRow addressFields)</code> statically enters "AddressAddForm2" as the dialog's unique if for the XPath constructors.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">Static dialog ID</div></div><pre><code class="language-csharp">
public static void SetAddressFields(TableRow addressFields)
{
    OpenTab("Address");
    SetFields("AddressAddForm2", addressFields, SupportedFields);
}
</code>
</pre>

Instead of creating a separate method or class, we can create a list of supported dialog IDs.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">AddressDialog with supported dialog IDs</div></div><pre><code class="language-csharp">
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

### See Also

[SpecFlow Tables and TableRows](https://github.com/techtalk/SpecFlow/wiki/SpecFlow-Assist-Helpers)  



[AddAddressDialog]: /assets/img/AddAddressDialog.PNG