namespace MehrozFractions
{
    public partial struct Fraction
    {
        /// <summary>
        ///     Implicit conversion of a long integral value to a Fraction
        /// </summary>
        /// <param name="value">The long integral value to convert</param>
        /// <returns>A Fraction whose denominator is 1</returns>
        public static implicit operator Fraction(long value) => new Fraction(value);

        /// <summary>
        ///     Implicit conversion of a double floating point value to a Fraction
        /// </summary>
        /// <param name="value">The double value to convert</param>
        /// <returns>A reduced Fraction</returns>
        public static implicit operator Fraction(double value) => new Fraction(value);

        /// <summary>
        ///     Implicit conversion of a string to a Fraction
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>A reduced Fraction</returns>
        public static implicit operator Fraction(string value) => new Fraction(value);


        /// <summary>
        ///     Explicit conversion from a Fraction to an integer
        /// </summary>
        /// <param name="frac">the Fraction to convert</param>
        /// <returns>The integral representation of the Fraction</returns>
        public static explicit operator int(Fraction frac) => frac.ToInt32();

        /// <summary>
        ///     Explicit conversion from a Fraction to an integer
        /// </summary>
        /// <param name="frac">The Fraction to convert</param>
        /// <returns>The integral representation of the Fraction</returns>
        public static explicit operator long(Fraction frac) => frac.ToInt64();

        /// <summary>
        ///     Explicit conversion from a Fraction to a double floating-point value
        /// </summary>
        /// <param name="frac">The Fraction to convert</param>
        /// <returns>The double representation of the Fraction</returns>
        public static explicit operator double(Fraction frac) => frac.ToDouble();

        /// <summary>
        ///     Explicit conversion from a Fraction to a string
        /// </summary>
        /// <param name="frac">the Fraction to convert</param>
        /// <returns>The string representation of the Fraction</returns>
        public static implicit operator string(Fraction frac) => frac.ToString();
    }
}