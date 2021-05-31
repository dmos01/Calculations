using System;

namespace EquationElements.Functions
{
    public class TanFunction : TrigonometricFunction
    {
        protected override Number PerformOnAfterNullCheck(Number number, bool radians)
        {
            if (radians == false)
                number *= Math.PI / 180;
            return new Number(Math.Tan(number.AsDouble));
        }

        public override string ToString() => FunctionRepresentations.TanWord;
    }

    public class ATanFunction : TrigonometricFunction
    {
        protected override Number PerformOnAfterNullCheck(Number number, bool radians)
        {
            if (radians == false)
                number *= Math.PI / 180;
            return new Number(Math.Atan(number.AsDouble));
        }

        public override string ToString() => FunctionRepresentations.ATanWord;
    }

    public class TanhFunction : TrigonometricFunction
    {
        protected override Number PerformOnAfterNullCheck(Number number, bool radians)
        {
            if (radians == false)
                number *= Math.PI / 180;
            return new Number(Math.Tanh(number.AsDouble));
        }

        public override string ToString() => FunctionRepresentations.TanhWord;
    }
}