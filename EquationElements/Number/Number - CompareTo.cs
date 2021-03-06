using System;

namespace EquationElements
{
    partial class Number : IComparable
    {
        /// <summary>
        ///     <para>Compares the Numbers' AsDecimals, if possible; otherwise AsDoubles.</para>
        ///     <para>Less Than Zero - This precedes obj.</para>
        ///     <para>Zero - This occurs in the same position as obj.</para>
        ///     <para>Greater than Zero - This follows obj.</para>
        /// </summary>
        /// <param name="obj">A Number.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            switch (obj)
            {
                case null:
                    return 1;
                case Number b:
                    return IsDecimal ? AsDecimal.CompareTo(b.AsDecimal) : AsDouble.CompareTo(b.AsDouble);
                default:
                    throw new ArgumentException(ElementsExceptionMessages.NumberCompareFail);
            }
        }

        /// <summary>
        ///     <para>Compares the Numbers' AsDecimals, if possible; otherwise AsDoubles.</para>
        ///     <para>Less Than Zero - This precedes b.</para>
        ///     <para>Zero - This occurs in the same position as b.</para>
        ///     <para>Greater than Zero - This follows b.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public int CompareTo(Number b)
        {
            if (b is null)
                return 1; //Anything is greater than null.

            if (IsDecimal && b.IsDecimal)
                return AsDecimal.CompareTo(b.AsDecimal);
            return AsDouble.CompareTo(b.AsDouble);
        }

        /// <summary>
        ///     <para>Compares AsDecimal to b, if possible; otherwise AsDouble to b.</para>
        ///     <para>Less Than Zero - This precedes b.</para>
        ///     <para>Zero - This occurs in the same position as b.</para>
        ///     <para>Greater than Zero - This follows b.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public int CompareTo(int b) => IsDecimal ? AsDecimal.CompareTo(b) : AsDouble.CompareTo(b);

        /// <summary>
        ///     <para>Compares AsDecimal to b, if possible; otherwise AsDouble to decimal.ToDouble(b).</para>
        ///     <para>Less Than Zero - This precedes b.</para>
        ///     <para>Zero - This occurs in the same position as b.</para>
        ///     <para>Greater than Zero - This follows b.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public int CompareTo(decimal b) => IsDecimal ? AsDecimal.CompareTo(b) : AsDouble.CompareTo(decimal.ToDouble(b));

        /// <summary>
        ///     <para>Compares AsDouble to b.</para>
        ///     <para>Less Than Zero - This precedes b.</para>
        ///     <para>Zero - This occurs in the same position as b.</para>
        ///     <para>Greater than Zero - This follows b.</para>
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public int CompareTo(double b) => AsDouble.CompareTo(b);
    }
}