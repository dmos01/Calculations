using EquationElements;

namespace NumberFormats
{
    public abstract class NumberFormat
    {
        public abstract string TypeAsString { get; }
        public abstract string Display(Number toDisplay);

        /// <summary>
        ///     Return a new object of the applicable type.
        /// </summary>
        /// <param name="typeAsString">The TypeAsString value from the applicable NumberFormat.</param>
        /// <returns></returns>
        public static NumberFormat GetFromTypeAsString(string typeAsString)
        {
            if (typeAsString == new DecimalNumber2DPNumberFormat().TypeAsString)
                return new DecimalNumber2DPNumberFormat();

            if (typeAsString == new TopHeavyFractionNumberFormat().TypeAsString)
                return new TopHeavyFractionNumberFormat();

            if (typeAsString == new MixedFractionNumberFormat().TypeAsString)
                return new MixedFractionNumberFormat();

            return new DecimalNumberFormat();
        }
    }
}