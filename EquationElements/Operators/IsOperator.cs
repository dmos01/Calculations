using System;
using System.Collections.Generic;

namespace EquationElements.Operators
{
    /// <summary>
    ///     Static class.
    /// </summary>
    public static class IsOperator
    {
        static readonly Dictionary<string, Type> stringToOperator =
            new Dictionary<string, Type>(StringComparer.CurrentCultureIgnoreCase)
            {
                {OperatorRepresentations.AdditionSymbol, typeof(AdditionOperator)},
                {OperatorRepresentations.SubtractionSymbol, typeof(SubtractionOperator)},
                {OperatorRepresentations.ComputerMultiplicationSymbol, typeof(MultiplicationOperator)},
                {OperatorRepresentations.PaperMultiplicationSymbol, typeof(MultiplicationOperator)},
                {OperatorRepresentations.DotMultiplicationSymbol, typeof(MultiplicationOperator)},
                {OperatorRepresentations.ComputerDivisionSymbol, typeof(DivisionOperator)},
                {OperatorRepresentations.PaperDivisionSymbol, typeof(DivisionOperator)},

                {OperatorRepresentations.PowerSymbol, typeof(PowerOperator)},
                {OperatorRepresentations.RootSymbol, typeof(RootSymbolOperator)},
                {OperatorRepresentations.RootWord, typeof(RootWordOperator)},
                {OperatorRepresentations.ArgumentSeperatorSymbol, typeof(ArgumentSeparatorOperator)},
                {OperatorRepresentations.ModulusSymbol, typeof(ModulusSymbolOperator)},
                {OperatorRepresentations.ModulusWord, typeof(ModulusWordOperator)},

                {OperatorRepresentations.ParenthesisOpeningBracketSymbol, typeof(ParenthesisOpeningBracket)},
                {OperatorRepresentations.ParenthesisClosingBracketSymbol, typeof(ParenthesisClosingBracket)},
                {OperatorRepresentations.SquareOpeningBracketSymbol, typeof(SquareOpeningBracket)},
                {OperatorRepresentations.SquareClosingBracketSymbol, typeof(SquareClosingBracket)},
                {OperatorRepresentations.CurlyOpeningBracketSymbol, typeof(CurlyOpeningBracket)},
                {OperatorRepresentations.CurlyClosingBracketSymbol, typeof(CurlyClosingBracket)}
            };

        /// <summary>
        ///     Returns true if the strings for EulersSymbol and ExponentSymbol are different, as defined in ElementsResources.
        ///     Otherwise, false.
        /// </summary>
        public static bool EulersAndExponentSymbolsAreDifferent =>
            !ElementsResources.EulersSymbolUpperCase.Equals(
                ElementsResources.ExponentSymbolUpperCase);

        /// <summary>
        ///     Returns false if name is null or not an operator; otherwise true.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="operatorElement">Null if method returns false.</param>
        /// <returns></returns>
        public static bool Run(string name, out BaseElement operatorElement)
        {
            if (stringToOperator.TryGetValue(name, out Type value))
                operatorElement = (BaseElement) Activator.CreateInstance(value);
            else
                operatorElement = null;

            return !(operatorElement is null);
        }

        /// <summary>
        ///     Returns true if str matches PiSymbol, PiWord or EulersSymbol, as defined in ElementsResources; otherwise false.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StringIsPiOrEulers(string str) =>
            str.Equals(ElementsResources.PiSymbol, StringComparison.CurrentCultureIgnoreCase) ||
            str.Equals(ElementsResources.PiWord, StringComparison.CurrentCultureIgnoreCase) ||
            str.Equals(ElementsResources.EulersSymbolUpperCase, StringComparison.CurrentCultureIgnoreCase);
    }
}