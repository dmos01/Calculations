using System;
using System.Windows.Controls;
using System.Windows.Media;
using EquationElements;

namespace Calculations
{
    public partial class MainWindow
    {
        private Button[] DigitAndSymbolButtons =>
            new[]
            {
                btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn0, btnDecimalPoint, btnRoot,
                btnPower, btnPlus, btnMinus, btnDivide, btnBracketOpen, btnBracketClose, btnCurlyOpen,
                btnCurlyClose, btnSquareOpen, btnSquareClose
            };

        private Button[] FunctionAndEButtons =>
            new[]
            {
                btnSin, btnCos, btnTan, btnASin, btnACos, btnATan, btnSinh, btnCosh, btnTanh, btnLn, btnLog,
                btnFloor, btnRandom, btnCeiling, btnRound, btnTruncate, btnEvenRound, btnFactorial, btnAbsolute, btnMod,
                btnE
            };

        //Implicitly-typed array cannot contain textboxes and comboboxes (even when type is Control).
        private TextBox[] ConstantsTextboxes =>
            new[]
            {
                txtConstantsSearch, txtConstantValue, txtConstantUnit, txtConstantDescription
            };

        private Button[] UIButtons =>
            new[]
            {
                btnCopy, btnHistory, btnImportConstants, btnExportConstants, btnInsertConstant, btnDeleteConstant,
                btnSaveConstant, btnFontSettings
            };

        private Label[] UILabels =>
            new[]
            {
                lblConstantSearch, lblConstantName, lblConstantValue, lblConstantUnit, lblConstantDescription,
                lblSettingsShowAnswer, lblAbout
            };

        private CheckBox[] UICheckboxes =>
            new[]
            {
                chkRememberHistoryForNextTime, chkHistoryItemsOnlyAllowedOnce, chkHistoryMoveToTop
            };

        public void SetFontFamily(FontFamily forMainCalculationAndTextboxes,
            FontFamily forNumberOperatorAndFunctionButtons)
        {
            txtMainCalculation.FontFamily = forMainCalculationAndTextboxes;
            btnCalculate.FontFamily = forMainCalculationAndTextboxes;
            readOnlyTextboxAnswer.FontFamily = forMainCalculationAndTextboxes;
            cboConstants.FontFamily = forMainCalculationAndTextboxes;

            Array.ForEach(ConstantsTextboxes, txt => txt.FontFamily = forMainCalculationAndTextboxes);
            Array.ForEach(DigitAndSymbolButtons, btn => btn.FontFamily = forNumberOperatorAndFunctionButtons);
            Array.ForEach(FunctionAndEButtons, btn => btn.FontFamily = forNumberOperatorAndFunctionButtons);

            lblConstantName.Width = lblConstantSearch.Width;
            lblConstantValue.Width = lblConstantSearch.Width;
        }

        public void SetFontSizes(int mainCalcSize, int answerSize, int mainTextboxSize, int tabNameSize,
            int digitAndSymbolButtonSize, int functionAndEButtonSize, int uiButtonSize, int uiSize)
        {
            txtMainCalculation.FontSize = mainCalcSize;
            btnCalculate.FontSize = answerSize;
            readOnlyTextboxAnswer.FontSize = answerSize;

            tabKeypad.FontSize = tabNameSize;
            cboConstants.FontSize = mainTextboxSize;
            Array.ForEach(ConstantsTextboxes, txt => txt.FontSize = mainTextboxSize);
            Array.ForEach(DigitAndSymbolButtons, btn => btn.FontSize = digitAndSymbolButtonSize);
            Array.ForEach(FunctionAndEButtons, btn => btn.FontSize = functionAndEButtonSize);
            Array.ForEach(UIButtons, btn => btn.FontSize = uiButtonSize);
            Array.ForEach(UILabels, lbl => lbl.FontSize = uiSize);
            Array.ForEach(UICheckboxes, chk => chk.FontSize = uiSize);

            rbtDegrees.FontSize = uiSize;
            rbtRadians.FontSize = uiSize;
            cboAnswerFormat.FontSize = uiSize;

            lblConstantName.Width = lblConstantSearch.Width;
            lblConstantValue.Width = lblConstantSearch.Width;
        }

        public void ChangeFontSizesOfMainControls(int change)
        {
            txtMainCalculation.FontSize += change;
            btnCalculate.FontSize += change;
            readOnlyTextboxAnswer.FontSize += change;
            cboConstants.FontSize += change;

            Array.ForEach(ConstantsTextboxes, txt => txt.FontSize += change);
            Array.ForEach(DigitAndSymbolButtons, btn => btn.FontSize += change);
            Array.ForEach(FunctionAndEButtons, btn => btn.FontSize += change);

            lblConstantName.Width = lblConstantSearch.Width;
            lblConstantValue.Width = lblConstantSearch.Width;
        }

        public void ChangeFontSizesOfUIElements(int change)
        {
            tabKeypad.FontSize += change;
            Array.ForEach(UIButtons, btn => btn.FontSize += change);
            Array.ForEach(UILabels, lbl => lbl.FontSize += change);
            Array.ForEach(UICheckboxes, chk => chk.FontSize += change);

            rbtDegrees.FontSize += change;
            rbtRadians.FontSize += change;
            cboAnswerFormat.FontSize += change;

            lblConstantName.Width = lblConstantSearch.Width;
            lblConstantValue.Width = lblConstantSearch.Width;
        }
    }
}