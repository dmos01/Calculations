using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Calculations
{
    /// <summary>
    ///     Interaction logic for winFont.xaml
    /// </summary>
    public partial class WinFont
    {
        public WinFont()
        {
            InitializeComponent();
            rbtConsolas.FontFamily = new FontFamily("Consolas");
            rbtHack.FontFamily = (FontFamily) Application.Current.Resources["Hack"];

            switch (Controller.FontController.Family.MainFamilyAsString)
            {
                case "SegoeUI":
                    rbtSegoeUI.IsChecked = true;
                    chkAlsoForKeypad.Visibility = Visibility.Hidden;
                    break;
                case "Consolas":
                    rbtConsolas.IsChecked = true;
                    break;
                case "Hack":
                    rbtHack.IsChecked = true;
                    break;
            }

            chkAlsoForKeypad.IsChecked = Settings.Default.FontFamilyIsAlsoForNumberOperatorAndFunctionButtons;
            btnLargerFont.IsEnabled = Settings.Default.FontSizeRelativeToDefault != 2;
            btnSmallerFont.IsEnabled = Settings.Default.FontSizeRelativeToDefault != -2;
        }

        public void SetSizeForControls(int uiSize)
        {
            rbtSegoeUI.FontSize = uiSize;
            rbtConsolas.FontSize = uiSize;
            rbtHack.FontSize = uiSize;
            chkAlsoForKeypad.FontSize = uiSize;
            btnReset.FontSize = uiSize;
        }

        private void ChangeSizeForControls(int change)
        {
            rbtSegoeUI.FontSize += change;
            rbtConsolas.FontSize += change;
            rbtHack.FontSize += change;
            chkAlsoForKeypad.FontSize += change;
            btnReset.FontSize += change;
        }

        private void WinFont_Closing(object sender, CancelEventArgs e)
            => Controller.CloseFontWindow();

        private void RbtSegoeUI_Checked(object sender, RoutedEventArgs e)
        {
            Controller.FontController.Family.ChangeTo("SegoeUI");
            chkAlsoForKeypad.Visibility = Visibility.Hidden;
            chkAlsoForKeypad.IsChecked = false;
        }

        private void RbtHack_Checked(object sender, RoutedEventArgs e)
        {
            Controller.FontController.Family.ChangeTo("Hack");
            chkAlsoForKeypad.Visibility = Visibility.Visible;
        }

        private void RbtConsolas_Checked(object sender, RoutedEventArgs e)
        {
            Controller.FontController.Family.ChangeTo("Consolas");
            chkAlsoForKeypad.Visibility = Visibility.Visible;
        }

        private void ChkFontAlsoForKeypad_Checked(object sender, RoutedEventArgs e)
            => Controller.FontController.Family.SetMainFamilyIsAlsoForNumberOperatorAndFunctionButtons(true);

        private void ChkFontAlsoForKeypad_Unchecked(object sender, RoutedEventArgs e)
            => Controller.FontController.Family.SetMainFamilyIsAlsoForNumberOperatorAndFunctionButtons(false);

        private void BtnLarger_Click(object sender, RoutedEventArgs e)
        {
            Controller.FontController.Size.Increase();
            ChangeSizeForControls(1);
            btnSmallerFont.IsEnabled = true;
            btnLargerFont.IsEnabled = Settings.Default.FontSizeRelativeToDefault != 2;
        }

        private void BtnSmaller_Click(object sender, RoutedEventArgs e)
        {
            Controller.FontController.Size.Decrease();
            ChangeSizeForControls(-1);
            btnLargerFont.IsEnabled = true;
            btnSmallerFont.IsEnabled = Settings.Default.FontSizeRelativeToDefault != -2;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to reset all fonts?", "Reset Fonts?", MessageBoxButton.OKCancel) ==
                MessageBoxResult.OK)
            {
                Controller.FontController.Reset();
                rbtSegoeUI.IsChecked = true;
                chkAlsoForKeypad.IsChecked = false;
                btnLargerFont.IsEnabled = true;
                btnSmallerFont.IsEnabled = true;
            }
        }
    }
}