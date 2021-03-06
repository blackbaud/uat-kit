﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SystemTest.Core.Features
{
    using TechTalk.SpecFlow;
    using System.Configuration;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium;
    using System.Collections.Generic;
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Drawing.Imaging;
    using ICSharpCode.SharpZipLib.Zip;
    using Blackbaud.UAT.SpecFlow.Selenium;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("IndividualConstituent")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class IndividualConstituentFeature : BaseTest
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext { get; set; }//;
        
#line 1 "IndividualConstituent.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "IndividualConstituent", "\tIn order to record information about alumni and supporters\r\n\tAs a BBCRM user\r\n\tI" +
                    " want to add and edit Individual constituent information in the database", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "IndividualConstituent")))
            {
                SystemTest.Core.Features.IndividualConstituentFeature.FeatureSetup(null);
            }
        }
        
        [NUnit.Framework.TearDownAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
            SaveChromeArtifacts(IsPass());
            StopDriver();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            ScenarioContext.Current.Add("Test", this);
            ScenarioContext.Current.Add("uniqueStamp", (DateTime.UtcNow.Subtract(new DateTime(1970, 7, 4)).TotalSeconds).ToString());
            StartDriver();
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual bool IsPass()
        {
            Boolean pass = false;
            try { 
                pass = (NUnit.Framework.TestContext.CurrentContext.Result.Status == NUnit.Framework.TestStatus.Passed);
            } catch {
                pass = (TestContext.CurrentTestOutcome == Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestOutcome.Passed);
            }
            return pass;
        }
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        protected virtual void FeatureSetup()
        {
            FeatureSetup(null);
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Individual Constituent: Add a new Individual Constituent record")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Individual Constituent: Add a new Individual Constituent record")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "IndividualConstituent")]
        [NUnit.Framework.CategoryAttribute("Ready")]
        [NUnit.Framework.CategoryAttribute("IndividualConstituent")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("Ready")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("IndividualConstituent")]
        public virtual void IndividualConstituentAddANewIndividualConstituentRecord()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Individual Constituent: Add a new Individual Constituent record", null, new string[] {
                        "Ready",
                        "IndividualConstituent"});
#line 8
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 9
 testRunner.Given("I have logged into the BBCRM home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "Nickname",
                        "Information source",
                        "Last name",
                        "First name"});
            table1.AddRow(new string[] {
                        "Mr.",
                        "Bobby",
                        "Other",
                        "Prospect",
                        "Bob"});
#line 10
 testRunner.When("I add individual(s) with address", ((string)(null)), table1, "When ");
#line 13
 testRunner.Then("individual constituent is created named \"Bob Prospect\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "First name",
                        "Last name",
                        "Title",
                        "Nickname"});
            table2.AddRow(new string[] {
                        "Bob",
                        "Prospect",
                        "Mr.",
                        "Bobby"});
#line 14
 testRunner.And("Personal information is displayed on Personal Info tab", ((string)(null)), table2, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Individual Constituent: Add a new Individual Constituent record with an address")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Individual Constituent: Add a new Individual Constituent record with an address")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "IndividualConstituent")]
        [NUnit.Framework.CategoryAttribute("Ready")]
        [NUnit.Framework.CategoryAttribute("IndividualConstituent")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("Ready")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("IndividualConstituent")]
        public virtual void IndividualConstituentAddANewIndividualConstituentRecordWithAnAddress()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Individual Constituent: Add a new Individual Constituent record with an address", null, new string[] {
                        "Ready",
                        "IndividualConstituent"});
#line 20
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 21
 testRunner.Given("I have logged into the BBCRM home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "Nickname",
                        "Information source",
                        "Address type",
                        "Country",
                        "Address",
                        "City",
                        "ZIP",
                        "Last name",
                        "First name"});
            table3.AddRow(new string[] {
                        "Mr.",
                        "Bobby",
                        "Other",
                        "Home",
                        "United Kingdom",
                        "12 Main Street",
                        "Glasgow",
                        "G2 BOB",
                        "Prospect",
                        "Bob"});
#line 22
 testRunner.When("I add individual(s) with address", ((string)(null)), table3, "When ");
