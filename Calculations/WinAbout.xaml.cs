using System;
using System.ComponentModel;
using System.Windows;

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

            txtAbout.Text = CalculationsAboutInfo.CalculationsAndDllVersionInformation() + Environment.NewLine +
                            Environment.NewLine + CalculationsAboutInfo.LicenseInfo() + Environment.NewLine +
                            Environment.NewLine +
                            Environment.NewLine + CalculationsAboutInfo.ChangesInThisVersion();
        }

        private void WinAbout_Closing(object sender, CancelEventArgs e)
            => Controller.CloseAboutWindow();

        public void SetFontSize(double size) => txtAbout.FontSize = size;

        public void ChangeFontSize(double change) => txtAbout.FontSize += change;
    }
}