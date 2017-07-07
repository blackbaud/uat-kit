using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Major Giving Setup' panel.
    /// </summary>
    public class MajorGivingSetupPanel : Panel
    {

        /// <summary>
        /// Add a major giving plan outline.
        /// </summary>
        /// <param name="planName">The name of the plan outline.</param>
        /// <param name="steps">Table where each row represents a step to add.  Row keys correspond to the column captions.</param>
        public static void AddPlanOutline(string planName, Table steps)
        {
            //click add button and set the plan name
            SelectTab("Plan Outlines");
            ClickSectionAddButton("Plan outlines");
            SetTextField(Dialog.getXInput("PlanOutlineAddForm", "NAME"), planName);

            int rowCount = 1;
            //map the column captions to their index
            IDictionary<string, int> columnCaptionToIndex = new Dictionary<string, int>();
            foreach (string caption in steps.Rows[0].Keys)
            {
                columnCaptionToIndex.Add(caption,
                    GetDatalistColumnIndex(Dialog.getXGridHeaders("PlanOutlineAddForm", "PLANOUTLINESTEPS_value"), caption));
            }

            //add the rows
            foreach (TableRow step in steps.Rows)
            {
                foreach (string caption in step.Keys)
                {
                    //Add if value provided using appropriate field set.  Use rowCount and column caption mapping when making XPath
                    if (step[caption] == string.Empty) continue;
                    string gridXPath = Dialog.getXGridCell("PlanOutlineAddForm", "PLANOUTLINESTEPS_value", rowCount, columnCaptionToIndex[caption]);
                    string gridRowXPath = Dialog.getXGridRow("PlanOutlineAddForm", "PLANOUTLINESTEPS_value", rowCount);
                    string value = step[caption];
                    switch (caption)
                    {
                        case "Fundraiser role":
                            Dialog.SetGridDropDown(gridXPath, value);
                            break;
                        case "Stage":
                            Dialog.SetGridDropDown(gridXPath, value);
                            break;
                        case "Contact method":
                            Dialog.SetGridDropDown(gridXPath, value);
                            break;
                        case "Objective":
                            Dialog.SetGridTextField(gridXPath, value);
                            break;
                        case "Days from start":
                            Dialog.SetGridTextField(gridXPath, value);
                            break;
                        default:
                            throw new NotSupportedException(String.Format("Column '{0}' is not supported by the default AddPlanOutline method.  Additional implementation is required.", caption));
                    }
                }
                rowCount++;
            }
            Dialog.Save();
        }

        /// <summary>
        /// Check if a major giving plan outline exists
        /// </summary>
        /// <param name="plan">Mapping of field caption keys to their values.</param>
        /// <returns>True if the plan exists, false otherwise.</returns>
        public static bool PlanOutlineExists(TableRow plan)
        {
            SelectTab("Plan Outlines");

            return SectionDatalistRowExists(plan, "Plan outlines");
        }

    }
}
