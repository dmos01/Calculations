using System;

namespace EquationElements.Operators
{
    public class ParenthesisClosingBracket : ClosingBracket
    {
        public override string ToString() => OperatorRepresentations.ParenthesisClosingBracketSymbol;

        public override Type GetReverseType() => typeof(ParenthesisOpeningBracket);

        public override string GetReverseSymbol() => new ParenthesisOpeningBracket().ToString();
    }

    public class CurlyClosingBracket : ClosingBracket
    {
        public override string ToString() => OperatorRepresentations.CurlyClosingBracketSymbol;

        public override Type GetReverseType() => typeof(CurlyOpeningBracket);

        public override string GetReverseSymbol() => new CurlyOpeningBracket().ToString();
    }

    public class SquareClosingBracket : ClosingBracket
    {
        public override string ToString() => OperatorRepresentations.SquareClosingBracketSymbol;

        public override Type GetReverseType() => typeof(SquareOpeningBracket);

        public override string GetReverseSymbol() => new SquareOpeningBracket().ToString();
    }
}