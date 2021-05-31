using System;
using System.Globalization;
using static EquationElements.Utils;

namespace EquationElements
{
    /// <summary>
    ///     An example of a Constant is Pi, 3.14. Their values can be equations of their own, though.
    /// </summary>
    public class Constant : Word
    {
        public string Value { get; }

        public Constant(string name, string value, bool throwExceptionIfNameIsInvalid = true) : base(name,
            throwExceptionIfNameIsInvalid)
        {
            ThrowExceptionIfNullEmptyOrOnlySpaces(value, nameof(value));
            Value = value;
        }

        /// <summary>
        ///     Returns the value.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value;

        /// <summary>
        ///     Returns the value.
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public new string ToString(IFormatProvider formatProvider) => Value.ToString(formatProvider);
    }

    public class Pi : BaseElement, IFormattable
    {
        /// <summary>
        ///     Returns Math.PI.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider) =>
            Math.PI.ToString(format, formatProvider);

        /// <summary>
        ///     Returns the CurrentCulture of Math.PI.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Math.PI.ToString(CultureInfo.CurrentCulture);

        /// <summary>
        ///     Returns Math.PI.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format) => Math.PI.ToString(format);

        /// <summary>
        ///     Returns Math.PI.
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(IFormatProvider formatProvider) => Math.PI.ToString(formatProvider);
    }
}