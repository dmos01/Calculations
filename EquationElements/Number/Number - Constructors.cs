using System;
using static EquationElements.Utils;

namespace EquationElements
{
    partial class Number : BaseElement
    {
        /// <summary>
        ///     Uses decimal.Parse() and double.Parse() to determine type. Throws ArgumentNullException if asString is null.
        /// </summary>
        /// <param name="asString"></param>
        public Number(string asString)
        {
            if (IsNullEmptyOrOnlySpaces(asString))
                throw new ArgumentException(ElementsExceptionMessages.StringWasNotANumberDefault);

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
                    throw new ArgumentException(ElementsExceptionMessages.OverloadedNumberDefault);
                AsDouble = dou;
            }
            else
                throw new ArgumentException(ElementsExceptionMessages.StringWasNotANumberBeforeParameter + asString +
                                            ElementsExceptionMessages.StringWasNotANumberAfterParameter);
        }


        /// <summary>
        ///     Stores both the integer (as a decimal) and its double representation. Both can be used.
        /// </summary>
        /// <param name="asInteger"></param>
        public Number(int asInteger)
        {
            IsDecimal = true;
            AsDecimal = asInteger;
            AsDouble = asInteger;
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
        ///     Stores the double. IsDecimal is set to false and AsDecimal will be the decimal default value.
        /// </summary>
        /// <param name="asDouble"></param>
        public Number(double asDouble)
        {
            if (double.IsInfinity(asDouble) || double.IsNaN(asDouble))
                throw new ArgumentException(ElementsExceptionMessages.OverloadedNumberDefault);
            IsDecimal = false;
            AsDouble = asDouble;
        }
    }
}