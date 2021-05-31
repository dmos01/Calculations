using static EquationElements.Utils;

namespace EquationElements
{ /* By having the abstract method seperate to the publicly-accessible method, derived classes will not have to implement their own null-checks, while all external calls will still have to pass null-checks.
     * Based on https://softwareengineering.stackexchange.com/questions/354959/should-i-null-check-in-base-class-or-derived-class-c.
     */

    //Random and Log Functions use two arguments, making this and IFunction necessary distinctions.

    /// <summary>
    ///     Operators and Functions that require one parameter.
    /// </summary>
    public abstract class OneArgumentElement : BaseElement
    {
        /// <summary>
        ///     Throws ArgumentNullException if number is null.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public virtual Number PerformOn(Number number)
        {
            ThrowExceptionIfNull(number, nameof(number));
            return PerformOnAfterNullCheck(number);
        }

        protected abstract Number PerformOnAfterNullCheck(Number number);
    }

    /// <summary>
    ///     Operators and Functions that require two parameters.
    /// </summary>
    public abstract class TwoArgumentElement : BaseElement
    {
        public Number PerformOn(Number a, Number b)
        {
            ThrowExceptionIfNull(a, nameof(a));
            ThrowExceptionIfNull(b, nameof(b));
            return PerformOnAfterNullCheck(a, b);
        }

        protected abstract Number PerformOnAfterNullCheck(Number a, Number b);
    }
}