using System;

namespace EquationElements.Functions
{
    /// <summary>
    ///     The only function where the argument is before the function, not after. It also does not require the argument to be
    ///     in brackets.
    /// </summary>
    public abstract class FactorialFunction : OneArgumentElement, IFunction, IInvalidWhenFirst
    {
        protected override Number PerformOnAfterNullCheck(Number number)
        {
            if (number < 1)
                throw new ArgumentException(ElementsExceptionMessages.FactorialWasNotAnIntegerBeforeParameter + number +
                                            ElementsExceptionMessages.FactorialWasNotAnIntegerAfterParameter);
            decimal value = 1;
            for (decimal i = 2; i <= number; i++)
                value *= i;
            return new Number(value);
        }
    }

    public class FactorialWordFunction : FactorialFunction
    {
        public override string ToString() => FunctionRepresentations.FactorialWord;
    }

    public class FactorialSymbolFunction : FactorialFunction
    {
        public override string ToString() => FunctionRepresentations.FactorialSymbol;
    }
}