using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Base;
using Blackbaud.UAT.Core.Crm;
using Blackbaud.UAT.Core.Base;
using SystemTest.Common;
using TechTalk.SpecFlow.Assist;
using Blackbaud.UAT.Core.Crm.Dialogs;
using Blackbaud.UAT.Core.Crm.Panels;
using SystemTest.Common.Crm;

namespace SystemTest.Core.Steps
{
    [Binding]
    public sealed class RevenueSteps : BaseSteps
    {
        private struct RecurringGiftConstants
        {
            public const string Date = "Date";
            public const string InstallmentScheduleBegins = "Installment schedule begins";
            public const string EndDate = "End date (optional)";
            public const string Today = "today";
        }

        [Given(@"I add a Payment to constituent ""(.*)"" with account system ""(.*)""")]
        [When(@"I add a Payment to constituent ""(.*)"" with account system ""(.*)""")]
        public void WhenIAddAPaymentToConstituentWithAccountSystem(string ConstituentName, string AccountSystem, Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            var anon = false;
            if (!string.IsNullOrEmpty(Convert.ToString(objectData.PaymentIsAnonymous))) anon = Convert.ToBoolean(objectData.PaymentIsAnonymous);
            Boolean DeclinesGiftAid = false;
            if (((IDictionary<String, object>)objectData).ContainsKey("ConsituentDeclinesGiftAidForThisApplication") &&
                !string.IsNullOrEmpty(Convert.ToString(objectData.ConsituentDeclinesGiftAidForThisApplication)) &&
                Convert.ToString(objectData.ConsituentDeclinesGiftAidForThisApplication).ToLower() == "checked")
            {
                DeclinesGiftAid = true;
            }
            //search and select constituent
            StepHelper.SearchAndSelectConstituent(ConstituentName);
            //click add payment
            BaseComponent.WaitClick(string.Format(XpathHelper.xPath.Button, XpathHelper.PaymentAddActions.Payment));
            //check is visible
            BaseComponent.GetEnabledElement(XpathHelper.xPath.ConstituentCaption);
            //set fields
            StepHelper.SetAccountSystem(AccountSystem);
            StepHelper.SetCurrentThreadCultureToConfigValue();
            switch ((string)(objectData.Application).ToLower())
            {
                case "pledge":
                    PaymentPledge(objectData);
                    break;
                case "event registration":
                    PaymentEventRegistration(objectData);
                    break;
                default:
                    //Donation
                    PaymentDonation(objectData, DeclinesGiftAid, anon);
                    break;
            }
            if ((string)(objectData.Application).ToLower() != "pledge") Dialog.Save();
        }

        private void PaymentPledge(dynamic objectData)
        {
            //set fields
            Dialog.SetDropDown("//input[contains(@id,'_APPLICATIONCODE_value')]", objectData.Application);
            Dialog.SetTextField("//input[contains(@id,'_AMOUNT_value')]", objectData.Amount);
            Dialog.SetTextField("//input[contains(@id,'_DATE_value')]", DateTime.Now.ToShortDateString());
            string paymentMethod = Convert.ToString(objectData.PaymentMethod);
            if (paymentMethod.ToLower() != "check")
            {
                Dialog.SetDropDown("//input[contains(@id,'_PAYMENTMETHODCODE_value')]", objectData.PaymentMethod);
            }
            //select pledge
            Dialog.WaitClick(string.Format("//div[./text()='Designations: {0}']", objectData.Designation));
            //click Add
            Dialog.WaitClick("//table[contains(@id,'_ADDSELECTEDCOMMITMENT_action')]//button[./text()='Add']");
            try
            {
                BaseComponent.GetEnabledElement("//span[./text()='Amount to apply']", 5);
            }
            catch
            {
                Dialog.WaitClick("//table[contains(@id,'_ADDSELECTEDCOMMITMENT_action')]//button[./text()='Add']");
            }
            //handle plegde amount pop up
            Dialog.WaitClick("//input[contains(@id,'_APPLYTO_3')]");
            Dialog.OK();
            //save main form
            Dialog.Save();
        }

        private void PaymentEventRegistration(dynamic objectData)
        {
            Dialog.SetDropDown("//input[contains(@id,'_APPLICATIONCODE_value')]", objectData.Application);
            Dialog.SetTextField("//input[contains(@id,'_AMOUNT_value')]", objectData.Amount);
            Dialog.SetTextField("//input[contains(@id,'_DATE_value')]", DateTime.Now.ToShortDateString());
            string paymentMethod = Convert.ToString(objectData.PaymentMethod);
            if (paymentMethod.ToLower() != "check")
            {
                Dialog.SetDropDown("//input[contains(@id,'_PAYMENTMETHODCODE_value')]", objectData.PaymentMethod);
            }
            //select event
            BaseComponent.WaitClick(XpathHelper.xPath.VisibleDialog + "//div[contains(@id,'OUTSTANDINGCOMMITMENTS_GRID')]//div[contains(@class,'x-grid3-body')]/div/table");
            //select add
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisibleDialog + "//div[contains(@id,'_ADDSELECTEDCOMMITMENTSACTION_WRAP')]//button[./text()='Add']", 30);
            BaseComponent.WaitClick(XpathHelper.xPath.VisibleDialog + "//div[contains(@id,'_ADDSELECTEDCOMMITMENTSACTION_WRAP')]//button[./text()='Add']", 10);

