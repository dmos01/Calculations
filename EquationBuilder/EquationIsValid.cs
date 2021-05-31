using System;
using System.Collections.Generic;

namespace EquationBuilder
{
    /// <summary>
    ///     Static class that runs the Splitter and Validator.
    /// </summary>
    public static class EquationIsValid
    {
        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <returns>True if the equation is valid.</returns>
        public static bool Run(string equation) => Run(equation, new ElementBuilder(), false);

        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="elementBuilder"></param>
        /// <returns>True if the equation is valid.</returns>
        public static bool Run(string equation, ElementBuilder elementBuilder) => Run(equation, elementBuilder, false);

        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="constants">An example of a Constant is Pi, 3.14.</param>
        /// <returns>True if the equation is valid.</returns>
        public static bool Run(string equation, IDictionary<string, string> constants) =>
            Run(equation, new ElementBuilder(constants), false);

        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="constants"></param>
        /// <param name="allowUnrecognizedElements">If true, unrecognized elements will be allowed.</param>
        /// <returns>True if the equation is valid.</returns>
        public static bool Run(string equation, IDictionary<string, string> constants,
            bool allowUnrecognizedElements) =>
            Run(equation, new ElementBuilder(constants), allowUnrecognizedElements);

        /// <summary>
        ///     Splits and validates the equation. Returns true if the equation is valid.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="elementBuilder"></param>
        /// <param name="allowUnrecognizedElements">If true, unrecognized elements will be allowed.</param>
        /// <returns>True if the equation is valid.</returns>
        public static bool Run(string equation, ElementBuilder elementBuilder, bool allowUnrecognizedElements)
        {
            try
            {
                SplitAndValidate.Run(equation, elementBuilder, allowUnrecognizedElements);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}