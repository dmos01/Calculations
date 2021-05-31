using System;
using System.Collections.Generic;
using EquationElements;
using EquationElements.Functions;
using EquationElements.Operators;
using static EquationElements.Utils;

#pragma warning disable 162

namespace EquationBuilder
{
    /// <summary>
    ///     Creates single Elements from strings/characters.
    /// </summary>
    public class ElementBuilder
    {
        IDictionary<string, string> constants { get; }

        public ElementBuilder() : this(null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="constants">An example of a Constant is Pi, 3.14.</param>
        public ElementBuilder(IDictionary<string, string> constants) => this.constants = constants;

        /// <summary>
        ///     <para>Returns the applicable Element object based on the provided string.</para>
        ///     <para>
        ///         Returns null if name is null, empty or only spaces;
        ///         returns the UnrecognizedElement Element if the string does not match a legal element.
        ///     </para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BaseElement CreateElement(string name)
        {
            name = RemoveSpaces(name);

            if (name is null || name == "")
                return null;

            if (IsOperator.Run(name, out BaseElement operatorElement))
                return operatorElement;

            if (name.Equals(ElementsResources.DecimalSymbol, StringComparison.CurrentCultureIgnoreCase))
                return new DecimalPoint();

            if (name.Equals(ElementsResources.EulersSymbolUpperCase, StringComparison.CurrentCultureIgnoreCase))
                return IsOperator.EulersAndExponentSymbolsAreDifferent ? (BaseElement) new Number(Math.E) : new E();

            if (IsOperator.EulersAndExponentSymbolsAreDifferent && name.Equals(
                ElementsResources.ExponentSymbolUpperCase,
                StringComparison.CurrentCultureIgnoreCase))
                return new E();

            if (name.Equals(ElementsResources.PiSymbol, StringComparison.CurrentCultureIgnoreCase))
                return new Number(Math.PI);

            if (name.Equals(ElementsResources.PiWord, StringComparison.CurrentCultureIgnoreCase))
                return new Number(Math.PI);

            if (IsFunction.Run(name, out BaseElement function))
                return function;

            if (IsConstant(name, out Constant constant))
                return constant;

            if (Number.TryParse(name, out Number number))
                return number;

            return new UnrecognizedElement(name, false);
        }

        /// <summary>
        ///     <para>Returns true if the name appears as a key in the list of constants provided to the constructor.</para>
        ///     <para>Returns false if the name is null or not a key, or ElementBuilder was not instantiated with constants.</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="constant">Null if method returns false.</param>
        /// <returns></returns>
        public bool IsConstant(string name, out Constant constant)
        {
            if (constants is null || name is null)
            {
                constant = null;
                return false;
            }

            name = RemoveSpaces(name);
            if (name == "")
            {
                constant = null;
                return false;
            }

            if (constants.TryGetValue(name, out string value))
            {
                constant = new Constant(name, value);
                return true;
            }

            constant = null;
            return false;
        }
    }
}