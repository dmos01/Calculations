﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Calculations.Controller;

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
            miInsertCalculation.ToolTip =
                TooltipMessages.InsertHistoryCalculation;
            miInsertAnswer.ToolTip = TooltipMessages.InsertHistoryAnswer;

            miCopyCalculation.Header = CalculationsResources.HistoryCopyCalculation;
            miCopyAnswer.Header = CalculationsResources.HistoryCopyAnswer;
            miCopyBoth.Header = CalculationsResources.HistoryCopyCalculationAndAnswer;
            miInsertCalculation.Header = CalculationsResources.HistoryInsertCalculation;
            miInsertAnswer.Header = CalculationsResources.HistoryInsertAnswer;
            miRemove.Header = CalculationsResources.HistoryDelete;

            btnUseSelected.ContextMenu = contextUseItem;
            if (lstHistory.Items.Count > 0)
                lstHistory.SelectedIndex = 0;
        }

        private void WinHistory_Closing(object sender, CancelEventArgs e) =>
            CloseHistoryWindow();

        private void LstHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblMoveUp.IsEnabled = lstHistory.SelectedIndex > 0;
            if (lstHistory.SelectedIndex == -1)
            {
                lblDelete.IsEnabled = false;
                btnUseSelected.IsEnabled = false;
                lblMoveDown.IsEnabled = false;
            }
            else
            {
                lblDelete.IsEnabled = true;
                btnUseSelected.IsEnabled = true;
                lblMoveDown.IsEnabled = lstHistory.SelectedIndex != (lstHistory.Items.Count - 1);
            }

            if (lblMoveUp.IsEnabled)
                imgMoveUp.Source = new BitmapImage(new Uri("Resources/ArrowUp.png", UriKind.Relative));
            else
                imgMoveUp.Source = new BitmapImage(new Uri("Resources/ArrowUpDisabled.png", UriKind.Relative));

            if (lblMoveDown.IsEnabled)
                imgMoveDown.Source = new BitmapImage(new Uri("Resources/ArrowDown.png", UriKind.Relative));
            else
                imgMoveDown.Source = new BitmapImage(new Uri("Resources/ArrowDownDisabled.png", UriKind.Relative));

            if (lblDelete.IsEnabled)
                imgRemove.Source = new BitmapImage(new Uri("Resources/Bin.png", UriKind.Relative));
            else
                imgRemove.Source = new BitmapImage(new Uri("Resources/BinDisabled.png", UriKind.Relative));
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            lstHistory.Items.Clear();
            History.Clear();
            btnExport.IsEnabled = false;
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            string file = RunImportDialogAndReturnFilePath("Import History",
                CalculationsResources.TextFile + "|*.txt", "txt");
            if (file.Any())
                History.ImportHistory(file);
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            string file = RunExportDialogAndReturnFilePath("Export History",
                CalculationsResources.TextFile + "|*.txt", "txt");
            if (file.Any())
                History.ExportHistory(file);
        }

        private void LblMoveUp_MouseUp(object sender, RoutedEventArgs e) => MoveSelectedItem(-1);

        private void LblMoveDown_MouseUp(object sender, RoutedEventArgs e) => MoveSelectedItem(1);

        private void MoveSelectedItem(int moveBy)
        {
            int index = lstHistory.SelectedIndex;

            //Move item in HistoryController first because changing lstHistory.Items will change the SelectedIndex.
            //-moveBy because the items are being displayed in lstHistory in reverse-order, so the item in the HistoryController will need to be moved in the reverse direction to lstHistory.
            History.MoveItem(GetReverseOfSelectedIndex(), -moveBy);

            ListBoxItem toMove = (ListBoxItem)lstHistory.Items[index];
            lstHistory.Items.RemoveAt(index);
            lstHistory.Items.Insert(index + moveBy, toMove);
            lstHistory.SelectedIndex = index + moveBy;
        }

        private void LblDelete_MouseUp(object sender, RoutedEventArgs e)
        {
            //Delete item in HistoryController first because changing lstHistory.Items will change the SelectedIndex.
            History.RemoveAt(GetReverseOfSelectedIndex());

            lstHistory.Items.RemoveAt(lstHistory.SelectedIndex);
            if (lstHistory.Items.Count == 0)
                btnExport.IsEnabled = false;
        }

        /// <summary>
        ///     Display items in reverse order (most recently added first).
        /// </summary>
        /// <param name="items"></param>
        public void DisplayItems(IList<string> items)
        {
            lstHistory.Items.Clear();

            for (int i = items.Count - 1; i >= 0; i--)
            {
                ListBoxItem newItem = new()
                {
                    Content = items[i],
                    ContextMenu = contextUseItem
                };
                lstHistory.Items.Add(newItem);
            }

            if (lstHistory.Items.Count == 0)
                btnExport.IsEnabled = false;
            else
            {
                btnExport.IsEnabled = true;
                lstHistory.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///     Items are being displayed in lstHistory in reverse-order so, when referencing items in the HistoryController, find
        ///     the "proper-order" index.
        /// </summary>
        /// <returns></returns>
        public int GetReverseOfSelectedIndex() => Math.Abs(lstHistory.SelectedIndex - lstHistory.Items.Count + 1);

        private void BtnUseSelected_Click(object sender, RoutedEventArgs e) => contextUseItem.IsOpen = true;


        private void MiCopyCalculation_Click(object sender, RoutedEventArgs e) =>
            History.CopyCalculation(GetReverseOfSelectedIndex());

        private void MiCopyAnswer_Click(object sender, RoutedEventArgs e) =>
            History.CopyAnswer(GetReverseOfSelectedIndex());

        private void MiCopyBoth_Click(object sender, RoutedEventArgs e) =>
            History.CopyCalculationAndAnswer(GetReverseOfSelectedIndex());

        private void MiInsertCalculation_Click(object sender, RoutedEventArgs e) =>
            History.UseCalculationInMainCalculation(GetReverseOfSelectedIndex());

        private void MiInsertAnswer_Click(object sender, RoutedEventArgs e) =>
            History.UseAnswerInMainCalculation(GetReverseOfSelectedIndex());


        public void SetButtonFontSizes(int size)
        {
            btnClear.FontSize = size;
            btnImport.FontSize = size;
            btnExport.FontSize = size;
            btnUseSelected.FontSize = size;
        }

        public void ChangeButtonFontSizes(int change)
        {
            btnClear.FontSize += change;
            btnImport.FontSize += change;
            btnExport.FontSize += change;
            btnUseSelected.FontSize += change;
        }

        public void SetListboxFontSize(int size) => lstHistory.FontSize = size;

        public void SetListboxFontFamily(FontFamily family) => lstHistory.FontFamily = family;
    }
}