namespace MehrozFractions
{
    public partial struct Fraction
    {
        /// <summary>
        ///     Reduces (simplifies) a Fraction by dividing down to lowest possible denominator (via GCD)
        /// </summary>
        /// <param name="frac">The Fraction to be reduced [WILL BE MODIFIED IN PLACE]</param>
        /// <remarks>
        ///     Modifies the input arguments in-place! Will normalize the NaN and infinites
        ///     representation. Will set Denominator to 1 for any zero numerator. Moves sign to the
        ///     Numerator.
        /// </remarks>
        /// <example>2/4 will be reduced to 1/2</example>
        public static void ReduceFraction(ref Fraction frac)
        {
            // clean up the NaNs and infinites
            if (frac.Denominator == 0)
            {
                frac.Numerator = (long) NormalizeIndeterminate(frac.Numerator);
                return;
            }

            // all forms of zero are alike.
            if (frac.Numerator == 0)
            {
                frac.Denominator = 1;
                return;
            }

            long iGCD = GCD(frac.Numerator, frac.Denominator);
            frac.Numerator /= iGCD;
            frac.Denominator /= iGCD;

            // if negative sign in denominator
            if (frac.Denominator < 0)
            {
                //move negative sign to numerator
                frac.Numerator = -frac.Numerator;
                frac.Denominator = -frac.Denominator;
            }
        }

        /// <summary>
        ///     Cross-reduces a pair of Fractions so that we have the best GCD-reduced values for multiplication
        /// </summary>
        /// <param name="frac1">The first Fraction [WILL BE MODIFIED IN PLACE]</param>
        /// <param name="frac2">The second Fraction [WILL BE MODIFIED IN PLACE]</param>
        /// <remarks>Modifies the input arguments in-place!</remarks>
        /// <example>(3/4, 5/9) = (1/4, 5/3)</example>
        public static void CrossReducePair(ref Fraction frac1, ref Fraction frac2)
        {
            // leave the indeterminates alone!
            if (frac1.Denominator == 0 || frac2.Denominator == 0)
                return;

            long gcdTop = GCD(frac1.Numerator, frac2.Denominator);
            frac1.Numerator /= gcdTop;
            frac2.Denominator /= gcdTop;

            long gcdBottom = GCD(frac1.Denominator, frac2.Numerator);
            frac2.Numerator /= gcdBottom;
            frac1.Denominator /= gcdBottom;
        }
    }
}