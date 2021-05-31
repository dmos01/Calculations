namespace EquationElements.Operators
{
    public class AdditionOperator : TwoArgumentElement, IOperatorExcludingBrackets, IInvalidWhenFirst
    {
        public override string ToString() => OperatorRepresentations.AdditionSymbol;

        protected override Number PerformOnAfterNullCheck(Number a, Number b) => a + b;
    }

    public class SubtractionOperator : TwoArgumentElement, IOperatorExcludingBrackets
    {
        public override string ToString() => OperatorRepresentations.SubtractionSymbol;

        protected override Number PerformOnAfterNullCheck(Number a, Number b) => a - b;
    }

    public class MultiplicationOperator : TwoArgumentElement, IOperatorExcludingBrackets, IInvalidWhenFirst,
        IMayPrecedeNegativeNumber
    {
        public override string ToString() => OperatorRepresentations.ComputerMultiplicationSymbol;

        protected override Number PerformOnAfterNullCheck(Number a, Number b) => a * b;
    }

    public class DivisionOperator : TwoArgumentElement, IOperatorExcludingBrackets, IInvalidWhenFirst,
        IMayPrecedeNegativeNumber
    {
        public override string ToString() => OperatorRepresentations.ComputerDivisionSymbol;

        protected override Number PerformOnAfterNullCheck(Number a, Number b) => a / b;
    }
}