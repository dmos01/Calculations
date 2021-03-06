using System;
using System.Reflection;
using EquationElements;

namespace NumberFormats
{
    public class AssemblyInfo
    {
        /// <summary>
        ///     Returns the AssemblyVersion of the EquationElements project. (Not to be confused with PackageVersion or
        ///     AssemblyFile Version.)
        /// </summary>
        public static string AssemblyVersion
        {
            get
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                return v.Major + ElementsResources.DecimalSymbol + v.Minor + ElementsResources.DecimalSymbol + v.Build;
            }
        }

        public static string ReleaseDate => "";

        public static string VersionAndReleaseDate =>
            NumberFormatsResources.ProjectTitle + " " + AssemblyVersion + NumberFormatsResources.ReleaseDate;
    }
}