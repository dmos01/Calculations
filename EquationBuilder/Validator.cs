using System;
using System.Collections.Generic;
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
        Stack<OpeningBracket> openingBracketsStack;
        private ElementBuilder elementBuilder { get; }

        /// <summary>
        ///     Validates the order of elements, expands Constants, adds implied operators and brackets, and replaces E where
        ///     possible. Returns the expanded list. Will throw exceptions if the order of elements is valid.
        /// </summary>
        /// <param name="elementsList"></param>
        /// <param name="castUnrecognizedElementsAsVariables">
        ///     If true, casts any unrecognized elements as Variables. If false,
        ///     Variables and unrecognized elements will throw an exception.
        /// </param>
        /// <returns>The validated and expanded list of elements.</returns>
        public LinkedList<BaseElement> Run(LinkedList<BaseElement> elementsList,
            bool castUnrecognizedElementsAsVariables)
        {
            if (elementsList is null)
                throw new ArgumentNullException(null, BuilderExceptionMessages.NoEquationDefault);

            if (elementsList.First is null)
                throw new ArgumentException(BuilderExceptionMessages.NoEquationDefault);

            elements = elementsList;
            this.castUnrecognizedElementsAsVariables = castUnrecognizedElementsAsVariables;

            FirstAndLast();

            insideTwoArgFunction = false;
            openingBracketsStack = new Stack<OpeningBracket>();
            currentNode = elements.First;
            previousNode = null;
            nextNode = elements.First.Next;
            current = currentNode.Value;
            previous = null;
            next = nextNode?.Value;

            Loop();
            previousNode = null;
            nextNode = null;
            previous = null;
            current = null;
            next = null;

            if (openingBracketsStack.Any())
            {
                CloseUnclosedBracketsAndValidateOrder();

                //throw new Exception(BuilderExceptionMessages.InvalidBracketUseDefault);
            }

            currentNode = null;
            openingBracketsStack = null;
            return elements;
        }

        private void FirstAndLast()
        {
            LinkedListNode<BaseElement> last = elements.Last;
            if (last.Value is IInvalidWhenLast _)
                throw new Exception(BuilderExceptionMessages.LastElementInvalidBeforeParameter +
                                    last.Value +
                                    BuilderExceptionMessages.LastElementInvalidAfterParameter);

            switch (elements.First.Value)
            {
                case AdditionOperator _:
                case SubtractionOperator _:
                    elements.AddFirst(new Number(NumberRepresentations.ZeroSymbol));
                    break;
                case IInvalidWhenFirst _:
                    throw new Exception(BuilderExceptionMessages.FirstElementInvalidBeforeParameter +
                                        elements.First.Value +
                                        BuilderExceptionMessages.FirstElementInvalidAfterParameter);
            }
        }

        private void Loop()
        {
            while (current != null)
            {
                switch (current)
                {
                    case ArgumentSeparatorOperator _:
                        ValidateArgumentSeparator();
                        break;
                    case ClosingBracket closingBracket:
                        ValidateClosingBracket(closingBracket);
                        break;
                    case Constant _:
                        ExpandConstant(currentNode);
                        if (previous != null)
                            Retract();
                        continue; //Don't advance.
                    case DecimalPoint _:
                        throw new Exception(BuilderExceptionMessages.SeparationFailedDefault);
                    case E e:
                        HandleE(e);
                        break;
                    case Factorial _:
                        ValidateFactorial();
                        break;

                    //Single-number log base which is <= 0.
                    case LogFunction _ when nextNode?.Next.Value is Number logBase &&
                                            nextNode?.Next?.Next?.Value is ArgumentSeparatorOperator && logBase <= 0:
                        throw new Exception(BuilderExceptionMessages.LogToNegativeOrZeroBeforeParameter + logBase +
                                            BuilderExceptionMessages.LogToNegativeOrZeroAfterParameter);

                    case TwoArgumentFunction _:
                        ValidateTwoArgFunction();
                        break;
                    case OneArgumentFunction _:
                        ValidateOneArgFunction();
                        break;

                    case SubtractionOperator _ when next is AdditionOperator _:
                        ValidateSubtractionOperator();
                        break;
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
                    case SubtractionOperator _:
                        ValidateSubtractionOperator();
                        break;

                    case Number _:
                        ValidateNumber();
                        break;
                    case OpeningBracket openingBracket:
                        openingBracketsStack.Push(openingBracket);
                        break;
                    case Variable _:
                        if (!castUnrecognizedElementsAsVariables)
                            throw new Exception(BuilderExceptionMessages.UnidentifiableElementDefault);
                        break;
                    case UnrecognizedElement unrecognized:
                        ValidateUnrecognized(unrecognized.OuterNumbersAndWordsElement);
                        break;
                }

                Advance();
            }
        }

        private void ValidateArgumentSeparator()
        {
            if (previous is IInvalidWhenLast)
                throw new Exception(
                    BuilderExceptionMessages.InvalidCharacterBeforeArgumentSeparatorBeforeParameter + previous +
                    BuilderExceptionMessages.InvalidCharacterBeforeArgumentSeparatorAfterParameter);

            if (next is IInvalidWhenFirst)
                throw new Exception(
                    BuilderExceptionMessages.InvalidCharacterAfterArgumentSeparatorBeforeParameter + next +
                    BuilderExceptionMessages.InvalidCharacterAfterArgumentSeparatorAfterParameter);

            if (insideTwoArgFunction == false)
                throw new Exception(BuilderExceptionMessages.ArgumentSeperatorNotInsideFunctionDefault);
        }

        private void ValidateClosingBracket(ClosingBracket closingBracket)
        {
            if (openingBracketsStack.Any() == false)
                throw new Exception(BuilderExceptionMessages.InvalidBracketUseDefault);

            if (closingBracket.IsReverseOf(openingBracketsStack.Pop()) == false)
                throw new Exception(BuilderExceptionMessages.InvalidBracketUseDefault);

            if (previous is IInvalidWhenLast)
                throw new Exception(BuilderExceptionMessages.InvalidCharacterBeforeClosingBracketBeforeParameter +
                                    previous + BuilderExceptionMessages
                                        .InvalidCharacterBeforeClosingBracketAfterParameter);

            switch (next)
            {
                case IFunction _:
                case OpeningBracket _:
                case Number _:
                case Word _:
                    AddTimesAfter(currentNode);
                    break;
            }

            insideTwoArgFunction = false;
        }

        private void ValidateFactorial()
        {
            switch (previous)
            {
                //Constant and Variable validity untested.

                case E _:
                case Number number when number < 1 || number % 1 != 0:
                    throw new Exception(BuilderExceptionMessages.FactorialWasNotAnIntegerBeforeParameter + previous +
                                        BuilderExceptionMessages.FactorialWasNotAnIntegerAfterParameter);

                case ClosingBracket _:
                case Number _:
                case Word _:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(null,
                        BuilderExceptionMessages.InvalidCharacterBeforeFactorialBeforeParameter +
                        previous +
                        BuilderExceptionMessages.InvalidCharacterBeforeFactorialAfterParameter);
            }

            switch (next)
            {
                case null:
                case IOperatorExcludingBrackets _:
                case ClosingBracket _:
                    break;

                case OpeningBracket _:
                case Number _:
                case Word _:
                    AddTimesAfter(currentNode);
                    break;

                default:
                    throw new Exception(BuilderExceptionMessages.InvalidCharacterAfterFactorialBeforeParameter + next +
                                        BuilderExceptionMessages.InvalidCharacterAfterFactorialAfterParameter);
            }
        }

        public void ValidateTwoArgFunction()
        {
            if ((next is OpeningBracket) == false)
                throw new Exception(
                    BuilderExceptionMessages.TwoArgFunctionNotFollowedByOpeningBracketBeforeParameter +
                    current + BuilderExceptionMessages
                        .TwoArgFunctionNotFollowedByOpeningBracketAfterParameter);

            insideTwoArgFunction = true;
            LinkedListNode<BaseElement> _currentNode = nextNode;
            while (!(_currentNode.Value is ArgumentSeparatorOperator))
            {
                _currentNode = _currentNode.Next;
                if (_currentNode is null)
                    throw new Exception(BuilderExceptionMessages.NoArgumentSeparatorDefault);
            }
        }

        private void ValidateOneArgFunction()
        {
            switch (next)
            {
                case E _:
                    nextNode.Value = new Number(Math.E);
                    next = nextNode.Value;
                    break;
                case Number _:
                case OpeningBracket _:
                case Word _:
                    break;

                case SubtractionOperator _:
                    switch (nextNode.Next?.Value)
                    {
                        case Number number:
                            nextNode.Value = number * -1;
                            elements.Remove(nextNode.Next);
                            break;

                        case OpeningBracket _: //sin-( where current is sin.

                            //Start at opening bracket node + 1.
                            LinkedListNode<BaseElement> _currentNode = nextNode.Next.Next;
                            elements.AddAfter(currentNode, SplitAndValidate.CreateImpliedOpeningBracket());
                            nextNode = currentNode.Next;
                            next = nextNode.Value;

                            //In case further brackets are found, inside the ones to be closed. E.g. sin-((1)+2).
                            int openingBracketsFound = 0;
                            while (!(_currentNode is null))
                            {
                                switch (_currentNode.Value)
                                {
                                    //Add implied closing bracket after the actual closing bracket of the original opening bracket.
                                    case ClosingBracket _ when openingBracketsFound == 0:
                                        elements.AddAfter(_currentNode, SplitAndValidate.CreateImpliedClosingBracket());
                                        return;
                                    case ClosingBracket _:
                                        openingBracketsFound--;
                                        break;
                                    case OpeningBracket _:
                                        openingBracketsFound++;
                                        break;
                                }

                                _currentNode = _currentNode.Next;
                            }

                            //Sin-(1 or sin-(6}
                            throw new Exception(BuilderExceptionMessages.InvalidBracketUseDefault);

                        case Word _:
                            break;
                        default:
                            //Next next cannot be null because - cannot be last.
                            throw new Exception(
                                BuilderExceptionMessages.OneArgFunctionNotFollowedByValidElementBeforeParameters +
                                current +
                                BuilderExceptionMessages.OneArgFunctionNotFollowedByValidElementBetweenParameters +
                                next + nextNode.Next.Value + BuilderExceptionMessages
                                    .OneArgFunctionNotFollowedByValidElementAfterParameters);
                    }

                    break;
                default:
                    throw new Exception(
                        BuilderExceptionMessages.OneArgFunctionNotFollowedByValidElementBeforeParameters + current +
                        BuilderExceptionMessages.OneArgFunctionNotFollowedByValidElementBetweenParameters + next +
                        BuilderExceptionMessages.OneArgFunctionNotFollowedByValidElementAfterParameters);
            }
        }

        private void ValidateNumber()
        {
            switch (previous)
            {
                case SubtractionOperator _:
                    //This will not be null because FirstAndLast() adds a "0" in front of a starting -.
                    //!IMayPrecedeNegativeNumber handled in ValidateSubtractionOperator().
                    if (previousNode.Previous.Value is IMayPrecedeNegativeNumber)
                    {
                        currentNode.Value = (Number) current * -1;
                        elements.Remove(previousNode);
                    }

                    break;

                case ClosingBracket _:
                case Number _:
                case Factorial _:
                case Word _:
                    elements.AddBefore(currentNode, new MultiplicationOperator());
                    break;

                case null:
                case E _:
                case IFunction _:
                case IOperatorOrOpeningBracket _:
                    break;

                default:
                    throw new Exception(BuilderExceptionMessages.UnidentifiableElementDefault);
            }

            switch (next)
            {
                case null:
                case IOperatorExcludingBrackets _:
                case ClosingBracket _:
                case E _:
                case Factorial _:
                    break;

                case IFunction _:
                case Number _:
                case OpeningBracket _:
                case Word _:
                    AddTimesAfter(currentNode);
                    break;

                default:
                    throw new Exception(BuilderExceptionMessages.UnidentifiableElementDefault);
            }
        }

        private void ValidateSubtractionOperator()
        {
            //Combining SubtractionOperators into Numbers is handled in ValidateNumber() so it can run when CurrentNodeIsEulersNumber().
            if (previous is IInvalidBeforeMinus)
                throw new Exception(BuilderExceptionMessages.InvalidOperatorOrderBeforeParameters + previous +
                                    BuilderExceptionMessages.InvalidOperatorOrderBetweenParameters + current +
                                    BuilderExceptionMessages.InvalidOperatorOrderAfterParameters);

            if (HandlePlusMinusCombination())
            {
                if (current is OpeningBracket)
                {
                    switch (next)
                    {
                        case AdditionOperator _ when nextNode.Next?.Value is IFunction:
                            elements.Remove(nextNode);
                            break;
                        case SubtractionOperator _ when nextNode.Next?.Value is IFunction:
                            elements.AddBefore(nextNode, new Number(0));
                            break;
                    }
                }
            }
            else if (previous is OpeningBracket && next is IFunction)
            {
                elements.AddBefore(currentNode, new Number(0));
                Retract();
            }


            bool HandlePlusMinusCombination()
            {
                switch (previous)
                {
                    case SubtractionOperator _:
                        currentNode.Value = new AdditionOperator();
                        elements.Remove(previousNode);
                        Retract();
                        return true;
                    case AdditionOperator _:
                        elements.Remove(previousNode);
                        Retract();
                        return true;
                }

                switch (next)
                {
                    case SubtractionOperator _:
                        currentNode.Value = new AdditionOperator();
                        elements.Remove(nextNode);
                        Retract();
                        return true;
                    case AdditionOperator _:
                        elements.Remove(nextNode);
                        Retract();
                        return true;
                    default:
                        return false;
                }
            }
        }

        private void ValidateUnrecognized(string outerNumbersAndWordsElement)
        {
            if (castUnrecognizedElementsAsVariables)
            {
                //Name validation should have happened when creating the UnrecognizedElement.
                currentNode.Value = new Variable(current.ToString());
                current = currentNode.Value;
            }
            else
            {
                if (IsNullEmptyOrOnlySpaces(outerNumbersAndWordsElement))
                    throw new Exception(BuilderExceptionMessages.UnidentifiableElementDefault);
                else
                    throw new Exception(
                        BuilderExceptionMessages.UnidentifiableElementBeforeParameter +
                        outerNumbersAndWordsElement + BuilderExceptionMessages.UnidentifiableElementAfterParameter);
            }
        }

        private void ExpandConstant(LinkedListNode<BaseElement> nodeWithConstant)
        {
            string constantValue = ((Constant) nodeWithConstant.Value).Value;
            ICollection<BaseElement> expandedConstant = SplitAndValidate.Run(constantValue, elementBuilder);

            switch (expandedConstant.Count)
            {
                case 0:
                    return;
                case 1:
                    nodeWithConstant.Value = expandedConstant.First();
                    return;
            }

            //- Number or 0 - Number on their own.
            using (IEnumerator<BaseElement> iterator = expandedConstant.GetEnumerator())
            {
                iterator.MoveNext();
                if (iterator.Current is Number first)
                {
                    if (first == 0)
                        iterator.MoveNext();
                    else
                    {
                        AddElementsBetweenBrackets();
                        return;
                    }
                }

                if (iterator.Current is SubtractionOperator && iterator.MoveNext() &&
                    iterator.Current is Number num && !iterator.MoveNext())
                    nodeWithConstant.Value = num * -1;
                else
                    AddElementsBetweenBrackets();
            }

            void AddElementsBetweenBrackets()
            {
                nodeWithConstant.Value = SplitAndValidate.CreateImpliedOpeningBracket();
                foreach (BaseElement baseElement in expandedConstant)
                {
                    elements.AddAfter(nodeWithConstant, baseElement);
                    nodeWithConstant = nodeWithConstant.Next;
                }

                elements.AddAfter(nodeWithConstant, SplitAndValidate.CreateImpliedClosingBracket());
            }
        }

        private void AddTimesAfter(LinkedListNode<BaseElement> addAfter) =>
            elements.AddAfter(addAfter, new MultiplicationOperator());

        /// <summary>
        ///     Will fail if there is no previous.
        /// </summary>
        private void Retract()
        {
            nextNode = currentNode;
            currentNode = currentNode?.Previous;
            previousNode = currentNode?.Previous;

            previous = previousNode?.Value;
            current = currentNode?.Value;
            next = nextNode?.Value;
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

        private void CloseUnclosedBracketsAndValidateOrder()
        {
            //Give all remaining opening brackets equivalent closing brackets at the end of the equation.
            while (openingBracketsStack.Any())
            {
                elements.AddLast(openingBracketsStack.Pop().GetNewObjectOfReverseType());
            }

            //Validate bracket order.
            currentNode = elements.First;
            while (currentNode != null)
            {
                switch (currentNode.Value)
                {
                    case OpeningBracket openingBracket:
                        openingBracketsStack.Push(openingBracket);
                        break;
                    case ClosingBracket _ when openingBracketsStack.Any() == false:
                    case ClosingBracket closingBracket when !closingBracket.IsReverseOf(openingBracketsStack.Pop()):
                        throw new Exception(BuilderExceptionMessages.InvalidBracketUseDefault);
                }

                currentNode = currentNode.Next;
            }
        }
    }
}