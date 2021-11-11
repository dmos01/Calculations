using static EquationElements.Utils;
// ReSharper disable ConvertIfStatementToReturnStatement

namespace EquationElements
{
    public partial class Number
    {  
        public override int GetHashCode() => AsDouble.GetHashCode();

        /// <summary>
        ///     <para>Compares for equality the Numbers' AsDecimals, if possible; otherwise AsDoubles.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals(Number b)
        {
            if (b is null)
                return false;
            if (ReferenceEquals(this, b))
                return true;

            return IsDecimal && b.IsDecimal ? AsDecimal == b.AsDecimal : HasMinimalDifference(AsDouble, b.AsDouble);
        }

        /// <summary>
        ///     <para>Compares for equality AsDecimal and b, if possible; otherwise AsDouble and b.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals(long b) =>
            IsDecimal
                ? AsDecimal == b
                : HasMinimalDifference(AsDouble, b);

        /// <summary>
        ///     <para>Compares for equality AsDecimal to b, if possible; otherwise AsDouble to decimal.ToDouble(b).</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals(decimal b) =>
            IsDecimal
                ? AsDecimal == b
                : HasMinimalDifference(AsDouble, decimal.ToDouble(b));

        /// <summary>
        ///     <para>Compares for equality AsDouble to b.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals(double b) => HasMinimalDifference(AsDouble, b);

        /// <summary>
        ///     <para>Compares for equality the Numbers' AsDecimals, if possible; otherwise AsDoubles.</para>
        /// </summary>
        /// <param name="obj">A Number.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            switch (obj)
            {
                case Number b:
                    return Equals(b);
                case long b:
                    return Equals(b);
                case decimal b:
                    return Equals(b);
                case double b:
                    return Equals(b);
                default:
                    return false;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b">A Number.</param>
        /// <returns></returns>
        public static bool operator ==(Number a, object b)
        {
            if (a is null)
                return b is null;

            return a.Equals(b);
        }

        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b">A Number.</param>
        /// <returns></returns>
        public static bool operator !=(Number a, object b) => !(a == b);

        /// <summary>
        /// </summary>
        /// <param name="a">A Number.</param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(object a, Number b)
        {
            if (b is null)
                return a is null;

            return b.Equals(a);
        }

        /// <summary>
        /// </summary>
        /// <param name="a">A Number.</param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(object a, Number b) => !(a == b);


        public static bool operator ==(Number a, Number b)
        {
            if (a is null)
                return b is null;

            return a.Equals(b);
        }

        public static bool operator !=(Number a, Number b) => !(a == b);

        public static bool operator ==(Number a, long b)
        {
            if (a is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Number a, long b) => !(a == b);

        public static bool operator ==(Number a, decimal b)
        {
            if (a is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Number a, decimal b) => !(a == b);

        public static bool operator ==(Number a, double b)
        {
            if (a is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Number a, double b) => !(a == b);


        public static bool operator ==(long a, Number b)
        {
            if (b is null)
                return false;

            return b.Equals(a);
        }
        
        public static bool operator !=(long a, Number b) => !(a == b);

        public static bool operator ==(decimal a, Number b)
        {
            if (b is null)
                return false;

            return b.Equals(a);
        }
        
        public static bool operator !=(decimal a, Number b) => !(a == b);

        public static bool operator ==(double a, Number b)
        {
            if (b is null)
                return false;

            return b.Equals(a);
        }
        
        public static bool operator !=(double a, Number b) => !(a == b);
    }
}