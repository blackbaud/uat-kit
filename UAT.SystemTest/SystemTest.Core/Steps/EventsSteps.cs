using System;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Base;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Crm.Panels;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using SpecFlow.Assist.Dynamic;
using SystemTest.Common;
using System.Collections.Generic;
using System.Linq;

namespace SystemTest.Core.Steps
{
    [Binding]
    public sealed class EventsSteps : BaseSteps
    {
        [Given(@"Event ""(.*)"" exists")]
        public void GivenAnEventExists(string eventName)
        {
            GivenAnEventExists(eventName, DateTime.Now);
        }

        public void GivenAnEventExists(string eventName, DateTime StartDate)
        {
            StepHelper.SetCurrentThreadCultureToConfigValue();
            if (eventName == string.Empty)
            {
                throw new MissingFieldException("no value provided in the table for the field 'Name'");
            }

            BBCRMHomePage.OpenEventsFA();
            var headers = new string[3] { "Name", "Start date", "Category" };
            var firstRow = new string[3] { eventName, StartDate.ToShortDateString(), "Sport" };

            var events = new Table(headers);
            events.AddRow(firstRow);

            GivenAnEventExists(events);
            //check is visible to allow extra time for page load
            BaseComponent.GetEnabledElement(string.Format("//span[text()='{0}']", eventName + uniqueStamp), 240);
        }

        [Given(@"an event exists")]
        public void GivenAnEventExists(Table events)
        {
            foreach (var e in events.Rows)
            {
                StepHelper.SetTodayDateInTableRow("Start date", e);
                e["Name"] += uniqueStamp;
                //navigate to event and add event
                BBCRMHomePage.OpenEventsFA();
                Panel.WaitClick(XpathHelper.xPath.VisiblePanel + "//button[./text()='Add new']");
                Panel.WaitClick("//span[./text()='Event']");
                //test for "Cannot read property 'createChild' of null" popup?
                try
                {
                    BaseComponent.GetEnabledElement("//span[contains(.,'Cannot read property')]", 5);
                    Dialog.OK();
                }
                catch
                {
                    //eat and move on
                }
                //check is visible
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisibleBlock + "//span[text()='Add an event']");
                //set fields
                Dialog.SetTextField("//input[contains(@id,'_STARTDATE_value')]", e["Start date"]);
                Dialog.SetTextField("//input[contains(@id,'_NAME_value')]", e["Name"]);
                Dialog.SetTextField("//input[contains(@id,'_EVENTCATEGORYCODEID_value')]", e["Category"]);
                StepHelper.AddEntryOnTheFly();
                Dialog.Save();
            }
        }

        [Given(@"Event management template ""(.*)"" exists")]
        public void GivenEventManagementTemplateExists(string templateName)
        {
            templateName += uniqueStamp;
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.EventManagementTemplates();
            if (!EventManagementTemplatesPanel.TemplateExists(templateName))
                EventManagementTemplatesPanel.AddTemplate(templateName);
        }

        [When(@"I create a multi-event using template ""(.*)""")]
        public void WhenICreateAMulti_EventUsingTemplate(string template, Table events)
        {
            template += uniqueStamp;
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.AddMultiEvent();
            MultiLevelEventDialog.SetTemplate(template);
            foreach (var eventToAdd in events.Rows)
            {
                eventToAdd["event"] += uniqueStamp;
                if (eventToAdd["parent"] == string.Empty) MultiLevelEventDialog.AddExistingEvent(eventToAdd["event"]);
                else MultiLevelEventDialog.AddChildEvent(eventToAdd["event"], eventToAdd["parent"] + uniqueStamp);
            }
            MultiLevelEventDialog.Save();
        }

        [Then(@"a multi-level event ""(.*)"" exists with hierarchy")]
        public void ThenAMulti_LevelEventExistsWithHierarchy(string multiEventName, Table hierachy)
        {
            multiEventName = multiEventName.Insert(multiEventName.IndexOf(" - Default Summary"), uniqueStamp);
            if (!MultiEventPanel.IsMultiEventName(multiEventName))
                throw new ArgumentException("Current multi event page does not have the name " + multiEventName);
            foreach (var row in hierachy.Rows)
            {
                row["event"] += uniqueStamp;
                if (row["parent"] != string.Empty) row["parent"] += uniqueStamp;
            }
            if (!MultiEventPanel.IsHierarchy(hierachy))
                throw new ArgumentException("Current multi event page does not have the hierarchy " + hierachy);
        }

