using System;

namespace EquationElements.Functions
{
    public class FloorFunction : OneArgumentElement, IFunction
    {
        public override string ToString() => FunctionRepresentations.FloorWord;

        protected override Number PerformOnAfterNullCheck(Number number) =>
            number.IsDecimal
                ? new Number(Math.Floor(number.AsDecimal))
                : new Number(Math.Floor(number.AsDouble));
    }

    public class CeilingFunction : OneArgumentElement, IFunction
    {
        public override string ToString() => FunctionRepresentations.CeilingWord;

        protected override Number PerformOnAfterNullCheck(Number number) =>
            number.IsDecimal
                ? new Number(Math.Ceiling(number.AsDecimal))
                : new Number(Math.Ceiling(number.AsDouble));
    }

    public class TruncateFunction : OneArgumentElement, IFunction
    {
        public override string ToString() => FunctionRepresentations.TruncateWord;

        protected override Number PerformOnAfterNullCheck(Number number) =>
            number.IsDecimal
                ? new Number(Math.Truncate(number.AsDecimal))
                : new Number(Math.Truncate(number.AsDouble));
    }
}