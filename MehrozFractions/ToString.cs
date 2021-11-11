using System;
using MehrozFractions.Properties;

namespace MehrozFractions
{
    public partial struct Fraction : IFormattable
    {
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

        string IFormattable.ToString(string format, IFormatProvider formatProvider) =>
            Numerator.ToString(format, formatProvider) + Resources.SeperatorSymbol +
            Denominator.ToString(format, formatProvider);
    }
}