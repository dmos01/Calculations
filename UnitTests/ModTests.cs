using EquationElements;
using NUnit.Framework;
using static UnitTests.BaseMethods;

namespace UnitTests
{
    [TestFixture]
    internal class ModTests
    {
        static readonly object[] AsSymbolTestCases =
        {
            new object[] {"5%3", new Number(2)},
            new object[] {"5%(-3)", new Number(2)},
            new object[] {"(-5)%3", new Number(-2)},
            new object[] {"(-5)%(-3)", new Number(-2)},
            new object[] {"-5%-3", new Number(-2)}
        };

        [Test]
        [TestCaseSource(nameof(AsSymbolTestCases))]
        public void AsSymbol(object[] currentCase) => TestBuilderAndCalculator(currentCase);


        static readonly object[] AsWordTestCases =
        {
            new object[] {"5mod3", new Number(2)},
            new object[] {"5mod(-3)", new Number(2)},
            new object[] {"(-5)mod3", new Number(-2)},
            new object[] {"(-5)mod(-3)", new Number(-2)},
            new object[] {"-5mod-3", new Number(-2)}
        };


        [Test]
        [TestCaseSource(nameof(AsWordTestCases))]
        public void AsWord(object[] currentCase) => TestBuilderAndCalculator(currentCase);
    }
}