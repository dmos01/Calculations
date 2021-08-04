using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using EquationElements;
using EquationElements.Operators;

namespace EquationBuilder
{
    public partial class Splitter
    {
        LinkedList<BaseElement> splitterOutput;
        ElementBuilder elementBuilder { get; }

        /// <summary>
        ///     Splits the string into a LinkedList of Elements and expands Constants. Validates use of decimal points. Can throw
        ///     exceptions.
        /// </summary>
        /// <param name="equation"></param>
        /// <returns>The split list of elements.</returns>
        public LinkedList<BaseElement> Run(string equation)
        {
            if (equation is null)
                throw new ArgumentNullException(null, BuilderExceptionMessages.NoEquationDefault);

            equation = Utils.RemoveSpaces(equation);
            if (equation == "")
                throw new ArgumentOutOfRangeException(null, BuilderExceptionMessages.NoEquationDefault);

            splitterOutput = new LinkedList<BaseElement>();
            ICollection<BaseElement> operatorsAndOther = SplitIntoOperatorsAndOther(equation);

            foreach (BaseElement numbersAndWordsElement in operatorsAndOther)
            {
                switch (numbersAndWordsElement)
                {
                    case Constant constant:
                        ExpandAndAddConstant(constant);
                        continue;

                    case UnrecognizedElement _:
                        {
                            if (AttemptToRecognize(numbersAndWordsElement.ToString()))
                                continue;

                            ICollection<BaseElement> numbersAndOther =
                                SplitIntoNumbersAndOther(numbersAndWordsElement.ToString());
                            foreach (BaseElement wordOnlyElement in numbersAndOther)
                            {
                                switch (wordOnlyElement)
                                {
                                    case Constant constant:
                                        ExpandAndAddConstant(constant);
                                        continue;

                                    case UnrecognizedElement unrecognized:
                                        if (!AttemptToRecognize(wordOnlyElement.ToString()))
                                        {
                                            unrecognized.OuterNumbersAndWordsElement = numbersAndWordsElement.ToString();
                                            splitterOutput.AddLast(unrecognized);
                                        }

                                        continue;

                                    default:
                                        splitterOutput.AddLast(wordOnlyElement);
                                        continue;
                                }
                            }

                            continue;
                        }

                    default:
                        splitterOutput.AddLast(numbersAndWordsElement);
                        continue;
                }
            }

            return splitterOutput;
        }

        private List<BaseElement> SplitIntoOperatorsAndOther(string text)
        {
            List<BaseElement> operatorsAndOther = new List<BaseElement>();
            StringBuilder elementBeingBuilt = new StringBuilder();

            //Thanks to https://docs.microsoft.com/en-us/dotnet/api/system.globalization.stringinfo?view=net-5.0 and https://docs.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=net-5.0

            TextElementEnumerator characterEnumerator = StringInfo.GetTextElementEnumerator(text);
            while (characterEnumerator.MoveNext())
            {
                if (IsOperator.Run(characterEnumerator.GetTextElement(), out BaseElement element))
                {
                    if (elementBeingBuilt.Length != 0)
                    {
                        operatorsAndOther.Add(elementBuilder.CreateElement(elementBeingBuilt.ToString()));
                        elementBeingBuilt.Clear();
                    }

                    operatorsAndOther.Add(element);
                }
                else
                    elementBeingBuilt.Append(characterEnumerator.GetTextElement());
            }

            if (elementBeingBuilt.Length != 0)
                operatorsAndOther.Add(elementBuilder.CreateElement(elementBeingBuilt.ToString()));
            return operatorsAndOther;
        }

        private List<BaseElement> SplitIntoNumbersAndOther(string text)
        {
            List<BaseElement> numbersAndOther = new List<BaseElement>();
            StringBuilder elementBeingBuilt = new StringBuilder();

            //Thanks to https://docs.microsoft.com/en-us/dotnet/api/system.globalization.stringinfo?view=net-5.0 and https://docs.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=net-5.0

            bool buildingNumber = false;
            bool containsDecimalPoint = false;

            TextElementEnumerator characterEnumerator = StringInfo.GetTextElementEnumerator(text);
            while (characterEnumerator.MoveNext())
            {
                string character = characterEnumerator.GetTextElement();
                BaseElement current = elementBuilder.CreateElement(character);

                switch (current)
                {
                    case DecimalPoint _ when containsDecimalPoint:
                        throw new Exception(BuilderExceptionMessages.DecimalPointDefault);

                    case DecimalPoint _:
                        {
                            containsDecimalPoint = true;

                            if (elementBeingBuilt.Length == 0)
                            {
                                elementBeingBuilt.Append(NumberRepresentations.ZeroSymbol);
                                elementBeingBuilt.Append(character);
                                buildingNumber = true;
                            }
                            else if (buildingNumber)
                            {
                                elementBeingBuilt.Append(character);
                            }
                            else
                            {
                                numbersAndOther.Add(elementBuilder.CreateElement(elementBeingBuilt.ToString()));
                                elementBeingBuilt.Clear();
                                elementBeingBuilt.Append(character);
                                buildingNumber = true;
                            }

                            break;
                        }

                    case Number _ when elementBeingBuilt.Length == 0:
                        elementBeingBuilt.Append(character);
                        buildingNumber = true;
                        break;
                    case Number _ when buildingNumber:
                        elementBeingBuilt.Append(character);
                        break;
                    case Number _:
                        numbersAndOther.Add(elementBuilder.CreateElement(elementBeingBuilt.ToString()));
                        elementBeingBuilt.Clear();
                        elementBeingBuilt.Append(character);
                        buildingNumber = true;
                        break;

                    case IOperator _:
                        {
                            if (elementBeingBuilt.Length != 0)
                            {
                                numbersAndOther.Add(elementBuilder.CreateElement(elementBeingBuilt.ToString()));
                                elementBeingBuilt.Clear();
                                buildingNumber = false;
                                containsDecimalPoint = false;
                            }

                            numbersAndOther.Add(current);
                            break;
                        }

                    default:
                        {
                            if (elementBeingBuilt.Length == 0)
                            {
                                elementBeingBuilt.Append(character);
                                buildingNumber = false;
                            }
                            else if (buildingNumber)
                            {
                                numbersAndOther.Add(elementBuilder.CreateElement(elementBeingBuilt.ToString()));
                                elementBeingBuilt.Clear();
                                elementBeingBuilt.Append(character);
                                buildingNumber = false;
                                containsDecimalPoint = false;
                            }
                            else
                                elementBeingBuilt.Append(character);

                            break;
                        }
                }
            }

            if (elementBeingBuilt.Length != 0)
                numbersAndOther.Add(elementBuilder.CreateElement(elementBeingBuilt.ToString()));
            return numbersAndOther;
        }

