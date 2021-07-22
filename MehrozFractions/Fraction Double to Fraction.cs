using System;

namespace MehrozFractions
{
    public partial struct Fraction
    {
        private static Fraction ConvertPositiveDouble(int sign, double inValue)
        {
            // Shamelessly stolen from http://homepage.smc.edu/kennedy_john/CONFRAC.PDF
            // with AccuracyFactor == double.Episilon
            long fractionNumerator = (long) inValue;
            double fractionDenominator = 1;
            double previousDenominator = 0;
            double remainingDigits = inValue;
            int maxIterations = 594; // found at http://www.ozgrid.com/forum/archive/index.php/t-22530.html

            while (remainingDigits != Math.Floor(remainingDigits)
                   && Math.Abs(inValue - (fractionNumerator / fractionDenominator)) > double.Epsilon)
            {
                remainingDigits = 1.0 / (remainingDigits - Math.Floor(remainingDigits));

                double scratch = fractionDenominator;

                fractionDenominator = (Math.Floor(remainingDigits) * fractionDenominator) + previousDenominator;
                fractionNumerator = (long) (inValue * fractionDenominator + 0.5);

                previousDenominator = scratch;

                if (maxIterations-- < 0)
                    break;
            }

            return new Fraction(fractionNumerator * sign, (long) fractionDenominator);
        }
    }
}