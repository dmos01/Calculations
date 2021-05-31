namespace EquationElements.Operators
{
    public class ArgumentSeparatorOperator : BaseElement, IOperatorExcludingBrackets, IInvalidWhenFirst,
        IMayPrecedeNegativeNumber
    {
        public override string ToString() => OperatorRepresentations.ArgumentSeperatorSymbol;
    }
}