using System;
using EquationElements;
using AssemblyInfo = EquationBuilder.AssemblyInfo;

namespace Calculations
{
    class CalculationsAboutInfo
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
                "*Fraction class by Syed Mehroz Alam, licensed under the Code Project Open License (CPOL).",
                "Changes by Marc C. Brooks and Jeffery Sax.",
                "*'Hack' font Copyright 2018 Source Foundry Authors and licensed under the MIT License.",
                " -DejaVu font project was committed to the public domain.",
                " -Bitstream Vera Sans Mono Copyright 2003 Bitstream Inc. and licensed under the Bitstream Vera License with Reserved Font Names 'Bitstream' and 'Vera.')",
                "*Images from iconmonstr.com (Arrow 12, Trash Can 1).",
                "https://iconmonstr.com/license/",
                "*Icon created using Affinity Designer.",
                "*Thanks to Resharper for code suggestions."
            );

        /// <summary>
        ///     For version x.y.z. If a .z release, includes changes in the previous version (recursively).
        /// </summary>
        /// <returns></returns>
        public static string ChangesInThisVersion() =>
            string.Join(Environment.NewLine,
                "Featured changes in 4.3:",
                "*Big changes to determining E (*10^ or Eulers), particularly relating to brackets, Constants and Functions.",
                "*Big changes to validation (tightening some rules and loosening others).",
                " -Attempts are made to close unclosed brackets.",
                " -One-Argument Functions no longer need to be followed by an opening bracket.",
                "",
                "",
                "Featured changes in 4.0:",
                "*Constants support mod, root and e surrounded by digits.",
                "*Constants dropdown and Name textbox merged; Multi-line Description textbox; Search, Name and Value textboxes now line up.",
                "*Digit and symbol buttons are now 1 font point larger than function buttons.",
                "*Expanded use of error messages containing the applicable parameter.",
                "*Fractions now approximate sixths.",
                "*Root now available as a word operator; ! now available as a factorial symbol.",
                "",
                "*New and rewritten class libraries, with increased separation of responsibilities.",
                "*All projects and class libraries moved to the same solution.",
                "*Constants, Numbers and Words implement IComparable and overload equality.",
                "*Resource files to assist with localization.",
                "*String equality now primarily uses CurrentCultureIgnoreCase."
            );
    }
}