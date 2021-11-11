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
            // ensure we're as close to normalized as possible first
            ReduceFraction(ref this);

            int numeratorHash = Numerator.GetHashCode();
            int denominatorHash = Denominator.GetHashCode();

            return numeratorHash ^ denominatorHash;
        }

        public static bool operator ==(Fraction left, Fraction right) => left.CompareEquality(right, false);
        public static bool operator ==(Fraction left, long right) => left.CompareEquality(new Fraction(right), false);
        public static bool operator ==(Fraction left, double right) => left.CompareEquality(new Fraction(right), false);


        public static bool operator !=(Fraction left, Fraction right) => left.CompareEquality(right, true);
        public static bool operator !=(Fraction left, long right) => left.CompareEquality(new Fraction(right), true);
        public static bool operator !=(Fraction left, double right) => left.CompareEquality(new Fraction(right), true);


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
            // ensure we're normalized first
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

    }
}