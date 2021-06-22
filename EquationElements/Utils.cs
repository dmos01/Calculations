using System;
using System.Collections.Generic;
using System.Linq;

namespace EquationElements
{
    /// <summary>
    ///     Static class
    /// </summary>
    public static class Utils
    {
        /// <summary>
        ///     Returns a string with all ' ' removed.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveSpaces(string text)
        {
            if (text is null)
                return null;

            List<char> list = new List<char>(text.Length);
            list.AddRange(from x in text where x != ' ' select x);
            return string.Join(null, list);
        }

        /// <summary>
        ///     Throws ArgumentNullException if toCheck is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toCheck"></param>
        /// <param name="nameofToCheck">nameof(toCheck) because this method won't know the original variable name.</param>
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        //public static void NotNull<T>(T toCheck, string nameofToCheck)
        //    where T : class
        //{
        //    if (toCheck is null)
        //        throw new ArgumentNullException(nameofToCheck);
        //}
        public static void ThrowExceptionIfNull(Number toCheck, string nameofToCheck)
        {
            if (toCheck is null)
                throw new ArgumentNullException(nameofToCheck);
        }

        public static void ThrowExceptionIfNullOrEmpty<T>(ICollection<T> toCheck, string nameofToCheck)
        {
            if (toCheck is null)
                throw new ArgumentNullException(nameofToCheck);
            if (toCheck.Count == 0)
                throw new ArgumentOutOfRangeException(nameofToCheck);
        }

        /// <summary>
        ///     Throws ArgumentNullException if toCheck is null. Throws ArgumentException if toCheck is empty or contains only
        ///     spaces.
        /// </summary>
        /// <param name="toCheck"></param>
        /// <param name="nameofToCheck">nameof(toCheck) because this method won't know the original variable name.</param>
        public static void ThrowExceptionIfNullEmptyOrOnlySpaces(string toCheck, string nameofToCheck)
        {
            if (toCheck is null)
                throw new ArgumentNullException(nameofToCheck);
            if (toCheck.ToCharArray().Any(x => x != ' ') == false)
                throw new ArgumentException(nameofToCheck +
                                            ElementsExceptionMessages.StringIsNullEmptyOrOnlySpacesAfterParameter);
        }

        public static bool IsNullEmptyOrOnlySpaces(string toCheck)
        {
            if (toCheck is null || toCheck == "")
                return true;

            return toCheck.ToCharArray().All(x => x == ' ');
        }

        public static bool IsNotNullNotEmptyAndNotOnlySpaces(string toCheck) => !IsNullEmptyOrOnlySpaces(toCheck);

        //From https://docs.microsoft.com/en-us/dotnet/api/system.double.equals?redirectedfrom=MSDN&view=netframework-4.8#System_Double_Equals_System_Double_
        public static bool HasMinimalDifference(double a, double b, int units = 1)
        {
            long lValue1 = BitConverter.DoubleToInt64Bits(a);
            long lValue2 = BitConverter.DoubleToInt64Bits(b);

            // If the signs are different, return false except for +0 and -0.
            if ((lValue1 >> 63) != (lValue2 >> 63))
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return a == b;

            long difference = Math.Abs(lValue1 - lValue2);

            return difference <= units;
        }
    }
}