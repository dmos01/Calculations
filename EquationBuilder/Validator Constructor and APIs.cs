using System;
using System.Collections.Generic;
using EquationElements;

namespace EquationBuilder
{
    /// <summary>
    ///     Validates and expands the order of elements.
    /// </summary>
    public partial class Validator
    {
        public Validator() => elementBuilder = new ElementBuilder();

        /// <summary>
        /// </summary>
        /// <param name="elementBuilder">
        ///     Allows multiple equations to be run using the same constants, if applicable. An example of
        ///     a Constant is Pi, 3.14.
        /// </param>
        public Validator(ElementBuilder elementBuilder) => this.elementBuilder = elementBuilder ?? new ElementBuilder();

        /// <summary>
        /// </summary>
        /// <param name="constants">
        ///     Allows multiple equations to be run using the same constants, if applicable. An example of a
        ///     Constant is Pi, 3.14.
        /// </param>
        public Validator(IDictionary<string, string> constants) =>
            elementBuilder = new ElementBuilder(constants);

        /// <summary>
        ///     Validates the order of elements, expands Constants, adds implied operators and brackets, and replaces E where possible. Returns true if the order of elements is valid.
        /// </summary>
        /// <param name="elementsList"></param>
        /// <param name="validatedElementsList">The validated and expanded list of elements.</param>
        /// <returns>True if the order of elements is valid.</returns>
        public bool TryRun(LinkedList<BaseElement> elementsList, out LinkedList<BaseElement> validatedElementsList) =>
            TryRun(elementsList, false, out validatedElementsList);

        /// <summary>
        ///     Validates the order of elements, expands Constants, adds implied operators and brackets, and replaces E where possible. Returns true if the order of elements is valid.
        /// </summary>
        /// <param name="elementsList"></param>
        /// <param name="castUnrecognizedElementsAsVariables">
        ///     If true, casts any unrecognized elements as Variables. If false,
        ///     Variables and unrecognized elements will throw an exception.
        /// </param>
        /// <param name="validatedElementsList">The validated and expanded list of elements.</param>
        /// <returns>True if the order of elements is valid.</returns>
        public bool TryRun(LinkedList<BaseElement> elementsList,
            bool castUnrecognizedElementsAsVariables, out LinkedList<BaseElement> validatedElementsList)
        {
            try
            {
                validatedElementsList = Run(elementsList, castUnrecognizedElementsAsVariables);
                return true;
            }
            catch (Exception)
            {
                validatedElementsList = null;
                return false;
            }
        }

        /// <summary>
        ///     Validates the order of elements, expands Constants, adds implied operators and brackets, and replaces E where possible. Returns the expanded list. Will throw exceptions if the order of elements is valid.
        /// </summary>
        /// <param name="elementsList"></param>
        /// <returns>The validated and expanded list of elements.</returns>
        public LinkedList<BaseElement> Run(LinkedList<BaseElement> elementsList) => Run(elementsList, false);
    }
}