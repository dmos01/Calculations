namespace EquationElements
{
    /// <summary>
    ///     Includes brackets.
    /// </summary>
    public interface IOperator
    {
    }

    public interface IOperatorOrOpeningBracket : IOperator, IInvalidWhenLast
    {
    }

    public interface IOperatorOrClosingBracket : IOperator //Not IInvalidWhenFirst because of SubtractionOperator.
    {
    }

    public interface IOperatorExcludingBrackets : IOperatorOrOpeningBracket, IOperatorOrClosingBracket
    {
    }
}