using System;

namespace EquationElements.Functions
{
    /// <summary>
    ///     Rounds up on 5.
    /// </summary>
    public class RoundFunction : OneArgumentElement, IFunction
    {
        public override string ToString() => FunctionRepresentations.RoundWord;

        protected override Number PerformOnAfterNullCheck(Number number) =>
            number.IsDecimal
                ? new Number(Math.Round(number.AsDecimal, MidpointRounding.AwayFromZero))
                : new Number(Math.Round(number.AsDouble, MidpointRounding.AwayFromZero));
    }

    /// <summary>
    ///     Rounds to the even number on 5.
    /// </summary>
    public class EvenRoundFunction : OneArgumentElement, IFunction
    {
        public override string ToString() => FunctionRepresentations.EvenRoundWord;

        protected override Number PerformOnAfterNullCheck(Number number) =>
            number.IsDecimal
                ? new Number(Math.Round(number.AsDecimal, MidpointRounding.ToEven))
                : new Number(Math.Round(number.AsDouble, MidpointRounding.ToEven));
    }
}