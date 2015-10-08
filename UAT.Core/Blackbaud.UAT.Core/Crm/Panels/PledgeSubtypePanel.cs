using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Pledge Subtype' panel.
    /// </summary>
    public class PledgeSubtypePanel : Panel
    {
        /// <summary>
        /// Add a pledge subtype.
        /// </summary>
        /// <param name="name">The name of the subtype.</param>
        /// <param name="postToGL">Boolean indicating whether the 'Post to GL' checkbox should bet set or not.</param>
        public static void AddSubtype(string name, bool postToGL)
        {
            ClickSectionAddButton("Pledge subtypes");
            SetTextField(Dialog.getXInput("PledgeSubtypeAddForm", "NAME"), name);
            SetCheckbox(Dialog.getXInput("PledgeSubtypeAddForm", "POSTTOGL"), postToGL);
            Dialog.Save();
        }

        /// <summary>
        /// Add a pledge subtype.
        /// </summary>
        /// <param name="subtype">Mapping of the 'Add a pledge subtype' field captions to their values.</param>
        public static void AddSubtype(TableRow subtype)
        {
            ClickSectionAddButton("Pledge subtypes");
            foreach (string caption in subtype.Keys)
            {
                switch (caption)
                {
                    case "Name":
                        SetTextField(Dialog.getXInput("PledgeSubtypeAddForm", "NAME"), subtype[caption]);
                        break;
                    case "Post to GL":
                        SetCheckbox(Dialog.getXInput("PledgeSubtypeAddForm", "POSTTOGL"), subtype[caption]);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Field caption '{0}' is not implemeneted.", caption));
                }
            }
            Dialog.Save();
        }

        /// <summary>
        /// Check if a subtype exists.
        /// </summary>
        /// <param name="name">The name of the subtype/</param>
        /// <returns>True if the subtype exists, false otherwise.</returns>
        public static bool SubtypeExists(string name)
        {
            IDictionary<string, string> row = new Dictionary<string, string>();
            row.Add("Name", name);

            return SectionDatalistRowExists(row, "Pledge subtypes");
        }

        /// <summary>
        /// Check if a subtype exists.
        /// </summary>
        /// <param name="subtype">Mapping of column captions to a single row's values.</param>
        /// <returns>True if the subtype exists, false otherwise.</returns>
        public static bool SubtypeExists(TableRow subtype)
        {
            return SectionDatalistRowExists(subtype, "Pledge subtypes");
        }
    }
}
