---
layout: layout-sidebar
name: XPath Guidelines
description: Walkthrough of how to create XPaths to find elements in Blackbaud CRM.
order: 30
---

{{ include 'includes/eapwarning/index.md' }}

# XPath Guidelines
XPath is a syntax that defines parts of XML documents and uses path expressions to select nodes within those XML documents. The {{ stache.config.product_name_short }} relies on XPaths to parse HTML elements and find desired elements, and this tutorial describes how to create XPaths that consistently and uniquely locate elements within ***Blackbaud CRM***.

When you create XPaths for elements in ***Blackbaud CRM***, keep in mind that:
1. The UAT Automation Kit accepts XPath parameters under the assumption that they return a single element, so make sure your XPath filter out any invisible elements.
2. When you create XPaths, follow the steps that you want to reproduce in your automation. That way, the state of ***Blackbaud CRM*** when you create the XPath should match the state when the automation runs.

## Prerequisites

* Complete of the [Selenium WebDriver]({{stache.config.blue_walkthroughs_selenium}}) walkthrough.
* Access to a ***Blackbaud CRM*** instance to test against.
* Familiarity with XML syntax.
* Familiarity with the concepts of [Choosing Effective XPaths](http://www.toolsqa.com/selenium-webdriver/choosing-effective-xpath/).

## Objectives
This tutorial guides you through the steps to create an XPath for a feature in ***Blackbaud CRM***. In this walkthrough, you will:
* Use an XPath to find elements in the browser.
* Modify an XPath to find a single element.
* Use an XPath to find a single element from among multiple panels in ***Blackbaud CRM*** by filtering out elements that are present in the HTML but are not displayed in the browser.
* Use an XPath to find a single element on a dialog from among multiple dialogs that are being displayed.

## Introduction to XPaths

The {{ stache.config.product_name_short }} uses Selenium's WebDriver to interact with the web browser, and XPath is the selection methology to find elements in the browser.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">WebDriver search with XPath selector condition</div></div>
<pre><code class="language-csharp">
IWebElement someElement = driver.FindElement(By.XPath(String.Format("//button[contains(@class,'linkbutton')]/div[text()='Constituent search']")));
</code>
</pre>

Web browsers such as Google Chrome allow you to view HTML elements and their heirarchy. With ***Blackbaud CRM*** open in Chrome, you can right-click items such as buttons, links, or images to access the **Inspect element** option.

![InspectElement](/assets/img/XPaths/InspectElement.PNG)  

For example, right-click the **Constituent search** task in the ***Home*** functional area and select **Inspect element** to view the task's HTML element type, attribute values, and location in the Document Oject Model (DOM).

![HighlightedElement](/assets/img/XPaths/HighlightedElement.PNG)  

XPaths specify selector conditions to parse the entire DOM and return all matching elements.

From the Elements panel in Chrome's Developer Tools, you can press Ctrl+F and enter an XPath to view the search results for an XPath selector. To search for <code>[div]</code> elements with the text "Constituent search" that are direct descendants of <code>[button]</code> elements with <code>class</code> attributes that contains "linkbutton," enter the XPath <code>//button[contains(@class,'linkbutton')]/div[text()='Constituent search']</code> in the search field.

![XPathMultipleResults](/assets/img/XPaths/XPathMultipleResults.PNG)

The XPath returns multiple matching elements based on the XPath selector condition. However, the {{ stache.config.product_name_short }}'s API methods accept XPath parameters under the assumption that XPaths return a single element. So when a custom or static XPath returns multiple results, we recommend that you modify it to consistently return a single element.

For our **Constituent search** example to return to a single element, we can modify the XPath selector:

<pre><code>
//div[contains(@id,'contentpanel')]/div[contains(@id,'contentpanel') and not(contains(@class, 'hide-display'))]//button[contains(@class,'task-link')]/div[text()='Constituent search']
</code>
</pre>

![XPathSingleResult](/assets/img/XPaths/XPathSingleResult.PNG)

The next section discusses common ***Blackbaud CRM*** XPath selector patterns that help return a single element.

## ***Blackbaud CRM*** Patterns - Panels

In the previous section, we modified the XPath selector to go from multiple matching results to a single result.

Now let's look at how the modified XPath narrows the search criteria so that we can understand the differences between the original and modified XPaths.

First, let's enter just the first component of the XPath in the search field: <code>//div[contains(@id,'contentpanel')]</code>. This XPath returns any [div] whose <code>id</code> attribute contains the text "contentpanel."

![CRMRootXPath](/assets/img/XPaths/CRMRootXPath.PNG)  

The screenshot shows 4 matches with the first match highlighted, although the number of matches will vary based on your browsing history in ***Blackbaud CRM***. In the screenshot, a single parent <code>div</code> includes "contentpanel" in its ID, and several immediate children elements also include "contentpanel" in their IDs. You can hover your mouse over each matching element to see it highlighted in the browser.

![CRMContentPanel](/assets/img/XPaths/CRMContentPanel.PNG)  

When you navigate between different areas, ***Blackbaud CRM*** stores previous pages in the HTML but marks them as hidden. If a user tries to navigate to a previously visited panel, it loads much faster because panel components are stored in the browser.

When you hover the mouse over the child "contentpanel," only one results in the browser highlighting an element. The contentpanels that do not result in a browser highlight include <code>x-hide-display</code> in their <code>class</code> attribute and are not displayed in the browser.

![CRMContentPanelHiddenPanels](/assets/img/XPaths/CRMContentPanelHiddenPanels.PNG)

If we add the <code>/div[contains(@id,'contentpanel') and not(contains(@class, 'hide-display'))]</code> component to our XPath to make <code>//div[contains(@id,'contentpanel')]/div[contains(@id,'contentpanel') and not(contains(@class, 'hide-display'))]</code>, it only returns one result. The single <code>/</code> between the first and second components of our XPath selector specifies an immediate child instead of any descendant. We can add as much selection criteria necessary to any element search criteria.

![CRMRootPlusOneXPath](/assets/img/XPaths/CRMRootPlusOneXPath.PNG)  

When you search for an element in ***Blackbaud CRM***'s panel window, we recommend that you start the XPath with search criteria that narrows the search to the visible "contentpanel." From the visible panel, you can search through descendants to find elements such as buttons.

## ***Blackbaud CRM*** Patterns - Dialogs

The DOM is a quicksand of ever-changing components, but you can use proper XPath syntax and selection criteria to break down even complicated navigations with multiple dialogs. Let's look at another scenario that seems a bit more complex.

In ***Blackbaud CRM***, click **Add a pledge** in the ***Revenue*** functional area to display the Add a pledge screen. For this dialog, we can use the simple XPath <code>//button[text()='Save']</code> to find the **Save** button.

![DialogPledgeSave](/assets/img/XPaths/DialogPledgeSave.PNG)

To make things more interesting, click the search button in the **Constituent** field.

![DialogConstituentSearch](/assets/img/XPaths/DialogConstituentSearch.PNG)  

And then on the search dialog, click **Add**, **Individual** ot display the Add an individual screen.

![DialogSearchAddIndividual](/assets/img/XPaths/DialogSearchAddIndividual.PNG)  

If we use the same XPath of <code>//button[text()='Save']</code> to search for the **Save** button on this dialog, two results are now returned.

![DialogFirstSave](/assets/img/XPaths/DialogFirstSave.PNG)  

-------------

![DialogSecondSave](/assets/img/XPaths/DialogSecondSave.PNG)  

We need to modify the XPath to only match descendant buttons on the top-most dialog. To do this, we must find something unique about a parent element of the Add an individual screen's **Save** button. From the Elements panel of Chrome's Developer Tools, follow the heirarchy tree to find the element that highlights the entire dialog.

![DialogEntireAddDialog](/assets/img/XPaths/DialogEntireAddDialog.PNG)  

The <code>[div]</code> that contains all elements on the Add an individual screen includes two immediate child elements. One of the child <code>[div]</code> elements contains the **Save** button.

![DialogSaveBarAddDialog](/assets/img/XPaths/DialogSaveBarAddDialog.PNG)  

The other child <code>[div]</code> element contains the fields in the dialog.

![DialogFieldInputsAddDialog](/assets/img/XPaths/DialogFieldInputsAddDialog.PNG)  

Within the <code>[div]</code> for the fields, another <code>[div]</code> element contains a unique identifier in the <code>id</code> attribute.

![DialogUniqueId](/assets/img/XPaths/DialogUniqueId.PNG)  

We can construct an XPath to find use the <code>div</code> with this unique identifier: <code>//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'individualRecordAddDataForm')]</code>.

![DialogXPathDialogUniqueId](/assets/img/XPaths/DialogXPathDialogUniqueId.png)

Then we can add additional search criteria to the XPath to find a button with the text "Save" relative to the <code>[div]</code> with the unique ID. The updated XPath <code>//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'individualRecordAddDataForm')] /../../../../../../../..//*[text()="Save"]</code> only returns 1 match.

![DialogFinalXPath](/assets/img/XPaths/DialogFinalXPath.PNG)  

The {{ stache.config.product_name_short }} API provides many XPath constructors for various application components such as panels and dialogs. Use the API documentation to see the XPath constructors that are available, the parameters they require, and the types of elements that they uniquely locate.

### See Also
[Choosing Effective XPaths](http://www.toolsqa.com/selenium-webdriver/choosing-effective-xpath/)  
