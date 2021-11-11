using System;
using static EquationElements.Utils;

namespace EquationElements
{
    partial class Number : BaseElement
    {
        /// <summary>
        ///     Uses decimal.Parse() and double.Parse() to determine type. Can throw ArgumentNullException, ArgumentOutOfRangeException and OverflowException.
        /// </summary>
        /// <param name="asString"></param>
        public Number(string asString)
        {
            if (asString is null)
                throw new ArgumentNullException(null, ElementsExceptionMessages.StringWasNotANumberDefault);

            if (IsNullEmptyOrOnlySpaces(asString))
                throw new ArgumentOutOfRangeException(null, ElementsExceptionMessages.StringWasNotANumberDefault);

            if (decimal.TryParse(asString, out decimal dec))
            {
                IsDecimal = true;
                AsDecimal = dec;
                AsDouble = decimal.ToDouble(dec);
            }
            else if (double.TryParse(asString, out double dou))
            {
                IsDecimal = false;
                if (double.IsInfinity(dou) || double.IsNaN(dou))
                    throw new ArgumentOutOfRangeException(null, ElementsExceptionMessages.OverloadedNumberDefault);
                AsDecimal = decimal.Zero;
                AsDouble = dou;
            }
            else
                throw new OverflowException(ElementsExceptionMessages.StringWasNotANumberBeforeParameter + asString +
                                            ElementsExceptionMessages.StringWasNotANumberAfterParameter);
        }


        /// <summary>
        ///     Stores both the integer (as a decimal) and its double representation. Both can be used.
        /// </summary>
        /// <param name="asInt64"></param>
        public Number(long asInt64)
        {
            IsDecimal = true;
            AsDecimal = asInt64;
            AsDouble = asInt64;
        }


        /// <summary>
        ///     Stores both the decimal and its double representation. Both can be used.
        /// </summary>
        /// <param name="asDecimal"></param>
        public Number(decimal asDecimal)
        {
            IsDecimal = true;
            AsDecimal = asDecimal;
            AsDouble = decimal.ToDouble(asDecimal);
        }


        /// <summary>
        ///     Stores the double. IsDecimal is set to false and AsDecimal will be Zero. Can throw ArgumentOutOfRangeException.
        /// </summary>
        /// <param name="asDouble"></param>
        public Number(double asDouble)
        {
            if (double.IsInfinity(asDouble) || double.IsNaN(asDouble))
                throw new ArgumentOutOfRangeException(null, ElementsExceptionMessages.OverloadedNumberDefault);
            IsDecimal = false;
            AsDecimal = decimal.Zero;
            AsDouble = asDouble;
        }


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