using System;

namespace EquationElements.Functions
{
    /// <summary>
    ///     The only function where the argument is before the function, not after. It also does not require the argument to be
    ///     in brackets.
    /// </summary>
    public abstract class Factorial : OneArgumentElement, IInvalidWhenFirst
    {
        protected override Number PerformOnAfterNullCheck(Number number)
        {
            if (number < 1 || number % 1 != 0)
                throw new ArgumentException(ElementsExceptionMessages.FactorialWasNotAnIntegerBeforeParameter + number +
                                            ElementsExceptionMessages.FactorialWasNotAnIntegerAfterParameter);

            decimal value = 1;
            for (decimal i = 2; i <= number; i++)
                value *= i;
            return new Number(value);
        }
    }

    public class FactorialWord : Factorial
    {
        public override string ToString() => FunctionRepresentations.FactorialWord;
    }

    public class FactorialSymbol : Factorial
    {
        public override string ToString() => FunctionRepresentations.FactorialSymbol;
    }
}