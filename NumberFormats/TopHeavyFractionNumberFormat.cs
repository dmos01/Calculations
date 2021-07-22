using EquationElements;
using static NumberFormats.FractionUtils;

namespace NumberFormats
{
    /// <summary>
    ///     "Normal" fraction format.
    /// </summary>
    public class TopHeavyFractionNumberFormat : NumberFormat
    {
        public override string TypeAsString => "Top-Heavy Fraction";

        /// <summary>
        ///     Returns the double representation as a fraction. That is, a/b. Uses FractionUtils.IsApproximatelyThirdsOrSixths().
        /// </summary>
        /// <returns></returns>
        public override string Display(Number toDisplay)
        {
            if (StopIfNullZeroOrWholeNumber(toDisplay, out string nullZeroOrWhole))
                return nullZeroOrWhole;

            (bool isNegative, Number absoluteFloating) = GetNumberPortions(toDisplay);

            if (IsApproximatelyThirdsOrSixths(absoluteFloating, out long numerator, out long denominator))
            {
                if (isNegative)
                {
                    Number whole = toDisplay + absoluteFloating;

                    //No *(Number, long) overload.
                    if (whole.IsDecimal)
                    {
                        decimal dec = whole.AsDecimal * denominator;
                        numerator -= long.Parse(dec.ToString("0"));
                        numerator *= -1;
                    }
                    else
                    {
                        double dou = whole.AsDouble * denominator;
                        numerator -= long.Parse(dou.ToString("0"));
                        numerator *= -1;
                    }
                }
                else
                {
                    Number whole = toDisplay - absoluteFloating;

                    //No *(Number, long) overload.
                    if (whole.IsDecimal)
                    {
                        decimal dec = whole.AsDecimal * denominator;
                        numerator += long.Parse(dec.ToString("0"));
                    }
                    else
                    {
                        double dou = whole.AsDouble * denominator;
                        numerator += long.Parse(dou.ToString("0"));
                    }
                }
            }
            else
                (numerator, denominator) = ConvertToFraction(toDisplay.AsDouble);

            return denominator == 1
                ? numerator.ToString()
                : numerator + OperatorRepresentations.ComputerDivisionSymbol + denominator;
        }
    }
}