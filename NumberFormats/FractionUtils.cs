using System;
using System.Globalization;
using System.Linq;
using EquationElements;
using static EquationElements.Utils;

namespace NumberFormats
{
    /// <summary>
    ///     Static class
    /// </summary>
    public static class FractionUtils
    {
        private const int MinDP = 3;
        private const int MinDPForOneSixth = 5;

        /// <summary>
        ///     <para>Cheat to pretend that any number 0.333... is 1/3, 0.666... is 2/3 and 0.16666... is 1/6.</para>
        ///     <para>Minimum 3dp (5dp for 1/6) and all digits 3 or 6. (For 2/3 and 1/6, the last digit may be a 7.)</para>
        /// </summary>
        /// <param name="absoluteFloating">Very important that a decimal can be used, if possible.</param>
        /// <param name="numerator">0 if false.</param>
        /// <param name="denominator">0 if false.</param>
        /// <returns></returns>
        public static bool IsApproximatelyThirdsOrSixths(Number absoluteFloating, out long numerator,
            out long denominator)
        {
            string floatingString = absoluteFloating.ToString().Substring(2);

            //Minimum 3dp.
            if (floatingString.Length < MinDP)
            {
                numerator = 0;
                denominator = 0;
                return false;
            }

            if (floatingString.All(x => x == '3'))
            {
                numerator = 1;
                denominator = 3;
                return true;
            }

            //2/3 - All but the last digit must be 6 (which may be a 6 or 7).
            //1/6 - As above, but starts with a 1. Minimum 5dp.
            using (CharEnumerator enumerator = floatingString.GetEnumerator())
            {
                enumerator.MoveNext();
                char current = enumerator.Current;
                bool testingForTwoThirds; //If false, 1/6.

                if (current == 1.ToString()[0] && floatingString.Length >= MinDPForOneSixth)
                    testingForTwoThirds = false;
                else if (current == 6.ToString()[0])
                    testingForTwoThirds = true;
                else
                {
                    numerator = 0;
                    denominator = 0;
                    return false;
                }

                //Find the first non-6 character, or last character.
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current != 6.ToString()[0])
                        break;
                }

                if ((current == 6.ToString()[0] ||
                     current == 7.ToString()[0])
                    && enumerator.MoveNext() == false)
                {
                    if (testingForTwoThirds)
                    {
                        numerator = 2;
                        denominator = 3;
                    }
                    else
                    {
                        numerator = 1;
                        denominator = 6;
                    }

                    return true;
                }
            }

            numerator = 0;
            denominator = 0;
            return false;

            /*Alternate code (not requiring all digits to be a certain number):
            if (HasMinimalDifference(absoluteFloating, (double)1 / 3))
            {
                numerator = 1;
                denominator = 3;
                return true;
            }
            else if (HasMinimalDifference(absoluteFloating, (double)2 / 3))
            {
                numerator = 2;
                denominator = 3;
                return true;
            }
            else if (HasMinimalDifference(absoluteFloating, (double)1 / 6))
            {
                numerator = 1;
                denominator = 6;
                return true;
            }*/
        }

        /// <summary>
        ///     <para>
        ///         Based on the fraction class (version 2.3) by Syed Mehroz Alam, licensed under the Code Project Open
        ///         License(CPOL).
        ///     </para>
        ///     <para>Changes by Marc C. Brooks and Jeffery Sax.</para>
        /// </summary>
        /// <param name="asDouble"></param>
        /// <returns></returns>
        public static (long numerator, long denominator) ConvertToFraction(double asDouble)
        {
            int sign = Math.Sign(asDouble);
            asDouble = Math.Abs(asDouble);

            // Shamelessly stolen from http://homepage.smc.edu/kennedy_john/CONFRAC.PDF
            // with AccuracyFactor == double.Episilon
            long fractionNumerator = (long) asDouble;
            double fractionDenominator = 1;
            double previousDenominator = 0;
            double remainingDigits = asDouble;
            int maxIterations = 594; // found at http://www.ozgrid.com/forum/archive/index.php/t-22530.html

            while (HasMinimalDifference(remainingDigits, Math.Floor(remainingDigits)) == false
                   && Math.Abs(asDouble - (fractionNumerator / fractionDenominator)) > double.Epsilon)
            {
                remainingDigits = 1.0 / (remainingDigits - Math.Floor(remainingDigits));

                double scratch = fractionDenominator;

                fractionDenominator = (Math.Floor(remainingDigits) * fractionDenominator) + previousDenominator;
                fractionNumerator = (long) (asDouble * fractionDenominator + 0.5);

                previousDenominator = scratch;

                if (maxIterations-- < 0)
                    throw new ArgumentOutOfRangeException(null, ElementsExceptionMessages.OverloadedNumberDefault);
            }

            return (fractionNumerator * sign, (long) fractionDenominator);
        }

        internal static bool StopIfNullZeroOrWholeNumber(Number toDisplay, out string outputIfStopping)
        {
            if (toDisplay is null)
            {
                outputIfStopping = null;
                return true;
            }

            if (HasMinimalDifference(toDisplay.AsDouble, 0))
            {
                outputIfStopping = 0.ToString();
                return true;
            }

            if (HasMinimalDifference(toDisplay.AsDouble % 1, 0))
            {
                outputIfStopping = toDisplay.AsDouble.ToString(CultureInfo.CurrentCulture);
                return true;
            }

            outputIfStopping = null;
            return false;
        }

        /// <summary>
        ///     Returns whether the Number is less than 0; Math.abs(the part of the number after the decimal point).
        /// </summary>
        /// <param name="toDisplay"></param>
        /// <returns></returns>
        internal static (bool isNegative, Number absoluteFloating) GetNumberPortions(Number toDisplay)
        {
            bool isNegative = toDisplay < 0;
            Number absoluteFloating = toDisplay.IsDecimal
                ? new Number(Math.Abs(toDisplay.AsDecimal % 1))
                : new Number(Math.Abs(toDisplay.AsDouble % 1));
            return (isNegative, absoluteFloating);
        }
    }
}