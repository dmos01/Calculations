using System;
using static EquationElements.Utils;
// ReSharper disable ArrangeMethodOrOperatorBody

namespace Calculations
{
    partial class Controller
    {
        public partial class Constant : IComparable
        {
            private int PrivateCompareTo(string otherNameWithoutSpaces) =>
                string.Compare(NameWithoutSpaces, otherNameWithoutSpaces, StringComparison.Ordinal);

            private bool PrivateEquals(string otherNameWithoutSpaces) => NameWithoutSpaces == otherNameWithoutSpaces;

            /// <summary>
            ///     <para>Compares the Constants' Names.</para>
            ///     <para>Less Than Zero - This precedes obj.</para>
            ///     <para>Zero - This occurs in the same position as obj.</para>
            ///     <para>Greater than Zero - This follows obj.</para>
            /// </summary>
            /// <param name="obj">A Constant.</param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                return obj switch
                {
                    null => 1,
                    string otherName => PrivateCompareTo(RemoveSpaces(otherName)),
                    Constant otherConstant => PrivateCompareTo(otherConstant.NameWithoutSpaces),
                    _ => throw new ArgumentOutOfRangeException(null, CalculationsResources.CompareConstantFail)
                };
            }

            /// <summary>
            ///     <para>Compares the Constant's Name to otherName.</para>
            ///     <para>Less Than Zero - This precedes otherName.</para>
            ///     <para>Zero - This occurs in the same position as otherName.</para>
            ///     <para>Greater than Zero - This follows otherName.</para>
            /// </summary>
            /// <returns></returns>
            public int CompareTo(string otherName) => PrivateCompareTo(RemoveSpaces(otherName));

            /// <summary>
            ///     <para>Compares the Constant's Name to otherConstant's Name.</para>
            ///     <para>Less Than Zero - This precedes otherConstant.</para>
            ///     <para>Zero - This occurs in the same position as otherConstant.</para>
            ///     <para>Greater than Zero - This follows otherConstant.</para>
            /// </summary>
            /// <returns></returns>
            public int CompareTo(Constant otherConstant) => otherConstant is null ? 1 : PrivateCompareTo(otherConstant.NameWithoutSpaces);

            public bool Equals(string otherName) => PrivateEquals(RemoveSpaces(otherName));

            public bool Equals(Constant otherConstant) => otherConstant is not null && PrivateEquals(otherConstant.NameWithoutSpaces);

            /// <summary>
            /// </summary>
            /// <param name="obj">A Constant.</param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return obj switch
                {
                    null => false,
                    string otherName => PrivateEquals(RemoveSpaces(otherName)),
                    Constant otherConstant => PrivateEquals(otherConstant.NameWithoutSpaces),
                    _ => throw new ArgumentOutOfRangeException(null, CalculationsResources.CompareConstantFail)
                };
            }

            public override int GetHashCode() => NameWithoutSpaces.GetHashCode();

            public static bool operator ==(Constant a, string b)
            {
                if (a is null)
                    return b is null;
                if (b is null)
                    return false;
                return a.PrivateEquals(RemoveSpaces(b));
            }

            public static bool operator !=(Constant a, string b) => !(a == b);

            public static bool operator ==(string a, Constant b)
            {
                if (a is null)
                    return b is null;
                if (b is null)
                    return false;
                return b.PrivateEquals(RemoveSpaces(a));
            }

            public static bool operator !=(string a, Constant b) => !(a == b);


            public static bool operator ==(Constant a, Constant b)
            {
                if (a is null)
                    return b is null;
                if (b is null)
                    return false;

                if (ReferenceEquals(a, b))
                    return true;

                return a.PrivateEquals(b.NameWithoutSpaces);
            }

            public static bool operator !=(Constant a, Constant b) => !(a == b);

            public static bool operator ==(Constant a, object b)
            {
                if (a is null)
                    return b is null;
                if (b is null)
                    return false;
                if (ReferenceEquals(a, b))
                    return true;

                return b switch
                {
                    string otherName => a.PrivateEquals(RemoveSpaces(otherName)),
                    Constant otherConstant => a.PrivateEquals(otherConstant.NameWithoutSpaces),
                    _ => false
                };
            }

            public static bool operator !=(Constant a, object b) => !(a == b);

            public static bool operator ==(object a, Constant b)
            {
                if (a is null)
                    return b is null;
                if (b is null)
                    return false;
                if (ReferenceEquals(a, b))
                    return true;

                return a switch
                {
                    string otherName => b.PrivateEquals(RemoveSpaces(otherName)),
                    Constant otherConstant => b.PrivateEquals(RemoveSpaces(otherConstant.NameWithoutSpaces)),
                    _ => false
                };
            }

            public static bool operator !=(object a, Constant b) => !(a == b);
        }
    }
}