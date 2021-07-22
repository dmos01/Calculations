using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Calculations.Controller;

namespace Calculations
{
    public partial class MainWindow
    {
        private void BtnHistory_Click(object sender, RoutedEventArgs e) => ShowHistoryWindow();

        private void BtnKeypad_NotFollowed_Click(object sender, RoutedEventArgs e) =>
            InsertToCalculationTextboxAtCursor(((Button) sender).Content.ToString());

        private void BtnKeypad_FollowedByBrackets_Click(object sender, RoutedEventArgs e) =>
            InsertToCalculationTextboxAtCursor(((Button) sender).Content.ToString(), 1);

        private void BtnKeypad_FollowedByBracketsAndComma_Click(object sender, RoutedEventArgs e) =>
            InsertToCalculationTextboxAtCursor(((Button) sender).Content.ToString(), 2);

        private void RbtRadians_Checked(object sender, RoutedEventArgs e)
        {
            ChangeRadiansOrDegrees(true);
            UpdateAnswerWhenChangingRadiansOrDegrees();
        }

        private void RbtDegrees_Checked(object sender, RoutedEventArgs e)
        {
            ChangeRadiansOrDegrees(false);
            UpdateAnswerWhenChangingRadiansOrDegrees();
        }

        private void FocusOnMainCalculation(object sender, MouseButtonEventArgs e) => txtMainCalculation.Focus();
    }
}