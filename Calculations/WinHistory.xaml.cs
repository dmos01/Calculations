using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Calculations
{
    /// <summary>
    ///     Interaction logic for winHistory.xaml
    /// </summary>
    public partial class WinHistory
    {
        public WinHistory()
        {
            InitializeComponent();
            Controller.History.HistoryWindowConstructed(lstHistory, btnExport);

            miInsertHistoryCalculation.ToolTip =
                TooltipMessages.InsertHistoryCalculationTooltip + Environment.NewLine +
                TooltipMessages.InsertHistoryCalculationTooltipLine2;
            miInsertHistoryAnswer.ToolTip = TooltipMessages.InsertHistoryAnswerTooltip + Environment.NewLine +
                                            TooltipMessages.InsertHistoryAnswerTooltipLine2;
            if (lstHistory.Items.Count > 0)
                lstHistory.SelectedIndex = 0;
        }

        private void WinHistory_Closing(object sender, CancelEventArgs e) =>
            Controller.CloseHistoryWindow();

        private void LstHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnRemove.IsEnabled = lstHistory.SelectedIndex != -1;
            btnUse.IsEnabled = lstHistory.SelectedIndex != -1;
            btnMoveUp.IsEnabled = lstHistory.SelectedIndex > 0;
            btnMoveDown.IsEnabled = lstHistory.SelectedIndex != -1 &&
                                    lstHistory.SelectedIndex != (lstHistory.Items.Count - 1);

            if (btnMoveUp.IsEnabled)
                imgMoveUp.Source = new BitmapImage(new Uri("Resources/ArrowUp.png", UriKind.Relative));
            else
                imgMoveUp.Source = new BitmapImage(new Uri("Resources/ArrowUpDisabled.png", UriKind.Relative));

            if (btnMoveDown.IsEnabled)
                imgMoveDown.Source = new BitmapImage(new Uri("Resources/ArrowDown.png", UriKind.Relative));
            else
                imgMoveDown.Source = new BitmapImage(new Uri("Resources/ArrowDownDisabled.png", UriKind.Relative));

            if (btnRemove.IsEnabled)
                imgRemove.Source = new BitmapImage(new Uri("Resources/RecycleBinBlue.png", UriKind.Relative));
            else
                imgRemove.Source = new BitmapImage(new Uri("Resources/RecycleBinGrey.png", UriKind.Relative));
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) => Controller.History.Clear();

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            string file = Controller.RunImportDialogAndReturnFilePath("Import History",
                CalculationsResources.TextFile + "|*.txt", "txt");
            if (file.Any())
                Controller.History.ImportHistory(file);
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            string file = Controller.RunExportDialogAndReturnFilePath("Export History",
                CalculationsResources.TextFile + "|*.txt", "txt");
            if (file.Any())
                Controller.History.ExportHistory(file);
        }

        private void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            int index = lstHistory.SelectedIndex;
            Controller.History.MoveSelectedItem(-1);
            lstHistory.SelectedIndex = --index;
        }

        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            int index = lstHistory.SelectedIndex;
            Controller.History.MoveSelectedItem(1);
            lstHistory.SelectedIndex = ++index;
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e) => Controller.History.RemoveSelectedItem();

        private void BtnUse_Click(object sender, RoutedEventArgs e) => cntxtUseHistory.IsOpen = true;

        private void MiCopyCalculation_Click(object sender, RoutedEventArgs e) => Controller.History.CopyCalculation();

        private void MiCopyAnswer_Click(object sender, RoutedEventArgs e) => Controller.History.CopyAnswer();

        private void MiCopy_Click(object sender, RoutedEventArgs e) => Controller.History.CopyEntry();

        private void MiInsertCalculation_Click(object sender, RoutedEventArgs e) =>
            Controller.History.UseCalculationInMainCalculation();

        private void MiInsertAnswer_Click(object sender, RoutedEventArgs e) =>
            Controller.History.UseAnswerInMainCalculation();

        private void LstHistory_DoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        public void SetButtonFontSizes(int size)
        {
            btnClear.FontSize = size;
            btnImport.FontSize = size;
            btnExport.FontSize = size;
            btnUse.FontSize = size;
        }

        public void ChangeButtonFontSizes(int change)
        {
            btnClear.FontSize += change;
            btnImport.FontSize += change;
            btnExport.FontSize += change;
            btnUse.FontSize += change;
        }
    }
}