using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using EquationBuilder;
using EquationElements.Operators;
using static EquationElements.Utils;
using static Calculations.Controller;

namespace Calculations
{
    public partial class MainWindow
    {
        readonly string nextToDigitsElementsForRegex;

        private void PopulateConstantsCombobox()
        {
            ICollection<string> names = Constants.SearchAllFieldsAndReturnNames(txtConstantsSearch.Text);
            cboConstants.Items.Clear();
            if (names.Any() == false) //No matching constants.
            {
                cboConstants.Text = "";
                cboConstants.IsEnabled = false;
                btnDeleteConstant.IsEnabled = false;
                btnExportConstants.IsEnabled = false;

                if (txtConstantsSearch.Text.Any() == false) //No constants exist.
                {
                    txtConstantsSearch.IsEnabled = false;
                    cboConstants.Focus();
                }
                else
                    txtConstantsSearch.Focus();
            }
            else
            {
                if (!txtConstantsSearch.Text.Any()) //Empty
                    cboConstants.Items.Add("");

                bool foundNonPiNonEulers = false;
                foreach (string name in names)
                {
                    cboConstants.Items.Add(name);
                    if (!IsOperator.StringIsPiOrEulers(name))
                        foundNonPiNonEulers = true;
                }

                btnExportConstants.IsEnabled = foundNonPiNonEulers;
                cboConstants.IsEnabled = true;
                txtConstantsSearch.IsEnabled = true;
                cboConstants.SelectedIndex = 0;
            }
        }

        private void SelectConstantInCombobox(string name)
        {
            PopulateConstantsCombobox();
            if (cboConstants.Items.Contains(name))
            {
                cboConstants.SelectedItem = name;
                txtConstantsSearch.Focus();
            }
        }

