namespace MehrozFractions
{
    public partial struct Fraction
    {
        public static readonly Fraction NaN = new Fraction(Indeterminates.NaN);
        public static readonly Fraction PositiveInfinity = new Fraction(Indeterminates.PositiveInfinity);
        public static readonly Fraction NegativeInfinity = new Fraction(Indeterminates.NegativeInfinity);
        public static readonly Fraction Zero = new Fraction(0, 1);
        public static readonly Fraction Epsilon = new Fraction(1, long.MaxValue);
        private static readonly double EpsilonDouble = 1.0 / long.MaxValue;
        public static readonly Fraction MaxValue = new Fraction(long.MaxValue, 1);
        public static readonly Fraction MinValue = new Fraction(long.MinValue, 1);
    }
}