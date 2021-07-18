using System;

namespace EquationElements.Operators
{
    public class ParenthesisOpeningBracket : OpeningBracket
    {
        public override string ToString() => OperatorRepresentations.ParenthesisOpeningBracketSymbol;

        public override Type GetReverseType() => typeof(ParenthesisClosingBracket);

        public override string GetReverseSymbol() => OperatorRepresentations.ParenthesisClosingBracketSymbol;
    }

    public class CurlyOpeningBracket : OpeningBracket
    {
        public override string ToString() => OperatorRepresentations.CurlyOpeningBracketSymbol;

        public override Type GetReverseType() => typeof(CurlyClosingBracket);

        public override string GetReverseSymbol() => OperatorRepresentations.CurlyClosingBracketSymbol;
    }

    public class SquareOpeningBracket : OpeningBracket
    {
        public override string ToString() => OperatorRepresentations.SquareOpeningBracketSymbol;

        public override Type GetReverseType() => typeof(SquareClosingBracket);

        public override string GetReverseSymbol() => OperatorRepresentations.SquareClosingBracketSymbol;
    }
}