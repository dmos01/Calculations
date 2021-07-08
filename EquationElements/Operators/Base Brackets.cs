using System;

namespace EquationElements.Operators
{
    /// <summary>
    /// Abstract class.
    /// </summary>
    public abstract class Bracket : BaseElement, IOperator
    {
        /// <summary>
        ///     If this is an OpeningBracket, get the Type of the equivalent ClosingBracket, or vice-versa.
        /// </summary>
        /// <returns></returns>
        public abstract Type GetReverseType();

        /// <summary>
        ///     If this is an OpeningBracket, get the ToString() of the equivalent ClosingBracket, or vice-versa.
        /// </summary>
        /// <returns></returns>
        public abstract string GetReverseSymbol();

        /// <summary>
        ///     <para>
        ///         Returns true if Bracket b is the equivalent opposite of this object. (ClosingBracket if this is an
        ///         OpeningBracket, OpeningBracket if this is a ClosingBracket.)
        ///     </para>
        ///     <para>Returns false if b is null.</para>
        /// </summary>
        /// <returns></returns>
        public virtual bool IsReverseOf(Bracket b)
        {
            if (b is null)
                return false;

            return GetReverseType() == b.GetType();
        }

        /// <summary>
        ///     Returns true if the character is the ToString() equivalent opposite of this object. (ClosingBracket if this is an
        ///     OpeningBracket, OpeningBracket if this is a ClosingBracket.)
        /// </summary>
        /// <returns></returns>
        public virtual bool IsReverseOf(char character) =>
            GetReverseSymbol().Equals(character.ToString(), StringComparison.CurrentCulture);

        /// <summary>
        ///     <para>
        ///         Returns true if str is the ToString() equivalent opposite of this object. (ClosingBracket if this is an
        ///         OpeningBracket, OpeningBracket if this is a ClosingBracket.)
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