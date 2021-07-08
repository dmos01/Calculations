using System;
using System.Collections.Generic;
using EquationElements;

namespace EquationBuilder
{
    /// <summary>
    ///     <para>Splits a string into a LinkedList of Elements and expands Constants. Validates use of decimal points.</para>
    ///     <para>
    ///         Limitation: Constants with a digit followed by (mod|root|E) or (mod|root|E) followed by a digit may not split
    ///         correctly.
    ///     </para>
    /// </summary>
    public partial class Splitter
    {
        public Splitter() => elementBuilder = new ElementBuilder();

        /// <summary>
        /// </summary>
        /// <param name="elementBuilder">
        ///     Allows multiple equations to be run using the same constants, if applicable. An example of
        ///     a Constant is Pi, 3.14.
        /// </param>
        public Splitter(ElementBuilder elementBuilder) => this.elementBuilder = elementBuilder ?? new ElementBuilder();

        /// <summary>
        /// </summary>
        /// <param name="constants">
        ///     Allows multiple equations to be run using the same constants, if applicable. An example of a
        ///     Constant is Pi, 3.14.
        /// </param>
        public Splitter(IDictionary<string, string> constants) =>
            elementBuilder = new ElementBuilder(constants);

        /// <summary>
        ///     Splits the string into a LinkedList of Elements and expands Constants. Validates use of decimal points. Returns
        ///     true if there were no problems splitting.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="splitElementsList">The split list of elements.</param>
        /// <returns>True if there were no problems splitting.</returns>
        public bool TryRun(string equation, out LinkedList<BaseElement> splitElementsList)
        {
            try
            {
                splitElementsList = Run(equation);
                return true;
            }
            catch (Exception)
            {
                splitElementsList = null;
                return false;
            }
        }
    }
}