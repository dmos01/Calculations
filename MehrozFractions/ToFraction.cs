using System;
using System.Globalization;
using MehrozFractions.Properties;

namespace MehrozFractions
{
    public partial struct Fraction
    {
        /// <summary>
        ///     Converts a long value to the exact Fraction
        /// </summary>
        /// <param name="inValue">The long to convert</param>
        /// <returns>An exact representation of the value</returns>
        public static Fraction ToFraction(long inValue) => new Fraction(inValue);

        /// <summary>
        ///     Converts a double value to the approximate Fraction
        /// </summary>
        /// <param name="inValue">The double to convert</param>
        /// <returns>A best-fit representation of the value</returns>
        /// <remarks>Supports double.NaN, double.PositiveInfinity and double.NegativeInfinity</remarks>
        public static Fraction ToFraction(double inValue)
        {
            // it's one of the indeterminates... which?
            if (double.IsNaN(inValue))
                return NaN;
            else if (double.IsNegativeInfinity(inValue))
                return NegativeInfinity;
            else if (double.IsPositiveInfinity(inValue))
                return PositiveInfinity;
            else if (inValue == 0.0d)
                return Zero;

            if (inValue > long.MaxValue)
                throw new OverflowException(Resources.OverloadedNumberBeforeParameter + inValue +
                                            Resources.OverloadedNumberAfterParameter);

            if (inValue < -long.MaxValue)
                throw new OverflowException(Resources.OverloadedNumberBeforeParameter + inValue +
                                            Resources.OverloadedNumberAfterParameter);

            if (-EpsilonDouble < inValue && inValue < EpsilonDouble)
                throw new ArithmeticException(Resources.OverloadedNumberBeforeParameter + inValue +
                                              Resources.OverloadedNumberAfterParameter);

            int sign = Math.Sign(inValue);
            inValue = Math.Abs(inValue);

            return ConvertPositiveDouble(sign, inValue);
        }

        /// <summary>
        ///     Converts a string to the corresponding reduced fraction
        /// </summary>
        /// <param name="inValue">The string representation of a fractional value</param>
        /// <returns>The Fraction that represents the string</returns>
        /// <remarks>
        ///     Four forms are supported, as a plain integer, as a double, or as Numerator/Denominator
        ///     and the representations for NaN and the infinites
        /// </remarks>
        /// <example>
        ///     "123" = 123/1 and "1.25" = 5/4 and "10/36" = 5/13 and NaN = 0/0 and
        ///     PositiveInfinity = 1/0 and NegativeInfinity = -1/0
        /// </example>
        public static Fraction ToFraction(string inValue)
        {
            switch (inValue)
            {
                case null:
                    throw new ArgumentNullException(null, nameof(inValue));
                case "":
                    throw new ArgumentOutOfRangeException(null, nameof(inValue));
            }

            // could also be NumberFormatInfo.InvariantInfo
            NumberFormatInfo info = NumberFormatInfo.CurrentInfo;

            // Is it one of the special symbols for NaN and such...
            string trimmedValue = inValue.Trim();

            if (trimmedValue == info.NaNSymbol)
                return NaN;
            else if (trimmedValue == info.PositiveInfinitySymbol)
                return PositiveInfinity;
            else if (trimmedValue == info.NegativeInfinitySymbol)
                return NegativeInfinity;
            else
            {
                // Not special, is it a Fraction?
                int slashPos = inValue.IndexOf('/');

                if (slashPos > -1)
                {
                    // string is in the form of Numerator/Denominator
                    long numerator = Convert.ToInt64(inValue.Substring(0, slashPos));
                    long denominator = Convert.ToInt64(inValue.Substring(slashPos + 1));

                    return new Fraction(numerator, denominator);
                }
                else
                {
                    // the string is not in the form of a fraction
                    // hopefully it is double or integer, do we see a decimal point?
                    // ReSharper disable once StringIndexOfIsCultureSpecific.1
                    int decimalPos = inValue.IndexOf(info.CurrencyDecimalSeparator);

                    if (decimalPos > -1)
                        return new Fraction(Convert.ToDouble(inValue));
                    else
                        return new Fraction(Convert.ToInt64(inValue));
                }
            }
        }

        private static Fraction ConvertPositiveDouble(int sign, double inValue)
        {
            // Shamelessly stolen from http://homepage.smc.edu/kennedy_john/CONFRAC.PDF
            // with AccuracyFactor == double.Episilon
            long fractionNumerator = (long)inValue;
            double fractionDenominator = 1;
            double previousDenominator = 0;
            double remainingDigits = inValue;
            int maxIterations = 594; // found at http://www.ozgrid.com/forum/archive/index.php/t-22530.html

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            while (remainingDigits != Math.Floor(remainingDigits)
                   && Math.Abs(inValue - (fractionNumerator / fractionDenominator)) > double.Epsilon)
            {
                remainingDigits = 1.0 / (remainingDigits - Math.Floor(remainingDigits));

                double scratch = fractionDenominator;

                fractionDenominator = (Math.Floor(remainingDigits) * fractionDenominator) + previousDenominator;
                fractionNumerator = (long)(inValue * fractionDenominator + 0.5);

                previousDenominator = scratch;

                if (maxIterations-- < 0)
                    break;
            }

            return new Fraction(fractionNumerator * sign, (long)fractionDenominator);
        }
    }
}