using System;
using System.Collections.Generic;
using EquationElements;

namespace EquationBuilder
{
    /// <summary>
    ///     Static class that runs the Splitter and Validator.
    /// </summary>
    public static class SplitAndValidate
    {
        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="elements">Returns a strongly-ordered list of elements that represents the expanded equation.</param>
        /// <returns>True if the equation is valid.</returns>
        public static bool TryRun(string equation, out ICollection<BaseElement> elements) =>
            TryRun(equation, new ElementBuilder(), false, out elements);

        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="constants">An example of a Constant is Pi, 3.14.</param>
        /// <param name="elements">Returns a strongly-ordered list of elements that represents the expanded equation.</param>
        /// <returns>True if the equation is valid.</returns>
        public static bool TryRun(string equation, IDictionary<string, string> constants,
            out ICollection<BaseElement> elements) =>
            TryRun(equation, new ElementBuilder(constants), false, out elements);

        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="elementBuilder"></param>
        /// <param name="elements">Returns a strongly-ordered list of elements that represents the expanded equation.</param>
        /// <returns>True if the equation is valid.</returns>
        public static bool TryRun(string equation, ElementBuilder elementBuilder,
            out ICollection<BaseElement> elements) =>
            TryRun(equation, elementBuilder, false, out elements);

        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="constants">An example of a Constant is Pi, 3.14.</param>
        /// <param name="castUnrecognizedElementsAsVariables">
        ///     If true, casts any unrecognized elements as Variables. If false,
        ///     Variables and unrecognized elements will throw an exception.
        /// </param>
        /// <param name="elements">Returns a strongly-ordered list of elements that represents the expanded equation.</param>
        /// <returns>True if the equation is valid.</returns>
        public static bool TryRun(string equation, IDictionary<string, string> constants,
            bool castUnrecognizedElementsAsVariables, out ICollection<BaseElement> elements) =>
            TryRun(equation, new ElementBuilder(constants), castUnrecognizedElementsAsVariables, out elements);

        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="elementBuilder"></param>
        /// <param name="castUnrecognizedElementsAsVariables">
        ///     If true, casts any unrecognized elements as Variables. If false,
        ///     Variables and unrecognized elements will throw an exception.
        /// </param>
        /// <param name="elements">Returns a strongly-ordered list of elements that represents the expanded equation.</param>
        /// <returns>True if the equation is valid.</returns>
        public static bool TryRun(string equation, ElementBuilder elementBuilder,
            bool castUnrecognizedElementsAsVariables, out ICollection<BaseElement> elements)
        {
            try
            {
                elements = Run(equation, elementBuilder, castUnrecognizedElementsAsVariables);
                return true;
            }
            catch (Exception)
            {
                elements = null;
                return false;
            }
        }


        /// <summary>
        ///     Splits and validates the equation. Returns a strongly-ordered list of elements that represents
        ///     the expanded equation. Will throw exceptions if equation is invalid.
        /// </summary>
        /// <param name="equation"></param>
        /// <returns>Returns a strongly-ordered list of elements that represents the expanded equation.</returns>
        public static ICollection<BaseElement> Run(string equation) => Run(equation, new ElementBuilder(), false);

        /// <summary>
        ///     Splits and validates the equation. Returns a strongly-ordered list of elements that represents
        ///     the expanded equation. Will throw exceptions if equation is invalid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="constants">An example of a Constant is Pi, 3.14.</param>
        /// <returns>Returns a strongly-ordered list of elements that represents the expanded equation.</returns>
        public static ICollection<BaseElement>
            Run(string equation, IDictionary<string, string> constants) =>
            Run(equation, new ElementBuilder(constants), false);

        /// <summary>
        ///     Splits and validates the equation. Returns a strongly-ordered list of elements that represents
        ///     the expanded equation. Will throw exceptions if equation is invalid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="elementBuilder"></param>
        /// <returns>Returns a strongly-ordered list of elements that represents the expanded equation.</returns>
        public static ICollection<BaseElement> Run(string equation, ElementBuilder elementBuilder) =>
            Run(equation, elementBuilder, false);

        /// <summary>
        ///     Splits and validates the equation. Returns Returns a strongly-ordered list of elements that represents the expanded
        ///     equation.
        ///     the expanded equation. Will throw exceptions if equation is invalid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="constants">An example of a Constant is Pi, 3.14.</param>
        /// <param name="castUnrecognizedElementsAsVariables">
        ///     If true, casts any unrecognized elements as Variables. If false,
        ///     Variables and unrecognized elements will throw an exception.
        /// </param>
        /// <returns>Returns a strongly-ordered list of elements that represents the expanded equation.</returns>
        public static ICollection<BaseElement> Run(string equation, IDictionary<string, string> constants,
            bool castUnrecognizedElementsAsVariables) =>
            Run(equation, new ElementBuilder(constants), castUnrecognizedElementsAsVariables);

        /// <summary>
        ///     Splits and validates the equation. Returns a strongly-ordered list of elements that represents
        ///     the expanded equation. Will throw exceptions if equation is invalid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="elementBuilder"></param>
        /// <param name="castUnrecognizedElementsAsVariables">
        ///     If true, casts any unrecognized elements as Variables. If false,
        ///     Variables and unrecognized elements will throw an exception.
        /// </param>
        /// <returns>Returns a strongly-ordered list of elements that represents the expanded equation.</returns>
        public static ICollection<BaseElement> Run(string equation, ElementBuilder elementBuilder,
            bool castUnrecognizedElementsAsVariables)
        {
            if (elementBuilder is null)
                elementBuilder = new ElementBuilder();

            LinkedList<BaseElement> elements = new Splitter(elementBuilder).Run(equation);
            return new Validator(elementBuilder).Run(elements, castUnrecognizedElementsAsVariables);
        }
    }
}