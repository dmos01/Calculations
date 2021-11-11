using System;
using System.Globalization;

namespace EquationElements
{
    partial class Number
    {
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
    }
}