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


        [Test]
        [TestCaseSource(nameof(ZeroTestCases))]
        public void ZeroTests(object[] currentCase)
        {
            TestBuilderAndCalculator(currentCase);
        }

        [Test]
        [TestCaseSource(nameof(SimpleTestCases))]
        public void SimpleTests(object[] currentCase)
        {
            TestBuilderAndCalculator(currentCase);
        }

        [Test]
        [TestCaseSource(nameof(NegativesTestCases))]
        public void NegativesTests(object[] currentCase)
        {
            TestBuilderAndCalculator(currentCase);
        }
    }
}