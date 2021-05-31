using EquationElements;

namespace EquationBuilder
{
    internal class UnrecognizedElement : Word
    {
        /// <summary>
        /// When throwing BuilderExceptionMessages.UnidentifiableElement, surrounding numbers should be included in the message, even though only the word will read as unrecognized.
        /// </summary>
        public string OuterNumbersAndWordsElement { get; set; }

        public UnrecognizedElement(string name, bool throwExceptionIfNameIsInvalid = true) : base(name,
            throwExceptionIfNameIsInvalid) =>
            OuterNumbersAndWordsElement = null;
    }
}