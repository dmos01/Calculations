using System;
using System.Globalization;
using MehrozFractions.Properties;

// ReSharper disable RedundantCaseLabel

namespace MehrozFractions
{
    public partial struct Fraction
    {
        /// <summary>
        ///     Get the integral value of the Fraction object as int/Int32
        /// </summary>
        /// <returns>The (approximate) integer value</returns>
        /// <remarks>
        ///     If the value is not a true integer, the fractional part is discarded
        ///     (truncated toward zero). If the valid exceeds the range of an Int32 and exception is thrown.
        /// </remarks>
        /// <exception cref="FractionException">
        ///     Will throw a FractionException for NaN, PositiveInfinity
        ///     or NegativeInfinity with the InnerException set to a System.NotFiniteNumberException.
        /// </exception>
        /// <exception>
        ///     cref="OverflowException" Will throw a System.OverflowException if the value is too
        ///     large or small to be represented as an Int32.
        /// </exception>
        public int ToInt32()
        {
            if (Denominator == 0)
            {
                throw new FractionException(
                    string.Format(Resources.CannotConvertBeforeParameter + IndeterminateTypeName(Numerator) +
                                  Resources.ToIntegerAfterParameter), new NotFiniteNumberException());
            }

            long bestGuess = Numerator / Denominator;

            if (bestGuess > int.MaxValue || bestGuess < int.MinValue)
                throw new FractionException(Resources.CannotConvertToIntegerDefault, new OverflowException());

            return (int) bestGuess;
        }

        /// <summary>
        ///     Get the integral value of the Fraction object as long/Int64
        /// </summary>
        /// <returns>The (approximate) integer value</returns>
        /// <remarks>
        ///     If the value is not a true integer, the fractional part is discarded
        ///     (truncated toward zero). If the valid exceeds the range of an Int32, no special
        ///     handling is guaranteed.
        /// </remarks>
        /// <exception cref="FractionException">
        ///     Will throw a FractionException for NaN, PositiveInfinity
        ///     or NegativeInfinity with the InnerException set to a System.NotFiniteNumberException.
        /// </exception>
        public long ToInt64()
        {
            if (Denominator == 0)
            {
                throw new FractionException(
                    string.Format(Resources.CannotConvertBeforeParameter + IndeterminateTypeName(Numerator) +
                                  Resources.ToLongAfterParameter),
                    new NotFiniteNumberException());
            }

            return Numerator / Denominator;
        }

        /// <summary>
        ///     Get the value of the Fraction object as double with full support for NaNs and infinities
        /// </summary>
        /// <returns>
        ///     The decimal representation of the Fraction, or double.NaN, double.NegativeInfinity
        ///     or double.PositiveInfinity
        /// </returns>
        public double ToDouble()
        {
            switch (Denominator)
            {
                case 1:
                    return Numerator;

                case 0:
                    switch (NormalizeIndeterminate(Numerator))
                    {
                        case Indeterminates.NegativeInfinity:
                            return double.NegativeInfinity;

                        case Indeterminates.PositiveInfinity:
                            return double.PositiveInfinity;

                        case Indeterminates.NaN:
                        default: // this can't happen
                            return double.NaN;
                    }

                default:
                    return Numerator / (double) Denominator;
            }
        }

        /// <summary>
        ///     Get the value of the Fraction as a string, with proper representation for NaNs and infinites
        /// </summary>
        /// <returns>
        ///     The string representation of the Fraction, or the culture-specific representations of
        ///     NaN, PositiveInfinity or NegativeInfinity.
        /// </returns>
        /// <remarks>The current culture determines the textual representation the Indeterminates</remarks>
        public override string ToString()
        {
            switch (Denominator)
            {
                case 1:
                    return Numerator.ToString();
                case 0:
                    return IndeterminateTypeName(Numerator);
                default:
                    return Numerator + Resources.SeperatorSymbol + Denominator;
            }
        }

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
            if (inValue is null)
                throw new ArgumentNullException(null, nameof(inValue));

            if (inValue == "")
                throw new ArgumentOutOfRangeException(null, nameof(inValue));

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
                    int decimalPos = inValue.IndexOf(info.CurrencyDecimalSeparator);

                    if (decimalPos > -1)
                        return new Fraction(Convert.ToDouble(inValue));
                    else
                        return new Fraction(Convert.ToInt64(inValue));
                }
            }
        }


        /// <summary>
        ///     Implicit conversion of a long integral value to a Fraction
        /// </summary>
        /// <param name="value">The long integral value to convert</param>
        /// <returns>A Fraction whose denominator is 1</returns>
        public static implicit operator Fraction(long value) => new Fraction(value);

        /// <summary>
        ///     Implicit conversion of a double floating point value to a Fraction
        /// </summary>
        /// <param name="value">The double value to convert</param>
        /// <returns>A reduced Fraction</returns>
        public static implicit operator Fraction(double value) => new Fraction(value);

        /// <summary>
        ///     Implicit conversion of a string to a Fraction
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>A reduced Fraction</returns>
        public static implicit operator Fraction(string value) => new Fraction(value);


        /// <summary>
        ///     Explicit conversion from a Fraction to an integer
        /// </summary>
        /// <param name="frac">the Fraction to convert</param>
        /// <returns>The integral representation of the Fraction</returns>
        public static explicit operator int(Fraction frac) => frac.ToInt32();

        /// <summary>
        ///     Explicit conversion from a Fraction to an integer
        /// </summary>
        /// <param name="frac">The Fraction to convert</param>
        /// <returns>The integral representation of the Fraction</returns>
        public static explicit operator long(Fraction frac) => frac.ToInt64();

        /// <summary>
        ///     Explicit conversion from a Fraction to a double floating-point value
        /// </summary>
        /// <param name="frac">The Fraction to convert</param>
        /// <returns>The double representation of the Fraction</returns>
        public static explicit operator double(Fraction frac) => frac.ToDouble();

        /// <summary>
        ///     Explicit conversion from a Fraction to a string
        /// </summary>
        /// <param name="frac">the Fraction to convert</param>
        /// <returns>The string representation of the Fraction</returns>
        public static implicit operator string(Fraction frac) => frac.ToString();
    }
}