using System;
using EquationBuilder;
using EquationElements;
using NUnit.Framework;
using static UnitTests.BaseMethods;

namespace UnitTests
{
    /// <summary>
    ///     In format of [Equation, Expected Answer]
    /// </summary>
    [TestFixture]
    public class NumberTests
    {
        static readonly object[] ZeroTestCases =
        {
            new object[] {"0", new Number(0)},
            new object[] {"00000", new Number(0)},
            new object[] {"0.0", new Number(0)},
            new object[] {"000.00000", new Number(0)},
            new object[] {"-0", new Number(0)},
            new object[] {"-00000", new Number(0)},
            new object[] {"-0.0", new Number(0)},
            new object[] {"-000.00000", new Number(0)}
        };

        static readonly object[] SimpleTestCases =
        {
            new object[] {"5", new Number(5)},
            new object[] {"   5   ", new Number(5)},
            new object[] {"1+1", new Number(2)},
            new object[] {"   1   +   1   ", new Number(2)},
            new object[] {"(5)", new Number(5)},
            new object[] {"((((5))))", new Number(5)},
            new object[] {"0.5", new Number(0.5)},
            new object[] {".5", new Number(0.5)}
        };

        static readonly object[] NegativesTestCases =
        {
            new object[] {"-5", new Number(-5)},
            new object[] {"-(5)", new Number(-5)},
            new object[] {"5*-(7)", new Number(-35)},
            new object[] {"(((-5)))", new Number(-5)},
            new object[] {"((-(5)))", new Number(-5)},
            new object[] {"(-((5)))", new Number(-5)},
            new object[] {"-(((5)))", new Number(-5)},
            new object[] {"((-(-5)))", new Number(5)},
            new object[] {"(-((-5)))", new Number(5)},
            new object[] {"-(((-5)))", new Number(5)},
            new object[] {"((-(-5)))", new Number(5)},
            new object[] {"(-(-(-5)))", new Number(-5)},
            new object[] {"-(-(-(-5)))", new Number(5)},
            new object[] {"(((-5)))", new Number(-5)},
            new object[] {"((-(5)))", new Number(-5)},
            new object[] {"(-(-(5)))", new Number(5)},
            new object[] {"-(-(-(5)))", new Number(-5)}
        };

        static readonly object[] PlusMinusComboTestCases =
        {
            new object[] {"1+-2", new Number(1-2)},
            new object[] {"1-+2", new Number(1-2)},
            new object[] {"1--2", new Number(1+2)},
            new object[] {"(-sin1)", new Number(0-Math.Sin(1))},
            new object[] {"(--sin1)", new Number(Math.Sin(1))},
            new object[] {"(-+sin1)", new Number(0-Math.Sin(1))}
        };


        [Test]
        [TestCaseSource(nameof(ZeroTestCases))]
        public void ZeroTests(object[] currentCase) => TestBuilderAndCalculator(currentCase);

        [Test]
        [TestCaseSource(nameof(SimpleTestCases))]
        public void SimpleTests(object[] currentCase) => TestBuilderAndCalculator(currentCase);

        [Test]
        [TestCaseSource(nameof(NegativesTestCases))]
        public void NegativesTests(object[] currentCase) => TestBuilderAndCalculator(currentCase);

        [Test]
        [TestCaseSource(nameof(PlusMinusComboTestCases))]
        public void PlusMinusComboTests(object[] currentCase) => TestBuilderAndCalculator(currentCase);


        [Test]
        public void NoClosingBracketsTest() => TestBuilderAndCalculator(new object[] { "(2[3", new Number(2 * 3) });

        [Test]
        public void IncorrectClosingBracketTest() => ShouldThrowException(new object[]
            {"(2[3)", BuilderExceptionMessages.InvalidBracketUseDefault});

        [Test]
        public void NoSecondaryClosingBracketTest() => ShouldThrowException(new object[]
            {"(1}", BuilderExceptionMessages.InvalidBracketUseDefault});
    }
}