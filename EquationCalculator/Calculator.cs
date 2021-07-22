using System;
using System.Collections.Generic;
using System.Linq;
using EquationElements;
using EquationElements.Functions;
using EquationElements.Operators;

namespace EquationCalculator
{
    public partial class Calculator
    {
        LinkedListNode<BaseElement> currentNode;

        LinkedList<BaseElement> currentRunElements;
        EquationSegment currentSection;

        /// <summary>
        ///     Null if Run() or TryRun() have never been called.
        /// </summary>
        private Number mostRecentAnswer;

        Queue<EquationSegment> nodesWithBrackets;
        bool radians;
        IDictionary<string, Number> valuesOfVariables;

        /// <summary>
        ///     Because objects are stored by reference, being ReadOnly does not prevent other code casting back to an ICollection
        ///     and changing the contents.
        /// </summary>
        private IReadOnlyCollection<BaseElement> readOnlyElements { get; }

        /// <summary>
        ///     Calculates the answer of the equation provided to the constructor. Replaces Variable Elements as per
        ///     valuesOfVariables. Returns the answer. Can throw exceptions. Can be run multiple times with different parameters.
        /// </summary>
        /// <param name="valuesOfVariables">Variable Elements with these names will be replaced with these Numbers.</param>
        /// <param name="radians">Calculate in radians. If false, degrees.</param>
        /// <returns>The answer of the equation.</returns>
        public Number Run(IDictionary<string, Number> valuesOfVariables, bool radians = true)
        {
            this.radians = radians;
            this.valuesOfVariables = valuesOfVariables;

            DuplicateReadOnlyElements();
            FindBracketsRandomAndVariables();

            while (nodesWithBrackets.Any())
            {
                currentSection = nodesWithBrackets.Dequeue();
                CalculateCurrentSection();
                RemoveBracketsAndMergeMultipleNegatives();
            }

            nodesWithBrackets = null;
            currentSection = new EquationSegment(currentRunElements.First, currentRunElements.Last);
            CalculateCurrentSection();
            currentSection = null;
            currentNode = null;

            if (currentRunElements.First() is Number answerNumber && currentRunElements.First.Next is null)
            {
                currentRunElements = null;
                mostRecentAnswer = answerNumber;
                return mostRecentAnswer;
            }

            throw new Exception(CalculatorExceptionMessages.UnknownErrorDefault);
        }

        private void DuplicateReadOnlyElements()
        {
            currentRunElements = new LinkedList<BaseElement>();
            foreach (BaseElement element in readOnlyElements)
                currentRunElements.AddLast(element);
        }

        private void FindBracketsRandomAndVariables()
        {
            nodesWithBrackets = new Queue<EquationSegment>();
            currentNode = currentRunElements.First;
            Stack<LinkedListNode<BaseElement>> openingBracketsNodes = new Stack<LinkedListNode<BaseElement>>();

            while (currentNode != null)
            {
                switch (currentNode.Value)
                {
                    case OpeningBracket _:
                        openingBracketsNodes.Push(currentNode);
                        break;
                    case ClosingBracket _ when openingBracketsNodes.Any() == false:
                        throw new Exception(CalculatorExceptionMessages.UnknownErrorDefault);
                    case ClosingBracket _:
                        nodesWithBrackets.Enqueue(new EquationSegment(openingBracketsNodes.Pop(), currentNode));
                        break;

                    case RandomFunction _:
                        ContainsRandom = true;
                        break;

                    case Variable _:
                        if (valuesOfVariables is null)
                            throw new Exception(
                                CalculatorExceptionMessages.ValueOfVariableNotProvidedBeforeParameter +
                                currentNode.Value +
                                CalculatorExceptionMessages.ValueOfVariableNotProvidedAfterParameter);

                        if (valuesOfVariables.TryGetValue(currentNode.Value.ToString(), out Number number))
                            currentNode.Value = number;
                        else
                            throw new Exception(
                                CalculatorExceptionMessages.ValueOfVariableNotProvidedBeforeParameter +
                                currentNode.Value +
                                CalculatorExceptionMessages.ValueOfVariableNotProvidedAfterParameter);
                        break;
                }

                currentNode = currentNode.Next;
            }
        }

