using System;
using System.Runtime.InteropServices;
using MehrozFractions.Properties;

// ReSharper disable RedundantCaseLabel
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace MehrozFractions
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Fraction : IComparable, IFormattable
    {
        string IFormattable.ToString(string format, IFormatProvider formatProvider) =>
            Numerator.ToString(format, formatProvider) + Resources.SeperatorSymbol +
            Denominator.ToString(format, formatProvider);

        /// <summary>
        ///     The 'top' part of the fraction
        /// </summary>
        /// <example>For 3/4ths, this is the 3</example>
        public long Numerator { get; set; }

        /// <summary>
        ///     The 'bottom' part of the fraction
        /// </summary>
        /// <example>For 3/4ths, this is the 4</example>
        public long Denominator { get; set; }
    }
}