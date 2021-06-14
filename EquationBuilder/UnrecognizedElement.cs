using EquationElements;

namespace EquationBuilder
{
    internal class UnrecognizedElement : Word
    {
        /// <summary>
        ///     When throwing BuilderExceptionMessages.UnidentifiableElement, surrounding numbers should be included in the
        ///     message, even though only the word will seen as unrecognized.
        /// </summary>
        public string OuterNumbersAndWordsElement { get; set; }

        /// <summary>
        ///     Throws exception if name is null, empty or only spaces. Does not test if name is an Operator or Function.
        /// </summary>
        /// <param name="name"></param>
        public UnrecognizedElement(string name) : base(name) =>
            OuterNumbersAndWordsElement = null;
    }
}