using System;

namespace EquationElements.Functions
{
    public class AbsoluteFunction : OneArgumentElement, IFunction
    {
        public override string ToString() => FunctionRepresentations.AbsoluteShortWord;

        protected override Number PerformOnAfterNullCheck(Number number) =>
            number.IsDecimal
                ? new Number(Math.Abs(number.AsDecimal))
                : new Number(Math.Abs(number.AsDouble));
    }
}