using System;
using EquationElements;
using AssemblyInfo = EquationBuilder.AssemblyInfo;

namespace Calculations
{
    class AboutInfo
    {
        public static string CalculationsAndDllVersionInformation() =>
            string.Join(Environment.NewLine,
                CalculationsResources.ProjectTitle + " " + CalculationsResources.VersionNumber +
                CalculationsResources.ReleaseDate, AssemblyInfo.VersionAndReleaseDate,
                EquationCalculator.AssemblyInfo.VersionAndReleaseDate,
                EquationElements.AssemblyInfo.VersionAndReleaseDate,
                NumberFormats.AssemblyInfo.VersionAndReleaseDate,
                ElementsResources.CreatedBy
            );


        public static string LicenseInfo() =>
            string.Join(Environment.NewLine,
                "Licenses:",
                "",
                "*Crystal Project Icons created by Everaldo Coelho, ©2007, licensed under the GNU Lesser General Public License.",
                "*Disabled arrow images from icons8.com, licensed under Creative Commons Attribution - NoDerivs 3.0 Unported.",
                "*Other images from all-free-download.com and graphicsfuel.com.",
                "*'Hack' font Copyright 2018 Source Foundry Authors and licensed under the MIT License.",
                "  -DejaVu font project was committed to the public domain.",
                "  -Bitstream Vera Sans Mono Copyright 2003 Bitstream Inc. and licensed under the Bitstream Vera License with Reserved Font Names 'Bitstream' and 'Vera.')",
                "*Fraction class by Syed Mehroz Alam, licensed under the Code Project Open License (CPOL).",
                "Changes by Marc C. Brooks and Jeffery Sax."
            );

        /// <summary>
        ///     For version x.y.z. If a .z release, includes changes in the previous version (recursively).
        /// </summary>
        /// <returns></returns>
        public static string ChangesInThisVersion() =>
            string.Join(Environment.NewLine,
                "Featured changes in 4.1:",
                "*History's 'Use Selected...' menu appears when right-clicking an item; delete option added.",
                "*Insert calculation tooltip mentions brackets.",
                "*HistoryController no longer controls the listbox and export buttons.",
                "",
                "",
                "Featured changes in 4.0:",
                "*Constants support mod, root and e surrounded by digits.",
                "*Constants dropdown and Name textbox merged; Multi-line Description textbox; Search, Name and Value textboxes now line up.",
                "*Digit and symbol buttons are now 1 font point larger than function buttons.",
                "*Expanded use of error messages containing the applicable parameter.",
                "*Fractions now approximate thirds and sixths.",
                "*Intersections removed.",
                "*Root now available as a word operator; ! now available as a factorial symbol.",
                "",
                "*New and rewritten class libraries, with increased separation of responsibilities.",
                "*All projects and class libraries moved to the same solution.",
                "*Constants, Numbers and Words implement IComparable and overload equality.",
                "*Rebuilt as a .NET Core 3.1 project.",
                "*Resource files to assist with localization.",
                "*String equality now primarily uses CurrentCultureIgnoreCase.",
                "",
                "(See Release History files for more.)"
            );
    }
}