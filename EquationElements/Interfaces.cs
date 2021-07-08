namespace EquationElements
{
    /// <summary>
    ///     All IOperatorOrClosingBracket (except SubtractionOperator) and Factorial.
    /// </summary>
    public interface IInvalidWhenFirst : IInvalidAfterOperator { }

    /// <summary>
    ///     IOperatorOrOpeningBracket, DecimalPoint and IFunction.
    /// </summary>
    public interface IInvalidWhenLast { }

    /// <summary>
    ///     Same as IInvalidWhenFirst.
    /// </summary>
    public interface IInvalidAfterOperator { }

    /// <summary>
    ///     ArgumentSeparatorOperator, DivisionOperator, ModulusOperator, MultiplicationOperator, BaseOpeningBracket,
    ///     PowerOperator.
    /// </summary>
    public interface IMayPrecedeNegativeNumber { }

    /// <summary>
    ///     RootOperator.
    /// </summary>
    public interface IInvalidBeforeMinus { }
}