        private bool AttemptToRecognize(string elementAsString)
        {
            //Split by root
            if (AttemptToRecognizeInner(elementAsString, OperatorRepresentations.RootWord))
                return true;

            //Split by mod
            if (AttemptToRecognizeInner(elementAsString, OperatorRepresentations.ModulusWord))
                return true;

            //Split by Eulers
            if (AttemptToRecognizeInner(elementAsString, ElementsResources.EulersSymbolUpperCase))
                return true;

            //Split by Exponent, if different from Eulers
            if (IsOperator.EulersAndExponentSymbolsAreDifferent)
            {
                if (AttemptToRecognizeInner(elementAsString, ElementsResources.ExponentSymbolUpperCase))
                    return true;
            }

            return false;
        }

        private bool AttemptToRecognizeInner(string elementAsString, string separator)
        {
            ICollection<string> MyStringSplit()
            {
                List<string> stringSplitOutput = new List<string>();

                //Characters found before finding the first in the separator.
                StringBuilder builder = new StringBuilder();

                //If some characters match part (but not all) of the separator, those characters need to be added to builder to prevent them being "dropped."
                StringBuilder separatorBuilder = new StringBuilder();

                TextElementEnumerator textEnum = StringInfo.GetTextElementEnumerator(elementAsString);
                TextElementEnumerator separatorEnum = StringInfo.GetTextElementEnumerator(separator);
                separatorEnum.MoveNext();

                while (textEnum.MoveNext())
                {
                    string currentTextChar = textEnum.GetTextElement();
                    string currentSeparatorChar = separatorEnum.GetTextElement();

                    if (currentTextChar
                        .Equals(currentSeparatorChar, StringComparison.CurrentCultureIgnoreCase))
                    {
                        separatorBuilder.Append(currentSeparatorChar);

                        if (separatorEnum.MoveNext() == false) //Found full separator
                        {
                            if (builder.Length != 0)
                            {
                                stringSplitOutput.Add(builder.ToString());
                                builder.Clear();
                            }

                            stringSplitOutput.Add(separatorBuilder.ToString());
                            separatorBuilder.Clear();
                            separatorEnum.Reset();
                            separatorEnum.MoveNext();
                        }
                    }
                    else
                    {
                        if (separatorBuilder.Length != 0)
                        {
                            builder.Append(separatorBuilder);
                            separatorBuilder.Clear();
                            separatorEnum.Reset();
                            separatorEnum.MoveNext();
                        }

                        builder.Append(currentTextChar);
                    }
                }

                if (separatorBuilder.Length != 0)
                    builder.Append(separatorBuilder);
                if (builder.Length != 0)
                    stringSplitOutput.Add(builder.ToString());

                return stringSplitOutput;
            }

            ICollection<string> split = MyStringSplit();
            if (split.Count < 2)
                return false;

            ICollection<BaseElement> recognizedElements = new List<BaseElement>(split.Count);
            foreach (string index in split)
            {
                BaseElement indexAsElement = elementBuilder.CreateElement(index);
                if (indexAsElement is UnrecognizedElement _)
                    return false;
                recognizedElements.Add(indexAsElement);
            }

            foreach (BaseElement recognizedElement in recognizedElements)
            {
                if (recognizedElement is Constant constant)
                    ExpandAndAddConstant(constant);
                else
                    splitterOutput.AddLast(recognizedElement);
            }

            return true;
        }

        private void ExpandAndAddConstant(Constant constant)
        {
            ICollection<BaseElement> expandedConstant =
                SplitAndValidate.Run(constant.Value, elementBuilder);

            switch (expandedConstant.Count)
            {
                case 0:
                    return;
                case 1:
                    splitterOutput.AddLast(expandedConstant.First());
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
                    splitterOutput.AddLast(num * -1);
                else
                    AddElementsBetweenBrackets();
            }

            void AddElementsBetweenBrackets()
            {
                splitterOutput.AddLast(SplitAndValidate.CreateImpliedOpeningBracket());
                foreach (BaseElement baseElement in expandedConstant)
                    splitterOutput.AddLast(baseElement);
                splitterOutput.AddLast(SplitAndValidate.CreateImpliedClosingBracket());
            }
        }
    }
}