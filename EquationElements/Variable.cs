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
        public Variable() : base("x")
        {
        }

        /// <summary>
        ///     Throws exception if name is null, empty or only spaces. Does not test if name is an Operator or Function.
        /// </summary>
        /// <param name="name"></param>
        public Variable(string name) : base(name)
        {
        }
    }
}