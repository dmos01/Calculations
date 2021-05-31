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

            txtAbout.Text = AboutInfo.CalculationsAndDllVersionInformation() + Environment.NewLine +
                            Environment.NewLine + AboutInfo.LicenseInfo() + Environment.NewLine + Environment.NewLine +
                            Environment.NewLine + AboutInfo.ChangesInThisVersion();
        }

        private void WinAbout_Closing(object sender, CancelEventArgs e)
            => Controller.CloseAboutWindow();

        public void SetFontSize(double size) => txtAbout.FontSize = size;

        public void ChangeFontSize(double change) => txtAbout.FontSize += change;
    }
}