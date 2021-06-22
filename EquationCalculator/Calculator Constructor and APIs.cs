using System;
using System.Collections.Generic;
using EquationElements;
using static EquationElements.Utils;

namespace EquationCalculator
{
    public partial class Calculator
    {
        /// <summary>
        ///     String.Join of the elements passed in when instantiated.
        /// </summary>
        public string ExpandedEquation { get; }

        /// <summary>
        ///     <para>True if the RandomFunction was found when running; otherwise false.</para>
        ///     False if Run() or TryRun() have never been called.
        /// </summary>
        public bool ContainsRandom { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="elements">
        ///     <para>A strongly-ordered list of elements that represents an equation to calculate.</para>
        ///     <para>
        ///         After Constants expanded, implied multiplication operators added and E as Euler's Number replaced with a
        ///         Number. Before Variable elements have been replaced, if applicable.
        ///     </para>
        /// </param>
        public Calculator(ICollection<BaseElement> elements)
        {
            ThrowExceptionIfNullOrEmpty(elements, nameof(elements));
            if (elements.Count == 0)
                throw new ArgumentOutOfRangeException();
            readOnlyElements = (IReadOnlyCollection<BaseElement>) elements;
            ExpandedEquation = string.Join(null, readOnlyElements);
            ContainsRandom = false;
            mostRecentAnswer = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="elements">
        ///     A strongly-ordered list of elements that represents an equation to calculate. Variables,
        ///     Constants and Words are not allowed.
        /// </param>
        /// <param name="answer"></param>
        /// <param name="radians">Calculate in radians. If false, degrees.</param>
        /// <returns></returns>
        public static bool TryConstructAndRun(ICollection<BaseElement> elements, out Number answer, bool radians = true)
        {
            try
            {
                answer = new Calculator(elements).Run(radians);
                return true;
            }
            catch (Exception)
            {
                answer = null;
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="elements">
        ///     A strongly-ordered list of elements that represents an equation to calculate. Variables are
        ///     allowed, Constants and Words are not.
        /// </param>
        /// <param name="valuesOfVariables"></param>
        /// <param name="answer"></param>
        /// <param name="radians">Calculate in radians. If false, degrees.</param>
        /// <returns></returns>
        public static bool TryConstructAndRun(ICollection<BaseElement> elements,
            IDictionary<string, Number> valuesOfVariables, out Number answer, bool radians = true)
        {
            try
            {
                answer = new Calculator(elements).Run(valuesOfVariables, radians);
                return true;
            }
            catch (Exception)
            {
                answer = null;
                return false;
            }
        }


        /// <summary>
        ///     Calculates the answer of the equation provided to the constructor. Returns true if the answer could be calculated.
        ///     Can be run multiple times with different parameters.
        /// </summary>
        /// <param name="answer">The answer of the equation.</param>
        /// <param name="radians">Calculate in radians. If false, degrees.</param>
        /// <returns>True if the answer could be calculated.</returns>
        public bool TryRun(out Number answer, bool radians = true)
        {
            try
            {
                answer = Run(radians);
                return true;
            }
            catch (Exception)
            {
                answer = null;
                return false;
            }
        }

        /// <summary>
        ///     Calculates the answer of the equation provided to the constructor. Replaces Variable Elements as per
        ///     valuesOfVariables. Returns true if the answer could be calculated. Can be run multiple times with different
        ///     parameters.
        /// </summary>
        /// <param name="valuesOfVariables">Variable Elements with these names will be replaced with these Numbers.</param>
        /// <param name="answer">The answer of the equation.</param>
        /// <param name="radians">Calculate in radians. If false, degrees.</param>
        /// <returns>True if the answer could be calculated.</returns>
        public bool TryRun(IDictionary<string, Number> valuesOfVariables, out Number answer,
            bool radians = true)
        {
            try
            {
                answer = Run(valuesOfVariables, radians);
                return true;
            }
            catch (Exception)
            {
                answer = null;
                return false;
            }
        }

        /// <summary>
        ///     Calculates the answer of the equation provided to the constructor. Returns that answer. Can throw exceptions. Can
        ///     be run multiple times with different parameters.
        /// </summary>
        /// <param name="radians">Calculate in radians. If false, degrees.</param>
        /// <returns>The answer of the equation.</returns>
        public Number Run(bool radians = true) => Run(null, radians);
    }
}