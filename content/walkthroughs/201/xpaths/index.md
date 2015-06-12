---
layout: layout-sidebar
name: XPath Guidelines
description: Walkthrough of how to create XPaths to find elements within the Blackbaud CRM application.
order: 20
---

<p class="alert alert-warning"><strong><em>Warning:</em></strong> This is preliminary documentation and is subject to change.</p>

# XPath Guidelines
In this walkthrough, you'll gain experience creating XPaths to find elements consistently and uniquely within the ***Blackbaud CRM*** application.

## Prerequisites

* You have completed the [Using the Selenium WebDriver]({{stache.config.blue_walkthroughs_201_selenium}}) walkthrough and have access to a ***Blackbaud CRM*** instance.
* You understand XML syntax.
* You have read through and familiarized yourself with the concepts at [Choosing Effective XPaths](http://www.toolsqa.com/selenium-webdriver/choosing-effective-xpath/)

## Introduction to XPaths

Selenium's WebDriver is what the {{ stache.config.product_name_short }} uses to interact with the web browser, and XPath is the selection methology for finding elements in the browser.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">WebDriver search using an XPath selector condition.</div></div>
<pre><code class="language-csharp">
IWebElement someElement = driver.FindElement(By.XPath(String.Format("//button[contains(@class,'linkbutton')]/div[text()='Constituent search']")));
</code>
</pre>

Web browsers such as Google Chrome provide an ability to view the HTML elements and their heirarchy.

In Chrome right-clicking on a button, link, image, etc. will bring up an options menu with the option to "Inspect element."

![InspectElement](/assets/img/XPaths/InspectElement.PNG)  

This will show you the HTML element type, attribute values, and location in the DOM.

![HighlightedElement](/assets/img/XPaths/HighlightedElement.PNG)  

XPaths specify a selector condition that parses the entire DOM returning all matching elements. While in the Elements Panel of Chrome's Developer Tools, you can bring up a search (Ctrl+F) input to enter an XPath and see the results of an XPath selector.

Entering the XPath <code>"//button[contains(@class,'linkbutton')]/div[text()='Constituent search']"</code> looks for all [div] elements with the text 'Constituent search' that are direct descendants of a [button] element with a class attribute containing 'linkbutton'.

![XPathMultipleResults](/assets/img/XPaths/XPathMultipleResults.PNG)

The above XPath returned multiple matching elements based on the XPath selector condition. The existing API methods of the {{ stache.config.product_name_short }} accepting an XPath parameter assume the provided XPaths will return a single element. If you provide a custom or static XPath returning multiple results, it is advised to modify the XPath until a single element is consistently returned. Common Enterprise CRM XPath selector patterns to aide in finding a single element are discussed in the next section of this walkthrough.

By modifying the XPath selector to <code>"//div[contains(@id,'contentpanel')]/div[contains(@id,'contentpanel') and not(contains(@class, 'hide-display'))]//button[contains(@class,'task-link')]/div[text()='Constituent search']"</code>, we can reduce our matching results to a single element.

![XPathSingleResult](/assets/img/XPaths/XPathSingleResult.PNG)

## Enterprise CRM Patterns - Panels

In the above section we used an original and modified XPath to change our matching results. Let's look at how the modified XPath narrowed our search criteria and understand the differences between the original and modified XPaths.

To see how the modified XPath narrows its selection critieria, let's start with the first component of the XPath <code>"//div[contains(@id,'contentpanel')]"</code> and enter that into the search input. This XPath returns any [div] whose id attribute contains the text 'contentpanel'. In the image below, we can see that 4 matches are found with the first match highlighted.

![CRMRootXPath](/assets/img/XPaths/CRMRootXPath.PNG)  

Look through the matching elements or at the below image. We can see a single parent div with the ID containing "contentpanel." We can also see several immediate children elements also containing 'contentpanel' in the ID. Hover your mouse over each matching element to what gets highlighted in the browser.

![CRMContentPanel](/assets/img/XPaths/CRMContentPanel.PNG)  

When navigating between different areas in the application, ***Blackbaud CRM*** stores previous pages in the HTML but marks them as hidden. If the user tries to navigate back to a previously visited panel, the load will be much faster because the panel's components are already stored in the browser. Notice how only one child "contentpanel" results in the browser highlighting an element. The contentpanels not resulting a browser highlight have the "x-hide-display" test in their class attribute and are not displayed in the browser.

![CRMContentPanelHiddenPanels](/assets/img/XPaths/CRMContentPanelHiddenPanels.PNG)

If we add the second component of the modified XPath to our search so that it reads as <code>"//div[contains(@id,'contentpanel')]/div[contains(@id,'contentpanel') and not(contains(@class, 'hide-display'))]"</code>, we only get one result. Note the single '/' between the first and second components of our XPath selector. This specifies an immediate child instead of any descendant. Also note how can add as much selection criteria as want to to any given element search criteria.
![CRMRootPlusOneXPath](/assets/img/XPaths/CRMRootPlusOneXPath.PNG)  

Whenever you are trying to find an element within the panel window of the application, it is best practice to add a search criteria at the beginning of your XPath that narrows the search to the visible 'contentpanel'. From the visible panel, you can search through descendants to find elements such as a button.

## Enterprise CRM Patterns - Dialogs

The DOM is a quicksand of ever-changing components, but even complicated navigations with multiple dialogs open can be broken down with proper XPath syntax and selection criteria. Let's look at a bit more seemingly complex of a situation. In your application start to add a pledge. If you cannot do so in your application, follow along with the screenshots below.

In this initial dialog, we can create a simple enough XPath <code>"//button[text()='Save']"</code> to find the Save button.

![DialogPledgeSave](/assets/img/XPaths/DialogPledgeSave.PNG)  

Let's make things interesting. In the Constituent dialog, click the search button.

![DialogConstituentSearch](/assets/img/XPaths/DialogConstituentSearch.PNG)  

In the search dialog, click the button to add a new individual.

![DialogSearchAddIndividual](/assets/img/XPaths/DialogSearchAddIndividual.PNG)  

If you use the same XPath <code>"//button[text()='Save']"</code> to search for the save button on the individual add dialog, then two results should now be returned.
![DialogFirstSave](/assets/img/XPaths/DialogFirstSave.PNG)  

-------------

![DialogSecondSave](/assets/img/XPaths/DialogSecondSave.PNG)  

We have to modify our XPath so that only descendant buttons of the top-most dialog will be matched. To do this we have to find something unique about a parent element of the add an individual save button. Right-click inspect the Save button element and follow the heirarchy tree up until the selected element highlights the entire dialog.

![DialogEntireAddDialog](/assets/img/XPaths/DialogEntireAddDialog.PNG)  

The [div] containing all elements in the individual add dialog contains two immediate children. One child [div] contains the Save button we want to click.

![DialogSaveBarAddDialog](/assets/img/XPaths/DialogSaveBarAddDialog.PNG)  

The other child [div] contains the fields of the dialog.

![DialogFieldInputsAddDialog](/assets/img/XPaths/DialogFieldInputsAddDialog.PNG)  

Digging into the [div] containing the dialog input fields, we come across a [div] with a unique identifier in its id attribute.

![DialogUniqueId](/assets/img/XPaths/DialogUniqueId.PNG)  

Using this unique id, we can construct an XPath <code>"//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'individualRecordAddDataForm')]"</code> to find the div with this id.
![DialogXPathDialogUniqueId]/assets/img/XPaths/(DialogXPathDialogUniqueId)  

****Missing Image here from original docs****

Then we can add additional search criteria to the XPath to find a button with the text "Save" relative to the [div] with the unique id. Search using the XPath <code>"//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'individualRecordAddDataForm')] /../../../../../../../..//*[text()="Save"]"</code> Only 1 match is found!

![DialogFinalXPath](/assets/img/XPaths/DialogFinalXPath.PNG)  

The API of the {{ stache.config.product_name_short }} provides many XPath constructors for various application components like Panels and Dialogs. Refer to our API documentation to see exactly what existing XPath constructors are available, what parameters they require, and what type of elements they try and uniquely locate.

## See Also

[Choosing Effective XPaths](http://www.toolsqa.com/selenium-webdriver/choosing-effective-xpath/)  

