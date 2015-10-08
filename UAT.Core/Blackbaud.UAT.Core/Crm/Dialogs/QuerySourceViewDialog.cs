using System;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Base object for QuerySourceViewDialog functionality and interactions in BBCRM.  Inherits the Dialog base class.
    /// </summary>
    public class QuerySourceViewDialog : Dialog
    {
        /// <summary>
        /// Constant XPath for the Record Field of a QuerySourceViewDialog.
        /// </summary>
        public const string getXRecordTypeField = "//*[contains(@id,\"_RECORDTYPE_value\") and contains(@class,\"x-form-text\")]";

        /// <summary>
        /// Returns an XPath for the Name column of a QuerySourceViewDialog.
        /// 
        /// </summary>
        /// <param name="name">The Name of the Column.</param>  
        public static string getXRecordTypeNameColumn(string name) { return String.Format("//*[./text()=\"{0}\" and contains(@class,\"x-grid3-col-NAME\")]",name);}
        

        /// <summary>
        /// Set the record Type on a QuerySourceViewDialog.
        /// </summary>
        /// <param name="type">The type to select.</param>
        public static void SetRecordType(string type)
        {
            SetDropDown(getXRecordTypeField, type);            
        }

        /// <summary>
        /// Select the record Type on a QuerySourceViewDialog
        /// </summary>
        /// <param name="type">The type to be selected.</param>
        public static void SelectRecordTypeName(string type)
        {
            WaitClick(getXRecordTypeNameColumn(type));
        }

    }
}
