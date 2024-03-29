﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using EquationElements;
using EquationElements.Operators;
using NumberFormats;
using static Calculations.Controller;

namespace Calculations
{
    public partial class MainWindow
    {
        //const byte KeypadColumnMargin = 15;
        //const byte KeypadRowMargin = 15;
        //const byte ThirdOfKeypadMargin = 5;
        //const byte TinyGap = 2;

        public MainWindow()
        {
            InitializeComponent();
            SetButtonText();
            SetTooltips();
            currentCalculatorAndAnswer = null;

            //Allows nextToDigitsElementsForRegex to be readonly.
            List<string> temp = new(4)
            {
                OperatorRepresentations.ModulusWord,
                OperatorRepresentations.RootWord,
                ElementsResources.EulersSymbolUpperCase
            };
            if (IsOperator.EulersAndExponentSymbolsAreDifferent)
                temp.Add(ElementsResources.ExponentSymbolUpperCase);
            nextToDigitsElementsForRegex = string.Join('|', temp);
        }

        private void SetButtonText()
        {
            btn1.Content = 1.ToString();
            btn2.Content = 2.ToString();
            btn3.Content = 3.ToString();
            btn4.Content = 4.ToString();
            btn5.Content = 5.ToString();
            btn6.Content = 6.ToString();
            btn7.Content = 7.ToString();
            btn8.Content = 8.ToString();
            btn9.Content = 9.ToString();
            btn0.Content = 0.ToString();

            btnE.Content = ElementsResources.EulersSymbolUpperCase;
            btnDecimalPoint.Content = ElementsResources.DecimalSymbol;
            btnRoot.Content = OperatorRepresentations.RootSymbol;
            btnPower.Content = OperatorRepresentations.PowerSymbol;
            btnPlus.Content = OperatorRepresentations.AdditionSymbol;
            btnMinus.Content = OperatorRepresentations.SubtractionSymbol;
            btnTimes.Content = OperatorRepresentations.ComputerMultiplicationSymbol;
            btnDivide.Content = OperatorRepresentations.ComputerDivisionSymbol;
            btnSquareOpen.Content = OperatorRepresentations.SquareOpeningBracketSymbol;
            btnSquareClose.Content = OperatorRepresentations.SquareClosingBracketSymbol;
            btnCurlyOpen.Content = OperatorRepresentations.CurlyOpeningBracketSymbol;
            btnCurlyClose.Content = OperatorRepresentations.CurlyClosingBracketSymbol;
            btnBracketOpen.Content = OperatorRepresentations.ParenthesisOpeningBracketSymbol;
            btnBracketClose.Content = OperatorRepresentations.ParenthesisClosingBracketSymbol;
        }

        private void SetTooltips()
        {
            string line = Environment.NewLine;

            btnE.ToolTip = TooltipMessages.E;
            btnPlus.ToolTip = TooltipMessages.Plus;
            btnMinus.ToolTip = TooltipMessages.Minus;
            btnTimes.ToolTip = TooltipMessages.Times;
            btnDivide.ToolTip = TooltipMessages.Divide;
            btnPower.ToolTip = TooltipMessages.Power;
            btnRoot.ToolTip = string.Join(line, TooltipMessages.RootMain,
                TooltipMessages.AlsoAllowedBeforeParameter + OperatorRepresentations.RootWord +
                TooltipMessages.AlsoAllowedAfterParameter);

            btnSin.ToolTip = TooltipMessages.Sin;
            btnCos.ToolTip = TooltipMessages.Cos;
            btnTan.ToolTip = TooltipMessages.Tan;
            btnASin.ToolTip = TooltipMessages.ASin;
            btnACos.ToolTip = TooltipMessages.ACos;
            btnATan.ToolTip = TooltipMessages.ATan;
            btnSinh.ToolTip = TooltipMessages.Sinh;
            btnCosh.ToolTip = TooltipMessages.Cosh;
            btnTanh.ToolTip = TooltipMessages.Tanh;
            btnLn.ToolTip = TooltipMessages.Ln;
            btnLog.ToolTip = TooltipMessages.Log;

            btnFloor.ToolTip = TooltipMessages.Floor;
            btnRandom.ToolTip =
                string.Join(line, TooltipMessages.RandomMain,
                    TooltipMessages.AlsoAllowedBeforeParameter + FunctionRepresentations.RandomShortWord +
                    TooltipMessages.AlsoAllowedAfterParameter);
            btnCeiling.ToolTip = TooltipMessages.Ceiling;
            btnRound.ToolTip = TooltipMessages.Round;
            btnTruncate.ToolTip = TooltipMessages.Truncate;
            btnEvenRound.ToolTip = TooltipMessages.EvenRound;
            btnMod.ToolTip = string.Join(line, TooltipMessages.ModMain,
                TooltipMessages.AlsoAllowedBeforeParameter + OperatorRepresentations.ModulusSymbol +
                TooltipMessages.AlsoAllowedAfterParameter);
            btnFactorial.ToolTip = string.Join(line, TooltipMessages.FactorialMain,
                TooltipMessages.AlsoAllowedBeforeParameter + FunctionRepresentations.FactorialSymbol +
                TooltipMessages.AlsoAllowedAfterParameter);
            btnAbsolute.ToolTip =
                string.Join(line, TooltipMessages.AbsoluteMain,
                    TooltipMessages.AlsoAllowedBeforeParameter + FunctionRepresentations.AbsoluteShortWord +
                    TooltipMessages.AlsoAllowedAfterParameter);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Startup(this);
            SetSettings();
            SetEventHandlers();
            PopulateConstantsCombobox();
            btnCalculate.IsDefault = true;
            txtMainCalculation.Focus();
        }

