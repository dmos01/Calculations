using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculations
{
    public partial class MainWindow
    {
        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            Controller.ShowHistoryWindow();
        }

        private void BtnKeypad_NotFollowed_Click(object sender, RoutedEventArgs e)
        {
            InsertToCalculationTextboxAtCursor(((Button) sender).Content.ToString());
        }

        private void BtnKeypad_FollowedByBrackets_Click(object sender, RoutedEventArgs e)
        {
            InsertToCalculationTextboxAtCursor(((Button) sender).Content.ToString(), 1);
        }

        private void BtnKeypad_FollowedByBracketsAndComma_Click(object sender, RoutedEventArgs e)
        {
            InsertToCalculationTextboxAtCursor(((Button) sender).Content.ToString(), 2);
        }

        private void RbtRadians_Checked(object sender, RoutedEventArgs e)
        {
            Controller.ChangeRadiansOrDegrees(true);
            UpdateAnswerWhenChangingRadiansOrDegrees();
        }

        private void RbtDegrees_Checked(object sender, RoutedEventArgs e)
        {
            Controller.ChangeRadiansOrDegrees(false);
            UpdateAnswerWhenChangingRadiansOrDegrees();
        }

        private void FocusOnMainCalculation(object sender, MouseButtonEventArgs e)
        {
            txtMainCalculation.Focus();
        }
    }
}