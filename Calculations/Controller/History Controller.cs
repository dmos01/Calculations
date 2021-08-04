using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using EquationElements;
using static EquationElements.Utils;

namespace Calculations
{
    partial class Controller
    {
        public class HistoryController
        {
            private List<CalculatorAndAnswer> historyItems { get; }
            public string HistoryPath { get; }

            public HistoryController(string historyPath)
            {
                historyItems = new List<CalculatorAndAnswer>();
                HistoryPath = historyPath;
            }

            public void Add(CalculatorAndAnswer resolver)
            {
                if (!NeedToAdd(resolver.OriginalEquation))
                    return;

                historyItems.Add(resolver);
                DisplayItems();
            }

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

                    //HistoryItemsCanAppear.OnceButMoveToTopIfAddedAgain by elimination.
                    historyItems.RemoveAt(i);
                    return true;
                }

                return true;
            }

            public void RemoveAt(int index) => historyItems.RemoveAt(index);

            public void MoveItem(int originalIndex, int moveBy)
            {
                if (moveBy == 0)
                    return;

                CalculatorAndAnswer toMove = historyItems[originalIndex];
                historyItems.RemoveAt(originalIndex);
                historyItems.Insert(originalIndex + moveBy, toMove);

                //DisplayItems();
            }

            public void Clear() => historyItems.Clear();

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
            ///     Imports Calculations, calculates tham and adds them to history. Skips duplicates depending on the
            ///     HistoryItemsCanAppear setting. Skips invalid Calculations.
            /// </summary>
            /// <param name="path">The full path and file name to import from.</param>
            public void ImportHistory(string path = null)
            {
                if (IsNullEmptyOrOnlySpaces(path))
                    path = HistoryPath;
                if (File.Exists(path) == false)
                    return;

                StreamReader reader = new(path);
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
                if (IsNullEmptyOrOnlySpaces(path))
                    path = HistoryPath;
                StreamWriter writer = new(path);
                foreach (CalculatorAndAnswer item in historyItems)
                    writer.WriteLine(item.OriginalEquation);
                writer.Close();
            }

            public void CopyCalculation(int index)
            {
                try
                {
                    Clipboard.SetText(historyItems[index].OriginalEquation);
                }
                catch
                {
                    // ignored
                }
            }

            public void CopyAnswer(int index)
            {
                try
                {
                    Clipboard.SetText(historyItems[index].CurrentAnswer);
                }
                catch
                {
                    // ignored
                }
            }

            public void CopyCalculationAndAnswer(int index)
            {
                try
                {
                    CalculatorAndAnswer item = historyItems[index];
                    Clipboard.SetText(item.OriginalEquation + " " + OperatorRepresentations.EqualsSymbol + " " +
                                      item.CurrentAnswer);
                }
                catch
                {
                    // ignored
                }
            }

            /// <summary>
            ///     Inserts the calculation into the main calculation textbox of the Main Window, at the cursor.
            /// </summary>
            public void UseCalculationInMainCalculation(int index) =>
                Default.CalculatorWindow.InsertToCalculationTextboxAtCursor(
                    OperatorRepresentations.ParenthesisOpeningBracketSymbol +
                    historyItems[index].OriginalEquation +
                    OperatorRepresentations.ParenthesisClosingBracketSymbol);

            /// <summary>
            ///     Inserts the answer into the main calculation textbox of the Main Window, at the cursor.
            /// </summary>
            public void UseAnswerInMainCalculation(int index) =>
                Default.CalculatorWindow.InsertToCalculationTextboxAtCursor(
                    historyItems[index].CurrentAnswer);

            public void DisplayItems() =>
                Default.HistoryWindow?.DisplayItems(historyItems.Select(x =>
                    x.OriginalEquation + " " + OperatorRepresentations.EqualsSymbol + " " + x.CurrentAnswer).ToList());
        }
    }
}