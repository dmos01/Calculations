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
    public abstract partial class Word : BaseElement, IComparable
    {
        public string Name { get; }

        protected Word(string name, bool throwExceptionIfNameIsInvalid = true)
        {
            if (name is null)
                throw new ArgumentException(ElementsExceptionMessages.NameOfWordIsNullOrEmpty);

            if (throwExceptionIfNameIsInvalid)
                ThrowExceptionIfNameIsInvalid(name);

            Name = name;
        }

        /// <summary>
        ///     Throw exception if null, empty, only spaces or same name as an Operator or Function.
        /// </summary>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        public static void ThrowExceptionIfNameIsInvalid(string toCheck)
        {
            if (IsNullEmptyOrOnlySpaces(toCheck))
                throw new ArgumentException(ElementsExceptionMessages.NameOfWordIsNullOrEmpty);

            if (IsOperator.Run(RemoveSpaces(toCheck), out _))
                throw new ArgumentException(ElementsExceptionMessages.NameWasSameAsOperatorOrFunction);

            if (IsFunction.Run(RemoveSpaces(toCheck), out _))
                throw new ArgumentException(ElementsExceptionMessages.NameWasSameAsOperatorOrFunction);
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