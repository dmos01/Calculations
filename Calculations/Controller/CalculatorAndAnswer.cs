using System.Collections.Generic;
using EquationBuilder;
using EquationCalculator;
using EquationElements;

namespace Calculations
{
    public partial class Controller
    {
        public class CalculatorAndAnswer
        {
            Number currentAnswer;
            Calculator calculator { get; }

            /// <summary>
            ///     The equation passed in when instantiated.
            /// </summary>
            public string OriginalEquation { get; }

            /// <summary>
            ///     After Constants expanded, implied multiplication operators added and E as Euler's Number replaced with a Number.
            ///     Before Variable elements have been replaced, if applicable.
            /// </summary>
            public string ExpandedEquation => calculator.ExpandedEquation;

            /// <summary>
            ///     In the current Controller.CurrentAnswerFormat.
            /// </summary>
            public string CurrentAnswer { get; private set; }

            /// <summary>
            ///     True if the RandomFunction was found when calculating. Otherwise, false.
            /// </summary>
            public bool ContainsRandom => calculator.ContainsRandom;

            /// <summary>
            ///     Split, validate and calculate using Controller.Constants and Controller.Radians. Set CurrentAnswer based on
            ///     Controller.CurrentAnswerFormat.
            /// </summary>
            /// <param name="equation"></param>
            public CalculatorAndAnswer(string equation)
            {
                OriginalEquation = equation;
                ICollection<BaseElement> splitElements =
                    SplitAndValidate.Run(equation, Constants.GetNameValuePairs());
                calculator = new Calculator(splitElements);
                currentAnswer = calculator.Run(Radians);
                CurrentAnswer = CurrentAnswerFormat.Display(currentAnswer);
            }

            /// <summary>
            ///     Recalculates the answer based on the current Controller.Radians setting.
            /// </summary>
            public void UpdateDegreesOrRadians()
            {
                currentAnswer = calculator.Run(Radians);
                CurrentAnswer = CurrentAnswerFormat.Display(currentAnswer);
            }

            /// <summary>
            ///     Updates the CurrentAnswer based on the current Controller.CurrentAnswerFormat.
            /// </summary>
            public void UpdateAnswerFormat()
            {
                CurrentAnswer = CurrentAnswerFormat.Display(currentAnswer);
            }
        }
    }
}