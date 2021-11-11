using System;
using System.Runtime.InteropServices;

// ReSharper disable RedundantCaseLabel
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace MehrozFractions
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Fraction
    {
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