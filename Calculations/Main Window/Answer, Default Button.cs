using System.Windows;

namespace Calculations
{
    public partial class MainWindow
    {
        private void ShowAnswer()
        {
            if (currentCalculatorAndAnswer is null)
            {
                StopShowingAnswer();
                return;
            }

            readOnlyTextboxAnswer.Text = currentCalculatorAndAnswer.CurrentAnswer;
            btnCalculate.Visibility = Visibility.Collapsed;
            readOnlyTextboxAnswer.Visibility = Visibility.Visible;
            txtMainCalculation.TextChanged += TxtMainCalculation_TextChanged;
        }

        private void StopShowingAnswer()
        {
            currentCalculatorAndAnswer = null;
            readOnlyTextboxAnswer.Visibility = Visibility.Collapsed;
            btnCalculate.Visibility = Visibility.Visible;
            txtMainCalculation.TextChanged -= TxtMainCalculation_TextChanged;
        }

        private void UpdateAnswerWhenChangingRadiansOrDegrees()
        {
            if (currentCalculatorAndAnswer is null)
                StopShowingAnswer();
            else
                ShowAnswer();
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Reset();
            Settings.Default.Save();

            if (currentCalculatorAndAnswer is null)
            {
                MessageBox.Show(DialogResources.CannotCopyAnswerMessage, DialogResources.CannotCopyAnswerTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtMainCalculation.Focus();
            }
            else
            {
                Clipboard.SetText(currentCalculatorAndAnswer.CurrentAnswer);
            }
        }

        private void SetDefaultButton(object sender, RoutedEventArgs e)
        {
            UIElement sent = (UIElement) sender;

            if (sent == txtMainCalculation)
            {
                btnCalculate.IsDefault = true;
                txtMainCalculation.GotFocus -= SetDefaultButton;
            }
            else
            {
                btnCalculate.IsDefault = false;
                txtMainCalculation.GotFocus += SetDefaultButton;
            }

            btnSaveConstant.IsDefault = sent == cboConstants || sent == txtConstantValue ||
                                        sent == txtConstantUnit || sent == txtConstantDescription;
        }
    }
}