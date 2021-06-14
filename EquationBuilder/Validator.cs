using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EquationElements;
using EquationElements.Functions;
using EquationElements.Operators;
using static EquationElements.Utils;

namespace EquationBuilder
{
    public partial class Validator
    {
        bool castUnrecognizedElementsAsVariables;
        BaseElement current, previous, next;

        LinkedListNode<BaseElement> currentNode, previousNode, nextNode;
        LinkedList<BaseElement> elements;
        bool insideTwoArgFunction;
        Stack<LinkedListNode<BaseElement>> openingBracketsStack;
        private ElementBuilder elementBuilder { get; }

        /// <summary>
        ///     Validates the order of elements, adds implied multiplication operators and replaces E as Euler's Number with a
        ///     Number. Returns
        ///     the updated list. Will throw exceptions if the order of elements is valid.
        /// </summary>
        /// <param name="elementsList"></param>
        /// <param name="castUnrecognizedElementsAsVariables">
        ///     If true, casts any unrecognized elements as Variables. If false,
        ///     Variables and unrecognized elements will throw an exception.
        /// </param>
        /// <returns>The validated and updated list of elements.</returns>
        public LinkedList<BaseElement> Run(LinkedList<BaseElement> elementsList,
            bool castUnrecognizedElementsAsVariables)
        {
            ThrowExceptionIfNullOrEmpty(elementsList, nameof(elementsList));

            elements = elementsList;
            this.castUnrecognizedElementsAsVariables = castUnrecognizedElementsAsVariables;

            FirstAndLast();

            insideTwoArgFunction = false;
            openingBracketsStack = new Stack<LinkedListNode<BaseElement>>();

            currentNode = elements.First;
            previousNode = null;
            nextNode = elements.First.Next;

            current = currentNode.Value;
            previous = null;
            next = nextNode?.Value;

            Loop();

            if (openingBracketsStack.Any())
                throw new Exception(BuilderExceptionMessages.NotAllOpeningBracketsClosedDefault);

            currentNode = null;
            previousNode = null;
            nextNode = null;
            current = null;
            previous = null;
            next = null;
            openingBracketsStack = null;
            return elements;
        }

        private void FirstAndLast()
        {
            LinkedListNode<BaseElement> firstNode = elements.First;
            if (firstNode is null)
                throw new Exception(BuilderExceptionMessages.FirstElementInvalidBeforeParameter + elements.First.Value +
                                    BuilderExceptionMessages.FirstElementInvalidAfterParameter);

            switch (elements.Last.Value)
            {
                case IInvalidWhenLast _:
                    throw new Exception(BuilderExceptionMessages.LastElementInvalidBeforeParameter +
                                        elements.Last.Value +
                                        BuilderExceptionMessages.LastElementInvalidAfterParameter);
                case E _:
                    currentNode = elements.Last;
                    CurrentNodeIsEulersNumber();
                    break;
            }

            BaseElement first = firstNode.Value;

            switch (first)
            {
                case AdditionOperator _:
                case SubtractionOperator _:
                    elements.AddFirst(new Number(NumberRepresentations.ZeroSymbol));
                    break;
                case E _:
                    currentNode = firstNode;
                    CurrentNodeIsEulersNumber();
                    break;
            }

            if (first is IInvalidWhenFirst)
                throw new Exception(BuilderExceptionMessages.FirstElementInvalidBeforeParameter + elements.First.Value +
                                    BuilderExceptionMessages.FirstElementInvalidAfterParameter);
        }

        private void Loop()
        {
            while (current != null)
            {
                switch (current)
                {
                    case DecimalPoint _:
                        throw new Exception(BuilderExceptionMessages.SeparationFailedDefault);
                    case ArgumentSeparatorOperator _:
                        ValidateArgumentSeperator();
                        break;
                    case ClosingBracket _:
                        ValidateClosingBracket();
                        break;
                    case Constant constant:
                        ExpandConstant(constant);
                        continue; //Don't advance.
                    case E _:
                        HandleE();
                        break;
                    case FactorialFunction _:
                        ValidateFactorial();
                        break;
                    case IFunction _:
                        ValidateFunction();
                        break;
                    case Number _:
                        ValidateNumber();
                        break;
                    case Variable _:
                        if (castUnrecognizedElementsAsVariables == false)
                            throw new Exception(BuilderExceptionMessages.UnidentifiableElementDefault);
                        break;
                    case UnrecognizedElement unrecognized:
                        if (castUnrecognizedElementsAsVariables)
                        {
                            currentNode.Value = new Variable(current.ToString());
                            current = currentNode.Value;
                        }
                        else
                        {
                            if (IsNullEmptyOrOnlySpaces(unrecognized.OuterNumbersAndWordsElement))
                                throw new Exception(BuilderExceptionMessages.UnidentifiableElementDefault);
                            throw new Exception(
                                BuilderExceptionMessages.UnidentifiableElementBeforeParameter +
                                unrecognized.OuterNumbersAndWordsElement +
                                BuilderExceptionMessages.UnidentifiableElementAfterParameter);
                        }

                        break;
                    default:
                        ValidateOperatorsAndOpeningBrackets();
                        break;
                }

                Advance();
            }
        }

