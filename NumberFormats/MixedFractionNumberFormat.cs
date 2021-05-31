using System;
using System.Text;
using EquationElements;
using static NumberFormats.FractionUtils;

namespace NumberFormats
{
    /// <summary>
    ///     a+b/c or -(a+b/c).
    /// </summary>
    public class MixedFractionNumberFormat : NumberFormat
    {
        public override string TypeAsString => "Mixed Fraction";

        /// <summary>
        ///     Returns the double representation as a mixed-fraction. That is, a+b/c or -(a+b/c). Uses
        ///     FractionUtils.IsApproximatelyThirdsOrSixths().
        /// </summary>
        /// <returns></returns>
        public override string Display(Number toDisplay)
        {
            if (StopIfNullZeroOrWholeNumber(toDisplay, out string nullZeroOrWhole))
                return nullZeroOrWhole;

            (bool isNegative, Number absoluteFloating) = GetNumberPortions(toDisplay);

            if (!IsApproximatelyThirdsOrSixths(absoluteFloating, out long numerator, out long denominator))
                (numerator, denominator) = ConvertToFraction(absoluteFloating.AsDouble);

            StringBuilder builder = new StringBuilder();
            if (isNegative)
            {
                builder.Append(OperatorRepresentations.SubtractionSymbol);
                Number whole = toDisplay + absoluteFloating;
                if (whole != 0)
                {
                    builder.Append(OperatorRepresentations.ParenthesisOpeningBracketSymbol);
                    AppendWhole(whole);
                }

                AppendFloatingFraction();
                if (whole != 0)
                    builder.Append(OperatorRepresentations.ParenthesisClosingBracketSymbol);
            }
            else
            {
                Number whole = toDisplay - absoluteFloating;
                if (whole != 0)
                    AppendWhole(whole);
                AppendFloatingFraction();
            }

            return builder.ToString();

            void AppendWhole(Number whole)
            {
                if (whole.IsDecimal)
                    builder.Append(Math.Abs(whole.AsDecimal).ToString("0"));
                else
                    builder.Append(Math.Abs(whole.AsDouble).ToString("0"));
                builder.Append(OperatorRepresentations.AdditionSymbol);
            }

            void AppendFloatingFraction()
            {
                builder.Append(numerator);
                builder.Append(OperatorRepresentations.ComputerDivisionSymbol);
                builder.Append(denominator);
            }
        }
    }
}