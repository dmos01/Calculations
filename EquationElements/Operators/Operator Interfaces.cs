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

    public interface IOperatorOrClosingBracket : IOperator
    {
    }

    public interface IOperatorExcludingBrackets : IOperatorOrOpeningBracket, IOperatorOrClosingBracket
    {
    }
}