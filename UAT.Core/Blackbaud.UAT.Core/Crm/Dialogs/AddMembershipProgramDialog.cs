using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Blackbaud.UAT.Core.Crm.Dialogs
{    
    /// <summary>
    /// Class to handle the unique fields and actions for filling out an 'Add Membership Program' dialog.
    /// </summary>
    public class AddMembershipProgramDialog : Dialog
    {
        /// <summary>
        /// XPath for the Membership program button
        /// </summary>
        public const string getXMembershipProgramButton = "//*[contains(@class,'x-btn-text') and .//text()='Memberships']";
        
        /// <summary>
        /// Clicks the Membership program button
        /// </summary>
        public static void MembershipPrograms()
        {
            WaitClick(getXMembershipProgramButton);
        }

        /// <summary>
        /// XPath for the add Membership program button
        /// </summary>
        public const string getXAddMembershipButton = "//*[contains(@class,'x-btn-text-icon') and .//text()='Add']";
        
        /// <summary>
        /// XPath for the Program name field
        /// </summary>
        public const string getXProgramNameField = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_PROGRAMNAME_value')]";

        /// <summary>
        /// Create Membership program
        /// </summary>
        /// <param name="name">Name of the Membership program to create</param>
        public static void CreateMembershipProgram(string name)
        {
            WaitClick(getXAddMembershipButton);
            SetTextField(getXProgramNameField,name);
        }

        /// <summary>
        /// XPath for the Membership program kind annual field
        /// </summary>
        public const string getXProgramKindAnnual = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_KINDOFPROGRAMRADIO_0')]";

        /// <summary>
        /// Create Membership program annual
        /// </summary>
        public static void CreateMembershipProgramAnnual()
        {
            SetCheckbox(getXProgramKindAnnual, "true");
        }

        /// <summary>
        /// XPath for the Program Obtain Dues HTML field
        /// </summary>
        public const string getXProgramObtainDues = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_OBTAINPROGRAMRADIO_0')]";
        
        /// <summary>
        /// Create Membership program dues by setting checkbox
        /// </summary>
        public static void CreateMembershipProgramDues()
        {
            SetCheckbox(getXProgramObtainDues, "true");
        }

        /// <summary>
        /// XPath for the add Membership next button
        /// </summary>
        public const string getXAddMembershipNextButton = "//*[contains(@class,'x-btn-text') and .//text()='Next']";

        /// <summary>
        /// Click the create Membership program next button
        /// </summary>
        public static void CreateMembershipProgramNext()
        {
            WaitClick(getXAddMembershipNextButton);
        }

        /// <summary>
        /// XPath for the Membership program level name header
        /// </summary>
        public const string getXProgramLevelNameHeader = "//*[contains(@class,'x-grid3-col-NAME')]";

        /// <summary>
        /// Element to represent the membership program level name header
        /// </summary>
        public static IWebElement ProgramLevelNameHeaderElement;

        /// <summary>
        /// XPath to the Membership program level current editor
        /// </summary>
        public const string getXProgramLevelCurrentEdit = "//*[contains(@class,'x-grid-editor') and contains(@style,'visibility: visible')]/input";
        /// <summary>
        /// Element to represent the membership program current editor
        /// </summary>
        public static IWebElement ProgramLevelCurrentEditElement;

        /// <summary>
        /// Create Membership program level of the string passed in
        /// </summary>
        /// <param name="name">Name of level to create</param>
        public static void CreateMembershipProgramLevelName(string name)
        {
            WaitClick(getXProgramLevelNameHeader);
            SetTextField(getXProgramLevelCurrentEdit,name);
        }

        /// <summary>
        /// XPath for the Membership program level price header
        /// </summary>
        public const string getXProgramLevelPriceHeader = "//*[contains(@class, 'x-grid3-col-PRICE')]";

        /// <summary>
        /// Element to represent the Membership program level price header
        /// </summary>
        public static IWebElement ProgramLevelPriceHeaderElement;

        /// <summary>
        /// Create Membership program level price
        /// </summary>
        /// <param name="price">Price to create</param>
        public static void CreateMembershipProgramLevelPrice(string price)
        {
            WaitClick(getXProgramLevelPriceHeader);
            SetTextField(getXProgramLevelCurrentEdit,price);
        }

        /// <summary>
        /// XPath for the Benefits card format dropdown list
        /// </summary>
        public const string getXBenefitsCardFormatDropDown = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_MEMBERSHIPCARDFORMATLIST_value')]";

        /// <summary>
        /// Create Membership program benefits options format fro the string passed
        /// </summary>
        /// <param name="format">The format to be created</param>
        public static void CreateMembershipProgramBenefitsOptionsFormat(string format)
        {
            SetTextField(getXBenefitsCardFormatDropDown,format);
        }

        /// <summary>
        /// XPath for the Dues installments
        /// </summary>
        public const string getXDuesInstallments = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_DUESMULTIPLEPAYMENTSEACHTERM_value')]";

        /// <summary>
        /// 
        /// </summary>
        public static void CreateMembershipProgramDuesOptionsInstallments()
        {
            SetCheckbox(getXDuesInstallments,"true");
        }

        /// <summary>
        /// XPath for the Membership term
        /// </summary>
        public const string getXMembershipTerm = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_MEMBERSHIPTERMTYPE_value')]";

        /// <summary>
        /// Create Membership program renewal options term
        /// </summary>
        /// <param name="term">The term to be created</param>
        public static void CreateMembershipProgramRenewalOptionsTerm(string term)
        {
            SetTextField(getXMembershipTerm, term);
        }

        /// <summary>
        /// Xpath for Review save
        /// </summary>
        public const string getXReviewSave = "//*[contains(@class,'x-btn-text') and .//text()='Save']";

        /// <summary>
        /// Create Membership program review
        /// </summary>
        public static void CreateMembershipProgramReviewSave()
        {
            WaitClick(getXReviewSave);
        }

        /// <summary>
        /// XPath for the header panel
        /// </summary>
        public const string getXHeaderPanel = "//*[starts-with(@id,'bbui-gen-contentpanel') and contains(@class, 'bbui-pages-contentcontainer') and not(contains(@class,'x-hide-display'))]/div/div[starts-with(@id,'bbui-gen-headerpanel')]/div/h2/span";
  
        /// <summary>
        /// Check Membership program exists for the passed string
        /// </summary>
        /// <param name="name">The name to check for</param>
        public static void CheckMembershipProgramExists(string name)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutSecs));
            wait.Until(d =>
            {
                IWebElement HeaderPanelElement = d.FindElement(By.XPath(getXHeaderPanel));

                if (HeaderPanelElement == null ||
                    HeaderPanelElement.Displayed == false ||
                    HeaderPanelElement.Text != name) return false;
                return true;
            });
        }

        /// <summary>
        /// Xpath for the Program link
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static string getXProgramLink(string caption)
        {
            return String.Format("//*[contains(@class,'x-grid3-cell-inner')]/*[text()=\"{0}\"]", caption);
        }

        /// <summary>
        /// Xpath for the delete Program link
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static string getXDeleteProgramLink(string caption)
        {
            return String.Format("//*[contains(@class,\"bbui-pages-task-link\")]/div[text()=\"Delete {0}\"]", caption);
        }

        //TODO - make unique
        /// <summary>
        /// Xpath for delete yes option
        /// </summary>
        public const string getXYesReallyDelete = "//button[contains(@class,'x-btn-text') and .//text()='Yes']";

        /// <summary>
        /// Elemt for Program link
        /// </summary>
        public static IWebElement ProgramLinkElement;

        /// <summary>
        /// Delete Mambership program by string passed in
        /// </summary>
        /// <param name="name">The name of the program to delete</param>
        public static void DeleteMembershipProgram(string name)
        {

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException));

            try
            {                
                wait.Until(d =>
                {
                    ProgramLinkElement = d.FindElement(By.XPath(getXProgramLink(name)));

                    if (ProgramLinkElement == null ||
                        ProgramLinkElement.Displayed == false) return false;
                    ProgramLinkElement.Click();

                    return true;
                });
            }
            catch (Exception)
            {
                throw new NoSuchElementException();
            }

            WaitClick(getXDeleteProgramLink(name));

            WaitClick(getXYesReallyDelete);
        }
    }
}
