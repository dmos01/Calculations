using System;
using MehrozFractions.Properties;

namespace MehrozFractions
{
    public partial struct Fraction
    {
        /// <summary>
        ///     Negates the Fraction
        /// </summary>
        /// <param name="frac">Value to negate</param>
        /// <returns>A new Fraction that is sign-flipped from the input</returns>
        private static Fraction Negate(Fraction frac) =>

            // for a NaN, it's still a NaN
            new Fraction(-frac.Numerator, frac.Denominator);

        /// <summary>
        ///     Adds two Fractions
        /// </summary>
        /// <param name="left">A Fraction</param>
        /// <param name="right">Another Fraction</param>
        /// <returns>Sum of the Fractions. Returns NaN if either Fraction is a NaN.</returns>
        /// <exception cref="FractionException">
        ///     Will throw if an overflow occurs when computing the
        ///     GCD-normalized values.
        /// </exception>
        private static Fraction Add(Fraction left, Fraction right)
        {
            if (left.IsNaN() || right.IsNaN())
                return NaN;

            long gcd = GCD(left.Denominator, right.Denominator); // cannot return less than 1
            long leftDenominator = left.Denominator / gcd;
            long rightDenominator = right.Denominator / gcd;

            try
            {
                checked
                {
                    long numerator = left.Numerator * rightDenominator + right.Numerator * leftDenominator;
                    long denominator = leftDenominator * rightDenominator * gcd;

                    return new Fraction(numerator, denominator);
                }
            }
            catch (Exception e)
            {
                throw new FractionException(Resources.AdditionError, e);
            }
        }

        /// <summary>
        ///     Multiplies two Fractions
        /// </summary>
        /// <param name="left">A Fraction</param>
        /// <param name="right">Another Fraction</param>
        /// <returns>Product of the Fractions. Returns NaN if either Fraction is a NaN.</returns>
        /// <exception cref="FractionException">
        ///     Will throw if an overflow occurs. Does a cross-reduce to
        ///     insure only the unavoidable overflows occur.
        /// </exception>
        private static Fraction Multiply(Fraction left, Fraction right)
        {
            if (left.IsNaN() || right.IsNaN())
                return NaN;

            // this would be unsafe if we were not a ValueType, because we would be changing the
            // caller's values.  If we change back to a class, must use temporaries
            CrossReducePair(ref left, ref right);

            try
            {
                checked
                {
                    long numerator = left.Numerator * right.Numerator;
                    long denominator = left.Denominator * right.Denominator;

                    return new Fraction(numerator, denominator);
                }
            }
            catch (Exception e)
            {
                throw new FractionException(Resources.MultiplicationError, e);
            }
        }

        /// <summary>
        ///     Returns the modulus (remainder after dividing) two Fractions
        /// </summary>
        /// <param name="left">A Fraction</param>
        /// <param name="right">Another Fraction</param>
        /// <returns>Modulus of the Fractions. Returns NaN if either Fraction is a NaN.</returns>
        /// <exception cref="FractionException">
        ///     Will throw if an overflow occurs. Does a cross-reduce to
        ///     insure only the unavoidable overflows occur.
        /// </exception>
        private static Fraction Modulus(Fraction left, Fraction right)
        {
            if (left.IsNaN() || right.IsNaN())
                return NaN;

            try
            {
                checked
                {
                    // this will discard any fractional places...
                    long quotient = (long) (left / right);
                    Fraction whole = new Fraction(quotient * right.Numerator, right.Denominator);
                    return left - whole;
                }
            }
            catch (Exception e)
            {
                throw new FractionException(Resources.ModulusError, e);
            }
        }

        /// <summary>
        ///     Computes the greatest common divisor for two values
        /// </summary>
        /// <param name="left">One value</param>
        /// <param name="right">Another value</param>
        /// <returns>The greatest common divisor of the two values</returns>
        /// <example>(6, 9) returns 3 and (11, 4) returns 1</example>
        private static long GCD(long left, long right)
        {
            // take absolute values
            if (left < 0)
                left = -left;

            if (right < 0)
                right = -right;

            // if we're dealing with any zero or one, the GCD is 1
            if (left < 2 || right < 2)
                return 1;

            do
            {
                if (left < right) 
                    (left, right) = (right, left); //Swap left and right.

                left %= right;
            } while (left != 0);

            return right;
        }
    }
}