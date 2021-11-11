using System;

namespace MehrozFractions
{
    public partial struct Fraction
    {
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
            left = Math.Abs(left);
            right = Math.Abs(right);

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