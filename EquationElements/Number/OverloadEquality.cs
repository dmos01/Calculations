// ReSharper disable PossibleNullReferenceException

using static EquationElements.Utils;

namespace EquationElements
{
    public partial class Number
    {
        /// <summary>
        ///     <para>Compares for equality the Numbers' AsDecimals, if possible; otherwise AsDoubles.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected bool Equals(Number b) =>
            IsDecimal && b.IsDecimal ? AsDecimal == b.AsDecimal : HasMinimalDifference(AsDouble, b.AsDouble);

        /// <summary>
        ///     <para>Compares for equality AsDecimal and b, if possible; otherwise AsDouble and b.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected bool Equals(int b) =>
            IsDecimal
                ? AsDecimal == b
                : HasMinimalDifference(AsDouble, b);

        /// <summary>
        ///     <para>Compares for equality AsDecimal to b, if possible; otherwise AsDouble to decimal.ToDouble(b).</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected bool Equals(decimal b) =>
            IsDecimal
                ? AsDecimal == b
                : HasMinimalDifference(AsDouble, decimal.ToDouble(b));

        /// <summary>
        ///     <para>Compares for equality AsDouble to b.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected bool Equals(double b) => HasMinimalDifference(AsDouble, b);

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
            if (!(obj is Number b))
                return false;

            return Equals(b);
        }

        public override int GetHashCode() => AsDouble.GetHashCode();

        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b">A Number.</param>
        /// <returns></returns>
        public static bool operator ==(Number a, object b)
        {
            if (a is null)
                return b is null;
            if (b is null)
                return false;
            if (ReferenceEquals(a, b))
                return true;

            if (!(b is Number other))
                return false;

            return a.Equals(other);
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
            if (a is null)
                return b is null;
            if (b is null)
                return false;
            if (ReferenceEquals(a, b))
                return true;

            if (!(a is Number other))
                return false;

            return b.Equals(other);
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
            if (b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Number a, Number b) => !(a == b);

        public static bool operator ==(Number a, int b) =>
            a.IsDecimal
                ? a.AsDecimal == b
                : HasMinimalDifference(a.AsDouble, b);

        public static bool operator !=(Number a, int b) => !(a == b);

        public static bool operator ==(Number a, decimal b) =>
            a.IsDecimal
                ? a.AsDecimal == b
                : HasMinimalDifference(a.AsDouble, decimal.ToDouble(b));

        public static bool operator !=(Number a, decimal b) => !(a == b);

        public static bool operator ==(Number a, double b) => HasMinimalDifference(a.AsDouble, b);

        public static bool operator !=(Number a, double b) => !(a == b);

        public static bool operator ==(int a, Number b) =>
            b.IsDecimal
                ? a == b.AsDecimal
                : HasMinimalDifference(a, b.AsDouble);

        public static bool operator !=(int a, Number b) => !(a == b);

        public static bool operator ==(decimal a, Number b) =>
            b.IsDecimal
                ? a == b.AsDecimal
                : HasMinimalDifference(decimal.ToDouble(a), b.AsDouble);

        public static bool operator !=(decimal a, Number b) => !(a == b);

        public static bool operator ==(double a, Number b) => HasMinimalDifference(a, b.AsDouble);

        public static bool operator !=(double a, Number b) => !(a == b);
    }
}