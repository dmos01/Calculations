namespace MehrozFractions
{
    public partial struct Fraction
    {
        /// <summary>
        ///     Inverts a Fraction
        /// </summary>
        /// <returns>The inverted Fraction (with Denominator over Numerator)</returns>
        /// <remarks>Does NOT throw for zero Numerators as later use of the fraction will catch the error.</remarks>
        public Fraction Inverse()
        {
            // don't use the obvious constructor because we do not want it normalized at this time
            // ReSharper disable once UseObjectOrCollectionInitializer
            Fraction frac = new Fraction();
            frac.Numerator = Denominator;
            frac.Denominator = Numerator;
            return frac;
        }

        /// <summary>
        ///     Creates an inverted Fraction
        /// </summary>
        /// <returns>The inverted Fraction (with Denominator over Numerator)</returns>
        /// <remarks>Does NOT throw for zero Numerators as later use of the fraction will catch the error.</remarks>
        public static Fraction Inverted(long value)
        {
            Fraction frac = new Fraction(value);
            return frac.Inverse();
        }

        /// <summary>
        ///     Creates an inverted Fraction
        /// </summary>
        /// <returns>The inverted Fraction (with Denominator over Numerator)</returns>
        /// <remarks>Does NOT throw for zero Numerators as later use of the fraction will catch the error.</remarks>
        public static Fraction Inverted(double value)
        {
            Fraction frac = new Fraction(value);
            return frac.Inverse();
        }
    }
}