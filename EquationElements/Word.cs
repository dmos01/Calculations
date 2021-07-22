using System;
using EquationElements.Functions;
using EquationElements.Operators;
using static EquationElements.Utils;

namespace EquationElements
{
    /// <summary>
    ///     Abstract class that uses an alpha-numeric string as its identifier but will resolve to a single number on its own.
    /// </summary>
    public abstract partial class Word : BaseElement
    {
        public string Name { get; }

        /// <summary>
        ///     Throws exception if name is null, empty or only spaces. Does not test if name is the same as an Operator or
        ///     Function.
        /// </summary>
        /// <param name="name"></param>
        protected Word(string name)
        {
            if (name is null)
                throw new ArgumentNullException(null, ElementsExceptionMessages.NameOfWordIsNullOrEmpty);

            if (IsNullEmptyOrOnlySpaces(name))
                throw new ArgumentOutOfRangeException(null, ElementsExceptionMessages.NameOfWordIsNullOrEmpty);

            Name = name;
        }

        /// <summary>
        ///     Throw exception if name is null, empty, only spaces or the same as an Operator or Function.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void ThrowExceptionIfNameIsInvalid(string name)
        {
            if (name is null)
                throw new ArgumentNullException(null, ElementsExceptionMessages.NameOfWordIsNullOrEmpty);

            if (IsNullEmptyOrOnlySpaces(name))
                throw new ArgumentOutOfRangeException(null, ElementsExceptionMessages.NameOfWordIsNullOrEmpty);

            if (IsOperator.Run(RemoveSpaces(name), out _) || IsFunction.Run(RemoveSpaces(name), out _))
                throw new ArgumentOutOfRangeException(null, ElementsExceptionMessages.NameWasSameAsOperatorOrFunction);
        }

        /// <summary>
        ///     False if name is null, empty, only spaces or the same as an Operator or Function; otherwise true.
        /// </summary>
        /// <returns></returns>
        public static bool NameIsValid(string name)
        {
            try
            {
                ThrowExceptionIfNameIsInvalid(name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Returns the name.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;

        /// <summary>
        ///     Returns the name.
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(IFormatProvider formatProvider) => Name.ToString(formatProvider);
    }
}