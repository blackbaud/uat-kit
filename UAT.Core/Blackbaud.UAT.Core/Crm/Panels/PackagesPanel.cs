using System.Collections.Generic;
using TechTalk.SpecFlow;
using Blackbaud.UAT.Core.Crm.Dialogs;

namespace Blackbaud.UAT.Core.Crm.Panels
{
    /// <summary>
    /// Class to handle interactions for the 'Packages' panel.
    /// </summary>
    public class PackagesPanel : Panel
    {
        /// <summary>
        /// Static mapping of the column captions to their DOM index for the 'Packages' datalist.
        /// </summary>
        private static readonly Dictionary<string, int> packagesColumnCaptionToIndex = new Dictionary<string, int> 
        { 
            {"Name", 3},
            {"Code", 4},
            {"Site", 5},
            {"Cost", 6},
            {"Distribution", 7},
            {"Channel", 8},
            {"Category", 10},
            {"Content", 11},
            {"Export definition", 12},
            {"Description", 13},
        };

        /// <summary>
        /// Delete a package.
        /// </summary>
        /// <param name="package">Mapping of the column captions to a single row's values.</param>
        public static void DeletePackage(TableRow package)
        {
            SelectSectionDatalistRow(package, "Packages", packagesColumnCaptionToIndex);
            WaitClick(getXSelectedDatalistRowButton("Delete"));
            Dialog.Yes();
        }

        /// <summary>
        /// Delete a package.
        /// </summary>
        /// <param name="package">Mapping of the column captions to a single row's values.</param>
        public static void DeletePackage(IDictionary<string, string> package)
        {
            SelectSectionDatalistRow(package, "Packages", packagesColumnCaptionToIndex);
            WaitClick(getXSelectedDatalistRowButton("Delete"));
            Dialog.Yes();
        }

        /// <summary>
        /// Start to add a mail package.
        /// </summary>
        public static void AddMailPackage()
        {
            WaitClick(BBCRMHomePage.getXTask("Add a mail package"));
        }

        /// <summary>
        /// Check if a package exists.
        /// </summary>
        /// <param name="package">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the package exists, false otherwise.</returns>
        public static bool PackageExists(IDictionary<string, string> package)
        {
            return SectionDatalistRowExists(package, "Packages", packagesColumnCaptionToIndex);
        }

        /// <summary>
        /// Check if a package exists.
        /// </summary>
        /// <param name="package">Mapping of the column captions to a single row's values.</param>
        /// <returns>True if the package exists, false otherwise</returns>
        public static bool PackageExists(TableRow package)
        {
            return SectionDatalistRowExists(package, "Packages", packagesColumnCaptionToIndex);
        }
    }
}
