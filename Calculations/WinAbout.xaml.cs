using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using EquationElements;
using static Calculations.Controller;
using AssemblyInfo = EquationBuilder.AssemblyInfo;

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

        public static string CalculationsAndDllVersionInformation()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            string version = v.Major + ElementsResources.DecimalSymbol + v.Minor + ElementsResources.DecimalSymbol + v.Build;

            return string.Join(Environment.NewLine,
                CalculationsResources.ProjectTitle + " " + version +
                CalculationsResources.ReleaseDate, 
                AssemblyInfo.VersionAndReleaseDate,
                EquationCalculator.AssemblyInfo.VersionAndReleaseDate,
                EquationElements.AssemblyInfo.VersionAndReleaseDate,
                NumberFormats.AssemblyInfo.VersionAndReleaseDate,
                ElementsResources.CreatedBy
            );
        }
    }
}