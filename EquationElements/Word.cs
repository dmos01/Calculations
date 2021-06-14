using System;
using EquationElements.Functions;
using EquationElements.Operators;
using static EquationElements.Utils;

namespace EquationElements
{
    /// <summary>
    ///     Abstract class that is a parent of any non-Operator, non-Function Element using a word as its identifier (such as
    ///     Constants and Variables).
    /// </summary>
    public abstract partial class Word : BaseElement
    {
        public string Name { get; }

        /// <summary>
        ///     Throws exception if name is null, empty or only spaces. Does not test if name is an Operator or Function.
        /// </summary>
        /// <param name="name"></param>
        protected Word(string name)
        {
            ThrowExceptionIfNullEmptyOrOnlySpaces(name, nameof(name));
            Name = name;
        }

        /// <summary>
        ///     Throw exception if name is null, empty, only spaces or the same as an Operator or Function.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void ThrowExceptionIfNameIsInvalid(string name)
        {
            if (IsNullEmptyOrOnlySpaces(name))
                throw new ArgumentException(ElementsExceptionMessages.NameOfWordIsNullOrEmpty);

            if (IsOperator.Run(RemoveSpaces(name), out _) || IsFunction.Run(RemoveSpaces(name), out _))
                throw new ArgumentException(ElementsExceptionMessages.NameWasSameAsOperatorOrFunction);
        }

        /// <summary>
        ///     False if name is null, empty, only spaces or the same as an Operator or Function. Otherwise, true.
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