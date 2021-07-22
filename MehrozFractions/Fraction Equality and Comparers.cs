using System;
using MehrozFractions.Properties;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace MehrozFractions
{
    public partial struct Fraction
    {
        /// <summary>
        ///     Compares for equality the current Fraction to the value passed.
        /// </summary>
        /// <param name="obj">A  Fraction,</param>
        /// <returns>
        ///     True if the value equals the current fraction, false otherwise (including for
        ///     non-Fraction types or null object.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Fraction right)
            {
                try
                {
                    return CompareEquality(right, false);
                }
                catch
                {
                    // can't throw in an Equals!
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        ///     Returns a hash code generated from the current Fraction
        /// </summary>
        /// <returns>The hash code</returns>
        /// <remarks>Reduces (in-place) the Fraction first.</remarks>
        public override int GetHashCode()
        {
            // insure we're as close to normalized as possible first
            ReduceFraction(ref this);

            int numeratorHash = Numerator.GetHashCode();
            int denominatorHash = Denominator.GetHashCode();

            return numeratorHash ^ denominatorHash;
        }

        /// <summary>
        ///     Compares an object to this Fraction
        /// </summary>
        /// <param name="obj">The object to compare against (null is less than everything)</param>
        /// <returns>
        ///     -1 if this is less than <paramref name="obj"></paramref>,
        ///     0 if they are equal,
        ///     1 if this is greater than <paramref name="obj"></paramref>
        /// </returns>
        /// <remarks>Will convert an object from longs, doubles, and strings as this is a value-type.</remarks>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1; // null is less than anything

            Fraction right;

            switch (obj)
            {
                case Fraction fraction:
                    right = fraction;
                    break;
                case long l:
                    right = l;
                    break;
                case double d:
                    right = d;
                    break;
                case string s:
                    right = s;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(null, Resources.MustBeConvertible, nameof(obj));
            }

            return CompareTo(right);
        }

        /// <summary>
        ///     Compares this Fraction to another Fraction
        /// </summary>
        /// <param name="right">The Fraction to compare against</param>
        /// <returns>
        ///     -1 if this is less than <paramref name="right"></paramref>,
        ///     0 if they are equal,
        ///     1 if this is greater than <paramref name="right"></paramref>
        /// </returns>
        public int CompareTo(Fraction right)
        {
            // if left is an indeterminate, punt to the helper...
            if (Denominator == 0)
                return IndeterminantCompare(NormalizeIndeterminate(Numerator), right);

            // if right is an indeterminate, punt to the helper...
            if (right.Denominator == 0)
            {
                // note sign-flip...
                return -IndeterminantCompare(NormalizeIndeterminate(right.Numerator), this);
            }

            // they're both normal Fractions
            CrossReducePair(ref this, ref right);

            try
            {
                checked
                {
                    long leftScale = Numerator * right.Denominator;
                    long rightScale = Denominator * right.Numerator;

                    if (leftScale < rightScale)
                        return -1;
                    else if (leftScale > rightScale)
                        return 1;
                    else
                        return 0;
                }
            }
            catch (Exception e)
            {
                throw new FractionException(
                    Resources.CompareToErrorBeforeParameters + this + Resources.CompareToErrorBetweenParameters +
                    right + Resources.CompareToErrorAfterParameters, e);
            }
        }

        /// <summary>
        ///     Compares for equality the current Fraction to the value passed.
        /// </summary>
        /// <param name="right">A Fraction to compare against</param>
        /// <param name="notEqualCheck">If true, we're looking for not-equal</param>
        /// <returns>
        ///     True if the <paramref name="right"></paramref> equals the current
        ///     fraction, false otherwise. If comparing two NaNs, they are always equal AND
        ///     not-equal.
        /// </returns>
        private bool CompareEquality(Fraction right, bool notEqualCheck)
        {
            // insure we're normalized first
            ReduceFraction(ref this);

            // now normalize the comperand
            ReduceFraction(ref right);

            if (Numerator == right.Numerator && Denominator == right.Denominator)
            {
                // special-case rule, two NaNs are always both equal
                if (notEqualCheck && IsNaN())
                    return true;
                else
                    return !notEqualCheck;
            }
            else
                return notEqualCheck;
        }

        /// <summary>
        ///     Determines how this Fraction, of an indeterminate type, compares to another Fraction
        /// </summary>
        /// <param name="leftType">What kind of indeterminate</param>
        /// <param name="right">The other Fraction to compare against</param>
        /// <returns>
        ///     -1 if this is less than <paramref name="right"></paramref>,
        ///     0 if they are equal,
        ///     1 if this is greater than <paramref name="right"></paramref>
        /// </returns>
        /// <remarks>
        ///     NaN is less than anything except NaN and Negative Infinity. Negative Infinity is less
        ///     than anything except Negative Infinity. Positive Infinity is greater than anything except
        ///     Positive Infinity.
        /// </remarks>
        private static int IndeterminantCompare(Indeterminates leftType, Fraction right)
        {
            switch (leftType)
            {
                case Indeterminates.NaN:
                    // A NaN is...
                    if (right.IsNaN())
                        return 0; // equal to a NaN
                    else if (right.IsNegativeInfinity())
                        return 1; // great than Negative Infinity
                    else
                        return -1; // less than anything else

                case Indeterminates.NegativeInfinity:
                    // Negative Infinity is...
                    if (right.IsNegativeInfinity())
                        return 0; // equal to Negative Infinity
                    else
                        return -1; // less than anything else

                case Indeterminates.PositiveInfinity:
                    if (right.IsPositiveInfinity())
                        return 0; // equal to Positive Infinity
                    else
                        return 1; // greater than anything else

                default:
                    // this CAN'T happen, something VERY wrong is going on...
                    return 0;
            }
        }
    }
}