        private void Advance()
        {
            previousNode = currentNode; //Previous starts as null, so previous.Next() would not work.
            currentNode = currentNode?.Next;
            nextNode = currentNode?.Next;
            previous = previousNode?.Value;
            current = currentNode?.Value;
            next = nextNode?.Value;
        }

        private void ValidateArgumentSeperator()
        {
            switch (previous)
            {
                case IOperatorExcludingBrackets _:
                case OpeningBracket _:
                case IFunction _:
                    throw new ArgumentException(
                        BuilderExceptionMessages.InvalidCharacterBeforeArgumentSeparatorBeforeParameter + previous +
                        BuilderExceptionMessages.InvalidCharacterBeforeArgumentSeparatorAfterParameter);
            }

            if (insideTwoArgFunction == false)
                throw new Exception(BuilderExceptionMessages.ArgumentSeperatorNotInsideFunctionDefault);
        }

        private void ValidateClosingBracket()
        {
            if (openingBracketsStack.Any() == false)
                throw new Exception(BuilderExceptionMessages.FoundClosingBracketWithoutOpeningBracket);

            LinkedListNode<BaseElement> mostRecentNodeWithOpeningBracket = openingBracketsStack.Pop();

            if (((OpeningBracket) mostRecentNodeWithOpeningBracket.Value).IsReverseOf((ClosingBracket) current) ==
                false)
                throw new Exception(
                    BuilderExceptionMessages.FoundClosingBracketWithoutOpeningBracket);

            if ((next is IFunction && !(next is FactorialFunction))
                || next is OpeningBracket)
                AddTimesAfter(currentNode);

            insideTwoArgFunction = false;
        }

        private void ExpandConstant(Constant constant)
        {
            ICollection<BaseElement> expandedConstant =
                SplitAndValidate.Run(constant.Value, elementBuilder);

            //Keep currentNode representing the start of the Constant as more nodes are added.
            LinkedListNode<BaseElement> temp = currentNode;
            temp.Value = new ParenthesisOpeningBracket();
            foreach (BaseElement baseElement in expandedConstant)
            {
                elements.AddAfter(temp, baseElement);
                temp = temp.Next;
            }

            elements.AddAfter(temp, new ParenthesisClosingBracket());

            //Move back a node (to before the opening bracket), if possible.
            if (currentNode.Previous != null)
                currentNode = currentNode.Previous;
            current = currentNode.Value;
            previousNode = currentNode.Previous;
            previous = previousNode?.Value;
            nextNode = currentNode.Next;
            next = nextNode?.Value;
        }

