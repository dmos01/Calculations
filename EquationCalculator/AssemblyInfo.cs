using System;
using System.Reflection;
using EquationElements;

namespace EquationCalculator
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
                return string.Join(ElementsResources.DecimalSymbol, v.Major, v.Minor, v.Build);
            }
        }

        public static string VersionInfo =>
            CalculatorResources.ProjectTitle + " " + AssemblyVersion + CalculatorResources.ReleaseDate;
    }
}