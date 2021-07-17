using System;
using System.Collections.Generic;
using System.Dynamic;
using EquationElements;
using EquationElements.Functions;
using EquationElements.Operators;

namespace EquationBuilder
{
    public partial class Validator
    {
        private void HandleE(E e)
        {
            switch (previous)
            {
                //5E? or Single-Number Constant E ?. Multiple-Number Constants will jump to ClosingBracket.
                case Number numberBeforeE:
                    if (HandleEPreviousWasNumber(e, numberBeforeE))
                        return;
                    else
                        break; //Depends on next.

                case null: //null E
                case ArgumentSeparatorOperator _: //,E
                case OpeningBracket _: //(E
                case IFunction _: //sinE or absE.
                case IOperatorExcludingBrackets _: //-E.
                    CurrentNodeIsEulersNumber();
                    return;

                //EE. Eulers cannot be an exponent because it is not an integer, so previous cannot be *10^.
                case E _:
                    Retract();
                    CurrentNodeIsEulersNumber();
                    return;

                //)E or !E. Shouldn't be *10^ because missed the point. If Eulers, be explicit.
                case ClosingBracket _: //Single-Number Constants are valid, caught by case Number.
                case Factorial _:
                    throw new Exception(BuilderExceptionMessages.InvalidUseOfExponentEDefault);

                case Word _: //Word E ?. Depends on next but cannot be combined.
                    break;

                default: //?E. Leave as E.
                    return;
            }

            //Word here refers to any element that cannot be combined.
            switch (next)
            {
                case Number number: //Word E 5
                    E.TestPower(number);
                    return;

                case AdditionOperator _: //Word E ± ?. ± could be part of exponent or its own operator.
                case SubtractionOperator _:
                    HandleENextIsAdditionSubtractionOperators();
                    return;

                case Constant _: //Word E Constant
                    ExpandConstantForE(nextNode, false);
                    return;

                case Word _: //Word E Word
                    return;

                case null: //Word E null
                case ArgumentSeparatorOperator _: //Word E ,
                case ClosingBracket _: //Word E )

                //EE. Eulers cannot be an exponent because it is not an integer, so current cannot be *10^.
                case E _:
                case IOperatorExcludingBrackets _: //Word E * or Word E mod
                    CurrentNodeIsEulersNumber();
                    return;

                //Word E(, Word E sin or Word E abs. Shouldn't be *10^ because missed the point. If Eulers, be explicit.
                case OpeningBracket _:
                case IFunction _:
                    throw new Exception(BuilderExceptionMessages.InvalidUseOfExponentEDefault);

                case Factorial _: //Word E Factorial. Factorial must be proceeded by an integer, which Eulers is not.
                    throw new Exception(BuilderExceptionMessages.FactorialWasNotAnIntegerBeforeParameter + current +
                                        BuilderExceptionMessages.FactorialWasNotAnIntegerAfterParameter);

                default: //Word E ?
                    throw new Exception(BuilderExceptionMessages.InvalidElementAfterEBeforeParameter + next +
                                        BuilderExceptionMessages.InvalidElementAfterEAfterParameter);
            }
        }