        private void BtnSaveConstant_Click(object sender, RoutedEventArgs e)
        {
            cboConstants.Text = cboConstants.Text.Trim();
            txtConstantValue.Text = txtConstantValue.Text.Trim();
            txtConstantUnit.Text = txtConstantUnit.Text.Trim();
            txtConstantDescription.Text = txtConstantDescription.Text.Trim();

            string nameWithoutSpaces = RemoveSpaces(cboConstants.Text);

            if (!ConstantsController.ConstantNameIsValid(nameWithoutSpaces, out string error))
            {
                MessageBox.Show(error, DialogResources.CannotAddConstantTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                cboConstants.Focus();
                return;
            }

            if (StopIfValueIsInvalidEquation())
                return;

            if (StopIfNameExistsOrDigitsSurroundRootModE())
                return;

            Constants.AddOrUpdate(cboConstants.Text, txtConstantValue.Text,
                txtConstantUnit.Text, txtConstantDescription.Text);
            SelectConstantInCombobox(cboConstants.Text);


            bool StopIfValueIsInvalidEquation()
            {
                try
                {
                    SplitAndValidate.Run(txtConstantValue.Text, Constants.GetNameValuePairs());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, DialogResources.CannotAddConstantTitle, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    txtConstantValue.Focus();
                    return true;
                }

                return false;
            }

            bool StopIfNameExistsOrDigitsSurroundRootModE()
            {
                //Constant name already exists
                if (Constants.Exists(nameWithoutSpaces))
                {
                    if (MessageBox.Show(
                        DialogResources.OverwriteConstantQuestionBeforeParameter + cboConstants.Text +
                        DialogResources.OverwriteConstantQuestionAfterParameter,
                        DialogResources.OverwriteConstantQuestionTitle,
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                    {
                        cboConstants.Focus();
                        return true;
                    }
                }
                else
                {
                    if (Regex.IsMatch(nameWithoutSpaces, ".*\\d+(" + nextToDigitsElementsForRegex + ")+.*",
                            RegexOptions.IgnoreCase) ||
                        Regex.IsMatch(nameWithoutSpaces, ".*(" + nextToDigitsElementsForRegex + ")+\\d+.*",
                            RegexOptions.IgnoreCase))
                    {
                        if (MessageBox.Show(DialogResources.ConstantNameNextToDigitsQuestion,
                            DialogResources.ConstantNameSurroundedByDigitsQuestionTitle, MessageBoxButton.OKCancel,
                            MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                        {
                            cboConstants.Focus();
                            return true;
                        }
                    }
                }

                return false;
            }
        }


        private void BtnDeleteConstant_Click(object sender, RoutedEventArgs e)
        {
            string nameWithoutSpaces = RemoveSpaces(cboConstants.Text);
            if (IsNullEmptyOrOnlySpaces(nameWithoutSpaces))
                return;

            if (IsOperator.StringIsPiOrEulers(nameWithoutSpaces))
            {
                MessageBox.Show(DialogResources.PiAndECannotBeDeletedMessage,
                    DialogResources.CannotDeleteConstantTitle);
                return;
            }

            if (MessageBox.Show(
                DialogResources.DeleteConstantQuestionDialogMessageBeforeParameter + cboConstants.Text.Trim() +
                DialogResources.DeleteConstantQuestionDialogMessageAfterParameter,
                DialogResources.DeleteConstantQuestionTitle,
                MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Constants.Delete(nameWithoutSpaces);
                PopulateConstantsCombobox();
                if (txtConstantsSearch.IsEnabled)
                    txtConstantsSearch.Focus();
                else
                    cboConstants.Focus();
            }
            else
                cboConstants.Focus();
        }

        private void BtnImportConstants_Click(object sender, RoutedEventArgs e)
        {
            string file =
                RunImportDialogAndReturnFilePath(DialogResources.ImportConstantsTitle,
                    CalculationsResources.XMLFile + "|*.xml|" + CalculationsResources.TextFile + "|*.txt",
                    "xml");
            if (file.Any())
                Constants.ImportConstants(file);

            if (txtConstantsSearch.Text == "")
                PopulateConstantsCombobox();
            else
                txtConstantsSearch.Text = "";

            if (txtConstantsSearch.IsEnabled)
                txtConstantsSearch.Focus();
            else
                cboConstants.Focus();
        }

        private void BtnExportConstants_Click(object sender, RoutedEventArgs e)
        {
            txtConstantsSearch.Text = txtConstantsSearch.Text.Trim();
            string file = RunExportDialogAndReturnFilePath(
                txtConstantsSearch.Text == ""
                    ? DialogResources.ExportAllConstantsTitle
                    : DialogResources.ExportConstantsSearchResultsTitle,
                CalculationsResources.XMLFile + "|*.xml|" + CalculationsResources.TextFile + "|*.txt", "xml");

            if (file.Any())
                Constants.ExportConstants(file, txtConstantsSearch.Text);
            txtConstantsSearch.Focus();
        }

        private void BtnInsertConstant_Click(object sender, RoutedEventArgs e) =>
            InsertToCalculationTextboxAtCursor(cboConstants.Text);

        private void TxtConstantsSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtConstantsSearch.Text == " ")
            {
                txtConstantsSearch.Text = "";
                return;
            }

            PopulateConstantsCombobox();
        }


        private void CboConstants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*SelectedIndex and SelectedItem instead of Text because Text will not update
              until the end of this method. CboConstantsTextChanged() will then be called. This code is
              here rather than there because TextChanged() only applies to editable comboboxes.*/
            if (cboConstants.SelectedIndex != -1 && Constants.TryGetProperties(cboConstants.SelectedItem.ToString(),
                out string value, out string unit, out string description))
            {
                txtConstantValue.Text = value;
                txtConstantUnit.Text = unit;
                txtConstantDescription.Text = description;
                btnInsertConstant.IsEnabled = true;

                bool piOrEulers = IsOperator.StringIsPiOrEulers(RemoveSpaces(cboConstants.SelectedItem.ToString()));
                txtConstantValue.IsReadOnly = piOrEulers;
                txtConstantUnit.IsReadOnly = piOrEulers;
                txtConstantDescription.IsReadOnly = piOrEulers;
                btnDeleteConstant.IsEnabled = !piOrEulers;
                //Overridden by CboConstantsTextChanged(). Not removed in case move back to non-editable dropdown.
                //btnSaveConstant.IsEnabled = !piOrEulers;
            }
            else
            {
                txtConstantValue.Text = "";
                txtConstantUnit.Text = "";
                txtConstantDescription.Text = "";

                btnInsertConstant.IsEnabled = false;
                txtConstantValue.IsReadOnly = false;
                txtConstantUnit.IsReadOnly = false;
                txtConstantDescription.IsReadOnly = false;
                btnDeleteConstant.IsEnabled = false;
                //Overridden by CboConstantsTextChanged(). Not removed in case move back to non-editable dropdown.
                //btnSaveConstant.IsEnabled = false;
            }
        }

        private void CboConstantsTextChanged(object sender, TextChangedEventArgs e)
        {
            //Called with TextBoxBase.TextChanged="CboConstantsTextChanged". Only applies to editable comboboxes.
            btnSaveConstant.IsEnabled = cboConstants.Text.Any() && txtConstantValue.Text.Any() &&
                                        !IsOperator.StringIsPiOrEulers(cboConstants.Text);
        }

        private void TxtConstantsValueTextChanged(object sender, TextChangedEventArgs e)
        {
            //Assumes value textbox was made not editable when name is Pi or Eulers.
            btnSaveConstant.IsEnabled = cboConstants.Text.Any() && txtConstantValue.Text.Any();
        }
    }
}