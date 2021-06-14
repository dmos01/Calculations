using System;
using System.Windows;
using static Calculations.Controller;

namespace Calculations
{
    public partial class MainWindow
    {
        CalculatorAndAnswer currentCalculatorAndAnswer;

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            txtMainCalculation.Text = txtMainCalculation.Text.Trim();

            try
            {
                //So that the CalculatorAndAnswer added to history is a new object.
                currentCalculatorAndAnswer = new CalculatorAndAnswer(txtMainCalculation.Text);
                History.Add(currentCalculatorAndAnswer);
                ShowAnswer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DialogResources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                StopShowingAnswer();
            }

            txtMainCalculation.Focus();
        }

        public void InsertToCalculationTextboxAtCursor(string toInsert, int setCursorBackRelativeToEndOfInsert = 0)
        {
            int start = txtMainCalculation.SelectionStart;
            txtMainCalculation.Text = txtMainCalculation.Text.Substring(0, start) + toInsert +
                                      txtMainCalculation.Text.Substring(start + txtMainCalculation.SelectionLength);
            txtMainCalculation.Select(start + toInsert.Length - setCursorBackRelativeToEndOfInsert, 0);
            txtMainCalculation.Focus();

            if (tabKeypad.SelectedIndex < 3)
                tabKeypad.SelectedIndex = 0;
        }

        private void TxtMainCalculation_TextChanged(object sender, RoutedEventArgs e)
        {
            StopShowingAnswer();
        }
    }
}