        /// <summary>
        /// Number E must be followed by Number, ± (followed by Number...?), single-Element Constant, Absolute or Word. Otherwise missed the point of *10^. If Eulers, be explicit.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="numberBeforeE"></param>
        /// <returns>True if fully handled E.</returns>
        private bool HandleEPreviousWasNumber(E e, Number numberBeforeE)
        {
            switch (next)
            {
                case Number exp: //5E5
                    if (nextNode.Next?.Value is E) //23E23E. Clearly Eulers but be explicit.
                        throw new Exception(BuilderExceptionMessages.InvalidUseOfExponentEDefault);
                    else
                        Combine(exp, false);
                    return true;

                case AdditionOperator _: //5E+. ± could be part of exponent or its own operator.
                    return NextIsAdditionSubtraction(false);

                case SubtractionOperator _: //5E-. ± could be part of exponent or its own operator.
                    return NextIsAdditionSubtraction(true);

                    bool NextIsAdditionSubtraction(bool subtraction)
                    {
                        switch (nextNode.Next.Value)
                        {
                            case Number exp: //5E±5
                                if (nextNode.Next.Next?.Value is E) //5E±5E
                                {
                                    //± are operators so already being explicit.
                                    CurrentNodeIsEulersNumber();
                                    return true;
                                }
                                else //5E±5 ? including null.
                                {
                                    if (subtraction)
                                        exp = -exp;
                                    Combine(exp, true);
                                    return true;
                                }

                            case Constant _: //5E± Constant
                                //± are operators so already being explicit.
                                ExpandConstantForE(nextNode.Next, true);
                                if (nextNode.Next.Value is Number exp2)
                                {
                                    if (subtraction)
                                        exp2 = -exp2;
                                    Combine(exp2, true);
                                }
                                return true;

                            //Including null, OpeningBracket, ClosingBracket, Function and Word.
                            default: //5 E ± Word ?.
                                return false;
                        }
                    }

                case Constant _: //5E Constant. Be explicit if Eulers.
                    ExpandConstantForE(nextNode, false);
                    return true;

                //Including null, OpeningBracket, ClosingBracket, Function and Word.
                default: //5 E ?. Depends on next. Cannot combine. Continue.
                    return false;
            }

            void Combine(Number exponent, bool operatorBetween)
            {
                E.TestPower(exponent);
                previousNode.Value = new Number(numberBeforeE + e.ToString() + exponent);
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
        }

        private void HandleENextIsAdditionSubtractionOperators()
        {
            switch (nextNode.Next?.Value)
            {
                case Number number: //Word E ± 5
                    E.TestPower(number);
                    if (nextNode.Next.Next?.Value is E) //Word E ± 5E
                    {
                        //± are operators so already being explicit.
                        CurrentNodeIsEulersNumber();
                    }
                    return;

                case Constant _: //Word E ± Constant
                    //± are operators so already being explicit.
                    ExpandConstantForE(nextNode.Next, true);
                    return;

                case Word _: //Word E ± Word
                    return;

                case E _: //Word E ± E
                    throw new ArgumentException(ElementsExceptionMessages.ExponentIsNotIntegerBeforeParameter +
                                                nextNode.Next.Value + ElementsExceptionMessages
                                                    .ExponentIsNotIntegerAfterParameter);

                //Including null, OpeningBracket, ClosingBracket,  Function and Word.
                default: //Word E ± ?.
                    CurrentNodeIsEulersNumber();
                    return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeWithConstant"></param>
        /// <param name="alreadyBeingExplicit">If Multi-Element Constant, Eulers; otherwise InvalidUseOfExponentEDefault.</param>
        private void ExpandConstantForE(LinkedListNode<BaseElement> nodeWithConstant, bool alreadyBeingExplicit)
        {
            ExpandConstant(nodeWithConstant);
            switch (nodeWithConstant.Value)
            {
                case Number n:
                    E.TestPower(n);
                    if (nodeWithConstant.Next?.Value is E) //? E Constant E
                    {
                        if (alreadyBeingExplicit)
                            CurrentNodeIsEulersNumber();
                        else
                            throw new Exception(BuilderExceptionMessages.InvalidUseOfExponentEDefault);
                    }
                    break;
                case OpeningBracket _ when alreadyBeingExplicit: //?E±(
                    CurrentNodeIsEulersNumber();
                    break;
                case OpeningBracket _: //? E  ?
                    throw new Exception(BuilderExceptionMessages.InvalidUseOfExponentEDefault);
            }
        }

        private void CurrentNodeIsEulersNumber()
        {
            currentNode.Value = new Number(Math.E);
            current = currentNode.Value;
            ValidateNumber();
        }
    }
}