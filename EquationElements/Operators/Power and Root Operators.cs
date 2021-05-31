using System;
using EquationElements.Functions;

namespace EquationElements.Operators
{
    /// <summary>
    ///     Works with doubles only.
    /// </summary>
    public class PowerOperator : TwoArgumentElement, IOperatorExcludingBrackets, IInvalidWhenFirst,
        IMayPrecedeNegativeNumber
    {
        public override string ToString() => OperatorRepresentations.PowerSymbol;

        protected override Number PerformOnAfterNullCheck(Number a, Number power)
        {
            if (power >= 0)
                return new Number(Math.Pow(a.AsDouble, power.AsDouble));

            Number absolutePower = new AbsoluteFunction().PerformOn(power);
            a = new Number(Math.Pow(a.AsDouble, absolutePower.AsDouble));
            return -a;
        }
    }

    /// <summary>
    ///     Works with doubles only.
    /// </summary>
    public abstract class RootOperator : TwoArgumentElement, IOperatorExcludingBrackets, IInvalidWhenFirst,
        IInvalidBeforeMinus
    {
        protected override Number PerformOnAfterNullCheck(Number a, Number b) =>
            new Number(Math.Pow(b.AsDouble, 1 / a.AsDouble));
    }

    /// <summary>
    ///     Works with doubles only.
    /// </summary>
    public class RootWordOperator : RootOperator
    {
        public override string ToString() => OperatorRepresentations.RootWord;
    }

    /// <summary>
    ///     Works with doubles only.
    /// </summary>
    public class RootSymbolOperator : RootOperator
    {
        public override string ToString() => OperatorRepresentations.RootSymbol;
    }
}