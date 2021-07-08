using System;
using System.Globalization;
using static EquationElements.Utils;

namespace EquationElements
{
    /// <summary>
    ///     <para>An example of a Constant is Pi, 3.14. Their values can be equations of their own, though.</para>
    ///     <para>Values may contain other Constants but cannot contain other Words.</para>
    /// </summary>
    public class Constant : Word
    {
        public string Value { get; }

        /// <summary>
        ///     Throws exception if name or value is null, empty or only spaces. Does not test if name is an Operator or Function.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Constant(string name, string value) : base(name)
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
}