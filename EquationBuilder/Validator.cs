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
        ///     Validates the order of elements, expands Constants, adds implied multiplication operators and replaces E as Euler's
        ///     Number with a Number. Returns the expanded list. Will throw exceptions if the order of elements is valid.
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
            if (elementsList?.First is null)
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
                throw new ArgumentException(
                    BuilderExceptionMessages.InvalidCharacterBeforeArgumentSeparatorBeforeParameter + previous +
                    BuilderExceptionMessages.InvalidCharacterBeforeArgumentSeparatorAfterParameter);

            if (next is IInvalidWhenFirst)
                throw new ArgumentException(
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

        private void HandleE(E e)
        {
            switch (previous)
            {
                //Potentially from one-Element Constant. Multiple-Element Constants will jump to ClosingBracket.
                case Number _base:
                    //NumberENumber? or NumberE±Number? or NumberEAbsNumber?.Combine into a single number if ? is not E.
                    switch (next)
                    {
                        case Number _ when nextNode.Next?.Value is E:
                            throw new Exception(BuilderExceptionMessages.UndeterminedUseOfEDefault);
                        case Number exp:
                            Combine(exp, false);
                            return;

                        case AdditionOperator _ when nextNode.Next?.Value is Number exp:
                            if (nextNode.Next.Next?.Value is E)
                                throw new Exception(ElementsExceptionMessages.ExponentIsNotIntegerBeforeParameter + e +
                                                    ElementsExceptionMessages.ExponentIsNotIntegerAfterParameter);
                            else
                                Combine(exp, true);
                            return;

                        case SubtractionOperator _
                            when nextNode.Next?.Value is Number exp:
                            if (nextNode.Next.Next?.Value is E)
                                throw new Exception(ElementsExceptionMessages.ExponentIsNotIntegerBeforeParameter + e +
                                                    ElementsExceptionMessages.ExponentIsNotIntegerAfterParameter);
                            else
                                Combine(-exp, true);
                            return;

                        case AbsoluteFunction _
                            when nextNode.Next?.Value is Number exp:
                            if (nextNode.Next.Next?.Value is E)
                                throw new Exception(ElementsExceptionMessages.ExponentIsNotIntegerBeforeParameter + e +
                                                    ElementsExceptionMessages.ExponentIsNotIntegerAfterParameter);
                            else
                            {
                                int absExp = Math.Abs(E.TestPower(exp));
                                Combine(new Number(absExp), true);
                                return;
                            }
                    }

                    break; //Else depends on next.

                    void Combine(Number exponent, bool operatorBetween)
                    {
                        E.TestPower(exponent);
                        previousNode.Value = new Number(_base + e.ToString() + exponent);
                        elements.Remove(previousNode.Next); //Old current, E.
                        if (operatorBetween)
                            elements.Remove(previousNode.Next); //Old next, operator.
                        elements.Remove(previousNode.Next); //Old next (or next next), Number exponent.
                        currentNode = previousNode;
                        current = currentNode?.Value;
                        nextNode = currentNode?.Next;
                        next = nextNode?.Value;
                        previousNode = currentNode.Previous;
                        previous = previousNode?.Value;
                        ValidateNumber();
                    }

                //Beginning of section.
                case null:
                case ArgumentSeparatorOperator _:
                case OpeningBracket _:
                case IFunction _: //sinE or roundE must be Eulers.
                case IOperatorExcludingBrackets _: //-E must be Eulers.
                    CurrentNodeIsEulersNumber();
                    return;

                //EE. Eulers cannot be an exponent because it is not an integer, so previous cannot be *10^.
                case E _:
                    Retract();
                    CurrentNodeIsEulersNumber();
                    return;

                //Shouldn't be *10^ because missed the point. If Eulers, be explicit.
                case Factorial _:
                //Could be a Constant made of multiple Elements, see Factorial.
                //Constants made of a single Element are valid (caught by case Number because no brackets are added around a Constant made of one Element).
                case ClosingBracket _:
                    throw new Exception(BuilderExceptionMessages.UndeterminedUseOfEDefault);

                case Word _: //Depends on next.
                    break;

                default:
                    return;
            }

            switch (next)
            {
                case Number number:
                    E.TestPower(number);
                    return;

                case AdditionOperator _:
                case SubtractionOperator _:
                    {
                        switch (nextNode.Next?.Value)
                        {
                            case Number number:
                                E.TestPower(number);
                                return;

                            case AbsoluteFunction _:
                                switch (nextNode.Next.Next?.Value)
                                {
                                    case Number exp1:
                                        try
                                        {
                                            E.TestPower(exp1);
                                            //Leave as E because Exponent E but previous was not number.
                                        }
                                        catch
                                        {
                                            CurrentNodeIsEulersNumber();
                                        }

                                        break;
                                    case Constant _:
                                        ExpandConstant(nextNode.Next.Next);
                                        switch (nextNode.Next.Next.Value)
                                        {
                                            case Number n:
                                                E.TestPower(n);
                                                break;
                                            case OpeningBracket _:
                                                CurrentNodeIsEulersNumber();
                                                break;
                                        }
                                        return;

                                    case Word _:
                                        break;
                                    default:
                                        CurrentNodeIsEulersNumber();
                                        break;
                                }

                                return;

                            //+ and - count as Operators.
                            case OpeningBracket _:
                            case IFunction _:
                            case IOperator _:
                                CurrentNodeIsEulersNumber();
                                return;

                            case Constant _:
                                ExpandConstant(nextNode.Next);
                                switch (nextNode.Next.Value)
                                {
                                    case Number n:
                                        E.TestPower(n);
                                        break;
                                    case OpeningBracket _:
                                        CurrentNodeIsEulersNumber();
                                        break;
                                }
                                return;

                            case Word _:
                                return;

                            case E _:
                                throw new ArgumentException(ElementsExceptionMessages.ExponentIsNotIntegerBeforeParameter +
                                                            nextNode.Next.Value + ElementsExceptionMessages
                                                                .ExponentIsNotIntegerAfterParameter);
                        }

                        return;
                    }

                case Constant _:
                    ExpandConstant(nextNode);
                    switch (nextNode.Value)
                    {
                        case Number n:
                            E.TestPower(n);
                            break;
                        case OpeningBracket _:
                            //Shouldn't be *10^ because missed the point. If Eulers, be explicit.
                            throw new Exception(BuilderExceptionMessages.UndeterminedUseOfEDefault);
                    }

                    return;

                case Word _:
                    return;

                case AbsoluteFunction _:
                    switch (nextNode.Next?.Value)
                    {
                        case Number exp2:
                            try
                            {
                                E.TestPower(exp2);
                                //Leave as E because Exponent E but previous was not number.
                            }
                            catch
                            {
                                CurrentNodeIsEulersNumber();
                            }

                            break;

                        case Constant _:
                            ExpandConstant(nextNode.Next);
                            switch (nextNode.Next.Value)
                            {
                                case Number n:
                                    E.TestPower(n);
                                    break;
                                case OpeningBracket _:
                                    //Shouldn't be *10^ because missed the point. If Eulers, be explicit.
                                    throw new Exception(BuilderExceptionMessages.UndeterminedUseOfEDefault);
                            }

                            return;

                        case Word _:
                            return;
                        default:
                            CurrentNodeIsEulersNumber();
                            break;
                    }

                    return;

                //End of section.
                case null:
                case ArgumentSeparatorOperator _:
                case ClosingBracket _:
                //EE. Eulers cannot be an exponent because it is not an integer, so current cannot be *10^.
                case E _:
                //Cannot be exponent E unless Addition or Subtraction (handled above).
                case IOperatorExcludingBrackets _:
                    CurrentNodeIsEulersNumber();
                    return;

                //Shouldn't be *10^ because missed the point. If Eulers, be explicit.
                case OpeningBracket _:
                case IFunction _:
                    throw new Exception(BuilderExceptionMessages.UndeterminedUseOfEDefault);

                case Factorial _: //Factorial must be proceeded by an integer, which Eulers is not.
                    throw new Exception(BuilderExceptionMessages.FactorialWasNotAnIntegerBeforeParameter + current +
                                        BuilderExceptionMessages.FactorialWasNotAnIntegerAfterParameter);

                default:
                    throw new Exception(BuilderExceptionMessages.InvalidElementAfterEBeforeParameter + next +
                                        BuilderExceptionMessages.InvalidElementAfterEAfterParameter);
            }
        }

        private void CurrentNodeIsEulersNumber()
        {
            currentNode.Value = new Number(Math.E);
            current = currentNode.Value;
            ValidateNumber();
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
                    throw new Exception(BuilderExceptionMessages.InvalidCharacterBeforeFactorialBeforeParameter +
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
                        currentNode.Value = (Number)current * -1;
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
                    if (next is AdditionOperator && nextNode.Next?.Value is IFunction)
                        elements.Remove(nextNode);
                    else if (next is SubtractionOperator && nextNode.Next?.Value is IFunction)
                        elements.AddBefore(nextNode, new Number(0));
                }

            }
            else if (previous is OpeningBracket && next is IFunction)
            {
                elements.AddBefore(currentNode, new Number(0));
                Retract();
            }


            bool HandlePlusMinusCombination()
            {
                if (previous is SubtractionOperator)
                {
                    currentNode.Value = new AdditionOperator();
                    elements.Remove(previousNode);
                    Retract();
                    return true;
                }

                if (previous is AdditionOperator)
                {
                    elements.Remove(previousNode);
                    Retract();
                    return true;
                }

                if (next is SubtractionOperator)
                {
                    currentNode.Value = new AdditionOperator();
                    elements.Remove(nextNode);
                    Retract();
                    return true;
                }

                if (next is AdditionOperator)
                {
                    elements.Remove(nextNode);
                    Retract();
                    return true;
                }

                return false;
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
                throw new Exception(
                    BuilderExceptionMessages.UnidentifiableElementBeforeParameter +
                    outerNumbersAndWordsElement + BuilderExceptionMessages.UnidentifiableElementAfterParameter);
            }
        }

        private void ExpandConstant(LinkedListNode<BaseElement> nodeWithConstant)
        {
            string constantValue = ((Constant)nodeWithConstant.Value).Value;
            ICollection<BaseElement> expandedConstant = SplitAndValidate.Run(constantValue, elementBuilder);

            if (expandedConstant.Count == 1)
            {
                nodeWithConstant.Value = expandedConstant.First();
            }
            else
            {
                nodeWithConstant.Value = new ParenthesisOpeningBracket();
                foreach (BaseElement baseElement in expandedConstant)
                {
                    elements.AddAfter(nodeWithConstant, baseElement);
                    nodeWithConstant = nodeWithConstant.Next;
                }

                elements.AddAfter(nodeWithConstant, new ParenthesisClosingBracket());
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
                Type closingBracketType = openingBracketsStack.Pop().GetReverseType();
                elements.AddLast((BaseElement)Activator.CreateInstance(closingBracketType));
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