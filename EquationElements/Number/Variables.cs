using System;
using System.Globalization;

namespace EquationElements
{
    /// <summary>
    ///     Stores a double, or a decimal and its double representation.
    ///     Simulates handling of decimals primarily and being able to "switch" from decimal to double if necessary.
    /// </summary>
    partial class Number : IFormattable
    {
        /// <summary>
        ///     True if instantiated with a decimal or integer (or applicable string representation).
        ///     If false, AsDecimal cannot be used.
        /// </summary>
        public bool IsDecimal { get; }

        /// <summary>
        ///     Gets the number as a decimal, if instantiated with a decimal or integer (or applicable
        ///     string representation). Use IsDecimal to determine if it can be used.
        /// </summary>
        public decimal AsDecimal { get; }

        /// <summary>
        ///     Gets the double representation of the number.
        /// </summary>
        public double AsDouble { get; }
    }
}