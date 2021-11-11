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
    }
}