        private void RemoveBracketsAndMergeMultipleNegatives()
        {
            currentNode = currentSection.Start.Previous;

            if (currentSection.Start.Value is OpeningBracket &&
                currentSection.End.Value is ClosingBracket)
            {
                currentRunElements.Remove(currentSection.Start);
                currentRunElements.Remove(currentSection.End);
            }

            //Merge multiple minus signs created by resolving brackets.
            if (currentNode?.Value is SubtractionOperator
                && (currentNode.Previous is null || currentNode.Previous.Value is IMayPrecedeNegativeNumber)
                && currentNode.Next?.Value is Number number)
            {
                //* - -6 or * - 6
                currentNode.Next.Value = number * -1;
                currentRunElements.Remove(currentNode);
            }
        }


        private void CalculateCurrentSection()
        {
            LoopWhileFunctions();
            LoopPowerRootAndE();
            LoopWhileMultiplicationDivisionModulusAndFactorials();
            LoopWhileAdditionAndSubtraction();
        }

        private void LoopWhileFunctions()
        {
            currentNode = currentSection.Start;
            while (currentNode != currentSection.End.Next && currentNode != null)
            {
                switch (currentNode.Value)
                {
                    case LnFunction ln when currentNode.Next.Value is Number number:
                        if (number <= 0)
                            throw new Exception(
                                CalculatorExceptionMessages.LogToNegativeOrZeroBeforeParameter + number +
                                CalculatorExceptionMessages.LogToNegativeOrZeroAfterParameter);
                        currentNode.Value = ln.PerformOn(number);
                        currentRunElements.Remove(currentNode.Next);
                        break;

                    case LogFunction _:
                        DoLog();
                        break;

                    case TrigonometricFunction trigFunction when currentNode.Next.Value is Number number:
                        currentNode.Value = trigFunction.PerformOn(number, radians);
                        currentRunElements.Remove(currentNode.Next);
                        break;

                    case Factorial _:
                        break;

                    case OneArgumentFunction oneArgumentFunction when currentNode.Next.Value is Number number:
                        currentNode.Value = oneArgumentFunction.PerformOn(number);
                        currentRunElements.Remove(currentNode.Next);
                        break;

                    case TwoArgumentFunction twoArgumentFunction when currentNode.Next.Value is Number number:
                        Number secondNumberElement = (Number) currentNode.Next.Next.Next.Value;
                        currentNode.Value = twoArgumentFunction.PerformOn(number, secondNumberElement);
                        currentRunElements.Remove(currentNode.Next);
                        currentRunElements.Remove(currentNode.Next);
                        currentRunElements.Remove(currentNode.Next);
                        break;
                }

                currentNode = currentNode.Next;
            }
        }

        private void LoopPowerRootAndE()
        {
            currentNode = currentSection.Start;
            while (currentNode != currentSection.End.Next && currentNode != null)
            {
                switch (currentNode.Value)
                {
                    case RootOperator rootOperator:
                        Do(rootOperator);
                        break;

                    case PowerOperator powerOperator:
                        Do(powerOperator);
                        break;

                    case E _:
                        DoE();
                        break;
                }

                currentNode = currentNode.Next;
            }
        }

