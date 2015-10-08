using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Blackbaud.UAT.Core.Crm
{
    /// <summary>
    /// Class to handle interactions for the 'Plan' panel.
    /// </summary>
    public class PlanPanel : Panel
    {

        private static Dictionary<string, int> plannedColumnCaptionToIndex = new Dictionary<string, int> 
        { 
            {"Status", 3},
            {"Date", 5},
            {"Start time", 6},
            {"End time", 7},
            {"Time zone", 8},
            {"Owner", 9},
            {"Objective", 10},
            {"Stage", 11},
            {"Contact method", 15},
            {"Has documentation", 16},
            {"Additional solicitors", 17},
            {"Participants", 18}
        };

        private static Dictionary<string, int> completedColumnCaptionToIndex = new Dictionary<string, int> 
        { 
            {"Status", 3},
            {"Date", 5},
            {"Start time", 6},
            {"End time", 7},
            {"Owner", 8},
            {"Objective", 9},
            {"Stage", 10},
            {"Contact method", 12},
            {"Has documentation", 13},
            {"Additional solicitors", 14},
            {"Participants", 15}
        };

        /// <summary>
        /// Add a completed step to the plan.
        /// </summary>
        /// <param name="step">Mapping of the field captions for adding a step to their desired values.</param>
        public static void AddCompletedStep(TableRow step)
        {
            SelectTab("Details");
            ClickSectionAddButton("Completed steps", "Add step");

            foreach (string caption in step.Keys)
            {
                string value = step[caption];
                if (value == null) continue;
                switch (caption)
                {
                    case "Objective":
                        SetTextField(Dialog.getXInput("StepEditForm4", "_OBJECTIVE_value"), value);
                        break;
                    case "Owner":
                        Dialog.SetSearchList(Dialog.getXInput("StepEditForm4", "_OWNERID_value"), 
                            Dialog.getXInput("FundraiserSearch", "KEYNAME"), value);
                        break;
                    case "Stage":
                        Dialog.SetDropDown(Dialog.getXInput("StepEditForm4", "_PROSPECTPLANSTATUSCODEID_value"), value);
                        break;
                    case "Status":
                        Dialog.SetDropDown(Dialog.getXInput("StepEditForm4", "_STATUSCODE_value"), value);
                        break;
                    case "Expected date":
                        SetTextField(Dialog.getXInput("StepEditForm4", "_EXPECTEDDATE_value"), value);
                        break;
                    case "Expected start time":
                        SetTextField(Dialog.getXInput("StepEditForm4", "_EXPECTEDSTARTTIME_value"), value);
                        break;
                    case "Expected end time":
                        SetTextField(Dialog.getXInput("StepEditForm4", "_EXPECTEDENDTIME_value"), value);
                        break;
                    case "Time zone":
                        Dialog.SetDropDown(Dialog.getXInput("StepEditForm4", "_TIMEZONEENTRYID_value"), value);
                        break;
                    case "Actual date":
                        SetTextField(Dialog.getXInput("StepEditForm4", "_ACTUALDATE_value"), value);
                        break;
                    case "Actual start time":
                        SetTextField(Dialog.getXInput("StepEditForm4", "_ACTUALSTARTTIME_value"), value);
                        break;
                    case "Actual end time":
                        SetTextField(Dialog.getXInput("StepEditForm4", "_ACTUALENDTIME_value"), value);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Field '{0}' is not implemented", caption));
                }
            }
            Dialog.Save();
        }

        /// <summary>
        /// Check if a completed step exists.
        /// </summary>
        /// <param name="step">Mapping of the column captions for the 'Completed steps' section to a single row's values.</param>
        /// <returns>True if the step exists, false otherwise.</returns>
        public static bool CompletedStepExists(TableRow step)
        {
            SelectTab("Details");
            return SectionDatalistRowExists(step, "Completed steps", completedColumnCaptionToIndex);
        }

        /// <summary>
        /// Check if a planned step exists.
        /// </summary>
        /// <param name="step">Mapping of the column captions for the 'Planned and pending steps' section to a single row's values.</param>
        /// <returns>True if the step exists, false otherwise.</returns>
        public static bool PlannedStepExists(TableRow step)
        {
            SelectTab("Details");
            return SectionDatalistRowExists(step, "Planned and pending steps", plannedColumnCaptionToIndex);
        }

        /// <summary>
        /// Click the 'Edit steps' button for the 'Planned and pending steps' section.
        /// </summary>
        public static void EditSteps()
        {
            SelectTab("Details");
            ClickSectionAddButton("Planned and pending steps", "Edit steps");
        }
        
    }
}
