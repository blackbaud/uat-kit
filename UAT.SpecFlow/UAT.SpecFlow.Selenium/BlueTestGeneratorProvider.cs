using System;
using System.CodeDom;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Utils;

namespace Blackbaud.UAT.SpecFlow.Selenium
{
    public class BlueTestGeneratorProvider : IUnitTestGeneratorProvider
    {
        //NUnit Attributes
        protected const string TESTFIXTURE_ATTR = "NUnit.Framework.TestFixtureAttribute";
        protected const string TEST_ATTR = "NUnit.Framework.TestAttribute";
        protected const string ROW_ATTR = "NUnit.Framework.TestCaseAttribute";
        protected const string CATEGORY_ATTR = "NUnit.Framework.CategoryAttribute";
        protected const string TESTSETUP_ATTR = "NUnit.Framework.SetUpAttribute";
        protected const string TESTFIXTURESETUP_ATTR = "NUnit.Framework.TestFixtureSetUpAttribute";
        protected const string TESTFIXTURETEARDOWN_ATTR = "NUnit.Framework.TestFixtureTearDownAttribute";
        protected const string TESTTEARDOWN_ATTR = "NUnit.Framework.TearDownAttribute";
        protected const string IGNORE_ATTR = "NUnit.Framework.IgnoreAttribute";
        protected const string DESCRIPTION_ATTR = "NUnit.Framework.DescriptionAttribute";

        //MSTest Attributes
        protected const string MS_TESTFIXTURE_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute";
        protected const string MS_TEST_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute";
        protected const string MS_CATEGORY_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory";
        protected const string MS_PROPERTY_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute";
        protected const string MS_TESTFIXTURESETUP_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute";
        protected const string MS_TESTFIXTURETEARDOWN_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute";
        protected const string MS_TESTSETUP_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute";
        protected const string MS_TESTTEARDOWN_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute";
        protected const string MS_IGNORE_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute";
        protected const string MS_DESCRIPTION_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute";

        protected const string MS_FEATURE_TITLE_PROPERTY_NAME = "FeatureTitle";
        protected const string MS_TESTCONTEXT_TYPE = "Microsoft.VisualStudio.TestTools.UnitTesting.TestContext";

        private CodeDomHelper CodeDomHelper { get; set; }

        // MSTest does not support Row Tests and we must chose the lowest common denimonator
        public virtual bool SupportsRowTests { get { return false; } }
        public virtual bool SupportsAsyncTests { get { return false; } }

        public BlueTestGeneratorProvider(CodeDomHelper codeDomHelper)
        {
            CodeDomHelper = codeDomHelper;
        }

        private void SetProperty(CodeTypeMember codeTypeMember, string name, string value)
        {
            CodeDomHelper.AddAttribute(codeTypeMember, MS_PROPERTY_ATTR, name, value);            
        }

        public UnitTestGeneratorTraits GetTraits()
        {
            // NUnit flags as Parallel and Row, but MsTest is only Row. Use Common Denominator
            return UnitTestGeneratorTraits.RowTests;
        }

