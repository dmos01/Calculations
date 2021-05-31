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
        //Readonly here only prevents the collection from being changed. It does not prevent elements inside them from being added, changed or removed.
        readonly IReadOnlyCollection<BaseElement> readOnlyElements;

        LinkedListNode<BaseElement> currentNode;
        LinkedList<BaseElement> currentRunElements;
        EquationSegment currentSection;
        Queue<EquationSegment> nodesWithBrackets;
        bool Radians;
        IDictionary<string, Number> ValuesOfVariables;

        /// <summary>
        ///     Calculates the answer of the equation provided to the constructor. Replaces Variable Elements as per
        ///     valuesOfVariables. Returns the answer. Can throw exceptions. Can be run multiple times with different parameters.
        /// </summary>
        /// <param name="valuesOfVariables">Variable Elements with these names will be replaced with these Numbers.</param>
        /// <param name="radians">Calculate in radians. If false, degrees.</param>
        /// <returns>The answer of the equation.</returns>
        public Number Run(IDictionary<string, Number> valuesOfVariables, bool radians = true)
        {
            ValuesOfVariables = valuesOfVariables;
            Radians = radians;

            DuplicateElementsList();
            nodesWithBrackets = FindBracketsRandomAndVariables();

            while (nodesWithBrackets.Any())
            {
                currentSection = nodesWithBrackets.Dequeue();
                CalculateCurrentSection();
                RemoveBracketsAndMergeMultipleNegatives();
            }

            currentSection = new EquationSegment(currentRunElements.First, currentRunElements.Last);
            CalculateCurrentSection();

            if (!(currentRunElements.First() is Number answerNumber) ||
                currentRunElements.First.Next != null)
                throw new Exception(CalculatorExceptionMessages.UnknownErrorDefault);

            return answerNumber;
        }


        private void DuplicateElementsList()
        {
            currentRunElements = new LinkedList<BaseElement>();
            foreach (BaseElement element in readOnlyElements)
                currentRunElements.AddLast(element);
        }

        private Queue<EquationSegment> FindBracketsRandomAndVariables()
        {
            Queue<EquationSegment> _output = new Queue<EquationSegment>();
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
                        _output.Enqueue(new EquationSegment(openingBracketsNodes.Pop(), currentNode));
                        break;

                    case RandomFunction _:
                        ContainsRandom = true;
                        break;

                    case Variable _:
                        if (ValuesOfVariables is null)
                            throw new Exception(
                                CalculatorExceptionMessages.ValueOfVariableNotProvidedBeforeParameter +
                                currentNode.Value +
                                CalculatorExceptionMessages.ValueOfVariableNotProvidedAfterParameter);

                        if (ValuesOfVariables.TryGetValue(currentNode.Value.ToString(), out Number value))
                            currentNode.Value = value;
                        else
                            throw new Exception(
                                CalculatorExceptionMessages.ValueOfVariableNotProvidedBeforeParameter +
                                currentNode.Value +
                                CalculatorExceptionMessages.ValueOfVariableNotProvidedAfterParameter);
                        break;
                }

                currentNode = currentNode.Next;
            }

            return _output;
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
            LoopPowerRootAndE();
            LoopWhileFunctions();
            LoopWhileMultiplicationDivisionModulusAndFactorials();
            LoopWhileAdditionAndSubtraction();
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
                    currentNode.Previous.Value = new Number(num + ElementsResources.ExponentSymbolUpperCase + pow);
                    currentNode = currentNode.Previous;
                    break;
                case SubtractionOperator _ when currentNode.Next.Next?.Value is Number pow:
                    currentNode.Previous.Value = new Number(num + ElementsResources.ExponentSymbolUpperCase +
                                                            OperatorRepresentations.SubtractionSymbol + pow);
                    currentNode = currentNode.Previous;
                    currentRunElements.Remove(currentNode.Next);
                    break;
                case AdditionOperator _ when currentNode.Next.Next?.Value is Number pow:
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

                    case FactorialFunction factorial:
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


        private void LoopWhileFunctions()
        {
            currentNode = currentSection.Start;
            while (currentNode != currentSection.End.Next && currentNode != null)
            {
                switch (currentNode.Value)
                {
                    case LnFunction ln when currentNode.Next.Value is Number number:
                        if (number <= 0)
                            throw new ArgumentException(
                                CalculatorExceptionMessages.LogToNegativeOrZeroBeforeParameter + number +
                                CalculatorExceptionMessages.LogToNegativeOrZeroAfterParameter);
                        currentNode.Value = ln.PerformOn(number);
                        currentRunElements.Remove(currentNode.Next);
                        break;

                    case LogFunction _:
                        DoLog();
                        break;

                    case TrigonometricFunction trig when currentNode.Next.Value is Number number:
                        currentNode.Value = trig.PerformOn(number, Radians);
                        currentRunElements.Remove(currentNode.Next);
                        break;

                    case FactorialFunction _:
                        break;

                    case IFunction _ when currentNode.Next.Value is Number number:
                        switch (currentNode.Value)
                        {
                            case OneArgumentElement oneArgumentElement:
                                currentNode.Value = oneArgumentElement.PerformOn(number);
                                break;
                            case TwoArgumentElement twoArgumentElement:
                                Number secondNumberElement = (Number) currentNode.Next.Next.Next.Value;
                                currentNode.Value = twoArgumentElement.PerformOn(number, secondNumberElement);
                                currentRunElements.Remove(currentNode.Next);
                                currentRunElements.Remove(currentNode.Next);
                                break;
                        }

                        currentRunElements.Remove(currentNode.Next);
                        break;
                }

                currentNode = currentNode.Next;
            }
        }

        private void DoLog()
        {
            switch (currentNode.Next.Value)
            {
                case Number logBase:
                    if (logBase <= 0)
                        throw new ArgumentException(CalculatorExceptionMessages.LogToNegativeOrZeroBeforeParameter +
                                                    logBase +
                                                    CalculatorExceptionMessages.LogToNegativeOrZeroAfterParameter);

                    Number logNumber = (Number) currentNode.Next.Next.Next.Value;
                    if (logNumber <= 0)
                        throw new ArgumentException(
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
                        throw new ArgumentException(
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