            try
            {
                //click OK on popup
                BaseComponent.WaitClick(XpathHelper.xPath.VisibleDialog + "//button[./text()='OK']", 10);
            }
            catch
            {
                //select add and then click OK on popup
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisibleDialog + "//div[contains(@id,'_ADDSELECTEDCOMMITMENTSACTION_WRAP')]//button[./text()='Add']", 10);
                BaseComponent.WaitClick(XpathHelper.xPath.VisibleDialog + "//div[contains(@id,'_ADDSELECTEDCOMMITMENTSACTION_WRAP')]//button[./text()='Add']", 10);
                BaseComponent.WaitClick(XpathHelper.xPath.VisibleDialog + "//button[./text()='OK']", 10);
            }
        }

        private void PaymentDonation(dynamic objectData, Boolean DeclinesGiftAid, Boolean Anon)
        {
            string paymentMethod = Convert.ToString(objectData.PaymentMethod);
            //set fields
            Dialog.SetDropDown("//input[contains(@id,'_APPLICATIONCODE_value')]", objectData.Application);
            SetDesignation("_DONATIONDESIGNATIONID_value", objectData.Designation);
            Dialog.SetTextField("//input[contains(@id,'_AMOUNT_value')]", objectData.Amount);
            Dialog.SetTextField("//input[contains(@id,'_DATE_value')]", DateTime.Now.ToShortDateString());
            if (paymentMethod.ToLower() != "check")
            {
                Dialog.SetDropDown("//input[contains(@id,'_PAYMENTMETHODCODE_value')]", objectData.PaymentMethod);
            }

            if (paymentMethod.ToLower().Equals("stock"))
            {
                //handle the fact that 1.00 is converted to an int and recast to a decimal
                decimal numberOfUnits = Convert.ToDecimal(objectData.NumberOfUnits + ".000");
                string med = Convert.ToString(objectData.PricePerShareMedian);
                decimal pricePerShareMedian;
                if (med.Contains("."))
                {
                    pricePerShareMedian = Convert.ToDecimal(objectData.PricePerShareMedian + "00");
                }
                else
                {
                    pricePerShareMedian = Convert.ToDecimal(objectData.PricePerShareMedian + ".00");
                }
                Dialog.SetTextField("//input[contains(@id,'_ISSUER_value')]", objectData.Issuer);
                Dialog.SetTextField("//input[contains(@id,'_NUMBEROFUNITS_value')]", numberOfUnits.ToString());
                Dialog.SetTextField("//input[contains(@id,'_MEDIANPRICE_value')]", pricePerShareMedian.ToString());
            }

            try
            {
                //Consituent Declines Gift Aid for this application can be disabled - let's get it and bail if it is
                Dialog.GetEnabledElement("//input[contains(@id,'DONATIONDECLINESGIFTAID_value')]", 5);
                Dialog.SetCheckbox("//input[contains(@id,'DONATIONDECLINESGIFTAID_value')]", DeclinesGiftAid.ToString());
            }
            catch
            {
                //eat as "Consituent Declines Gift Aid for this application can be disabled" is not enabled
            }

            if (Anon)
            {
                Dialog.SetCheckbox("//input[contains(@id,'_GIVENANONYMOUSLY_value')]", "true");
            }
            if (!string.IsNullOrEmpty((string)(objectData.Reference)))
            {
                Dialog.SetTextField("//input[contains(@id,'_REFERENCE_value')]", objectData.Reference);
            }
            if (!string.IsNullOrEmpty((string)(objectData.Benefit)))
            {
                StepHelper.SetBenefit(objectData.Benefit, false);
            }
            BaseComponent.WaitClick("//div[contains(@id,'_DONATIONDESIGNATIONID_container')]//button[contains(@class, 'x-btn-text') and ./text()='Add']");
            Dialog.GetEnabledElement(string.Format("//div[contains(@class,'x-grid3-cell-inner x-grid3-col-APPLICATIONCODE') and ./text()='{0}']", objectData.Application));
        }

        private void SetDesignation(string xpath, string Designation)
        {
            //popup and search for the Designation
            Dialog.WaitClick(string.Format("//input[contains(@id,'{0}')]/../span/img[contains(@class,'x-form-search-trigger')]", xpath));
            Dialog.SetTextField("//input[contains(@id,'_COMBINEDSEARCH_value')]", Designation);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
        }

        [Then(@"the revenue record is presented on the constituent")]
        public void ThenTheRevenueRecordIsPresentedOnTheConstituent()
        {
            BaseComponent.GetEnabledElement("//div[contains(@class,'xtb-text bbui-pages-section-tbarcaption') and ./text()='Donation - $100.00']");
        }

        [Given(@"A Pledge has been submitted today for the Constituent ""(.*)"" with account system ""(.*)""")]
        public void GivenAPledgeHasBeenSubmittedTodayForTheConstituent(string ConstituentName, string AccountSystem, Table table)
        {
            //lets set the thread culture to get the correct date for the browser
            StepHelper.SetTodayDateInTableRow("Start date", table);
            dynamic objectData = table.CreateDynamicInstance();
            DateTime startDate = objectData.StartDate;
            //search and select constituent
            StepHelper.SearchAndSelectConstituent(ConstituentName);
            //click add payment
            BaseComponent.WaitClick(string.Format(XpathHelper.xPath.Button, XpathHelper.PaymentAddActions.Pledge));
            //check for is visible
            BaseComponent.GetEnabledElement(XpathHelper.xPath.ConstituentCaption);
            //set the account system
            StepHelper.SetAccountSystem(AccountSystem);
            //set fields - general
            Dialog.SetTextField("//input[contains(@id,'_AMOUNT_value')]", Convert.ToString(objectData.Amount));
            Dialog.SetTextField("//input[contains(@id,'_DATE_value')]", startDate.ToShortDateString());
            SetDesignation("_SINGLEDESIGNATIONID_value", objectData.Designation);
            //set fields - installments
            Dialog.SetTextField("//input[contains(@id,'_STARTDATE_value')]", startDate.ToShortDateString());
            if (!string.IsNullOrEmpty((string)(objectData.Installments)))
            {
                Dialog.SetDropDown("//input[contains(@id,'_FREQUENCYCODE_value')]", objectData.Installments);
            }
            if (!string.IsNullOrEmpty((string)(Convert.ToString(objectData.NumberOfInstallments))))
            {
                Dialog.SetTextField("//input[contains(@id,'_NUMBEROFINSTALLMENTS_value')]", Convert.ToString(objectData.NumberOfInstallments));
            }
            Dialog.SetDropDown("//input[contains(@id,'_POSTSTATUSCODE_value')]", objectData.PostType);
            //save
            Dialog.Save();
        }

        [Then(@"the revenue record is presented on the constituent as Applied to the Pledge for the amount ""(.*)""")]
        public void ThenTheRevenueRecordIsPresentedOnTheConstituentAsAppliedToThePledgeForTheAmount(string amount)
        {
            string donationAmount = string.Format("Pledge - {0}", amount);
            try
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[contains(@class,'xtb-text bbui-pages-section-tbarcaption') and ./text()='{0}']", Convert.ToString(donationAmount)));
            }
            catch (Exception ex)
            {
                throw new Exception("Pledge payment not present on record " + donationAmount, ex);
            }
        }

        [Then(@"the Pledge record balance is reduced by the payment amount value for constituent ""(.*)""")]
        public void ThenThePledgeRecordBalanceIsReducedByThePaymentAmountValue(string ConstituentName)
        {
            var sectionCaption = "Designations";
            //lets set the thread culture to get the correct date for the browser
            StepHelper.SetCurrentThreadCultureToConfigValue();
            //navigate link "<date> Pledge for <Constituent>"
            var linkLabel = DateTime.Now.ToShortDateString() + " Pledge for " + ConstituentName + uniqueStamp;
            BaseComponent.WaitClick(string.Format("//a[contains(@id,'_APPLIEDNAME_value') and ./text()='{0}']", linkLabel));
            //check amount
            IDictionary<string, string> rowValues = new Dictionary<string, string>();
            rowValues.Add("Amount", "$1,200.00");
            rowValues.Add("Balance", "$1,100.00");
            //check grid loaded
            if (!Panel.SectionDatalistRowExists(rowValues, sectionCaption))
            {
                throw new Exception("Expected values not in the grid, no Amount and/or Balance");
            }
        }

        [Then(@"the Pledge Installment/Write-off Activity tab shows the payment linked to an installment")]
        public void ThenThePledgeInstallmentWrite_OffActivityTabShowsThePaymentLinkedToAnInstallment()
        {
            //select tab 
            Panel.SelectTab("Installment/Write-off Activity");
            //Installment 1 should exists 
            try
            {
                BaseComponent.GetEnabledElement("//span[contains(@title, 'Installment 1') and ./text()='Installment 1']");
            }
            catch (Exception ex)
            {
                throw new Exception("No 'Installment 1' in payment grid!", ex);
            }
            //and have sub-line payment
            try
            {
                BaseComponent.GetEnabledElement("//a[contains(@class,'bbui-pages-datalistgrid-rowlink bbui-pages-datalistgrid-rowlinklinkaction') and ./text()='Payment']");
            }
            catch (Exception ex)
            {
                throw new Exception("No payment link exists!", ex);
            }
        }

        [Then(@"the revenue record is presented on the constituent ""(.*)"" as Applied to Event Registration for ""(.*)""")]
        public void ThenTheRevenueRecordIsPresentedOnTheConstituentAsAppliedToEventRegistrationFor(string ConstituentName, string EventName)
        {
            //check event link is there correctly
            var linkText = string.Format("Event registration for {0}: {1}", ConstituentName + uniqueStamp, EventName + uniqueStamp);
            try
            {
                BaseComponent.GetEnabledElement(string.Format("//a[contains(@id,'_APPLIEDNAME_value') and ./text()='{0}']", linkText));
            }
            catch (Exception ex)
            {
                throw new Exception("ThenTheRevenueRecordIsPresentedOnTheConstituentAsAppliedToEventRegistrationFor: Event or constituent not as expected!", ex);
            }
        }

        [Then(@"Balance on Registration fee is ""(.*)"" for constituent ""(.*)"" as Applied to Event Registration for ""(.*)""")]
        public void ThenBalanceOnRegistrationFeeIsForConstituentAsAppliedToEventRegistrationFor(string Balance, string ConstituentName, string EventName)
        {
            //click link
            var linkText = string.Format("Event registration for {0}: {1}", ConstituentName + uniqueStamp, EventName + uniqueStamp);
            BaseComponent.WaitClick(string.Format("//a[contains(@id,'_APPLIEDNAME_value') and ./text()='{0}']", linkText));
            //check balance
            try
            {
                BaseComponent.GetEnabledElement(string.Format("//span[contains(@id,'_FEEBALANCE_value') and ./text()='{0}']", Balance));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Balance not {0} as expected!", Balance), ex);
            }
        }

        [Given(@"designation exists '(.*)'")]
        public void GivenDesignationExists(string designation)
        {
            BBCRMHomePage.OpenFundraisingFA();
            if (!FundraisingFunctionalArea.DesignationExists(designation))
                throw new ArgumentException(String.Format("Designation '{0}' does not exist."), designation);
        }

        [When(@"I start to add a pledge")]
        public void WhenIStartToAddAPledge(Table pledgeValues)
        {
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.AddAPledge();
            foreach (var pledgeValueRow in pledgeValues.Rows)
            {
                if (pledgeValueRow.ContainsKey("Constituent") && pledgeValueRow["Constituent"] != string.Empty) pledgeValueRow["Constituent"] += uniqueStamp;
                Dialog.SetTextField("//input[contains(@id,'_CONSTITUENTID_value')]", pledgeValueRow["Constituent"]);
                Dialog.GetEnabledElement("//input[contains(@id,'_AMOUNT_value')]");
                Dialog.SetTextField("//input[contains(@id,'_AMOUNT_value')]", pledgeValueRow["Amount"]);
            }
        }

        [When(@"split the pledge designations evenly")]
        public void WhenSplitThePledgeDesignationsEvenly(Table designations)
        {
            PledgeDialog.SplitDesignationsEvenly(designations);
        }

        [When(@"save the pledge")]
        public void WhenSaveThePledge()
        {
            Dialog.Save();
        }

        [Then(@"a pledge exists with designations")]
        public void ThenAPledgeExistsWithDesignations(Table designations)
        {
            foreach (var designation in designations.Rows)
            {
                if (!PledgePanel.DesignationExists(designation))
                    throw new ArgumentException(String.Format("Designation '{0}' does not exist for current pledge.", designation));
            }
        }

        [Given(@"a revenue type selection exists for the constituent ""(.*)"" donation payment called ""(.*)""")]
        public void GivenARevenueTypeSelectionExistsForTheConstituentDonationPaymentCalled(string ConstituentName, string SelectionName)
        {
            ConstituentName += uniqueStamp;
            //create ad-hoc query for ConstituentName
            AdHocSteps adhocSteps = new AdHocSteps();
            adhocSteps.WhenIAddAAd_HocQuery("Revenue");

            string[] headers = new string[2] { "field", "value" };
            string[] FirstRow = new string[2] { "FILTEROPERATOR", "Equal to" };
            string[] SecondRow = new string[2] { "VALUE1", ConstituentName };

            Table table = new Table(headers);
            table.AddRow(FirstRow);
            table.AddRow(SecondRow);

            NewAdhocQueryDialog.SetFindField("Last/Organization/Group/Household name");
            NewAdhocQueryDialog.IncludeRecordCriteria(@"Constituent\Last/Organization/Group/Household name", table);

            //save as static selection
            headers = new string[2] { "field", "value" };
            FirstRow = new string[2] { "Name", SelectionName };
            SecondRow = new string[2] { "Description", SelectionName };

            table = new Table(headers);
            table.AddRow(FirstRow);
            table.AddRow(SecondRow);

            //set selection to static for GL
            Dialog.OpenTab("Set save options");
            Dialog.SetCheckbox(Dialog.getXInput("AdHocQuerySaveOptionsViewDataForm", "CREATESELECTION_value"), true);
            Dialog.SetCheckbox(Dialog.getXInput("AdHocQuerySaveOptionsViewDataForm", "SELECTION_TYPE_1"), true);
            Dialog.OpenTab("Select filter and output fields");

            adhocSteps.WhenSaveWithTheFollowingOptions(table);
        }

        [Given(@"a Post to GL process exists")]
        [When(@"a Post to GL process exists")]
        public void WhenAPostToGLProcessExists(Table table)
        {
            string visibleDialog = "//div[contains(@class,'x-window bbui-dialog-tabbed bbui-dialog') and contains(@style,'visible')]";
            string dialogInput = "//input[contains(@id,'{0}')]";
            dynamic objectData = table.CreateDynamicInstance();
            //Navigate and add
            BBCRMHomePage.OpenRevenueFA();
            BaseComponent.GetEnabledElement("//div[contains(@class,'bbui-pages-contentcontainer')]//div[./text()='Post revenue to GL']");
            RevenueFunctionalArea.OpenLink("Processing", "Post revenue to GL");
            Panel.ClickSectionAddButton("Post to GL processes");
            //set fields
            Dialog.SetTextField("//input[contains(@id,'_NAME_value')]", objectData.Name + uniqueStamp);
            Dialog.SetTextField("//input[contains(@id,'_PDACCOUNTSYSTEMID_value')]", objectData.AccountSystem);
            Dialog.SetTextField("//input[contains(@id,'_BUSINESSPROCESSVIEWID_value')]", objectData.OutputFormat);
            //dropdowns
            Dialog.SetDropDown("//input[contains(@id,'_POSTDATEUPTOCODE_value')]", objectData.PostDateUpTo);
            Dialog.SetDropDown("//input[contains(@id,'_DEPOSITPOSTINGOPTIONCODE_value')]", objectData.Deposits);
            Dialog.SetDropDown("//input[contains(@id,'_POSTINGOPTIONCODE_value')]", objectData.Revenue);
            Dialog.SetDropDown("//input[contains(@id,'_ADJUSTMENTPOSTINGOPTIONCODE_value')]", objectData.BankAccountAdjustments);
            //handle popup selection
            BaseComponent.GetEnabledElement(visibleDialog + string.Format(dialogInput, "_IDSETREGISTERID_value"));
            BaseComponent.WaitClick(visibleDialog + string.Format(dialogInput, "_IDSETREGISTERID_value") + @"/../span/img");
            Dialog.SetTextField("//div[contains(@id,'searchdialog_')]//input[contains(@id,'_NAME_value')]", objectData.Selection + uniqueStamp);
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
            Dialog.SetCheckbox("//input[contains(@id,'_MARKASPOSTED_value')]", Convert.ToString(objectData.MarkRevenueRecordsPosted));
            //save original form
            Dialog.Save();
        }

        [Given(@"I Start the ""(.*)""")]
        [When(@"I Start the ""(.*)""")]
        public void WhenIStartThe(string ProcessName)
        {
            var caption = "Name";
            var sectionCaption = "Post to GL processes";
            ProcessName += uniqueStamp;

            string numberOfItems =
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel +
                                                "//div[contains(@class,'xtb-text bbui-pages-section-tbarcaption-detail')]")
                    .Text;
            numberOfItems = numberOfItems.Replace("(", " ");
            numberOfItems = numberOfItems.Replace(")", " ");
            var actualNumberOfItems = double.MinValue;

            if (!double.TryParse(numberOfItems, out actualNumberOfItems))
            {
                throw new Exception("Error getting page number as double!");
            }

            int actualNumberOfPages = Convert.ToInt32(Math.Ceiling(actualNumberOfItems / 30)); //30 items per page for this list

            //lets try the last page first and revert to looping through them if the process is not on the last one
            try
            {
                BaseComponent.WaitClick(string.Format(XpathHelper.xPath.VisiblePanel + "//button[contains(@data-pagenumber, '{0}')]", actualNumberOfPages.ToString()), 5);
                RunGLProcess(ProcessName, caption, sectionCaption, 60.00d);
            }
            catch
            {
                //process was not on the last page lets loop through for it - move to the first page first if we are not there already
                try
                {
                    BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//button[contains(@data-pagenumber, '1')]", 5);
                    BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + "//button[contains(@data-pagenumber, '1')]");
                }
                catch { }
                for (var i = 0; i <= actualNumberOfPages; i++)
                {
                    try
                    {
                        RunGLProcess(ProcessName, caption, sectionCaption, 15.00d);
                        break;
                    }
                    catch //(Exception Ex)
                    {
                        //eat exception - should just mean it's not visisble on the page yet
                    }
                    //move to next page if not on the last page
                    if (i != actualNumberOfPages)
                    {
                        BaseComponent.GetEnabledElement(
                            string.Format(XpathHelper.xPath.VisiblePanel + "//button[contains(@data-pagenumber, '{0}')]", i + 2)).Click();
                    }
                }
            }
            //wait for completed screen
            try
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//span[contains(@id,'_STATUS_value') and ./text()='Completed']");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Process 'Completed' screen not rendered - Process ({0}) failed to run!", ProcessName), ex);
            }
        }

        private void RunGLProcess(string ProcessName, string Caption, string SectionCaption, double TimeToWait)
        {
            if (BaseComponent.GetEnabledElement(string.Format(XpathHelper.xPath.VisiblePanel + "//a[contains(@title,'{0}') and ./text()='{0}']", ProcessName), TimeToWait) != null)
            {
                //expand mail package
                IDictionary<string, string> rowValues = new Dictionary<string, string>();
                rowValues.Add(Caption, ProcessName);

                IDictionary<string, int> columnIndex = new Dictionary<string, int>();
                columnIndex.Add(Caption,
                    Panel.GetDatalistColumnIndex(Panel.getXSectionDatalistColumnHeaders(SectionCaption), Caption));
                //expand row
                Panel.ExpandRow(Panel.getXSectionDatalistRow(SectionCaption, columnIndex, rowValues));
                //click start process
                BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel +
                                        "//td[contains(@class,'x-toolbar-cell') and not(contains(@class,'x-hide-display'))]//button[./text()='Start process']");
                Dialog.ClickButton("Start");
            }
        }

        [Given(@"the process ""(.*)"" runs without error")]
        [Then(@"the process ""(.*)"" runs without error")]
        public void ThenTheProcessRunsWithoutError(string ProcessName)
        {
            ProcessName += uniqueStamp;
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//h2/span[./text()='{0}']", ProcessName));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + "//span[contains(@id,'_STATUS_value') and ./text()='Completed']");
        }

        [When(@"I edit posted payment for constituent ""(.*)"" for type ""(.*)""")]
        public void WhenIEditPostedPaymentForConstituent(string ConstituentName, string type)
        {
            StepHelper.SearchAndSelectConstituent(ConstituentName);
            //select revenue tab
            Panel.SelectTab("Revenue");
            //select $100.00
            if (type.ToLower() == "default")
            {
                //selects top link
                Panel.WaitClick("//a[contains(@id,'_REVENUELINKFIRST_action') and ./text()='$100.00']");
            }
            else
            {
                //selects bottom link
                Panel.WaitClick("//a[contains(@id,'_REVENUELINKLATEST_action') and ./text()='$100.00']");
            }
            //edit posted payments
            Panel.WaitClick("//button/div[./text()='Edit posted payment']");
        }

        [When(@"I Remove the Current applications")]
        public void WhenIRemoveTheCurrentApplications()
        {
            string dialogId = "dataformdialog_";
            string gridId = "_REVENUESTREAMS_value";
            string gridXPath = Dialog.getXGridCell(dialogId, gridId, 1, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders(dialogId, gridId), "Application"));
            //select row
            Dialog.GetEnabledElement(gridXPath, 15);
            Dialog.WaitClick(gridXPath);
            //remove
            Dialog.GetEnabledElement("//table[contains(@id,'_REMOVESELECTEDAPPLICATION_action')]//button[./text()='Remove']", 15);
            Dialog.WaitClick("//table[contains(@id,'_REMOVESELECTEDAPPLICATION_action')]//button[./text()='Remove']");
            try
            {
                //sometime the remove does not seem to work - handle it here
                Dialog.WaitClick(gridXPath, 10);
                Dialog.GetEnabledElement("//table[contains(@id,'_REMOVESELECTEDAPPLICATION_action')]//button[./text()='Remove']", 15);
                Dialog.WaitClick("//table[contains(@id,'_REMOVESELECTEDAPPLICATION_action')]//button[./text()='Remove']");
            }
            catch
            {
                //fine move on
            }
        }

        [When(@"I change Application dropdown to ""(.*)""")]
        public void WhenIChangeApplicationDropdownTo(string ApplicationType)
        {
            Dialog.SetDropDown(XpathHelper.xPath.VisibleDialog + "//input[contains(@id,'_APPLICATIONCODE_value')]", ApplicationType);
        }

        [When(@"I select the Pledge for ""(.*)""")]
        public void WhenISelectThePledgeFor(string ConstituentName)
        {
            StepHelper.SetCurrentThreadCultureToConfigValue();
            string pledgeText = "Pledge: " + ConstituentName + uniqueStamp + " - " + DateTime.Now.ToShortDateString();
            Dialog.WaitClick(string.Format("//td/div/b[./text()='{0}']", pledgeText));
        }

        [When(@"I click Add")]
        public void WhenIClickAdd()
        {
            try
            {
                Dialog.GetEnabledElement(XpathHelper.xPath.VisibleDialog + "//table[contains(@id,'_ADDSELECTEDCOMMITMENT_action')]//button[./text()='Add']", 30);
                Dialog.WaitClick(XpathHelper.xPath.VisibleDialog + "//table[contains(@id,'_ADDSELECTEDCOMMITMENT_action')]//button[./text()='Add']");
                //check there is a pop up with the title "Amount to apply"
                BaseComponent.GetEnabledElement("//div[contains(@id,'dataformdialog') and contains(@style,'block')]//span[text()='Amount to apply']", 15);
            }
            catch
            {
                //If first time fails, try again
                Dialog.GetEnabledElement(XpathHelper.xPath.VisibleDialog + "//table[contains(@id,'_ADDSELECTEDCOMMITMENT_action')]//button[./text()='Add']", 30);
                Dialog.WaitClick(XpathHelper.xPath.VisibleDialog + "//table[contains(@id,'_ADDSELECTEDCOMMITMENT_action')]//button[./text()='Add']");
                //check there is a pop up with the title "Amount to apply"
                BaseComponent.GetEnabledElement("//div[contains(@id,'dataformdialog') and contains(@style,'block')]//span[text()='Amount to apply']", 15);
            }
        }

        [When(@"I select radio button for Pledge balance")]
        public void WhenISelectRadioButtonForPledgeBalance()
        {
            try
            {
                Dialog.GetEnabledElement("//div[contains(@class,'bbui-dialog') and contains(@style,'visible')]//button[./text() = 'OK']", 15);
                Dialog.OK();
            }
            catch { }
        }

        [When(@"I add Adjustment details")]
        public void WhenIAddAdjustmentDetails(Table table)
        {
            Dialog.OpenTab("Adjustment details");
            Dialog.SetDropDown(XpathHelper.xPath.VisibleDialog + "//input[contains(@id,'_ADJUSTMENTREASONCODEID_value')]", table.Rows[0]["Adjustment reason"]);
            Dialog.Save();
        }

        [Then(@"the revenue record \(Amount\) is presented on the constituent ""(.*)"" for amount ""(.*)""")]

        public void ThenTheRevenueRecordIsPresentedOnTheConstituent(string ConstituentName, string Amount)
        {
            Panel.GetEnabledElement(string.Format("//span[./text()='{0}']", ConstituentName + uniqueStamp + " (" + Amount + ")"));
        }

        [Then(@"Revenue Transaction Profile View Form displays")]
        public void ThenRevenueTransactionProfileViewFormDisplays(Table table)
        {

            dynamic objectData = table.CreateDynamicInstance();
            //lets set the thread culture to get the correct date for the browser
            StepHelper.SetCurrentThreadCultureToConfigValue();
            //Check date fields
            if (!string.IsNullOrEmpty(objectData.DateStarted) && objectData.DateStarted.ToLower() == RecurringGiftConstants.Today)
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_BADGESTARTDATE_value') and ./text()='{0}']", DateTime.Now.ToShortDateString()));
            }

            if (!string.IsNullOrEmpty(objectData.EndDate) && objectData.EndDate.ToLower() == RecurringGiftConstants.EndDate)
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_BADGEENDDATE_value') and ./text()='{0}']", DateTime.Now.AddYears(1).ToShortDateString()));
            }

            if (!string.IsNullOrEmpty(objectData.RemainAnonymous))
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[contains(@id,'_ANONYMOUSINFOBAR_caption') and ./text()='{0}']", objectData.RemainAnonymous));
            }
            //optional bits to check - check these exist first
            if (((IDictionary<String, object>)objectData).ContainsKey("PostStatus") &&
                !string.IsNullOrEmpty(Convert.ToString(objectData.PostStatus)))
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_PAYMENTPOSTSTATUS_value') and ./text()='{0}']", objectData.PostStatus));
            }

            if (((IDictionary<String, object>)objectData).ContainsKey("PaymentMethod") &&
                !string.IsNullOrEmpty(Convert.ToString(objectData.PaymentMethod)))
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_PAYMENTMETHOD_value') and ./text()='{0}']", objectData.PaymentMethod));
            }

            if (((IDictionary<String, object>)objectData).ContainsKey("PaymentAmount") &&
               !string.IsNullOrEmpty(Convert.ToString(objectData.PaymentAmount)))
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_AMOUNT_value') and ./text()='{0}']", objectData.PaymentAmount));
            }
        }

        [Given(@"Allow direct posting of all payments has been set for account system ""(.*)""")]
        public void GivenAllowDirectPostingOfAllPaymentsHasBeenSetForAccountSystem(string AccountSystem)
        {
            BBCRMHomePage.OpenFunctionalArea("Administration");
            BaseComponent.WaitClick("//li/button/div[./text()='General ledger setup']");
            BaseComponent.WaitClick(string.Format("//a[contains(@title,'Go to account system: {0}') and ./text()='{0}']", AccountSystem));
            BaseComponent.WaitClick("//li/button/div[./text()='Payment posting options']");
            BaseComponent.SetCheckbox("//input[contains(@id,'_SETTING_NOTREQUIRE')]", true);
            Dialog.Save();
        }

        [Then(@"the revenue record is presented on the constituent with Amount ""(.*)""")]
        public void ThenTheRevenueRecordIsPresentedOnTheConstituentWithAmount(string amount)
        {
            string donationAmount = string.Format("Donation - {0}", amount);
            try
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[contains(@class,'xtb-text bbui-pages-section-tbarcaption') and ./text()='{0}']", Convert.ToString(donationAmount)));
            }
            catch (Exception ex)
            {
                throw new Exception("Revenue not present on record " + donationAmount, ex);
            }
        }

        [Then(@"Revenue Transaction Page Transaction Summary displays payment method information")]
        public void ThenRevenueTransactionPageTransactionSummaryDisplaysPaymentMethodInformation(Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            string dateValue = string.Empty;
            string paymentMethodType = objectData.PaymentMethod;

            //setting up decimal places for number of units and median price
            decimal numberOfUnits = Convert.ToDecimal(objectData.NumberOfUnits + ".000");
            decimal pricePerShareMedian = Convert.ToDecimal(objectData.MedianPrice + "0000");
            //check fields
            RevenueTransactionSummary(table);
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_PAYMENTMETHOD_value') and ./text()='{0}']", paymentMethodType), 15);
            if (((IDictionary<String, object>)objectData).ContainsKey("Sold") &&
                !string.IsNullOrEmpty(Convert.ToString(objectData.Sold)))
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_ISSOLDSTOCK_caption') and ./text()='{0}']", objectData.Sold), 15);
            }

            try
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_NUMBEROFUNITS_value') and ./text()='{0}']", numberOfUnits), 15);
            }
            catch
            {
                //eat exception 
            }
            try
            {
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_MEDIANPRICE_value') and ./text()='{0}']", pricePerShareMedian), 15);
            }
            catch
            {
                //eat exception
            }
        }

        [Then(@"the revenue record is presented on the constituent with notification ""(.*)""")]
        public void ThenTheRevenueRecordIsPresentedOnTheConstituentWithNotification(string AnonText)
        {
            try
            {
                BaseComponent.GetEnabledElement(string.Format("//div[contains(@id,'_ANONYMOUSINFOBAR_caption') and ./text()='{0}']", AnonText));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Anon text is not '{0}'", AnonText), ex);
            }
        }

        [When(@"I enter Marketing appeal ""(.*)""")]
        public void WhenIEnterMarketingData(string appeal)
        {
            //edit payment
            BaseComponent.WaitClick(string.Format(XpathHelper.xPath.Button, XpathHelper.PaymentEditActions.Payment));
            //select tab
            BaseComponent.WaitClick(XpathHelper.xPath.VisibleDialog + "//li[contains(@id,'_MARKETINGTAB_caption_tab')]//a[contains(@class,'x-tab-right')]");
            //set appeal
            try
            {
                Dialog.SetTextField(XpathHelper.xPath.VisibleDialog + "//input[contains(@id,'_APPEALID_value')]", appeal + uniqueStamp);
            }
            catch
            {
                //eat exception 
            }
            //save
            Dialog.Save();
        }

        [Then(@"the revenue record is presented on the constituent with Appeal ""(.*)""")]
        public void ThenTheRevenueRecordIsPresentedOnTheConstituentWithAppeal(string appeal)
        {
            try
            {
                BaseComponent.GetEnabledElement(string.Format("//a[contains(@id,'_APPEAL_value') and ./text()='{0}']", appeal + uniqueStamp), 30);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Appeal '{0}' is not displayed. ", appeal + uniqueStamp));
            }
        }

        [Given(@"I add campaign\(s\)")]
        public void GivenIAddCampaignS(Table table)
        {
            //Date malarkey
            StepHelper.SetTodayDateInTableRow("Start date", table.Rows[0]);
            StepHelper.SetTodayDateInTableRow("End date", table.Rows[0]);
            dynamic objectData = table.CreateDynamicInstance();
            //Navigate to Fundraising
            BBCRMHomePage.OpenFundraisingFA();
            FundraisingFunctionalArea.OpenLink("Add a campaign");
            //Populate Add Campaign form using Dialog.SetField        
            Dialog.SetTextField("//input[contains(@id,'_NAME_value')]", objectData.Name + uniqueStamp);
            Dialog.SetTextField("//input[contains(@id,'_USERID_value')]", objectData.LookupID + uniqueStamp);
            Dialog.SetTextField("//input[contains(@id,'_CAMPAIGNTYPECODEID_value')]", objectData.Type);
            StepHelper.AddEntryOnTheFly();
            //Populate Sites grid
            string gridXPath = Dialog.getXGridCell("CampaignAddForm", "_SITES_value", 1, BaseComponent.GetDatalistColumnIndex(Dialog.getXGridHeaders("CampaignAddForm", "_SITES_value"), "Site"));
            Dialog.SetGridDropDown(gridXPath, objectData.Site + uniqueStamp);
            //need to convert date to a string 
            Dialog.SetTextField("//input[contains(@id,'_STARTDATE_value')]", String.Format("{0:d}", objectData.StartDate));
            Dialog.SetTextField("//input[contains(@id,'_ENDDATE_value')]", String.Format("{0:d}", objectData.EndDate));
            Dialog.Save();
        }

        [Given(@"I Add Fundraisers via Fundraiser tab")]
        public void GivenIAddFundraisersViaFundraiserTab(Table table)
        {
            #region
            //Date malarkey
            StepHelper.SetTodayDateInTableRow("Start date", table.Rows[0]);
            dynamic objectData = table.CreateDynamicInstance();
            #endregion
            //Move to Fundraiser tab
            Panel.OpenTab("Fundraisers");
            //Add Fundraiser action
            Panel.ClickSectionAddButton("Fundraisers");
            Panel.WaitClick("//div[contains(@style,'visible')]//span[./text() = 'Fundraiser']");
            //Enter Fundraiser data from gherkin table
            Dialog.SetTextField("//input[contains(@id,'_CONSTITUENTID_value')]", objectData.Fundraiser + uniqueStamp);
            Dialog.SetTextField("//input[contains(@id,'_DATEFROM_value')]", String.Format("{0:d}", objectData.StartDate));
            Dialog.Save();
        }

        [Given(@"I add a Goal to ""(.*)""")]
        public void GivenIAddAGoalTo(string Campaign, Table table)
        {
            #region
            //Date malarkey
            StepHelper.SetTodayDateInTableRow("Start date", table.Rows[0]);
            StepHelper.SetTodayDateInTableRow("End date", table.Rows[0]);
            dynamic objectData = table.CreateDynamicInstance();
            #endregion
            //Add Goal action
            BaseComponent.WaitClick(XpathHelper.xPath.VisiblePanel + ("//button[./text () ='Add']"));
            //Enter Goal data from gherkin table
            Dialog.SetTextField("//input[contains(@id,'_NAME_value')]", objectData.Name);
            Dialog.SetTextField("//input[contains(@id,'_AMOUNT_value')]", objectData.Amount);
            //need to convert date to a string 
            Dialog.SetTextField("//input[contains(@id,'_STARTDATE_value')]", String.Format("{0:d}", objectData.StartDate));
            Dialog.SetTextField("//input[contains(@id,'_ENDDATE_value')]", String.Format("{0:d}", objectData.EndDate));
            Dialog.Save();
        }

        [Given(@"I add site ""(.*)""")]
        public void GivenIAddSite(string site)
        {
            //navigate to Administration > Sites
            BBCRMHomePage.OpenFunctionalArea("Administration");
            FunctionalArea.OpenLink("Sites");
            //click add
            Panel.ClickSectionAddButton("Sites");
            //check is visible
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisibleBlock + "//span[text()='Add site']");
            //Enter site name
            BaseComponent.SetTextField(XpathHelper.xPath.VisibleBlock + "//input[contains(@id,'_NAME_value')]", site + uniqueStamp);
            //Save
            Dialog.Save();
        }

        [Then(@"Payment applications details are correct")]
        public void ThenRevenueTransactionDetailsTabApplicationDetailsSectionIncludesAdditionalDonation(Table table)
        {
            IList<dynamic> objectData = table.CreateDynamicSet().ToList();
            foreach (dynamic application in objectData)
            {
                string applicationDetail = string.Format("{0} - {1}", application.Application, application.Amount);
                //Check each application and amount
                BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[contains(@class,'xtb-text bbui-pages-section-tbarcaption') and ./text()='{0}']", applicationDetail));
            }
        }

        [When(@"I edit the campaign hierarchy")]
        public void WhenIEditTheCampaignHierarchy()
        {
            //Move to Fundraiser tab
            Panel.OpenTab("Hierarchy");
            //Select edit action
            Panel.WaitClick("//button[text () ='Edit campaign hierarchy']");
        }

        [When(@"I add a campaign to the hierarchy for ""(.*)""")]
        public void WhenIAddACampaignToTheHierarchyFor(string Campaign)
        {
            //check is visible
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisibleBlock + ("//span[text()='Edit campaign hierarchy']"));
            //Select existing Campaign to enable Add 
            BaseComponent.WaitClick(String.Format(XpathHelper.xPath.VisibleBlock + ("//span[text()='{0}']"), Campaign + uniqueStamp));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisibleBlock + ("//button[text()='Add']"));
            //Click Add button
            BaseComponent.WaitClick(XpathHelper.xPath.VisibleBlock + ("//button[text()='Add']"));
        }

        [When(@"I use Campaign Search for ""(.*)""")]
        public void WhenIUseCampaignSearchFor(string Campaign)
        {
            //lets check the form is open
            try
            {
                Dialog.GetEnabledElement("//input[contains(@id,'_NAME_value')]", 10);
            }
            catch
            {
                //if its not open, lets open it rather than fail the test
                BaseComponent.WaitClick(XpathHelper.xPath.VisibleBlock + ("//button[text()='Add']"));
            }
            //Search and select sub Campaign
            Dialog.SetTextField("//input[contains(@id,'_NAME_value')]", Campaign + uniqueStamp);
            //click Search button
            Dialog.ClickButton("Search");
            //Select correct result in grid and save
            SearchDialog.SelectFirstResult();
            Dialog.Save();
        }

        [When(@"I add Campaign Goal\(s\) to ""(.*)""")]
        public void WhenIAddCampaignGoalSTo(string Campaign, Table table)
        {
            #region
            dynamic objectData = table.CreateDynamicInstance();
            #endregion
            //Navigate to Fundraising
            BBCRMHomePage.OpenFundraisingFA();
            FundraisingFunctionalArea.OpenLink("Campaign search");
            //Search Campaign
            Dialog.SetTextField("//input[contains(@id,'_NAME_value')]", Campaign + uniqueStamp);
            //click Search button
            Dialog.ClickButton("Search");
            //Select correct result in grid
            SearchDialog.SelectFirstResult();
            //Add Goal action
            Panel.ClickSectionAddButton("Goals");
            //add Goal data from gherkin table
            Dialog.SetTextField("//input[contains(@id,'_CAMPAIGNHIERARCHYGOALID_value')]", objectData.Name);
            Dialog.SetTextField("//input[contains(@id,'_AMOUNT_value')]", objectData.Amount);
            Dialog.Save();
        }

        [Then(@"""(.*)"" Goal tab displays")]
        public void ThenGoalTabDisplays(string Campaign, Table table)
        {
            #region
            //Date helper
            StepHelper.SetTodayDateInTableRow("Start date", table.Rows[0]);
            StepHelper.SetTodayDateInTableRow("End date", table.Rows[0]);
            dynamic objectData = table.CreateDynamicInstance();
            #endregion
            //Verify Goal data is as expected
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[text()='{0}']", objectData.Name));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[table/tbody/tr/td/div[text()='{0}'] and contains(@class,'x-grid3-row')]//div[text()='{1}']", objectData.Name, objectData.Amount));
            //d is used to format the dates as short date (see msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx) 
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[table/tbody/tr/td/div[text()='{0}'] and contains(@class,'x-grid3-row')]//div[text()='{1:d}']", objectData.Name, objectData.StartDate));
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//div[table/tbody/tr/td/div[text()='{0}'] and contains(@class,'x-grid3-row')]//div[text()='{1:d}']", objectData.Name, objectData.EndDate));
        }

        [Then(@"""(.*)"" Fundraisers tab displays")]
        public void ThenFundraisersTabDisplays(string Campaign, Table table)
        {
            #region
            //Date helper
            StepHelper.SetTodayDateInTableRow("Start date", table.Rows[0]);
            dynamic objectData = table.CreateDynamicInstance();
            //sorts out date format due to datetime adding 00:00:00
            DateTime dateToFind = objectData.StartDate;
            #endregion
            //move to Fundraisers tab of campaign
            Panel.OpenTab("Fundraisers");
            //Verify Fundraiser data is as expected
            BaseComponent.GetEnabledElement(string.Format(XpathHelper.xPath.VisibleContainerBlock + ("//a[text()='{0}']"), objectData.Fundraiser + uniqueStamp));
            BaseComponent.GetEnabledElement(string.Format("//div[contains(@class,'bbui-pages-pagesection') and not(contains(@class,'x-hide-display'))]//tr[//a[./text()='{0}']]//div[text()='{1}']", objectData.Fundraiser + uniqueStamp, dateToFind.ToShortDateString()));
        }

        [Then(@"""(.*)"" Campaign hierarchy displays")]
        public void ThenCampaignHierarchyDisplays(string Name, Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            //Click campaign Hierarchy link
            BaseComponent.WaitClick("//button[text()='Campaign hierarchy']");
            //Verify Campaign Hierarchy data is as expected        
            BaseComponent.GetEnabledElement(string.Format("//a[contains(@class,'x-tree-node-anchor')]/span[text()='{0}']", objectData.Name + uniqueStamp));
        }

        [When(@"navigate to the revenue record for ""(.*)""")]
        public void WhenNavigateToTheRevenueRecordFor(string constituent)
        {
            constituent += uniqueStamp;
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.TransactionSearchByConstituent(constituent);
        }

        [Given(@"I add a Recurring gift to constituent ""(.*)""")]
        [When(@"I add a Recurring gift to constituent ""(.*)""")]
        public void WhenIAddARecurringGiftToConstituent(string ConstituentName, Table table)
        {
            //handle date defensively before we supply table to the other methods
            StepHelper.SetTodayDateInTableRow("Date", table);
            StepHelper.SetTodayDateInTableRow("Installment schedule begins", table);
            StepHelper.SetTodayDateInTableRow("End date (optional)", table);
            //Search and select constituent
            StepHelper.SearchAndSelectConstituent(ConstituentName);
            //click Add Recurring Gift
            BaseComponent.WaitClick(string.Format(XpathHelper.xPath.Button, XpathHelper.PaymentAddActions.RecurringGift));
            //check is visible
            BaseComponent.GetEnabledElement(XpathHelper.xPath.ConstituentCaption);
            //set fields
            RecurringGiftDialog.SetRecurringGiftFields(table);
            //Save dialog
            Dialog.Save();
        }

        [When(@"I navigate to pledge")]
        public void WhenINavigateToPledge(Table table)
        {
            dynamic objectData = table.CreateDynamicInstance();
            var dialogId = "TransactionSearch";
            //declared variables
            string groupCaption = "Transactions";
            string taskCaption = "Transaction search";
            //lets set the thread culture to get the correct date for the browser
            StepHelper.SetCurrentThreadCultureToConfigValue();
            if (((IDictionary<String, object>)objectData).ContainsKey("Date") &&
              !string.IsNullOrEmpty(Convert.ToString(objectData.Date)) &&
              Convert.ToString(objectData.Date).ToLower() == "today")
            {
                objectData.Date = DateTime.Now.ToShortDateString();
            }

            //set fields for Transaction search fields on form
            IDictionary<string, CrmField> SupportedFields = new Dictionary<string, CrmField>
            {
                {"Last/Org/Group name", new CrmField("_KEYNAME_value", FieldType.TextInput)},
                {"Transaction type", new CrmField("_TRANSACTIONTYPE_value", FieldType.Dropdown)}
                
            };
            //search for pledge transaction
            objectData.LastName += uniqueStamp;
            BBCRMHomePage.OpenRevenueFA();
            RevenueFunctionalArea.OpenLink(groupCaption, taskCaption);
            //Set search fields
            Dialog.SetField(dialogId, "Last/Org/Group name", objectData.LastName, SupportedFields);
            Dialog.SetField(dialogId, "Transaction type", objectData.TransactionType, SupportedFields);
            //Click Search and select first result
            SearchDialog.Search();
            SearchDialog.SelectFirstResult();
            //Confirm pledge is displaying
            string pledgeName = string.Format("{0} Pledge: {1}", objectData.Date, objectData.Amount);
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[./text()='{0}']", pledgeName), 60);
        }

        [Then(@"the transaction summary shows")]
        public void ThenTheTransactionSummaryShows(Table pledgeDetails)
        {
            if (pledgeDetails.RowCount != 1) throw new ArgumentException("Only provide one row to evaluate.");

            TableRow pledgeDetailsRow = pledgeDetails.Rows[0];
            foreach (string fieldName in pledgeDetails.Header)
            {
                switch (fieldName)
                {
                    case "Constituent":
                        if (pledgeDetailsRow["Constituent"] != string.Empty) pledgeDetailsRow["Constituent"] += uniqueStamp;
                        Panel.GetEnabledElement(string.Format("//button[./text()='{0}']", pledgeDetailsRow["Constituent"]));
                        break;
                    default:
                        if (!TransactionSummaryPanel.SpanContains(fieldName, pledgeDetailsRow[fieldName]))
                            throw new ArgumentException(String.Format("Transaction summary label '{0}' does not equal '{1}'", fieldName, pledgeDetailsRow[fieldName]));
                        break;
                }
            }
        }

        public void RevenueTransactionSummary(Table table)
        {
            //setup date field.  StepHelper for date must come before dynamic objects
            StepHelper.SetTodayDateInTableRow("Date", table);
            dynamic objectData = table.CreateDynamicInstance();
            //sorts out date format due to datetime adding 00:00:00
            DateTime findDate = objectData.Date;
            //check fields
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_AMOUNT_value') and ./text()='{0}']", objectData.PaymentAmount), 15);
            BaseComponent.GetEnabledElement(XpathHelper.xPath.VisiblePanel + string.Format("//span[contains(@id,'_DATE_value') and ./text()='{0}']", findDate.ToShortDateString()), 15);
        }
    }
}
