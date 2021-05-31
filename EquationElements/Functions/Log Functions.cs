using System;

namespace EquationElements.Functions
{
    /// <summary>
    ///     Works with doubles only.
    /// </summary>
    public class LnFunction : OneArgumentElement, IFunction
    {
        public override string ToString() => FunctionRepresentations.NaturalLogShortWord;

        protected override Number PerformOnAfterNullCheck(Number number) => new Number(Math.Log(number.AsDouble));
    }

    /// <summary>
    ///     Works with doubles only.
    /// </summary>
    public class LogFunction : TwoArgumentElement, IFunction
    {
        public override string ToString() => FunctionRepresentations.LogWord;

        protected override Number PerformOnAfterNullCheck(Number log, Number logNumber) =>
            new Number(Math.Log(logNumber.AsDouble, log.AsDouble));
    }
}