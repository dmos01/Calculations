using System;
using System.Globalization;

// ReSharper disable RedundantCaseLabel

namespace MehrozFractions
{
    // These are used to represent the indeterminate with a Denominator of zero

    public partial struct Fraction
    {
        /// <summary>
        ///     Gives the culture-related representation of the indeterminate types NaN, PositiveInfinity
        ///     and NegativeInfinity
        /// </summary>
        /// <param name="numerator">The value in the numerator</param>
        /// <returns>The culture-specific string representation of the implied value</returns>
        /// <remarks>Only the sign and zero/non-zero information is relevant.</remarks>
        private static string IndeterminateTypeName(long numerator)
        {
            // could also be NumberFormatInfo.InvariantInfo
            NumberFormatInfo info = NumberFormatInfo.CurrentInfo;

            switch (NormalizeIndeterminate(numerator))
            {
                case Indeterminates.PositiveInfinity:
                    return info.PositiveInfinitySymbol;

                case Indeterminates.NegativeInfinity:
                    return info.NegativeInfinitySymbol;

                case Indeterminates.NaN:
                default: // if this happens, something VERY wrong is going on...
                    return info.NaNSymbol;
            }
        }

        /// <summary>
        ///     Gives the normalize representation of the indeterminate types NaN, PositiveInfinity
        ///     and NegativeInfinity
        /// </summary>
        /// <param name="numerator">The value in the numerator</param>
        /// <returns>The normalized version of the indeterminate type</returns>
        /// <remarks>Only the sign and zero/non-zero information is relevant.</remarks>
        private static Indeterminates NormalizeIndeterminate(long numerator)
        {
            switch (Math.Sign(numerator))
            {
                case 1:
                    return Indeterminates.PositiveInfinity;

                case -1:
                    return Indeterminates.NegativeInfinity;

                case 0:
                default: // if this happens, your Math.Sign function is BROKEN!
                    return Indeterminates.NaN;
            }
        }

        private enum Indeterminates
        {
            NaN = 0,
            PositiveInfinity = 1,
            NegativeInfinity = -1
        }

        /// <summary>
        ///     Determines if a Fraction represents a Not-a-Number
        /// </summary>
        /// <returns>True if the Fraction is a NaN</returns>
        public bool IsNaN() =>
            Denominator == 0 && NormalizeIndeterminate(Numerator) == Indeterminates.NaN;

        /// <summary>
        ///     Determines if a Fraction represents Any Infinity
        /// </summary>
        /// <returns>True if the Fraction is Positive Infinity or Negative Infinity</returns>
        public bool IsInfinity() => Denominator == 0 && NormalizeIndeterminate(Numerator) != Indeterminates.NaN;

        /// <summary>
        ///     Determines if a Fraction represents Positive Infinity
        /// </summary>
        /// <returns>True if the Fraction is Positive Infinity</returns>
        public bool IsPositiveInfinity() =>
            Denominator == 0 && NormalizeIndeterminate(Numerator) == Indeterminates.PositiveInfinity;

        /// <summary>
        ///     Determines if a Fraction represents Negative Infinity
        /// </summary>
        /// <returns>True if the Fraction is Negative Infinity</returns>
        public bool IsNegativeInfinity() =>
            Denominator == 0 && NormalizeIndeterminate(Numerator) == Indeterminates.NegativeInfinity;
    }
}