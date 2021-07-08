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

        /// <summary>
        ///     Returns the string representation of AsDecimal, if possible. Otherwise AsDouble.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider) => IsDecimal
            ? AsDecimal.ToString(format, formatProvider)
            : AsDouble.ToString(format, formatProvider);

        /// <summary>
        ///     Returns the CurrentCulture string representation of AsDecimal, if possible; otherwise AsDouble.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => IsDecimal
            ? AsDecimal.ToString(CultureInfo.CurrentCulture)
            : AsDouble.ToString(CultureInfo.CurrentCulture);

        /// <summary>
        ///     Returns the string representation of AsDecimal, if possible; otherwise AsDouble.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format) => IsDecimal
            ? AsDecimal.ToString(format)
            : AsDouble.ToString(format);

        /// <summary>
        ///     Returns the string representation of AsDecimal, if possible; otherwise AsDouble.
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(IFormatProvider formatProvider) => IsDecimal
            ? AsDecimal.ToString(formatProvider)
            : AsDouble.ToString(formatProvider);


        /// <summary>
        ///     Returns true if a number can be instantiated from the value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="number">Null if method returns false.</param>
        /// <returns></returns>
        public static bool TryParse(string value, out Number number)
        {
            try
            {
                number = new Number(value);
                return true;
            }
            catch
            {
                number = null;
                return false;
            }
        }
    }
}