        private void SetSettings()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;

            chkRememberHistoryForNextTime.IsChecked = Settings.Default.RememberHistoryForNextTime;
            lblAbout.Content = CalculationsResources.VersionLiteral + " " + v.Major + ElementsResources.DecimalSymbol +
                               v.Minor + ElementsResources.DecimalSymbol + v.Build;

            if (Radians)
                rbtRadians.IsChecked = true;
            else
                rbtDegrees.IsChecked = true;


            cboAnswerFormat.Items[0] = new DecimalNumberFormat().TypeAsString;
            cboAnswerFormat.Items[1] = new DecimalNumber2DPNumberFormat().TypeAsString;
            cboAnswerFormat.Items[2] = new TopHeavyFractionNumberFormat().TypeAsString;
            cboAnswerFormat.Items[3] = new MixedFractionNumberFormat().TypeAsString;

            cboAnswerFormat.SelectedIndex = CurrentAnswerFormat switch
            {
                DecimalNumber2DPNumberFormat => 1,
                TopHeavyFractionNumberFormat => 2,
                MixedFractionNumberFormat => 3,
                _ => 0
            };

            switch (HistoryItemsSetting)
            {
                case HistoryItemsCanAppear.ManyTimes:
                    chkHistoryMoveToTop.Visibility = Visibility.Hidden;
                    break;
                case HistoryItemsCanAppear.Once:
                    chkHistoryItemsOnlyAllowedOnce.IsChecked = true;
                    break;
                case HistoryItemsCanAppear.OnceButMoveToTopIfAddedAgain:
                    chkHistoryItemsOnlyAllowedOnce.IsChecked = true;
                    chkHistoryMoveToTop.IsChecked = true;
                    break;
            }
        }

        private void SetEventHandlers()
        {
            cboAnswerFormat.SelectionChanged += CboAnswerFormat_SelectionChanged;
            chkRememberHistoryForNextTime.Checked += ChkRememberHistoryForNextTime_CheckedStateChanged;
            chkRememberHistoryForNextTime.Unchecked += ChkRememberHistoryForNextTime_CheckedStateChanged;
            chkHistoryItemsOnlyAllowedOnce.Checked += ChkHistoryItemsOnlyAppearOnce_Checked;
            chkHistoryItemsOnlyAllowedOnce.Unchecked += ChkHistoryItemsOnlyAppearOnce_Unchecked;
            chkHistoryMoveToTop.Checked += ChkHistoryMoveToTop_CheckedStateChanged;
            chkHistoryMoveToTop.Unchecked += ChkHistoryMoveToTop_CheckedStateChanged;
            rbtDegrees.Checked += RbtDegrees_Checked;
            rbtRadians.Checked += RbtRadians_Checked;
        }

        private void WinMain_Closing(object sender, CancelEventArgs e) => Quit();
    }
}