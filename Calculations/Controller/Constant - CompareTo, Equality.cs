using System;

namespace Calculations
{
    partial class Controller
    {
        public partial class Constant : IComparable
        {
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
                switch (obj)
                {
                    case null:
                        return 1;
                    case string str:
                        return string.Compare(Name, str, StringComparison.Ordinal);
                    case Constant b:
                        return string.Compare(Name, b.Name, StringComparison.Ordinal);
                    default:
                        throw new ArgumentOutOfRangeException(null, CalculationsResources.CompareConstantFail);
                }
            }

            /// <summary>
            ///     <para>Compares the Constant's Name to otherName.</para>
            ///     <para>Less Than Zero - This precedes otherName.</para>
            ///     <para>Zero - This occurs in the same position as otherName.</para>
            ///     <para>Greater than Zero - This follows otherName.</para>
            /// </summary>
            /// <returns></returns>
            public int CompareTo(string otherName) => string.Compare(Name, otherName, StringComparison.Ordinal);

            /// <summary>
            ///     <para>Compares the Constant's Name to b's Name.</para>
            ///     <para>Less Than Zero - This precedes b.</para>
            ///     <para>Zero - This occurs in the same position as b.</para>
            ///     <para>Greater than Zero - This follows b.</para>
            /// </summary>
            /// <returns></returns>
            public int CompareTo(Constant b) => b is null ? 1 : string.Compare(Name, b.Name, StringComparison.Ordinal);

            public bool Equals(string otherNameWithoutSpaces) => Name == otherNameWithoutSpaces;

            public bool Equals(Constant b) => !(b is null) && Name == b.Name;

            /// <summary>
            /// </summary>
            /// <param name="obj">A Constant.</param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                switch (obj)
                {
                    case null:
                        return false;
                    case string str:
                        return Name == str;
                    case Constant b:
                        return Name == b.Name;
                    default:
                        throw new ArgumentOutOfRangeException(null, CalculationsResources.CompareConstantFail);
                }
            }

            public override int GetHashCode() => Name.GetHashCode();

            public static bool operator ==(Constant a, string b)
            {
                if (a is null)
                    return b is null;
                if (b is null)
                    return false;
                return a.Equals(b);
            }

            public static bool operator !=(Constant a, string b) => !(a == b);

            public static bool operator ==(string a, Constant b)
            {
                if (a is null)
                    return b is null;
                if (b is null)
                    return false;
                return b.Equals(a);
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

                return a.Equals(b);
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

                switch (b)
                {
                    case string str:
                        return a.Equals(str);
                    case Constant Constant:
                        return a.Equals(Constant);
                    default:
                        return false;
                }
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

                switch (a)
                {
                    case string str:
                        return b.Equals(str);
                    case Constant Constant:
                        return b.Equals(Constant);
                    default:
                        return false;
                }
            }

            public static bool operator !=(object a, Constant b) => !(a == b);
        }
    }
}