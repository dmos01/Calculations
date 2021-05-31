namespace EquationElements
{
    public partial class Number
    {
        public static bool operator <=(Number a, Number b) => a < b || a == b;

        public static bool operator <=(Number a, int b) => a < b || a == b;

        public static bool operator <=(Number a, decimal b) => a < b || a == b;

        public static bool operator <=(Number a, double b) => a < b || a == b;

        public static bool operator <=(int a, Number b) => a < b || a == b;

        public static bool operator <=(decimal a, Number b) => a < b || a == b;

        public static bool operator <=(double a, Number b) => a < b || a == b;
    }
}