        [When(@"I add a Registration Option to event ""(.*)""")]
        public void WhenIAddARegistrationOptionToEvent(string eventName, Table registrationOptions)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            foreach (var option in registrationOptions.Rows)
            {
                EventPanel.AddRegistrationOption(option);
            }
        }

        [Then(@"event ""(.*)"" has registration option")]
        public void ThenEventHasRegistrationOption(string eventName, Table registrationOptions)
        {
            eventName += uniqueStamp;
            GetEventPanel(eventName);
            foreach (var option in registrationOptions.Rows)
            {
                if (!EventPanel.RegistrationOptionExists(option))
                    throw new ArgumentException("'" + option +
                                                "' was not found on the Registration options datalist for event " +
                                                eventName + ".");
            }
        }

        [Given(@"an invitation ""(.*)"" exists")]
        public void GivenAnInvitationExists(string InvitationName)
        {
            try
            {
                this.AddInvitations(InvitationName);
            }
            catch (ApplicationException Ex)
            {
                throw new ApplicationException(
                    "Exception in EventFeatureManagingInvitationsSteps.GivenAnInvitationExists(string)", Ex);
            }
        }

        [Given(@"Constituent ""(.*)"" is marked as deceased with source of ""(.*)""")]
        public void GivenConstituentIsMarkedAsDeceasedWithSourceOf(string ConstituentName, string source)
        {
            //lets set the thread culture to get the correct date for the browser
            StepHelper.SetCurrentThreadCultureToConfigValue();
            SearchAndSelectConstituent(ConstituentName);
            //select personal info
            Panel.SelectTab("Personal Info");
            //select "mark deceased"
            BaseComponent.WaitClick("//table[.//div[./text()='Personal information']]/tbody/tr/td[8]//button[./text()='Mark deceased']");
            Dialog.SetTextField(XpathHelper.xPath.VisibleDialog + "//input[contains(@id,'DECEASEDDATE_value')]", DateTime.Now.ToShortDateString());
            Dialog.SetTextField(XpathHelper.xPath.VisibleDialog + "//input[contains(@id,'DECEASEDSOURCECODEID_value')]", source);
            StepHelper.AddEntryOnTheFly();
            Dialog.Save();
        }

        [Given(@"I select event ""(.*)""")]
        public void GivenISelectEvent(string TargetEvent)
        {
            BBCRMHomePage.OpenEventsFA();
            EventsFunctionalArea.EventSearch(TargetEvent += uniqueStamp);
        }

        [When(@"I Add multiple invitees to invitation ""(.*)"" from selection ""(.*)""")]
        public void WhenIAddMultipleInviteesToInvitationFromSelection(string InviteName, string SelectionName)
        {
            //select Invitations tab
            Panel.SelectTab("Invitations");
            BaseComponent.WaitClick(string.Format("//a[contains(@title,'{0}') and ./text()='{0}']", InviteName));
            //select add button
            Panel.ClickSectionAddButton("Invitees");
            //select Constituent dropdown
            BaseComponent.WaitClick("//span[contains(@class,'x-menu-item-text') and ./text()='Multiple constituents']");
            //enter selection
            BaseComponent.WaitClick(XpathHelper.xPath.VisibleDialog + "//img[contains(@class,'x-form-trigger x-form-search-trigger')]");
            Dialog.SetTextField("//div[contains(@class,'x-window bbui-dialog-search bbui-dialog x-resizable-pinned') and contains(@style,'visible')]//input[contains(@id,'NAME_value')]", SelectionName + uniqueStamp);
            Dialog.ClickButton("Search");
            BaseComponent.WaitClick(string.Format("//span[contains(@title,'{0}') and ./text()='{0} (Ad-hoc Query)']", SelectionName + uniqueStamp));
            //save
            Dialog.Save();
        }

        [Then(@"Event Invitation Invitee List shows Invitees")]
        public void ThenEventInvitationInviteeListShowsInvitees(Table table)
        {
            //wait for grid to load
            BaseComponent.GetEnabledElement(
                string.Format(
                    "//a[contains(@class,'bbui-pages-datalistgrid-rowlink bbui-pages-datalistgrid-rowlinklinkaction') and contains(@title,'{0}')]",
                    FormatInviteeName(table.Rows[0]["Forename"], table.Rows[0]["Surname"])));

            foreach (var row in table.Rows)
            {
                if (!this.InviteeExists(FormatInviteeName(row["Forename"], row["Surname"])))
                {
                    throw new ArgumentException("Current event panel does not have the Invitee " + row[0]);
                }
            }
        }

        [Given(@"Event ""(.*)"" exists with Registration Option and start date ""(.*)""")]
        public void GivenEventExistsWithRegistrationOption(string eventName, string StartDate, Table options)
        {
            DateTime actualStartDate = StepHelper.SetTodayDateForVariable(StartDate);

            eventName += uniqueStamp;
            //navigate to event and add event
            BBCRMHomePage.OpenEventsFA();
            Panel.WaitClick(XpathHelper.xPath.VisiblePanel + "//button[./text()='Add new']");
            Panel.WaitClick("//span[./text()='Event']");
            //check is visible
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisibleBlock + "//span[text()='Add an event']");
            //set fields
            Dialog.SetTextField("//input[contains(@id,'_STARTDATE_value')]", actualStartDate.ToShortDateString());
            Dialog.SetTextField("//input[contains(@id,'_NAME_value')]", eventName);
            Dialog.SetTextField("//input[contains(@id,'_EVENTCATEGORYCODEID_value')]", "Sport");
            StepHelper.AddEntryOnTheFly();
            Dialog.Save();

            foreach (var option in options.Rows)
            {
                EventPanel.AddRegistrationOption(option);
            }
        }

        [Given(@"Event ""(.*)"" exists with Registration Option")]
        public void GivenEventExistsWithRegistrationOption(string eventName, Table options)
        {
            GivenAnEventExists(eventName);
            WhenIAddARegistrationOptionToEvent(eventName, options);
        }

        [Given(@"Constituent ""(.*)"" is registered for event named ""(.*)"" with ""(.*)"" registration option")]
        public void GivenConstituentIsRegisteredForEventNamedWithRegistrationOption(string ConstituentName, string EventName, string RegistrationType)
        {
            //var sectionCaption = "Registrants";
            var sectionCaption = "Registrations";
            var caption = "Registration option";
            var dialogId = "RegistrantUnifiedAddForm";
            var gridId = "_EVENTREGISTRANTS";
            ConstituentName += uniqueStamp;
            //click Registrations tab
            Panel.SelectTab(sectionCaption);
            //select Add
            Panel.ClickSectionAddButton(sectionCaption);
            Dialog.SetTextField("//input[contains(@id,'_CONSTITUENTID_value')]", ConstituentName);
            //set grid         
            string gridXPath = Dialog.getXGridCell(dialogId, gridId, 1, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), caption));
            Dialog.SetGridDropDown(gridXPath, RegistrationType);
            //save
            Dialog.Save();
        }

        [Given(@"Invitation to the event ""(.*)"" includes a mail package")]
        public void GivenInvitationToTheEventIncludesAMailPackage(string EventName, Table table)
        {
            #region data setup
            //setup date field.  StepHelper for date must come before dynamic objects
            StepHelper.SetTodayDateInTableRow("Mail date", table);
            dynamic objectData = table.CreateDynamicInstance();
            var dialogId = "InvitationAddForm";
            string sendType = objectData.HowToSendInvitation;
            string dateValue = string.Empty;
            EventName += uniqueStamp;
            objectData.Name += uniqueStamp;
            //sorts out date format due to datetime adding 00:00:00
            DateTime findDate = objectData.MailDate;
            //fields for adding invitation and mail package
            IDictionary<string, CrmField> Supportedfields = new Dictionary<string, CrmField>
            {
                {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
                {"Description", new CrmField("_DESCRIPTION_value", FieldType.TextArea)},
                {"Mail date", new CrmField("_MAILDATE_value", FieldType.TextInput)},
                {"Send through both mail and email, according to each constituent's preferences", new CrmField("__CHANNELCODE_0", FieldType.Checkbox)},
                {"Send through email only", new CrmField("_CHANNELCODE_1", FieldType.Checkbox)},
                {"Send through mail only", new CrmField("_CHANNELCODE_2", FieldType.Checkbox)},
                {"Mail package", new CrmField("_MAILPACKAGEID_value", FieldType.Searchlist, "PackageSearch", "_NAME_value")}
            };
            #endregion
            //navigation
            GetEventPanel(EventName);
            Panel.SelectTab("Invitations");
            Panel.ClickSectionAddButton("Invitations");
            //tab 1
            Dialog.SetField(dialogId, "Name", objectData.Name, Supportedfields);
            Dialog.SetField(dialogId, "Description", objectData.Description, Supportedfields);
            Dialog.SetField(dialogId, "Mail date", findDate.ToShortDateString(), Supportedfields);
            //tab 2
            Dialog.OpenTab("Processing Options");
            //wait for tab to load
            Dialog.GetEnabledElement(
                "//div[contains(@class, 'x-window bbui-dialog-tabbed bbui-dialog') and contains(@style,'visible')]//label[contains(@for,'CHANNELCODE_0')]",
                15);
            //switch case for how to send invitations
            switch (sendType)
            {
                case "Send through both mail and email, according to each constituent's preferences":
                    break;
                case "Send through email only":
                    Dialog.SetField(dialogId, "Send through email only", "true", Supportedfields);
                    break;
                case "Send through mail only":
                    Dialog.SetField(dialogId, "Send through mail only", "true", Supportedfields);
                    break;
                default:
                    FailTest(string.Format("Test failed checking send type {0}.", sendType));
                    break;
            }
            Dialog.SetField(dialogId, "Mail package", objectData.MailPackage + uniqueStamp, Supportedfields);
            //save dialog
            Dialog.Save();
        }

        [Given(@"I add Invitees to invitation ""(.*)""")]
        public void WhenIAddAnInviteeToAnInvitation(string InviteName, Table Invitees)
        {
            this.AddInvitees(InviteName, Invitees);
        }

        [Given(@"I send the invitation")]
        [When(@"I send the invitation")]
        public void WhenISendTheInvitation()
        {
            string inviteeList = "Invitee List";
            //lets set the thread culture to get the correct date for the browser
            StepHelper.SetCurrentThreadCultureToConfigValue();
            //use Business process method from Batch steps class
            BatchSteps BatchSteps = new BatchSteps();
            //click send    
            BaseComponent.WaitClick("//button[contains(@class,'bbui-linkbutton')]//div[contains(text(), 'Send')]");
            //check is visible
            BaseComponent.GetEnabledElement("//label[contains(@id, '_MAILDATE_caption')]");
            //click start
            Panel.ClickButton("Start");
            BatchSteps.ThenTheBatchCommitsWithoutErrorsOrExceptionsAndRecordProcessed(1);
            //Click Invitee list
            BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + string.Format("//button[contains(@class,'bbui-linkbutton') and ./text()='{0}']", inviteeList));
        }

        [When(@"I register the invitee")]
        public void WhenIRegisterTheInvitee(Table table)
        {
            IList<dynamic> objectData = table.CreateDynamicSet().ToList();
            string sectionCaption = "Invitees";
            string caption = "Registration option";
            string dialogId = "RegistrantfromInviteeUnifiedAddForm";
            string gridId = "_EVENTREGISTRANTS";
            string unnamed = "(Unnamed guest)";
            int i = 1;
            //check is visible
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[contains(@class, 'xtb-text bbui-pages-section-tbarcaption') and ./text()='Invitees']"));
            //select tab and expand invitee dropdown
            Panel.SelectTab(sectionCaption);
            BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + "//button[contains(@class, 'bbui-pages-section-expandbutton')]");
            //Click register
            Panel.ClickButton("Register");
            //Check is visible
            BaseComponent.GetEnabledElement("//div[contains(@style,'visible')]//label[contains(@for,'_CONSTITUENTID_value')]");
            //Add registrants
            foreach (dynamic registrant in objectData)
            {
                //Add Adult registration option  
                string gridXPath = Dialog.getXGridCell(dialogId, gridId, i, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), caption));
                Dialog.SetGridDropDown(gridXPath, registrant.RegistrationOption);
                //add name
                gridXPath = Dialog.getXGridCell(dialogId, gridId, i, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), "Registrant"));
                //Check if Registrant is Unnamed guest
                if (registrant.Registrant == unnamed)
                {
                    Dialog.SetGridDropDown(gridXPath, unnamed);
                }
                else
                {
                    Dialog.SetGridDropDown(gridXPath, registrant.Registrant + uniqueStamp);
                }
                i++;
            }
            //save
            Dialog.Save();
        }

        [Then(@"Registration\(s\) are listed on Registrant page")]
        public void ThenRegistrationSAreListedOnRegistrantPage(Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            //check fields
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_REGISTRATIONTYPE_value') and ./text()='{0}']", objectData.Type));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_REGISTRANTSTATUS_value') and ./text()='{0}']", objectData.Status));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//h4[contains(@id,'SECTIONROWVALUE_0') and ./text()='{0}']", objectData.Registrant + uniqueStamp));
            if (!string.IsNullOrEmpty(objectData.Guest1))
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//h4[contains(@id,'SECTIONROWVALUE_1') and ./text()='{0}']", objectData.Guest1 + uniqueStamp));
            }
            if (!string.IsNullOrEmpty(objectData.Guest2))
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//h4[contains(@id,'SECTIONROWVALUE_2') and ./text()='{0}']", objectData.Guest2 + uniqueStamp));
            }
        }

        [Then(@"Event ""(.*)"" displays registrant\(s\) on Registrations tab")]
        public void ThenEventDisplaysRegistrantSOnRegistrationsTab(string EventName, Table table)
        {
            IList<dynamic> registrants = table.CreateDynamicSet().ToList();
            //navigate to Event 
            EventName += uniqueStamp;

            BBCRMHomePage.OpenEventsFA();
            FunctionalArea.OpenLink("Fundraising events", "Event search");
            BaseComponent.SetTextField("//div[contains(@style,'visible')]//input[contains(@id,'_NAME_value')]", EventName);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();

            //test for "Cannot read property 'createChild' of null" popup?
            try
            {
                BaseComponent.GetEnabledElement("//span[contains(.,'Cannot read property')]", 5);
                Dialog.OK();
            }
            catch
            {
                //eat and move on
            }

            //Click Registrations tab
            Panel.SelectTab("Registrations");
            foreach (dynamic registrant in registrants)
            {
                string guestOf = registrant.Extra + " " + registrant.RegistrantFirstName + " " + registrant.RegistrantLastName + uniqueStamp;
                guestOf = guestOf.Trim();
                string registeredConstituent = registrant.RegistrantLastName + uniqueStamp + ", " + registrant.RegistrantFirstName;
                //check fields
                if (!string.IsNullOrEmpty(registrant.Extra))
                {
                    BaseComponent.GetEnabledElement(string.Format(XpathHelper.xPath.VisiblePanel + "//div[not(contains(@class,'x-hide-display')) and contains(@class,'bbui-pages-pagesection') and not(contains(@class,'row'))]//a[text()='{0}']", guestOf));
                }
                else
                {
                    BaseComponent.GetEnabledElement(string.Format(XpathHelper.xPath.VisiblePanel + "//div[not(contains(@class,'x-hide-display')) and contains(@class,'bbui-pages-pagesection') and not(contains(@class,'row'))]//a[text()='{0}']", registeredConstituent));

                }
                BaseComponent.GetEnabledElement(string.Format(XpathHelper.xPath.VisiblePanel + "//div[not(contains(@class,'x-hide-display')) and contains(@class,'bbui-pages-pagesection') and not(contains(@class,'row'))]//div[text()='{0}']", Convert.ToString(registrant.Balance)));
            }
        }

        [When(@"I mark the invitee ""(.*)"" as declined")]
        public void WhenIMarkTheInviteeAsDeclined(string inviteeName)
        {
            var sectionCaption = "Invitees";
            //check is visible
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[contains(@class, 'xtb-text bbui-pages-section-tbarcaption') and ./text()='Invitees']"));
            //select tab and expand invitee dropdown
            Panel.SelectTab(sectionCaption);
            BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + "//button[contains(@class, 'bbui-pages-section-expandbutton')]");
            //Click Mark as declined
            Panel.ClickButton("Mark as declined");
            Dialog.ClickButton("Yes");
        }

        [Then(@"Invitees list displays where Declined is ""(.*)""")]
        public void ThenInviteesListDisplaysWhereDeclinedIs(string Declined, Table table)
        {
            string sectionCaption = "Invitees";
            StepHelper.SetTodayDateInTableRow("Invitation sent on", table);
            //name format is assumed to be "firstname Surname" we need to reformat to "surnameUniqustamp, forename" like so:
            string[] inviteeNames = table.Rows[0]["Invitee"].Split(' ');
            string invitee = inviteeNames[1] + uniqueStamp + ", " + inviteeNames[0];

            TableRow tr = table.Rows[0];
            tr["Invitee"] = invitee;

            if (Panel.SectionDatalistRowExists(table.Rows[0], sectionCaption) != true)
            {
                throw new Exception("Data for " + invitee + " was not displayed as expected!");
            }
            if (Convert.ToBoolean(Declined) != false)
            {
                //check box is class driven by the style "bbui-pages-datalistgrid-check"
                //so we need to find the declined and then nav to the check
                Panel.GetEnabledElement("//div[contains(@style,'visible')]//td/div[./text()='Declined' and not(contains(@title,'Declined'))]/../../../../../../../../div[contains(@class,'x-grid3-scroller')]/div/div/table/tbody/tr/td[contains(@class,'x-grid3-col x-grid3-cell x-grid3-td-formattedValues[8]  bbui-pages-datalistgrid-check')]");
            }
        }

        [Then(@"I navigate to payment from Event")]
        public void ThenINavigateToPaymentFromEvent(Table table)
        {
            #region data setup
            //setup date field.  StepHelper for date must come before dynamic objects
            StepHelper.SetTodayDateInTableRow("Date", table);
            dynamic objectData = table.CreateDynamicInstance();
            string sectionCaption = "Registrations";
            //sorts out date format due to datetime adding 00:00:00
            DateTime findDate = objectData.Date;
            string eventAmount = string.Format("{0} on {1}", objectData.PaymentAmount, findDate.ToShortDateString());
            //string regXPath = string.Format("//a[contains(@title,'{0}') and ./text()='{0}']", objectData.Surname + uniqueStamp + ", " + objectData.FirstName);
            string regXPath = XpathHelper.xPath.VisiblePanel + string.Format("//a[./text()='{0}']", objectData.Surname + uniqueStamp + ", " + objectData.FirstName);
            #endregion
            //check is visible
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[contains(@class, 'xtb-text bbui-pages-section-tbarcaption') and ./text()='{0}']", sectionCaption));
            try
            {
                //Click Go to registrant
                BaseComponent.GetEnabledElement(regXPath);
                BaseComponent.WaitClick(regXPath, 20);
            }
            catch (WebDriverTimeoutException ex)
            {
                //lets try that again if the first time fails
                BaseComponent.GetEnabledElement(regXPath);
                BaseComponent.WaitClick(regXPath, 30);
            }

            //Click payment history link          
            try
            {
                BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + string.Format("//tr[contains(@id,'_PAYMENT1TEXT_container')]/td/a[./text()='{0}']", eventAmount));
            }
            catch (Exception ex)
            {
                throw new Exception(eventAmount + " does not display", ex);
            }
        }

        #region Private
        private void GetEventPanel(string eventName)
        {
            if (!Panel.IsPanelHeader(eventName))
            {
                BBCRMHomePage.OpenEventsFA();
                //Panel.CollapseSection("Event calendar", "CalendarViewForm"); // this causes an issues sometimes?
                FunctionalArea.OpenLink("Fundraising events", "Event search");
                BaseComponent.SetTextField("//div[contains(@style,'visible')]//input[contains(@id,'_NAME_value')]", eventName);
                SearchDialog.Search();
                SearchDialog.SelectFirstResult();
            }
        }

        private void AddInvitations(TableRow fields)
        {
            IDictionary<string, CrmField> Supportedfields = new Dictionary<string, CrmField>
            {
                {"Name", new CrmField("_NAME_value", FieldType.TextInput)},
                {"Description", new CrmField("_DESCRIPTION_value", FieldType.TextArea)},
                {"Mail date", new CrmField("_MAILDATE_value", FieldType.TextInput)},
                {"Send through mail only", new CrmField("_CHANNELCODE_2", FieldType.Checkbox)},
                {"Mail package", new CrmField("_MAILPACKAGEID_value", FieldType.TextInput)}
            };
            Panel.SelectTab("Invitations");
            Panel.ClickSectionAddButton("Invitations");
            //tab 1
            Dialog.SetField("InvitationAddForm", "Name", fields[0], Supportedfields);
            Dialog.SetField("InvitationAddForm", "Description", fields[1], Supportedfields);
            Dialog.SetField("InvitationAddForm", "Mail date", fields[2], Supportedfields);
            Dialog.Save();
        }

        private void AddInvitations(String inviteName)
        {
            var headers = new string[3] { "Name", "Description", "Mail date" };
            var parameters = new string[3] { inviteName, "description", "01/05/2015" };

            var _table = new Table(headers);
            _table.AddRow(parameters);

            AddInvitations(_table.Rows[0]);
        }

        private void SearchAndSelectConstituent(string ConstituentName)
        {
            var names = new string[2];
            names = ConstituentName.Split(' ');
            BBCRMHomePage.OpenConstituentsFA();
            ConstituentsFunctionalArea.OpenConstituentSearchDialog();
            SearchDialog.SetFirstNameToSearch(names[0]);
            SearchDialog.SetLastNameToSearch(names[1] + uniqueStamp);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }

        private string FormatInviteeName(string forename, string surname)
        {
            return surname + uniqueStamp + ", " + forename;
        }

        private bool InviteeExists(string inviteesName)
        {
            var returnValue = false;

            const string visibleContainer =
                "//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'x-hide-display'))]";

            try
            {
                //find in grid
                if (
                    BaseComponent.GetEnabledElement(
                        visibleContainer + string.Format("//a[contains(@title,'{0}') and ./text()='{0}']", inviteesName),
                        10) != null)
                {
                    returnValue = true;
                }
            }
            catch
            {
                //eat exception
            }

            return returnValue;
        }

        private void AddInvitees(string inviteName, Table invitees)
        {
            //move to invite
            SelectInvite(inviteName);
            foreach (var row in invitees.Rows)
            {
                //add invitees - this assumes the Constituent exists
                IDictionary<string, CrmField> Supportedfields = new Dictionary<string, CrmField>
                {
                    {"Invitee", new CrmField("_CONSTITUENTID_value", FieldType.TextInput)}
                };

                var headers = new string[1] { "Invitee" };
                var parameters = new string[1] { row[0] + uniqueStamp };

                var fields = new Table(headers);
                fields.AddRow(parameters);
                //select add button
                Panel.ClickSectionAddButton("Invitees");
                //select Constituent dropdown
                BaseComponent.WaitClick("//span[contains(@class,'x-menu-item-text') and contains(.,'Constituent')]");
                //set fields
                Dialog.SetFields("InviteeAddForm", fields.Rows[0], Supportedfields);
                Dialog.Save();
            }
        }

        private void SelectInvite(string Invite)
        {
            //move to "inviteName" link
            BaseComponent.WaitClick(
                string.Format(
                    "//*[contains(@class,'bbui-datalist-container') and contains(@style,'visible')]//a[contains(@class,'bbui-pages-datalistgrid-rowlink bbui-pages-datalistgrid-rowlinklinkaction') and contains(@title,'{0}')]",
                    Invite));
        }
        #endregion
    }
}
