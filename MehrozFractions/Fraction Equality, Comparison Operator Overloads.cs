namespace MehrozFractions
{
    public partial struct Fraction
    {
        public static bool operator ==(Fraction left, Fraction right) => left.CompareEquality(right, false);
        public static bool operator ==(Fraction left, long right) => left.CompareEquality(new Fraction(right), false);
        public static bool operator ==(Fraction left, double right) => left.CompareEquality(new Fraction(right), false);


        public static bool operator !=(Fraction left, Fraction right) => left.CompareEquality(right, true);
        public static bool operator !=(Fraction left, long right) => left.CompareEquality(new Fraction(right), true);
        public static bool operator !=(Fraction left, double right) => left.CompareEquality(new Fraction(right), true);


        /// <summary>
        ///     Compares two Fractions to see if left is less than right
        /// </summary>
        /// <param name="left">The first Fraction</param>
        /// <param name="right">The second Fraction</param>
        /// <returns>
        ///     True if <paramref name="left">left</paramref> is less
        ///     than <paramref name="right">right</paramref>
        /// </returns>
        /// <remarks>Special handling for indeterminates exists. <see>IndeterminateLess</see></remarks>
        /// <exception cref="FractionException">
        ///     Throws an error if overflows occur while computing the
        ///     difference with an InnerException of OverflowException
        /// </exception>
        public static bool operator <(Fraction left, Fraction right) => left.CompareTo(right) < 0;

        /// <summary>
        ///     Compares two Fractions to see if left is greater than right
        /// </summary>
        /// <param name="left">The first Fraction</param>
        /// <param name="right">The second Fraction</param>
        /// <returns>
        ///     True if <paramref name="left">left</paramref> is greater
        ///     than <paramref name="right">right</paramref>
        /// </returns>
        /// <remarks>Special handling for indeterminates exists. <see>IndeterminateLess</see></remarks>
        /// <exception cref="FractionException">
        ///     Throws an error if overflows occur while computing the
        ///     difference with an InnerException of OverflowException
        /// </exception>
        public static bool operator >(Fraction left, Fraction right) => left.CompareTo(right) > 0;

        /// <summary>
        ///     Compares two Fractions to see if left is less than or equal to right
        /// </summary>
        /// <param name="left">The first Fraction</param>
        /// <param name="right">The second Fraction</param>
        /// <returns>
        ///     True if <paramref name="left">left</paramref> is less than or
        ///     equal to <paramref name="right">right</paramref>
        /// </returns>
        /// <remarks>Special handling for indeterminates exists. <see>IndeterminateLessEqual</see></remarks>
        /// <exception cref="FractionException">
        ///     Throws an error if overflows occur while computing the
        ///     difference with an InnerException of OverflowException
        /// </exception>
        public static bool operator <=(Fraction left, Fraction right) => left.CompareTo(right) <= 0;

        /// <summary>
        ///     Compares two Fractions to see if left is greater than or equal to right
        /// </summary>
        /// <param name="left">The first Fraction</param>
        /// <param name="right">The second Fraction</param>
        /// <returns>
        ///     True if <paramref name="left">left</paramref> is greater than or
        ///     equal to <paramref name="right">right</paramref>
        /// </returns>
        /// <remarks>Special handling for indeterminates exists. <see>IndeterminateLessEqual</see></remarks>
        /// <exception cref="FractionException">
        ///     Throws an error if overflows occur while computing the
        ///     difference with an InnerException of OverflowException
        /// </exception>
        public static bool operator >=(Fraction left, Fraction right) => left.CompareTo(right) >= 0;
    }
}