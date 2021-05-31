using System;

namespace EquationElements.Operators
{
    public class ParenthesisOpeningBracket : OpeningBracket
    {
        public override string ToString() => OperatorRepresentations.ParenthesisOpeningBracketSymbol;

        public override Type GetReverseType() => typeof(ParenthesisClosingBracket);

        public override string GetReverseSymbol() => new ParenthesisClosingBracket().ToString();
    }

    public class CurlyOpeningBracket : OpeningBracket
    {
        public override string ToString() => OperatorRepresentations.CurlyOpeningBracketSymbol;

        public override Type GetReverseType() => typeof(CurlyClosingBracket);

        public override string GetReverseSymbol() => new CurlyClosingBracket().ToString();
    }

    public class SquareOpeningBracket : OpeningBracket
    {
        public override string ToString() => OperatorRepresentations.SquareOpeningBracketSymbol;

        public override Type GetReverseType() => typeof(SquareClosingBracket);

        public override string GetReverseSymbol() => new SquareClosingBracket().ToString();
    }
}