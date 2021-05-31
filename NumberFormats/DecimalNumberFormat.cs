using EquationElements;

namespace NumberFormats
{
    /// <summary>
    ///     "Normal" number format.
    /// </summary>
    public class DecimalNumberFormat : NumberFormat
    {
        public override string TypeAsString => "Decimal Number";

        /// <summary>
        ///     Returns toDisplay.ToString().
        /// </summary>
        /// <param name="toDisplay"></param>
        /// <returns></returns>
        public override string Display(Number toDisplay) => toDisplay?.ToString();
    }
}