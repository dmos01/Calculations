using System;

namespace EquationElements.Operators
{
    public class ParenthesisClosingBracket : ClosingBracket
    {
        public override string ToString() => OperatorRepresentations.ParenthesisClosingBracketSymbol;

        public override Type GetReverseType() => typeof(ParenthesisOpeningBracket);

        public override string GetReverseSymbol() => OperatorRepresentations.ParenthesisOpeningBracketSymbol;
    }

    public class CurlyClosingBracket : ClosingBracket
    {
        public override string ToString() => OperatorRepresentations.CurlyClosingBracketSymbol;

        public override Type GetReverseType() => typeof(CurlyOpeningBracket);

        public override string GetReverseSymbol() => OperatorRepresentations.CurlyOpeningBracketSymbol;
    }

    public class SquareClosingBracket : ClosingBracket
    {
        public override string ToString() => OperatorRepresentations.SquareClosingBracketSymbol;

        public override Type GetReverseType() => typeof(SquareOpeningBracket);

        public override string GetReverseSymbol() => OperatorRepresentations.SquareOpeningBracketSymbol;
    }
}