        public virtual void SetTestClass(TestClassGenerationContext generationContext, string featureTitle, string featureDescription)
        {
            generationContext.TestClass.BaseTypes.Add("BaseTest");
            
            //Nunit
            CodeDomHelper.AddAttribute(generationContext.TestClass, TESTFIXTURE_ATTR);
            CodeDomHelper.AddAttribute(generationContext.TestClass, DESCRIPTION_ATTR, featureTitle);
            //MSTest
            CodeDomHelper.AddAttribute(generationContext.TestClass, MS_TESTFIXTURE_ATTR);

            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("System.Configuration"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("OpenQA.Selenium.Chrome"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("OpenQA.Selenium.Remote"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("OpenQA.Selenium"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("System"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("Newtonsoft.Json"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("Newtonsoft.Json.Linq"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("System.IO"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("System.Drawing.Imaging"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("System.Configuration"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("ICSharpCode.SharpZipLib.Zip"));
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("Blackbaud.UAT.SpecFlow.Selenium"));

            var testcontext = new CodeMemberField("Microsoft.VisualStudio.TestTools.UnitTesting.TestContext", "TestContext");
            testcontext.Attributes = MemberAttributes.Public;
            // slightly hacky
            testcontext.Name += " { get; set; }//";
            generationContext.TestClass.Members.Add(testcontext);

            // test runner needs to be static due to MsTest.
            generationContext.TestRunnerField.Attributes |= MemberAttributes.Static;

            // this need to be a method in the derived class
            var isPass = new CodeMemberMethod();
            isPass.Attributes = MemberAttributes.Public;
            isPass.Name = "IsPass";
            isPass.ReturnType = new CodeTypeReference(typeof(Boolean));
            isPass.Statements.Add(new CodeSnippetStatement("            Boolean pass = false;"));
            isPass.Statements.Add(new CodeSnippetStatement("            try { "));
            isPass.Statements.Add(new CodeSnippetStatement("                pass = (NUnit.Framework.TestContext.CurrentContext.Result.Status == NUnit.Framework.TestStatus.Passed);"));
            isPass.Statements.Add(new CodeSnippetStatement("            } catch {"));
            isPass.Statements.Add(new CodeSnippetStatement("                pass = (TestContext.CurrentTestOutcome == Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestOutcome.Passed);"));
            isPass.Statements.Add(new CodeSnippetStatement("            }"));
            isPass.Statements.Add(new CodeSnippetStatement("            return pass;"));
            generationContext.TestClass.Members.Add(isPass);
        }

        public void SetTestClassCategories(TestClassGenerationContext generationContext, IEnumerable<string> featureCategories)
        {
            //NUnit
            CodeDomHelper.AddAttributeForEachValue(generationContext.TestClass, CATEGORY_ATTR, featureCategories);
            //MsTest does not support caregories... :(
        }

        public void SetTestClassIgnore(TestClassGenerationContext generationContext)
        {
            //NUnit
            CodeDomHelper.AddAttribute(generationContext.TestClass, IGNORE_ATTR);
            //MSTest
            CodeDomHelper.AddAttribute(generationContext.TestClass, MS_IGNORE_ATTR);
        }

        public virtual void FinalizeTestClass(TestClassGenerationContext generationContext)
        {
            generationContext.ScenarioInitializeMethod.Statements.Add(new CodeSnippetStatement("            ScenarioContext.Current.Add(\"Test\", this);"));
            generationContext.ScenarioInitializeMethod.Statements.Add(new CodeSnippetStatement("            ScenarioContext.Current.Add(\"uniqueStamp\", (DateTime.UtcNow.Subtract(new DateTime(1970, 7, 4)).TotalSeconds).ToString());"));
            generationContext.ScenarioInitializeMethod.Statements.Add(new CodeSnippetStatement("            StartDriver();"));

            // From SeleniumNunit + BBTest changes
            // Can't be move to SetTestCleanupMethod, as the code at that point misses the .OnScenarioEnd() call.
            // Make sure this code is at the end!
            generationContext.TestCleanupMethod.Statements.Add(new CodeSnippetStatement("            SaveChromeArtifacts(IsPass());"));
            generationContext.TestCleanupMethod.Statements.Add(new CodeSnippetStatement("            StopDriver();"));
        }


        public virtual void SetTestClassInitializeMethod(TestClassGenerationContext generationContext)
        {
            // MSTest - must be static
            generationContext.TestClassInitializeMethod.Attributes |= MemberAttributes.Static;
            generationContext.TestClassInitializeMethod.Parameters.Add(new CodeParameterDeclarationExpression(MS_TESTCONTEXT_TYPE, "testContext"));
            CodeDomHelper.AddAttribute(generationContext.TestClassInitializeMethod, MS_TESTFIXTURESETUP_ATTR);

            //NUnit
            var nunitFeatureSetup = new CodeMemberMethod();
            nunitFeatureSetup.Name = "FeatureSetup";
            nunitFeatureSetup.Attributes = MemberAttributes.Family;
            nunitFeatureSetup.Statements.Add(new CodeSnippetStatement("            FeatureSetup(null);"));
            CodeDomHelper.AddAttribute(nunitFeatureSetup, TESTFIXTURESETUP_ATTR);                        
            generationContext.TestClass.Members.Add(nunitFeatureSetup);
        }

        public void SetTestClassCleanupMethod(TestClassGenerationContext generationContext)
        {
            //Nunit
            CodeDomHelper.AddAttribute(generationContext.TestClassCleanupMethod, TESTFIXTURETEARDOWN_ATTR);

            //MSTest
            generationContext.TestClassCleanupMethod.Attributes |= MemberAttributes.Static;            
            CodeDomHelper.AddAttribute(generationContext.TestClassCleanupMethod, MS_TESTFIXTURETEARDOWN_ATTR);
        }


        public virtual void SetTestInitializeMethod(TestClassGenerationContext generationContext)
        {
            //NUnit
            CodeDomHelper.AddAttribute(generationContext.TestInitializeMethod, TESTSETUP_ATTR);

            //MSTest
            CodeDomHelper.AddAttribute(generationContext.TestInitializeMethod, MS_TESTSETUP_ATTR);

            FixTestRunOrderingIssue(generationContext);
        }

        protected virtual void FixTestRunOrderingIssue(TestClassGenerationContext generationContext)
        {
            //see https://github.com/techtalk/SpecFlow/issues/96
            generationContext.TestInitializeMethod.Statements.Add(
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeBinaryOperatorExpression(
                            new CodePropertyReferenceExpression(
                                new CodeTypeReferenceExpression(typeof(FeatureContext)),
                                "Current"),
                            CodeBinaryOperatorType.IdentityInequality,
                            new CodePrimitiveExpression(null)),
                        CodeBinaryOperatorType.BooleanAnd,
                        new CodeBinaryOperatorExpression(
                            new CodePropertyReferenceExpression(
                                new CodePropertyReferenceExpression(
                                    new CodePropertyReferenceExpression(
                                        new CodeTypeReferenceExpression(typeof(FeatureContext)),
                                        "Current"),
                                    "FeatureInfo"),
                                "Title"),
                            CodeBinaryOperatorType.IdentityInequality,
                            new CodePrimitiveExpression(generationContext.Feature.Name))),
                    new CodeExpressionStatement(
                        new CodeMethodInvokeExpression(
                            new CodeTypeReferenceExpression(
                                generationContext.Namespace.Name + "." + generationContext.TestClass.Name
                                ),
                            generationContext.TestClassInitializeMethod.Name,
                            new CodePrimitiveExpression(null)))));
        }

        public void SetTestCleanupMethod(TestClassGenerationContext generationContext)
        {
            //NUnit
            CodeDomHelper.AddAttribute(generationContext.TestCleanupMethod, TESTTEARDOWN_ATTR);

            //MSTest
            CodeDomHelper.AddAttribute(generationContext.TestCleanupMethod, MS_TESTTEARDOWN_ATTR);
        }


        public virtual void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
            //NUnit
            CodeDomHelper.AddAttribute(testMethod, TEST_ATTR);
            CodeDomHelper.AddAttribute(testMethod, DESCRIPTION_ATTR, scenarioTitle);

            //MSTest
            CodeDomHelper.AddAttribute(testMethod, MS_TEST_ATTR);
            CodeDomHelper.AddAttribute(testMethod, MS_DESCRIPTION_ATTR, scenarioTitle);
            SetProperty(testMethod, MS_FEATURE_TITLE_PROPERTY_NAME, generationContext.Feature.Name);

            const string browser = "Chrome";

            testMethod.UserData.Add("Browser:" + browser, browser);
        }

        public virtual void SetTestMethodCategories(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, IEnumerable<string> scenarioCategories)
        {
            //NUnit
            CodeDomHelper.AddAttributeForEachValue(testMethod, CATEGORY_ATTR, scenarioCategories);            
            
            //MSTest
            CodeDomHelper.AddAttributeForEachValue(testMethod, MS_CATEGORY_ATTR, scenarioCategories);            

        }

        public void SetTestMethodIgnore(TestClassGenerationContext generationContext, CodeMemberMethod testMethod)
        {
            //NUnit
            CodeDomHelper.AddAttribute(testMethod, IGNORE_ATTR);

            //MSTest
            CodeDomHelper.AddAttribute(testMethod, MS_IGNORE_ATTR);
        }


        public virtual void SetRowTest(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
            //NUnit
            //SetTestMethod(generationContext, testMethod, scenarioTitle);
            
            //MsTest does not support row tests... :(
            throw new NotSupportedException();
        }

        public void SetRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, IEnumerable<string> arguments, IEnumerable<string> tags, bool isIgnored)
        {
            throw new NotSupportedException();
        }

        public void SetTestMethodAsRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle, string exampleSetName, string variantName, IEnumerable<KeyValuePair<string, string>> arguments)
        {
        }

        public void SetTestClassParallelize(TestClassGenerationContext generationContext)
        {
            throw new NotImplementedException();
        }
    }
}