        private void DoE()
        {
            if (!(currentNode.Previous?.Value is Number num))
                return;

            switch (currentNode.Next?.Value)
            {
                case Number pow:
                    E.TestPower(pow); //Prevents bug 23E23E23
                    currentNode.Previous.Value = new Number(num + ElementsResources.ExponentSymbolUpperCase + pow);
                    currentNode = currentNode.Previous;
                    break;
                case SubtractionOperator _ when currentNode.Next.Next?.Value is Number pow:
                    E.TestPower(pow);
                    currentNode.Previous.Value = new Number(num + ElementsResources.ExponentSymbolUpperCase + pow * -1);
                    currentNode = currentNode.Previous;
                    currentRunElements.Remove(currentNode.Next);
                    break;
                case AdditionOperator _ when currentNode.Next.Next?.Value is Number pow:
                    E.TestPower(pow);
                    currentNode.Previous.Value = new Number(num + ElementsResources.ExponentSymbolUpperCase + pow);
                    currentNode = currentNode.Previous;
                    currentRunElements.Remove(currentNode.Next);
                    break;

                default:
                    throw new Exception(CalculatorExceptionMessages.UndeterminedUseOfEDefault);
            }

            currentRunElements.Remove(currentNode.Next);
            currentRunElements.Remove(currentNode.Next);
        }

        private void DoLog()
        {
            switch (currentNode.Next.Value)
            {
                case Number logBase:
                    if (logBase <= 0)
                        throw new Exception(CalculatorExceptionMessages.LogToNegativeOrZeroBeforeParameter +
                                            logBase +
                                            CalculatorExceptionMessages.LogToNegativeOrZeroAfterParameter);

                    Number logNumber = (Number) currentNode.Next.Next.Next.Value;
                    if (logNumber <= 0)
                        throw new Exception(
                            CalculatorExceptionMessages.LogToNegativeOrZeroBeforeParameter + logNumber +
                            CalculatorExceptionMessages.LogToNegativeOrZeroAfterParameter);

                    currentNode.Value = new LogFunction().PerformOn(logBase, logNumber);
                    currentRunElements.Remove(currentNode.Next);
                    currentRunElements.Remove(currentNode.Next);
                    currentRunElements.Remove(currentNode.Next);
                    break;

                case E _:
                    logNumber = (Number) currentNode.Next.Next.Next.Value;
                    if (logNumber <= 0)
                        throw new Exception(
                            CalculatorExceptionMessages.LogToNegativeOrZeroBeforeParameter + logNumber +
                            CalculatorExceptionMessages.LogToNegativeOrZeroAfterParameter);

                    currentNode.Value = new LnFunction().PerformOn(logNumber);
                    currentRunElements.Remove(currentNode.Next);
                    currentRunElements.Remove(currentNode.Next);
                    currentRunElements.Remove(currentNode.Next);
                    break;
                default:
                    throw new Exception(CalculatorExceptionMessages.UnknownErrorDefault);
            }
        }

        private void LoopWhileMultiplicationDivisionModulusAndFactorials()
        {
            currentNode = currentSection.Start;
            while (currentNode != currentSection.End.Next && currentNode != null)
            {
                switch (currentNode.Value)
                {
                    case DivisionOperator divisionOperator:
                        Do(divisionOperator);
                        break;

                    case MultiplicationOperator multiplicationOperator:
                        Do(multiplicationOperator);
                        break;

                    case ModulusOperator modulusOperator:
                        Do(modulusOperator);
                        break;

                    case Factorial factorial:
                        currentNode.Previous.Value = factorial.PerformOn((Number) currentNode.Previous.Value);
                        currentNode = currentNode.Previous;
                        currentRunElements.Remove(currentNode.Next);
                        break;
                }

                currentNode = currentNode.Next;
            }
        }

        private void LoopWhileAdditionAndSubtraction()
        {
            currentNode = currentSection.Start;
            while (currentNode != currentSection.End.Next && currentNode != null)
            {
                switch (currentNode.Value)
                {
                    case AdditionOperator additionOperator:
                        Do(additionOperator);
                        break;

                    case SubtractionOperator subtractionOperator:
                        Do(subtractionOperator);
                        break;
                }

                currentNode = currentNode.Next;
            }
        }

        private void Do(TwoArgumentElement basicOperator)
        {
            currentNode.Previous.Value = basicOperator.PerformOn((Number) currentNode.Previous.Value,
                (Number) currentNode.Next.Value);
            currentNode = currentNode.Previous;
            currentRunElements.Remove(currentNode.Next);
            currentRunElements.Remove(currentNode.Next);
        }
    }
}