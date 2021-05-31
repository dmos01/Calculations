using static EquationElements.Utils;

namespace EquationElements.Functions
{
    /// <summary>
    ///     All Trig Elements implement IFunction, and therefore are considered Functions. Works with doubles only.
    /// </summary>
    public abstract class TrigonometricFunction : OneArgumentElement, IFunction
    {
        /// <summary>
        ///     Uses radians. Throws ArgumentNullException if number is null.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public override Number PerformOn(Number number) => PerformOn(number, true);

        protected override Number PerformOnAfterNullCheck(Number number) => PerformOnAfterNullCheck(number, true);

        /// <summary>
        ///     Throws ArgumentNullException if number is null.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="radians">Calculate in radians. If false, degrees.</param>
        /// <returns></returns>
        public Number PerformOn(Number number, bool radians)
        {
            ThrowExceptionIfNull(number, nameof(number));
            return PerformOnAfterNullCheck(number, radians);
        }

        protected abstract Number PerformOnAfterNullCheck(Number number, bool radians);
    }
}