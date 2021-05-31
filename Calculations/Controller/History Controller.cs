using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EquationElements;

namespace Calculations
{
    partial class Controller
    {
        public class HistoryController
        {
            readonly List<CalculatorAndAnswer> historyItems;
            Button exportButton;
            ListBox listbox;
            public string HistoryPath { get; }


            public HistoryController(string historyPath)
            {
                historyItems = new List<CalculatorAndAnswer>();
                HistoryPath = historyPath;
            }

            public void AddIfNeedToAdd(CalculatorAndAnswer resolver)
            {
                if (NeedToAdd(resolver.OriginalEquation) == false)
                    return;

                historyItems.Add(resolver);
                DisplayItems();
            }

            public void RemoveSelectedItem()
            {
                historyItems.RemoveAt(ReverseIndex(listbox.SelectedIndex));
                DisplayItems();
            }

            public void MoveSelectedItem(int moveByIndicies)
            {
                if (moveByIndicies == 0)
                    return;

                int realIndex = ReverseIndex(listbox.SelectedIndex);
                CalculatorAndAnswer temp = historyItems[realIndex];
                historyItems.RemoveAt(realIndex);
                //History items are displayed in reverse-order, so it actually needs to be moved in the opposite direction.
                historyItems.Insert(realIndex - moveByIndicies, temp);
                DisplayItems();
            }

            private int ReverseIndex(int index) => Math.Abs(index - historyItems.Count + 1);

            public void Clear()
            {
                historyItems.Clear();
                DisplayItems();
            }

            public void UpdateRadiansOrDegrees()
            {
                foreach (CalculatorAndAnswer x in historyItems)
                    x.UpdateDegreesOrRadians();

                DisplayItems();
            }

            public void UpdateAnswerFormats()
            {
                foreach (CalculatorAndAnswer x in historyItems)
                    x.UpdateAnswerFormat();
                DisplayItems();
            }

            /// <summary>
            ///     Load in the listbox and export button. Sets the fonts and displays any existing history.
            /// </summary>
            /// <param name="historyListBox"></param>
            /// <param name="export"></param>
            public void HistoryWindowConstructed(ListBox historyListBox, Button export)
            {
                listbox = historyListBox;
                exportButton = export;
                SetHistoryItemsFontSize(
                    FontController.Size.DefaultMainCalc + Settings.Default.FontSizeRelativeToDefault);
                SetHistoryItemsFontFamily(FontController.Family.MainFamily);
                DisplayItems();
            }

            /// <summary>
            ///     Set listbox to null.
            /// </summary>
            public void HistoryWindowClosed() => listbox = null;

            private bool NeedToAdd(string originalEquation)
            {
                if (HistoryItemsSetting == HistoryItemsCanAppear.ManyTimes)
                    return true;

                for (int i = 0; i < historyItems.Count; i++)
                {
                    if (historyItems[i].OriginalEquation != originalEquation)
                        continue;

                    if (historyItems[i].ContainsRandom)
                        return true;

                    if (HistoryItemsSetting == HistoryItemsCanAppear.Once)
                        return false;

                    //Move to top.
                    historyItems.RemoveAt(i);
                    return true;
                }

                return true;
            }


            //--------------------Import, Export, Copy, Use--------------------

            /// <summary>
            ///     Imports Calculations, calculates tham and adds them to history. Skips duplicates depending on the
            ///     HistoryItemsCanAppear setting. Skips invalid Calculations.
            /// </summary>
            /// <param name="path">The full path and file name to import from.</param>
            public void ImportHistory(string path = null)
            {
                if (string.IsNullOrEmpty(path))
                    path = HistoryPath;
                if (File.Exists(path) == false)
                    return;

                StreamReader reader = new StreamReader(path);
                while (reader.Peek() != -1)
                {
                    string equation = reader.ReadLine();
                    if (NeedToAdd(equation))
                    {
                        try
                        {
                            historyItems.Add(new CalculatorAndAnswer(equation));
                        }
                        catch
                        {
                            //ignored
                        }
                    }
                }

                reader.Close();
                DisplayItems();
            }

            public void ExportHistory(string path = null)
            {
                if (string.IsNullOrEmpty(path))
                    path = HistoryPath;
                StreamWriter writer = new StreamWriter(path);
                foreach (CalculatorAndAnswer item in historyItems)
                    writer.WriteLine(item.OriginalEquation);
                writer.Close();
            }

            public void CopyEntry()
            {
                try
                {
                    Clipboard.SetText(listbox.Items[listbox.SelectedIndex].ToString());
                }
                catch
                {
                    // ignored
                }
            }

            public void CopyCalculation()
            {
                try
                {
                    Clipboard.SetText(historyItems[ReverseIndex(listbox.SelectedIndex)].OriginalEquation);
                }
                catch
                {
                    // ignored
                }
            }

            public void CopyAnswer()
            {
                try
                {
                    Clipboard.SetText(historyItems[ReverseIndex(listbox.SelectedIndex)].CurrentAnswer);
                }
                catch
                {
                    // ignored
                }
            }

            /// <summary>
            ///     Inserts the calculation into the main calculation textbox of the Main Window, at the cursor.
            /// </summary>
            public void UseCalculationInMainCalculation()
            {
                Default.CalculatorWindow.InsertToCalculationTextboxAtCursor(
                    OperatorRepresentations.ParenthesisOpeningBracketSymbol +
                    historyItems[Math.Abs(listbox.SelectedIndex - historyItems.Count + 1)].OriginalEquation +
                    OperatorRepresentations.ParenthesisClosingBracketSymbol);
            }

            /// <summary>
            ///     Inserts the answer into the main calculation textbox of the Main Window, at the cursor.
            /// </summary>
            public void UseAnswerInMainCalculation()
            {
                Default.CalculatorWindow.InsertToCalculationTextboxAtCursor(
                    historyItems[ReverseIndex(listbox.SelectedIndex)].CurrentAnswer);
            }


            //--------------------Listbox Control--------------------
            public void DisplayItems()
            {
                //The items will be displayed in reverse order, due to the performance difference of adding to the start/end of a list.
                //(When adding to the start, all other items must be moved down, therefore O(n) instead of O(1).)

                if (listbox is null)
                    return;

                listbox.Items.Clear();
                for (int i = historyItems.Count - 1; i >= 0; i--) listbox.Items.Add(EquationAndAnswer(historyItems[i]));

                if (listbox.Items.Count == 0)
                {
                    exportButton.IsEnabled = false;
                }
                else
                {
                    exportButton.IsEnabled = true;
                    listbox.SelectedIndex = 0;
                }
            }

            private string EquationAndAnswer(CalculatorAndAnswer resolver) =>
                resolver.OriginalEquation + " " + OperatorRepresentations.EqualsSymbol + " " + resolver.CurrentAnswer;

            /// <summary>
            ///     Changes the font size of the history listbox to the exact size specified.
            /// </summary>
            /// <param name="size"></param>
            public void SetHistoryItemsFontSize(int size)
            {
                listbox.FontSize = size;
            }

            public void SetHistoryItemsFontFamily(FontFamily family)
            {
                if (listbox != null)
                    listbox.FontFamily = family;
            }
        }
    }
}