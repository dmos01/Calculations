using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using EquationElements;
using static Calculations.Controller;

namespace Calculations
{
    /// <summary>
    ///     Interaction logic for winAbout.xaml
    /// </summary>
    public partial class WinAbout
    {
        public WinAbout()
        {
            InitializeComponent();
            txtAbout.Margin = new Thickness(MarginConstants.MainFromEdge, MarginConstants.MainFromEdge,
                MarginConstants.MainFromEdge, MarginConstants.MainFromEdge);

            Title = CalculationsResources.AboutWindowTitle;

            txtAbout.Text = CalculationsAndDllVersionInformation() + Environment.NewLine +
                            Environment.NewLine + CalculationsResources.LicenseInfo + Environment.NewLine +
                            Environment.NewLine +
                            Environment.NewLine + CalculationsResources.ReleaseHistory;
        }

        private void WinAbout_Closing(object sender, CancelEventArgs e)
            => CloseAboutWindow();

        public void SetFontSize(double size) => txtAbout.FontSize = size;

        public void ChangeFontSize(double change) => txtAbout.FontSize += change;

        [SuppressMessage("ReSharper", "RedundantNameQualifier")]
        public static string CalculationsAndDllVersionInformation()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            string version = string.Join(ElementsResources.DecimalSymbol, v.Major, v.Minor, v.Build);

            return string.Join(Environment.NewLine,
                CalculationsResources.ProjectTitle + " " + version +
                CalculationsResources.ReleaseDate,
                EquationBuilder.AssemblyInfo.VersionInfo,
                EquationCalculator.AssemblyInfo.VersionInfo,
                EquationElements.AssemblyInfo.VersionInfo,
                NumberFormats.AssemblyInfo.VersionInfo,
                ElementsResources.CreatedBy
            );
        }
    }
}