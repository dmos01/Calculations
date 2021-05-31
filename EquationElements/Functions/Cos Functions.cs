using System;

namespace EquationElements.Functions
{
    public class CosFunction : TrigonometricFunction
    {
        protected override Number PerformOnAfterNullCheck(Number number, bool radians)
        {
            if (radians == false)
                number *= Math.PI / 180;
            return new Number(Math.Cos(number.AsDouble));
        }

        public override string ToString() => FunctionRepresentations.CosWord;
    }

    public class ACosFunction : TrigonometricFunction
    {
        protected override Number PerformOnAfterNullCheck(Number number, bool radians)
        {
            if (radians == false)
                number *= Math.PI / 180;
            return new Number(Math.Acos(number.AsDouble));
        }

        public override string ToString() => FunctionRepresentations.ACosWord;
    }

    public class CoshFunction : TrigonometricFunction
    {
        protected override Number PerformOnAfterNullCheck(Number number, bool radians)
        {
            if (radians == false)
                number *= Math.PI / 180;
            return new Number(Math.Cosh(number.AsDouble));
        }

        public override string ToString() => FunctionRepresentations.CoshWord;
    }
}