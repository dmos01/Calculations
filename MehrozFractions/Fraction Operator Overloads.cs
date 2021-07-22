namespace MehrozFractions
{
    public partial struct Fraction
    {
        /// <summary>
        ///     Negates the Fraction
        /// </summary>
        /// <param name="left">The Fraction to negate</param>
        /// <returns>The negative version of the Fraction</returns>
        public static Fraction operator -(Fraction left) => Negate(left);

        public static Fraction operator +(Fraction left, Fraction right) => Add(left, right);
        public static Fraction operator +(long left, Fraction right) => Add(new Fraction(left), right);
        public static Fraction operator +(Fraction left, long right) => Add(left, new Fraction(right));
        public static Fraction operator +(double left, Fraction right) => Add(ToFraction(left), right);
        public static Fraction operator +(Fraction left, double right) => Add(left, ToFraction(right));


        public static Fraction operator -(Fraction left, Fraction right) => Add(left, -right);
        public static Fraction operator -(long left, Fraction right) => Add(new Fraction(left), -right);
        public static Fraction operator -(Fraction left, long right) => Add(left, new Fraction(-right));
        public static Fraction operator -(double left, Fraction right) => Add(ToFraction(left), -right);
        public static Fraction operator -(Fraction left, double right) => Add(left, ToFraction(-right));


        public static Fraction operator *(Fraction left, Fraction right) => Multiply(left, right);
        public static Fraction operator *(long left, Fraction right) => Multiply(new Fraction(left), right);
        public static Fraction operator *(Fraction left, long right) => Multiply(left, new Fraction(right));
        public static Fraction operator *(double left, Fraction right) => Multiply(ToFraction(left), right);
        public static Fraction operator *(Fraction left, double right) => Multiply(left, ToFraction(right));


        public static Fraction operator /(Fraction left, Fraction right) => Multiply(left, right.Inverse());
        public static Fraction operator /(long left, Fraction right) => Multiply(new Fraction(left), right.Inverse());
        public static Fraction operator /(Fraction left, long right) => Multiply(left, Inverted(right));
        public static Fraction operator /(double left, Fraction right) => Multiply(ToFraction(left), right.Inverse());
        public static Fraction operator /(Fraction left, double right) => Multiply(left, Inverted(right));

        public static Fraction operator %(Fraction left, Fraction right) => Modulus(left, right);
        public static Fraction operator %(long left, Fraction right) => Modulus(new Fraction(left), right);
        public static Fraction operator %(Fraction left, long right) => Modulus(left, right);
        public static Fraction operator %(double left, Fraction right) => Modulus(ToFraction(left), right);
        public static Fraction operator %(Fraction left, double right) => Modulus(left, right);
    }
}