using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            List<string> temp = new List<string>(4)
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
            btn1.Content = NumberRepresentations.OneSymbol;
            btn2.Content = NumberRepresentations.TwoSymbol;
            btn3.Content = NumberRepresentations.ThreeSymbol;
            btn4.Content = NumberRepresentations.FourSymbol;
            btn5.Content = NumberRepresentations.FiveSymbol;
            btn6.Content = NumberRepresentations.SixSymbol;
            btn7.Content = NumberRepresentations.SevenSymbol;
            btn8.Content = NumberRepresentations.EightSymbol;
            btn9.Content = NumberRepresentations.NineSymbol;
            btn0.Content = NumberRepresentations.ZeroSymbol;

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

            btnE.ToolTip = TooltipMessages.ETooltip;
            btnPlus.ToolTip = TooltipMessages.PlusTooltip;
            btnMinus.ToolTip = TooltipMessages.MinusTooltip;
            btnTimes.ToolTip = TooltipMessages.TimesTooltip;
            btnDivide.ToolTip = TooltipMessages.DivideTooltip;
            btnPower.ToolTip = TooltipMessages.PowerTooltip;
            btnRoot.ToolTip = string.Join(line, TooltipMessages.RootDescription, TooltipMessages.RootFormat,
                TooltipMessages.RootFormatLine2,
                TooltipMessages.AlsoAllowedBeforeParameter + OperatorRepresentations.RootWord +
                TooltipMessages.AlsoAllowedAfterParameter);

            btnSin.ToolTip = TooltipMessages.SinFormat;
            btnCos.ToolTip = TooltipMessages.CosFormat;
            btnTan.ToolTip = TooltipMessages.TanFormat;
            btnASin.ToolTip = TooltipMessages.ASinFormat;
            btnACos.ToolTip = TooltipMessages.ACosFormat;
            btnATan.ToolTip = TooltipMessages.ATanFormat;
            btnSinh.ToolTip = TooltipMessages.SinhFormat;
            btnCosh.ToolTip = TooltipMessages.CoshFormat;
            btnTanh.ToolTip = TooltipMessages.TanhFormat;
            btnLn.ToolTip = string.Join(line, TooltipMessages.LnDescription, TooltipMessages.LnFormat);
            btnLog.ToolTip = string.Join(line, TooltipMessages.LogFormat, TooltipMessages.LogFormatLine2);

            btnFloor.ToolTip = string.Join(TooltipMessages.FloorDescription, TooltipMessages.FloorFormat);
            btnRandom.ToolTip =
                string.Join(line, TooltipMessages.RandomDescription, TooltipMessages.RandomDescriptionLine2,
                    TooltipMessages.RandomFormat,
                    TooltipMessages.AlsoAllowedBeforeParameter + FunctionRepresentations.RandomShortWord +
                    TooltipMessages.AlsoAllowedAfterParameter);
            btnCeiling.ToolTip = string.Join(line, TooltipMessages.CeilingDescription, TooltipMessages.CeilingFormat);
            btnRound.ToolTip = string.Join(line, TooltipMessages.RoundDescription, TooltipMessages.RoundOnMidpoint,
                TooltipMessages.RoundOnMidpointLine2, TooltipMessages.RoundFormat);
            btnTruncate.ToolTip =
                string.Join(line, TooltipMessages.TruncateDescription, TooltipMessages.TruncateFormat);
            btnEvenRound.ToolTip = string.Join(line, TooltipMessages.EvenRoundDescription,
                TooltipMessages.EvenRoundOnMidpoint,
                TooltipMessages.EvenRoundFormat);
            btnMod.ToolTip = string.Join(line, TooltipMessages.ModDescription, TooltipMessages.ModFormat,
                TooltipMessages.AlsoAllowedBeforeParameter + OperatorRepresentations.ModulusSymbol +
                TooltipMessages.AlsoAllowedAfterParameter);
            btnFactorial.ToolTip = string.Join(line, TooltipMessages.FactorialDescription,
                TooltipMessages.FactorialDescriptionLine2, TooltipMessages.FactorialFormat,
                TooltipMessages.AlsoAllowedBeforeParameter + FunctionRepresentations.FactorialSymbol +
                TooltipMessages.AlsoAllowedAfterParameter);
            btnAbsolute.ToolTip =
                string.Join(line, TooltipMessages.AbsoluteDescription, TooltipMessages.AbsoluteFormat,
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
            chkRememberHistoryForNextTime.IsChecked = Settings.Default.RememberHistoryForNextTime;
            lblAbout.Content = CalculationsResources.Version + " " + CalculationsResources.VersionNumber;

            if (Radians)
                rbtRadians.IsChecked = true;
            else
                rbtDegrees.IsChecked = true;


            cboAnswerFormat.Items[0] = new DecimalNumberFormat().TypeAsString;
            cboAnswerFormat.Items[1] = new DecimalNumber2DPNumberFormat().TypeAsString;
            cboAnswerFormat.Items[2] = new TopHeavyFractionNumberFormat().TypeAsString;
            cboAnswerFormat.Items[3] = new MixedFractionNumberFormat().TypeAsString;

            switch (CurrentAnswerFormat)
            {
                case DecimalNumber2DPNumberFormat _:
                    cboAnswerFormat.SelectedIndex = 1;
                    break;
                case TopHeavyFractionNumberFormat _:
                    cboAnswerFormat.SelectedIndex = 2;
                    break;
                case MixedFractionNumberFormat _:
                    cboAnswerFormat.SelectedIndex = 3;
                    break;
                default:
                    cboAnswerFormat.SelectedIndex = 0;
                    break;
            }


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

        private void WinMain_Closing(object sender, CancelEventArgs e)
        {
            Quit();
        }
    }
}