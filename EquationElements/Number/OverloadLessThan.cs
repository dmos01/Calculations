namespace EquationElements
{
    public partial class Number
    {
        public static bool operator <(Number a, Number b) =>
            a.IsDecimal && b.IsDecimal
                ? a.AsDecimal < b.AsDecimal
                : a.AsDouble < b.AsDouble;

        public static bool operator <(Number a, int b) =>
            a.IsDecimal
                ? a.AsDecimal < b
                : a.AsDouble < b;

        public static bool operator <(Number a, decimal b) =>
            a.IsDecimal
                ? a.AsDecimal < b
                : a.AsDouble < decimal.ToDouble(b);

        public static bool operator <(Number a, double b) => a.AsDouble < b;

        public static bool operator <(int a, Number b) =>
            b.IsDecimal
                ? a < b.AsDecimal
                : a < b.AsDouble;

        public static bool operator <(decimal a, Number b) =>
            b.IsDecimal
                ? a < b.AsDecimal
                : decimal.ToDouble(a) < b.AsDouble;

        public static bool operator <(double a, Number b) => a < b.AsDouble;
    }
}