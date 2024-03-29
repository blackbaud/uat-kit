﻿namespace SystemTest.Common
{
    public struct XpathHelper
    {
        public struct xPath
        {
            public const string Button = "//button[contains(@class,'bbui-linkbutton')]//div[./text()='{0}']";
            public const string ConstituentCaption = VisibleDialog + "//label[contains(@for,'_CONSTITUENTID_value')]";
            public const string VisibleDialog = "//div[contains(normalize-space(@class),'x-window bbui-dialog') and contains(@style,'visible')]";
            public const string VisiblePanel = "//div[contains(@class,'bbui-pages-contentcontainer') and not(contains(@class,'x-hide-display'))]";
            public const string VisibleBlock = "//div[contains(@id,'dataformdialog') and contains(@style,'block')]";
            public const string VisibleContainerBlock = "//div[contains(@class,'bbui-datalist-container') and contains(@style,'block')]";
            public const string VisibleBatchDialog = "//div[contains(@id,'batchdialog') and contains(@style,'block')]";
            public const string VisibleSearchBlock = "//div[contains(@id,'searchdialog') and contains(@style,'block') and not(contains(@style, 'hidden'))]";

        }

        public struct PaymentAddActions
        {
            public const string Payment = "Add payment";
            public const string Pledge = "Add pledge";
            public const string RecurringGift = "Add recurring gift";
            public const string Memebership = "Add Membership";
        }

        public struct PaymentEditActions
        {
            public const string Payment = "Edit payment";
            public const string Pledge = "Edit pledge";
            public const string RecurringGift = "Edit recurring gift";
            public const string OriginalAmount = "Edit original amount";
            public const string Vat = "Edit VAT";
            public const string SellProperty = "Sell property";
            public const string ViewEditSoldProperty = "View/edit sold property information";
            public const string SellStock = "Sell stock";
            public const string WriteOff = "Write-off";
            public const string EditPostedPayment = "Edit posted payment";
        }

        public struct PaymentDeleteActions
        {
            public const string Payment = "Delete payment";
        }
    }
}
