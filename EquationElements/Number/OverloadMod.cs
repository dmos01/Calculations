namespace EquationElements
{
    public partial class Number
    {
        public static Number operator %(Number a, Number b) =>
            a.IsDecimal && b.IsDecimal
                ? new Number(a.AsDecimal % b.AsDecimal)
                : new Number(a.AsDouble % b.AsDouble);

        public static Number operator %(Number a, int b) =>
            a.IsDecimal
                ? new Number(a.AsDecimal % b)
                : new Number(a.AsDouble % b);

        public static Number operator %(Number a, decimal b) =>
            a.IsDecimal
                ? new Number(a.AsDecimal % b)
                : new Number(a.AsDouble % decimal.ToDouble(b));

        public static Number operator %(Number a, double b) => new Number(a.AsDouble % b);

        public static Number operator %(int a, Number b) =>
            b.IsDecimal
                ? new Number(a % b.AsDecimal)
                : new Number(a % b.AsDouble);

        public static Number operator %(decimal a, Number b) =>
            b.IsDecimal
                ? new Number(a % b.AsDecimal)
                : new Number(decimal.ToDouble(a) % b.AsDouble);

        public static Number operator %(double a, Number b) => new Number(a % b.AsDouble);
    }
}