#line 25
 testRunner.Then("individual constituent is created named \"Bob Prospect\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Contact information",
                        "Type",
                        "Primary",
                        "Start date"});
            table4.AddRow(new string[] {
                        "12 Main Street",
                        "Home (Current)",
                        "Yes",
                        ""});
#line 26
 testRunner.And("An address is displayed on contact tab", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "First name",
                        "Last name",
                        "Title",
                        "Nickname"});
            table5.AddRow(new string[] {
                        "Bob",
                        "Prospect",
                        "Mr.",
                        "Bobby"});
#line 29
 testRunner.And("Personal information is displayed on Personal Info tab", ((string)(null)), table5, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Individual Constituent: Add a new address to an existing Individual Constituent")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Individual Constituent: Add a new address to an existing Individual Constituent")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "IndividualConstituent")]
        [NUnit.Framework.CategoryAttribute("Ready")]
        [NUnit.Framework.CategoryAttribute("IndividualConstituent")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("Ready")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("IndividualConstituent")]
        public virtual void IndividualConstituentAddANewAddressToAnExistingIndividualConstituent()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Individual Constituent: Add a new address to an existing Individual Constituent", null, new string[] {
                        "Ready",
                        "IndividualConstituent"});
#line 35
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 36
 testRunner.Given("I have logged into the BBCRM home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "Nickname",
                        "Information source",
                        "Last name",
                        "First name"});
            table6.AddRow(new string[] {
                        "Mr.",
                        "TC",
                        "Other",
                        "Case",
                        "Test"});
#line 37
 testRunner.And("I add an individual", ((string)(null)), table6, "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Address type",
                        "Country",
                        "Address",
                        "City",
                        "ZIP"});
            table7.AddRow(new string[] {
                        "Home",
                        "United Kingdom",
                        "12 Main Street",
                        "Glasgow",
                        "G2 BOB"});
#line 40
 testRunner.When("I add the address to the current individual", ((string)(null)), table7, "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Contact information",
                        "Type",
                        "Primary",
                        "Start date"});
            table8.AddRow(new string[] {
                        "12 Main Street",
                        "Home (Current)",
                        "Yes",
                        ""});
#line 43
 testRunner.Then("An address is displayed on contact tab", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Individual Constituent: Edit existing address on a Individual Constituent")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Individual Constituent: Edit existing address on a Individual Constituent")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "IndividualConstituent")]
        [NUnit.Framework.CategoryAttribute("Ready")]
        [NUnit.Framework.CategoryAttribute("IndividualConstituent")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("Ready")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("IndividualConstituent")]
        public virtual void IndividualConstituentEditExistingAddressOnAIndividualConstituent()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Individual Constituent: Edit existing address on a Individual Constituent", null, new string[] {
                        "Ready",
                        "IndividualConstituent"});
#line 49
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 50
 testRunner.Given("I have logged into the BBCRM home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Address type",
                        "Country",
                        "Address",
                        "City",
                        "ZIP",
                        "Information source",
                        "Last name",
                        "First name"});
            table9.AddRow(new string[] {
                        "Home",
                        "United Kingdom",
                        "Gilbert Street",
                        "Glasgow",
                        "G3 8QN",
                        "Other",
                        "Prospect",
                        "Robert"});
#line 51
 testRunner.And("Individual constituent exists with an address", ((string)(null)), table9, "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Contact information",
                        "Type"});
            table10.AddRow(new string[] {
                        "Gilbert Street",
                        "Home (Current)"});
#line 54
 testRunner.When("I select a row under Addresses", ((string)(null)), table10, "When ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Address",
                        "Postcode",
                        "Start date"});
            table11.AddRow(new string[] {
                        "Buccleuch Street",
                        "G3 6DY",
                        "Today -2 weeks"});
#line 57
 testRunner.And("I edit the address", ((string)(null)), table11, "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Contact information",
                        "Type",
                        "Primary",
                        "Start date"});
            table12.AddRow(new string[] {
                        "Buccleuch Street",
                        "Home (Current)",
                        "Yes",
                        "Today -2 weeks"});
#line 60
 testRunner.Then("An address is displayed on contact tab", ((string)(null)), table12, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion

