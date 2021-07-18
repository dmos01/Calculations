using System;

namespace EquationElements.Operators
{
    /// <summary>
    /// Abstract class.
    /// </summary>
    public abstract class Bracket : BaseElement, IOperator
    {
        /// <summary>
        ///    Get the Type of the equivalent and opposite bracket.
        /// </summary>
        /// <returns></returns>
        public abstract Type GetReverseType();

        /// <summary>
        ///     Get the ToString() of the equivalent and opposite bracket.
        /// </summary>
        /// <returns></returns>
        public abstract string GetReverseSymbol();

        /// <summary>
        ///     <para>
        ///         Returns true if type is the equivalent and opposite of this bracket.
        ///     </para>
        ///     <para>Returns false if type is null.</para>
        /// </summary>
        /// <returns></returns>
        public virtual bool IsReverseOf(Type type)
        {
            if (type is null)
                return false;

            return GetReverseType() == type;
        }

        /// <summary>
        ///     <para>
        ///         Returns true if bracket is the equivalent and opposite of this bracket.
        ///     </para>
        ///     <para>Returns false if bracket is null.</para>
        /// </summary>
        /// <returns></returns>
        public virtual bool IsReverseOf(Bracket bracket) => IsReverseOf(bracket.GetType());

        /// <summary>
        ///     Returns true if character is the ToString() equivalent and opposite of this bracket.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsReverseOf(char character) =>
            GetReverseSymbol().Equals(character.ToString(), StringComparison.CurrentCulture);

        /// <summary>
        ///     <para>
        ///         Returns true if str is the ToString() equivalent and opposite of this bracket.
        ///     </para>
        ///     <para>Returns false if str is null.</para>
        /// </summary>
        /// <returns></returns>
        public virtual bool IsReverseOf(string str)
        {
            if (str is null)
                return false;

            return GetReverseSymbol().Equals(str, StringComparison.CurrentCulture);
        }
    }

    /// <summary>
    /// Abstract class.
    /// </summary>
    public abstract class OpeningBracket : Bracket, IOperatorOrOpeningBracket, IMayPrecedeNegativeNumber
    {
    }

    /// <summary>
    /// Abstract class.
    /// </summary>
    public abstract class ClosingBracket : Bracket, IOperatorOrClosingBracket, IInvalidWhenFirst
    {
    }
}