using System;
using System.Reflection;

namespace EquationElements
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

        public static string VersionAndReleaseDate =>
            ElementsResources.ProjectTitle + " " + AssemblyVersion + ElementsResources.ReleaseDate;
    }
}