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

        public const string getXMembershipProgramButton = "//*[contains(@class,'x-btn-text') and .//text()='Memberships']";

        public static void MembershipPrograms()
        {
            WaitClick(getXMembershipProgramButton);
        }

        public const string getXAddMembershipButton = "//*[contains(@class,'x-btn-text-icon') and .//text()='Add']";
        public const string getXProgramNameField = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_PROGRAMNAME_value')]";

        public static void CreateMembershipProgram(string name)
        {
            WaitClick(getXAddMembershipButton);
            SetTextField(getXProgramNameField,name);
        }

        public const string getXProgramKindAnnual = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_KINDOFPROGRAMRADIO_0')]";

        public static void CreateMembershipProgramAnnual()
        {
            SetCheckbox(getXProgramKindAnnual, "true");
        }

        public const string getXProgramObtainDues = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_OBTAINPROGRAMRADIO_0')]";
        public static void CreateMembershipProgramDues()
        {
            SetCheckbox(getXProgramObtainDues, "true");
        }


        public const string getXAddMembershipNextButton = "//*[contains(@class,'x-btn-text') and .//text()='Next']";
        public static void CreateMembershipProgramNext()
        {
            WaitClick(getXAddMembershipNextButton);
        }

        public const string getXProgramLevelNameHeader = "//*[contains(@class,'x-grid3-col-NAME')]";
        public static IWebElement ProgramLevelNameHeaderElement;

        public const string getXProgramLevelCurrentEdit = "//*[contains(@class,'x-grid-editor') and contains(@style,'visibility: visible')]/input";
        public static IWebElement ProgramLevelCurrentEditElement;

        public static void CreateMembershipProgramLevelName(string name)
        {
            WaitClick(getXProgramLevelNameHeader);
            SetTextField(getXProgramLevelCurrentEdit,name);
        }

        public const string getXProgramLevelPriceHeader = "//*[contains(@class, 'x-grid3-col-PRICE')]";
        public static IWebElement ProgramLevelPriceHeaderElement;

        public static void CreateMembershipProgramLevelPrice(string price)
        {
            WaitClick(getXProgramLevelPriceHeader);
            SetTextField(getXProgramLevelCurrentEdit,price);
        }

        public const string getXBenefitsCardFormatDropDown = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_MEMBERSHIPCARDFORMATLIST_value')]";
        public static void CreateMembershipProgramBenefitsOptionsFormat(string format)
        {
            SetTextField(getXBenefitsCardFormatDropDown,format);
        }

        public const string getXDuesInstallments = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_DUESMULTIPLEPAYMENTSEACHTERM_value')]";

        public static void CreateMembershipProgramDuesOptionsInstallments()
        {
            SetCheckbox(getXDuesInstallments,"true");
        }

        public const string getXMembershipTerm = "//*[starts-with(@id, 'ctrl_') and contains(@id, '_MEMBERSHIPTERMTYPE_value')]";

        public static void CreateMembershipProgramRenewalOptionsTerm(string term)
        {
            SetTextField(getXMembershipTerm, term);
        }

        public const string getXReviewSave = "//*[contains(@class,'x-btn-text') and .//text()='Save']";

        public static void CreateMembershipProgramReviewSave()
        {
            WaitClick(getXReviewSave);
        }


        public const string getXHeaderPanel = "//*[starts-with(@id,'bbui-gen-contentpanel') and contains(@class, 'bbui-pages-contentcontainer') and not(contains(@class,'x-hide-display'))]/div/div[starts-with(@id,'bbui-gen-headerpanel')]/div/h2/span";
  
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


        public static string getXProgramLink(string caption)
        {
            return String.Format("//*[contains(@class,'x-grid3-cell-inner')]/*[text()=\"{0}\"]", caption);
        }
        public static string getXDeleteProgramLink(string caption)
        {
            return String.Format("//*[contains(@class,\"bbui-pages-task-link\")]/div[text()=\"Delete {0}\"]", caption);
        }

        //TODO - make unique
        public const string getXYesReallyDelete = "//button[contains(@class,'x-btn-text') and .//text()='Yes']";

        public static IWebElement ProgramLinkElement;

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
