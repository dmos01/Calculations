namespace EquationElements
{
    /// <summary>
    ///     An element represented by a word that has an unknown value when building the equation. Its value can vary from
    ///     calculation to calculation. An example is x.
    /// </summary>
    public class Variable : Word
    {
        /// <summary>
        ///     Uses "x" as the Variable's name.
        /// </summary>
        public Variable() : base("x", false)
        {
        }

        public Variable(string name, bool throwExceptionIfNameIsInvalid = true) : base(name,
            throwExceptionIfNameIsInvalid)
        {
        }
    }
}