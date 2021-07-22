namespace MehrozFractions
{
    public partial struct Fraction
    {
        /// <summary>
        ///     Construct a Fraction from an integral value
        /// </summary>
        /// <param name="wholeNumber">The value (eventual numerator)</param>
        /// <remarks>The denominator will be 1</remarks>
        public Fraction(long wholeNumber)
        {
            if (wholeNumber == long.MinValue)
                wholeNumber++; // prevent serious issues later..

            Numerator = wholeNumber;
            Denominator = 1;

            // no reducing required, we're a whole number
        }

        /// <summary>
        ///     Construct a Fraction from a floating-point value
        /// </summary>
        /// <param name="floatingPointNumber">The value</param>
        public Fraction(double floatingPointNumber) => this = ToFraction(floatingPointNumber);

        /// <summary>
        ///     Construct a Fraction from a string in any legal format
        /// </summary>
        /// <param name="inValue">A string with a legal fraction input format</param>
        /// <remarks>Will reduce the fraction to smallest possible denominator</remarks>
        /// <see>ToFraction(string strValue)</see>
        public Fraction(string inValue) => this = ToFraction(inValue);

        /// <summary>
        ///     Construct a Fraction from a numerator, denominator pair
        /// </summary>
        /// <param name="numerator">The numerator (top number)</param>
        /// <param name="denominator">The denominator (bottom number)</param>
        /// <remarks>Will reduce the fraction to smallest possible denominator</remarks>
        public Fraction(long numerator, long denominator)
        {
            if (numerator == long.MinValue)
                numerator++; // prevent serious issues later..

            if (denominator == long.MinValue)
                denominator++; // prevent serious issues later..

            Numerator = numerator;
            Denominator = denominator;
            ReduceFraction(ref this);
        }

        /// <summary>
        ///     Private constructor to synthesize a Fraction for indeterminates (NaN and infinites)
        /// </summary>
        /// <param name="type">Kind of inderterminate</param>
        private Fraction(Indeterminates type)
        {
            Numerator = (long) type;
            Denominator = 0;

            // do NOT reduce, we're clean as can be!
        }
    }
}