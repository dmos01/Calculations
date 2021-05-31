namespace EquationElements.Operators
{
    /// <summary>
    ///     Abstract class that is a parent to the two states, Symbol and Word.
    /// </summary>
    public abstract class ModulusOperator : TwoArgumentElement, IOperatorExcludingBrackets, IInvalidWhenFirst,
        IMayPrecedeNegativeNumber
    {
        protected override Number PerformOnAfterNullCheck(Number a, Number b) => a % b;
    }

    public class ModulusWordOperator : ModulusOperator
    {
        public override string ToString() => OperatorRepresentations.ModulusWord;
    }

    public class ModulusSymbolOperator : ModulusOperator
    {
        public override string ToString() => OperatorRepresentations.ModulusSymbol;
    }
}