﻿using System;
using System.Collections.Generic;
using EquationElements;

namespace EquationBuilder
{
    /// <summary>
    ///     Validates order of elements.
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
        ///     Validates the order of elements, adds implied multiplication operators and replaces E as Euler's Number with a
        ///     Number. Returns
        ///     true if the order of elements is valid.
        /// </summary>
        /// <param name="elementsList"></param>
        /// <param name="validatedElementsList">The validated and updated list of elements.</param>
        /// <returns>True if the order of elements is valid.</returns>
        public bool TryRun(LinkedList<BaseElement> elementsList, out ICollection<BaseElement> validatedElementsList) =>
            TryRun(elementsList, false, out validatedElementsList);

        /// <summary>
        ///     Validates the order of elements, adds implied multiplication operators and replaces E as Euler's Number with a
        ///     Number. Returns
        ///     true if the order of elements is valid.
        /// </summary>
        /// <param name="elementsList"></param>
        /// <param name="castUnrecognizedElementsAsVariables">
        ///     If true, casts any unrecognized elements as Variables. If false,
        ///     Variables and unrecognized elements will throw an exception.
        /// </param>
        /// <param name="validatedElementsList">The validated and updated list of elements.</param>
        /// <returns>True if the order of elements is valid.</returns>
        public bool TryRun(LinkedList<BaseElement> elementsList,
            bool castUnrecognizedElementsAsVariables, out ICollection<BaseElement> validatedElementsList)
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
        ///     Validates the order of elements, adds implied multiplication operators and replaces E as Euler's Number with a
        ///     Number. Returns
        ///     the updated list. Will throw exceptions if the order of elements is valid.
        /// </summary>
        /// <param name="elementsList"></param>
        /// <returns>The validated and updated list of elements.</returns>
        public ICollection<BaseElement> Run(LinkedList<BaseElement> elementsList) => Run(elementsList, false);
    }
}