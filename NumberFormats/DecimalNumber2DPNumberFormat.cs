using System;
using System.Globalization;
using EquationElements;

namespace NumberFormats
{
    /// <summary>
    ///     Rounds to 2DP with string format "0.00". Full number if rounds to 0 but is not exactly 0
    /// </summary>
    public class DecimalNumber2DPNumberFormat : NumberFormat
    {
        public override string TypeAsString => "Decimal Number (2 dp)";

        /// <summary>
        ///     Returns the number, rounded to 2 decimal places. Returns the full number without rounding
        ///     if it rounds to 0 but is not exactly 0. Uses AsDecimal, if possible; otherwise uses AsDouble.
        /// </summary>
        /// <returns></returns>
        public override string Display(Number toDisplay)
        {
            if (ReferenceEquals(toDisplay, null))
                return null;

            if (toDisplay.IsDecimal)
            {
                if (toDisplay.AsDecimal == 0)
                    return 0.ToString("0.00", CultureInfo.CurrentCulture);

                //If the number will round to 0.00, but is not 0, return the full number.
                if (Math.Abs(toDisplay.AsDecimal) < (decimal) 0.005)
                    return toDisplay.AsDecimal.ToString(CultureInfo.CurrentCulture);

                return Math.Round(toDisplay.AsDecimal, 2, MidpointRounding.AwayFromZero)
                    .ToString("0.00", CultureInfo.CurrentCulture);
            }

            if (Utils.HasMinimalDifference(toDisplay.AsDouble, 0))
                return 0.ToString("0.00", CultureInfo.CurrentCulture);

            //If the number will round to 0.00, but is not 0, return the full number.
            if (Math.Abs(toDisplay.AsDouble) < 0.005)
                return toDisplay.AsDouble.ToString(CultureInfo.CurrentCulture);

            return Math.Round(toDisplay.AsDouble, 2, MidpointRounding.AwayFromZero)
                .ToString("0.00", CultureInfo.CurrentCulture);
        }
    }
}