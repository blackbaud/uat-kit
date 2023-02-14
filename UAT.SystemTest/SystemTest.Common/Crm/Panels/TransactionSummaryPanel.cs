using System;
using System.Collections.Generic;
using Blackbaud.UAT.Core.Crm.Panels;

namespace SystemTest.Common.Crm
{
    public class TransactionSummaryPanel : Panel
    {
        private static readonly Dictionary<string, string> CustomSupportedSpans = new Dictionary<string, string>
        {
            {"Acknowledgements", getXSpan(null, "_PLEDGEACKNOWLEDGEMENTSTATUS_value")},
            {"Post status", getXSpan(null, "_PLEDGEPOSTSTATUS_value")},
            {"Past due", getXSpan(null, "_BADGEPASTDUE_value")}
        };

        public static bool SpanContains(string label, string value)
        {
            string xSpan = string.Empty;
            if (CustomSupportedSpans.TryGetValue(label, out xSpan))
            {
                return GetDisplayedElement(xSpan).Text.Contains(value);
            }
            throw new ArgumentException(String.Format("Custom Span '{0}' has not been created", label));
        }
    }
}