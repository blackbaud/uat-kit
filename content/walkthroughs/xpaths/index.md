---
layout: layout-sidebar
name: XPath Guidelines
description: Walkthrough of how to create XPaths to find elements within the Blackbaud CRM application.
order: 30
---

<p class="alert alert-warning"><strong><em>Warning:</em></strong> This is preliminary documentation and is subject to change.</p>

# XPath Guidelines
In this walkthrough, you'll gain experience creating XPaths to find elements consistently and uniquely within the ***Blackbaud CRM*** application.

## Prerequisites

* Completion of the [Using the Selenium WebDriver]({{stache.config.blue_walkthroughs_selenium}}) walkthrough.
* Access to a ***Blackbaud CRM*** instance to test against.
* Familiarity with XML syntax.
* Familiarity with the [Choosing Effective XPaths](http://www.toolsqa.com/selenium-webdriver/choosing-effective-xpath/) concepts.

## Introduction to XPaths

The {{ stache.config.product_name_short }} uses Selenium's WebDriver to interact with the web browser, and it uses XPaths as the selection methology to find elements in the browser.

<div class="codeSnippetContainerTabs"><div class="codeSnippetContainerTabSingle">WebDriver search with XPath selector condition</div></div>
<pre><code class="language-csharp">
IWebElement someElement = driver.FindElement(By.XPath(String.Format("//button[contains(@class,'linkbutton')]/div[text()='Constituent search']")));
</code>
</pre>

Web browsers such as Google Chrome provide the ability to view the HTML elements and their heirarchy.

With ***Blackbaud CRM*** open in Chrome, you can right-click items such as buttons, links, or images to access the **Inspect element** option.

![InspectElement](/assets/img/XPaths/InspectElement.PNG)  

For example, you can right-click the **Constituent search** task and select **Inspect element** to see the HTML element type, attribute values, and location in the Document Oject Model (DOM).

![HighlightedElement](/assets/img/XPaths/HighlightedElement.PNG)  

XPaths specify selector conditions to parse the entire DOM and return all matching elements.

From the Elements panel of Chrome's Developer Tools, you can press Ctrl+F and enter an Xpath to see the search results for an XPath selector. To search for all <code>[div]</code> elements with the text "Constituent search" that are direct descendants of <code>[button]</code> elements with <code>class</code> attributes that contains "linkbutton," enter the XPath <code>//button[contains(@class,'linkbutton')]/div[text()='Constituent search']</code> in the search field.

![XPathMultipleResults](/assets/img/XPaths/XPathMultipleResults.PNG)

The XPath returns multiple matching elements based on the XPath selector condition. The {{ stache.config.product_name_short }}'s API methods accept XPath parameters under the assumption that XPaths return a single element. If a custom or static XPath returns multiple results, we recommend that you modify it to consistently return a single element.

The next section discusses common ***Blackbaud CRM*** XPath selector patterns to help return a single element. For example, in the **Constituent search** example above, you can modify the XPath selector to <code>//div[contains(@id,'contentpanel')]/div[contains(@id,'contentpanel') and not(contains(@class, 'hide-display'))]//button[contains(@class,'task-link')]/div[text()='Constituent search']</code> to return to a single element.

![XPathSingleResult](/assets/img/XPaths/XPathSingleResult.PNG)

## ***Blackbaud CRM*** Patterns - Panels

At the end of the previous section, we showed how to modify the XPath selector to change our matching results. Let's look at how the modified XPath narrowed our search criteria so that we can understand the differences between the original and modified XPaths.

To see how the modified XPath narrows its selection critieria, let's enter just the first component of the XPath <code>//div[contains(@id,'contentpanel')]</code> in the search field. This XPath returns any [div] whose <code>id</code> attribute contains the text "contentpanel." In the image below, we can see that 4 matches are found with the first match highlighted.

![CRMRootXPath](/assets/img/XPaths/CRMRootXPath.PNG)  

Look through the matching elements or at the below image. We can see a single parent div with the ID containing "contentpanel." We can also see several immediate children elements also that contain "contentpanel" in the ID. You can hover your mouse over each matching element to see it highlighted in the browser.

![CRMContentPanel](/assets/img/XPaths/CRMContentPanel.PNG)  

When you navigate between different areas, ***Blackbaud CRM*** stores previous pages in the HTML but marks them as hidden. If a user tries to navigate to a previously visited panel, it loads much faster because panel components are stored in the browser.

Notice how only one child "contentpanel" results in the browser highlighting an element. The contentpanels that do not result in a browser highlight have <code>x-hide-display</code> in their <code>class</code> attribute and are not displayed in the browser.

![CRMContentPanelHiddenPanels](/assets/img/XPaths/CRMContentPanelHiddenPanels.PNG)

If we add the <code>/div[contains(@id,'contentpanel') and not(contains(@class, 'hide-display'))]</code> component to our XPath to make <code>//div[contains(@id,'contentpanel')]/div[contains(@id,'contentpanel') and not(contains(@class, 'hide-display'))]</code>, it only returns one result. The single <code>/</code> between the first and second components of our XPath selector specifies an immediate child instead of any descendant. We can add as much selection criteria as we want to any element search criteria.

![CRMRootPlusOneXPath](/assets/img/XPaths/CRMRootPlusOneXPath.PNG)  

When you try to find an element in the panel window of the application, it is best practice to add a search criteria at the beginning of your XPath to narrow the search to the visible "contentpanel." From the visible panel, you can search through descendants to find elements such as a button.

## ***Blackbaud CRM*** Patterns - Dialogs

The DOM is a quicksand of ever-changing components, but even complicated navigations with multiple dialogs open can be broken down with proper XPath syntax and selection criteria. Let's look at a bit more seemingly complex of a situation.

In ***Blackbaud CRM***, add a pledge. In the initial dialog, we can create the simple XPath <code>//button[text()='Save']</code> to find the **Save** button.

![DialogPledgeSave](/assets/img/XPaths/DialogPledgeSave.PNG)  

In the Constituent dialog, click the search button.

![DialogConstituentSearch](/assets/img/XPaths/DialogConstituentSearch.PNG)  

In the search dialog, click **Add**, **Individual**.

![DialogSearchAddIndividual](/assets/img/XPaths/DialogSearchAddIndividual.PNG)  

If you use the same XPath <code>//button[text()='Save']</code> to search for the **Save** button on the individual add dialog, then two results should be returned.

![DialogFirstSave](/assets/img/XPaths/DialogFirstSave.PNG)  

-------------

![DialogSecondSave](/assets/img/XPaths/DialogSecondSave.PNG)  

We have to modify our XPath to only match descendant buttons of the top-most dialog. To do this we must find something unique about a parent element of the add an individual save button. Right-click inspect the Save button element and follow the heirarchy tree up until the selected element highlights the entire dialog.

![DialogEntireAddDialog](/assets/img/XPaths/DialogEntireAddDialog.PNG)  

The <code>[div]</code> that contains all elements in the Add an individual dialog includes two immediate children. One child <code>[div]</code> contains the **Save** button that we want to click.

![DialogSaveBarAddDialog](/assets/img/XPaths/DialogSaveBarAddDialog.PNG)  

The other child <code>[div]</code> contains the fields of the dialog.

![DialogFieldInputsAddDialog](/assets/img/XPaths/DialogFieldInputsAddDialog.PNG)  

Digging into the <code>[div]</code> that contains the dialog input fields, we come across a <code>[div]</code> with a unique identifier in its <code>id</code> attribute.

![DialogUniqueId](/assets/img/XPaths/DialogUniqueId.PNG)  

With this unique <code>id</code>, we can construct an XPath <code>//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'individualRecordAddDataForm')]</code> to find the <code>div</code> with this ID.

![DialogXPathDialogUniqueId](/assets/img/XPaths/(DialogXPathDialogUniqueId.png)

****Missing Image here from original docs****

Then we can add additional search criteria to the XPath to find a button with the text "Save" relative to the <code>[div]</code> with the unique ID. When we search with the XPath <code>//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//div[contains(@id,'individualRecordAddDataForm')] /../../../../../../../..//*[text()="Save"]</code>, only 1 match is found!

![DialogFinalXPath](/assets/img/XPaths/DialogFinalXPath.PNG)  

The API of the {{ stache.config.product_name_short }} provides many XPath constructors for various application components such as panels and dialogs. Use the API documentation to see the XPath constructors that are available, the parameters they require, and the type of elements that they uniquely locate.

## See Also

[Choosing Effective XPaths](http://www.toolsqa.com/selenium-webdriver/choosing-effective-xpath/)  
