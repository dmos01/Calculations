using System;

namespace EquationElements.Functions
{
    public class SinFunction : TrigonometricFunction
    {
        public override string ToString() => FunctionRepresentations.SinWord;

        protected override Number PerformOnAfterNullCheck(Number number, bool radians)
        {
            if (radians == false)
                number *= Math.PI / 180;
            return new Number(Math.Sin(number.AsDouble));
        }
    }

    public class ASinFunction : TrigonometricFunction
    {
        protected override Number PerformOnAfterNullCheck(Number number, bool radians)
        {
            if (radians == false)
                number *= Math.PI / 180;
            return new Number(Math.Asin(number.AsDouble));
        }

        public override string ToString() => FunctionRepresentations.ASinWord;
    }

    public class SinhFunction : TrigonometricFunction
    {
        protected override Number PerformOnAfterNullCheck(Number number, bool radians)
        {
            if (radians == false)
                number *= Math.PI / 180;
            return new Number(Math.Sinh(number.AsDouble));
        }

        public override string ToString() => FunctionRepresentations.SinhWord;
    }
}