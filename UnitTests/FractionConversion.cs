using EquationBuilder;
using EquationCalculator;
using NUnit.Framework;
using static UnitTests.BaseMethods;

namespace UnitTests
{
    class FractionConversion
    {
        static readonly object[] FirstPassTestCases =
        {
            new[] {"0", "0.00", "0", "0"},
            new[] {"-0", "0.00", "0", "0"},
            new[] {"1", "1.00", "1", "1"},
            new[] {"1.5", "1.50", "3/2", "1+1/2"},
            new[] {"-1", "-1.00", "-1", "-1"},
            new[] {"-1.5", "-1.50", "-3/2", "-(1+1/2)"},
            new[] {"0.0049", "0.0049", "49/10000", "49/10000"},
            new[] {"-0.0049", "-0.0049", "-49/10000", "-49/10000"},
            new[] {"-1200.5", "-1200.50", "-2401/2", "-(1200+1/2)"}
        };

        static readonly object[] DP1TestCases =
        {
            new[] {"55.3", "55.30", "553/10", "55+3/10"},
            new[] {"55.6", "55.60", "278/5", "55+3/5"},
            new[] {"-55.3", "-55.30", "-553/10", "-(55+3/10)"},
            new[] {"-55.6", "-55.60", "-278/5", "-(55+3/5)"}
        };

        //.67s

        static readonly object[] DP2TestCases =
        {
            new[] {"55.33", "55.33", "5533/100", "55+33/100"},
            new[] {"55.66", "55.66", "2783/50", "55+33/50"},
            new[] {"-55.33", "-55.33", "-5533/100", "-(55+33/100)"},
            new[] {"-55.66", "-55.66", "-2783/50", "-(55+33/50)"}
        };

        static readonly object[] DP3TestCases =
        {
            new[] {"55.333", "55.33", "55333/1000", "55+1/3", "166/3"},
            new[] {"55.666", "55.67", "27833/500", "55+2/3", "167/3"},
            new[] {"-55.333", "-55.33", "-55333/1000", "-(55+1/3)", "-166/3"},
            new[] {"-55.666", "-55.67", "-27833/500", "-(55+2/3)", "-167/3"}
        };

        static readonly object[] DP5TestCases =
        {
            new[] {"55.33333", "55.33", "5533333/100000", "55+1/3", "166/3"},
            new[] {"55.66666", "55.67", "2783333/50000", "55+2/3", "167/3"},
            new[] {"-55.33333", "-55.33", "-5533333/100000", "-(55+1/3)", "-166/3"},
            new[] {"-55.66666", "-55.67", "-2783333/50000", "-(55+2/3)", "-167/3"}
        };

        static readonly object[] DP6TestCases =
        {
            new[] {"55.333336", "55.33", "6916667/125000", "55+41667/125000"},
            new[] {"55.666663", "55.67", "55666663/1000000", "55+666663/1000000"},
            new[] {"-55.333336", "-55.33", "-6916667/125000", "-(55+41667/125000)"},
            new[] {"-55.666663", "-55.67", "-55666663/1000000", "-(55+666663/1000000)"}
        };

        private static readonly object[] IsApproximatelyExactMatchCases =
        {
            new[] {"1/3", "0.33", null, "1/3", "1/3"},
            new[] {"2/3", "0.67", null, "2/3", "2/3"},
            new[] {"1/6", "0.17", null, "1/6", "1/6"},
            new[] {"1+1/3", "1.33", null, "1+1/3", "4/3"},
            new[] {"1+2/3", "1.67", null, "1+2/3", "5/3"},
            new[] {"1+1/6", "1.17", null, "1+1/6", "7/6"},
            new[] {"-(1+1/3)", "-1.33", null, "-(1+1/3)", "-4/3"},
            new[] {"-(1+2/3)", "-1.67", null, "-(1+2/3)", "-5/3"},
            new[] {"-(1+1/6)", "-1.17", null, "-(1+1/6)", "-7/6"}

        };

        [Test]
        [TestCaseSource(nameof(FirstPassTestCases))]
        public void FirstPass(string[] currentCase)
        {
            FractionConversionTest(currentCase);
        }
        
        [Test]
        [TestCaseSource(nameof(DP1TestCases))]
        public void DP1(string[] currentCase)
        {
            FractionConversionTest(currentCase);
        }

        [Test]
        [TestCaseSource(nameof(DP2TestCases))]
        public void DP2(string[] currentCase)
        {
            FractionConversionTest(currentCase);
        }

        [Test]
        [TestCaseSource(nameof(DP3TestCases))]
        public void DP3(string[] currentCase)
        {
            FractionConversionTest(currentCase);
        }

        [Test]
        [TestCaseSource(nameof(DP5TestCases))]
        public void DP5(string[] currentCase)
        {
            FractionConversionTest(currentCase);
        }

        [Test]
        [TestCaseSource(nameof(DP6TestCases))]
        public void DP6(string[] currentCase)
        {
            FractionConversionTest(currentCase);
        }

        [Test]
        [TestCaseSource(nameof(IsApproximatelyExactMatchCases))]
        public void IsApproximatelyExactMatches(string[] currentCase)
        {
            currentCase[0] = new Calculator(SplitAndValidate.Run(currentCase[0])).Run().ToString();
            FractionConversionTest(currentCase);
        }
    }
}