        private void HandleE()
        {
            switch (next)
            {
                case DecimalPoint _:
                    throw new Exception(BuilderExceptionMessages.SeparationFailedDefault);

                case null: //No next, this is the last element.
                case ClosingBracket _:
                case E _:
                    CurrentNodeIsEulersNumber();
                    return;

                case FactorialFunction _:
                    throw new Exception(BuilderExceptionMessages.InvalidCharacterBeforeFactorialBeforeParameter +
                                        current +
                                        BuilderExceptionMessages.InvalidCharacterBeforeFactorialAfterParameter);

                case Constant _:
                case OpeningBracket _:
                case IFunction _:
                    throw new Exception(BuilderExceptionMessages.UndeterminedUseOfEDefault);

                case ArgumentSeparatorOperator _:
                case IOperatorExcludingBrackets _:
                case Number _:
                case UnrecognizedElement _:
                case Variable _:
                    break;

                default:
                    throw new Exception(BuilderExceptionMessages.InvalidElementAfterEBeforeParameter + next +
                                        BuilderExceptionMessages.InvalidElementAfterEAfterParameter);
            }

            switch (previous)
            {
                case Constant _:
                case ClosingBracket _:
                case FactorialFunction _:
                    throw new Exception(BuilderExceptionMessages.UndeterminedUseOfEDefault);

                case IFunction _:
                    throw new Exception(BuilderExceptionMessages.FunctionNotFollowedByOpeningBracket);

                case E _:
                    PreviousNodeIsEulersNumber();
                    break;

                case Number _:
                case Variable _:
                case UnrecognizedElement _:
                    ElementBeforeEWasNumberVariableOrUnrecognized();
                    break;

                case OpeningBracket _ when previousNode.Previous?.Value is LogFunction:
                {
                    if (!(next is ArgumentSeparatorOperator))
                        throw new Exception(BuilderExceptionMessages.EIsNotAloneAsLogBaseDefault);
                    break;
                }

                case IOperatorExcludingBrackets _:
                case OpeningBracket _:
                    CurrentNodeIsEulersNumber();
                    break;
            }
        }

        /// <summary>
        ///     In some cases here, E refers to *10^. E Elements are kept in those cases.
        /// </summary>
        private void ElementBeforeEWasNumberVariableOrUnrecognized()
        {
            switch (next)
            {
                case AdditionOperator _:
                case SubtractionOperator _:
                    TestExponent(nextNode.Next);
                    break;

                case Variable _:
                case UnrecognizedElement _:
                    break;

                case Number _:
                    TestExponent(nextNode);
                    break;

                case null:
                case IOperatorExcludingBrackets _:
                    CurrentNodeIsEulersNumber();
                    break;

                case E _:
                    throw new ArgumentException(BuilderExceptionMessages.ExponentIsNotIntegerBeforeParameter + next +
                                                BuilderExceptionMessages.ExponentIsNotIntegerAfterParameter);

                default:
                    throw new Exception(BuilderExceptionMessages.UndeterminedUseOfEDefault);
            }
        }

        private static void TestExponent(LinkedListNode<BaseElement> nodeOfExponent)
        {
            if (int.TryParse(nodeOfExponent.Value.ToString(), out int asInteger) == false)
                throw new ArgumentException(BuilderExceptionMessages.ExponentIsNotIntegerBeforeParameter +
                                            nodeOfExponent.Value +
                                            BuilderExceptionMessages.ExponentIsNotIntegerAfterParameter);

            if (Math.Abs(asInteger) > E.MaxAbsoluteE)
                throw new ArgumentException(BuilderExceptionMessages.ExponentTooLargeOrSmallBeforeParameter +
                                            nodeOfExponent.Value +
                                            BuilderExceptionMessages.ExponentTooLargeOrSmallAfterParameter);
        }

        private void PreviousNodeIsEulersNumber()
        {
            SetCurrentToPrevious();
            CurrentNodeIsEulersNumber();
        }

        private void CurrentNodeIsEulersNumber()
        {
            currentNode.Value = new Number(Math.E);
            current = currentNode.Value;
            ValidateNumber();
        }

        private void ValidateFactorial()
        {
            if (!(previous is ClosingBracket || previous is Number))
                throw new Exception(BuilderExceptionMessages.InvalidCharacterBeforeFactorialBeforeParameter + previous +
                                    BuilderExceptionMessages.InvalidCharacterBeforeFactorialAfterParameter);

            switch (next)
            {
                case null:
                case IOperatorExcludingBrackets _:
                case ClosingBracket _:
                    break;
                case Constant _:
                case OpeningBracket _:
                case Number _:
                    AddTimesAfter(currentNode);
                    break;
                default:
                    throw new Exception(BuilderExceptionMessages.InvalidCharacterAfterFactorialBeforeParameter + next +
                                        BuilderExceptionMessages.InvalidCharacterAfterFactorialAfterParameter);
            }
        }

        private void ValidateFunction()
        {
            if (!(next is OpeningBracket))
                throw new Exception(BuilderExceptionMessages.FunctionNotFollowedByOpeningBracket);

            if (!(current is TwoArgumentElement))
                return;

            insideTwoArgFunction = true;
            LinkedListNode<BaseElement> temp = nextNode;
            while (!(temp.Value is ArgumentSeparatorOperator))
            {
                temp = temp.Next;
                if (temp is null)
                    throw new Exception(BuilderExceptionMessages.NoArgumentSeparatorDefault);
            }
        }

