using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NumberFormats;
using static Calculations.Controller;

namespace Calculations
{
    public partial class MainWindow
    {
        private void TabSettings_MouseUp(object sender, MouseButtonEventArgs e) => txtMainCalculation.Focus();

        private void CboAnswerFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cboAnswerFormat.SelectedIndex)
            {
                case 0:
                    ChangeAnswerFormat(new DecimalNumberFormat());
                    break;
                case 1:
                    ChangeAnswerFormat(new DecimalNumber2DPNumberFormat());
                    break;
                case 2:
                    ChangeAnswerFormat(new TopHeavyFractionNumberFormat());
                    break;
                case 3:
                    ChangeAnswerFormat(new MixedFractionNumberFormat());
                    break;
            }

            if (currentCalculatorAndAnswer is null)
                StopShowingAnswer();
            else
            {
                currentCalculatorAndAnswer.UpdateAnswerFormat();
                ShowAnswer();
            }
        }

        private void ChkRememberHistoryForNextTime_CheckedStateChanged(object sender, RoutedEventArgs e)
        {
            ChangeRememberHistory((bool)chkRememberHistoryForNextTime.IsChecked);
            txtMainCalculation.Focus();
        }

        private void ChkHistoryItemsOnlyAppearOnce_Checked(object sender, RoutedEventArgs e)
        {
            ChangeHistoryItemsCanAppear(true, (bool)chkHistoryMoveToTop.IsChecked);
            chkHistoryMoveToTop.Visibility = Visibility.Visible;
            txtMainCalculation.Focus();
        }

        private void ChkHistoryItemsOnlyAppearOnce_Unchecked(object sender, RoutedEventArgs e)
        {
            ChangeHistoryItemsCanAppear(false, (bool)chkHistoryMoveToTop.IsChecked);
            chkHistoryMoveToTop.Visibility = Visibility.Hidden;
            chkHistoryMoveToTop.IsChecked = false;
            txtMainCalculation.Focus();
        }

        private void ChkHistoryMoveToTop_CheckedStateChanged(object sender, RoutedEventArgs e)
        {
            ChangeHistoryItemsCanAppear((bool)chkHistoryItemsOnlyAllowedOnce.IsChecked,
                (bool)chkHistoryMoveToTop.IsChecked);
            txtMainCalculation.Focus();
        }

        private void BtnFontSettings_Click(object sender, RoutedEventArgs e) => ShowFontWindow();

        private void LblAbout_MouseDown(object sender, MouseButtonEventArgs e) => ShowAboutWindow();
    }
}