        private void ValidateNumber()
        {
            if (!(current is Number number))
                return;

            switch (previous)
            {
                case SubtractionOperator _:
                    //This will not be null because FirstAndLast() adds a "0" in front of a starting -.
                    if (previousNode.Previous.Value is IMayPrecedeNegativeNumber)
                    {
                        currentNode.Value = number * -1;
                        elements.Remove(previousNode);
                    }

                    break;

                case null:
                case E _:
                case IOperatorExcludingBrackets _:
                case OpeningBracket _:
                    break;

                case Constant _:
                case ClosingBracket _:
                case FactorialFunction _:
                case Number _:
                case Variable _:
                case UnrecognizedElement _:
                    elements.AddBefore(currentNode, new MultiplicationOperator());
                    break;

                case IFunction _:
                    throw new Exception(BuilderExceptionMessages.FunctionNotFollowedByOpeningBracket);

                default:
                    throw new Exception(BuilderExceptionMessages.UnidentifiableElementDefault);
            }

            switch (next)
            {
                case null:
                case IOperatorExcludingBrackets _:
                case ClosingBracket _:
                case E _:
                case FactorialFunction _:
                    break;

                case IFunction _:
                case Number _:
                case Constant _:
                case OpeningBracket _:
                case Variable _:
                case UnrecognizedElement _:
                    AddTimesAfter(currentNode);
                    break;

                default:
                    throw new Exception(BuilderExceptionMessages.UnidentifiableElementDefault);
            }
        }

        private void ValidateOperatorsAndOpeningBrackets()
        {
            switch (current)
            {
                case IOperatorOrOpeningBracket _ when next is IInvalidAfterOperator:
                    throw new Exception(
                        BuilderExceptionMessages.InvalidOperatorOrderBeforeParameters + current +
                        BuilderExceptionMessages.InvalidOperatorOrderBetweenParameters + next +
                        BuilderExceptionMessages.InvalidOperatorOrderAfterParameters);
                case IOperatorExcludingBrackets _ when current == next:
                    throw new Exception(
                        BuilderExceptionMessages.InvalidOperatorOrderBeforeParameters + current +
                        BuilderExceptionMessages.InvalidOperatorOrderBetweenParameters + current +
                        BuilderExceptionMessages.InvalidOperatorOrderAfterParameters);

                case OpeningBracket _:
                    openingBracketsStack.Push(currentNode);
                    break;

                case SubtractionOperator _:
                    //Combining SubtractOperators into Numbers handled in ValidateNumber()
                    //so it can run when CurrentNodeIsEulersNumber().
                    if (previous is IInvalidBeforeMinus)
                        throw new Exception(BuilderExceptionMessages.InvalidOperatorOrderBeforeParameters + previous +
                                            BuilderExceptionMessages.InvalidOperatorOrderBetweenParameters + current +
                                            BuilderExceptionMessages.InvalidOperatorOrderAfterParameters);
                    if (FoundPlusMinusCombination())
                        SetCurrentToPrevious();
                    if (previous is OpeningBracket && next is IFunction)
                        elements.AddBefore(currentNode, new Number((decimal) 0));
                    break;
            }
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        [SuppressMessage("ReSharper", "InvertIf")]
        bool FoundPlusMinusCombination()
        {
            if (previous is SubtractionOperator)
            {
                currentNode.Value = new AdditionOperator();
                elements.Remove(previousNode);
                return true;
            }

            if (previous is AdditionOperator)
            {
                elements.Remove(previousNode);
                return true;
            }

            if (next is SubtractionOperator)
            {
                currentNode.Value = new AdditionOperator();
                elements.Remove(nextNode);
                return true;
            }

            if (next is AdditionOperator)
            {
                elements.Remove(nextNode);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Will fail if there is no previous.
        /// </summary>
        private void SetCurrentToPrevious()
        {
            nextNode = currentNode;
            currentNode = currentNode?.Previous;
            previousNode = currentNode?.Previous;

            current = currentNode?.Value;
            previous = previousNode?.Value;
            next = nextNode?.Value;
        }

        private void AddTimesAfter(LinkedListNode<BaseElement> addAfter)
        {
            elements.AddAfter(addAfter, new MultiplicationOperator());
        }
    }
}