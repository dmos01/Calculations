using System;
using System.Collections.Generic;
using EquationBuilder;
using EquationCalculator;
using EquationElements;
using NUnit.Framework;
using static UnitTests.BaseMethods;

namespace UnitTests
{
    /// <summary>
    ///     Uses absolute answers for ease of consistency when comparing radians to degrees.
    /// </summary>
    internal class FunctionTests
    {
        static readonly object[] ShouldFailTestCases =
        {
            new object[]
            {
                "factorial+5", BuilderExceptionMessages.FirstElementInvalidDefault,
                BuilderExceptionMessages.FirstElementInvalidBeforeParameter
            },
            new object[]
            {
                "factorial5", BuilderExceptionMessages.FirstElementInvalidDefault,
                BuilderExceptionMessages.FirstElementInvalidBeforeParameter
            },
            new object[]
            {
                "log(0,1)", BuilderExceptionMessages.LogToNegativeOrZeroBeforeParameter,
                CalculatorExceptionMessages.LogToNegativeOrZeroBeforeParameter
            },
            new object[] {"1.5!", BuilderExceptionMessages.FactorialWasNotAnIntegerBeforeParameter},
            new object[] {"1.5factorial", BuilderExceptionMessages.FactorialWasNotAnIntegerBeforeParameter},
            new object[] {"log(e,0)", CalculatorExceptionMessages.LogToNegativeOrZeroDefault},
            new object[] {"ln(0)", CalculatorExceptionMessages.LogToNegativeOrZeroDefault},
            new object[] {"log(0,1)", CalculatorExceptionMessages.LogToNegativeOrZeroDefault}
        };


        static readonly object[] ShouldPassTestCases =
        {
            new object[] {"5factorial", new Number(120)},
            new object[] {"5!", new Number(120)},
            new object[] {"(5)factorial", new Number(120)},

            new object[] {"log(e,sin(-4))", new Number(Math.Log(Math.Sin(-4)))},
            new object[] {"Log(10   ,5)", new Number(Math.Log10(5))},
            new object[] {"Log(E,  5)", new Number(Math.Log(5))},
            new object[] {"log(1e, 2)", new Number(Math.Log(2))},
            new object[] {"log(e, 2)", new Number(Math.Log(2))},
            new object[] {"log(e1, 2)", new Number(Math.Log(2))},

            new object[] {"sin(4)-sin(4)", new Number(0)},
            new object[] {"sin(sin(4))", new Number(Math.Sin(Math.Sin(4)))},
            new object[] {"5factorial5", new Number(120 * 5)},
            new object[] {"sin(   4)", new Number(Math.Sin(4))},
            new object[] {"sin(   4)", new Number(Math.Sin(4))},
            new object[] {"sin(   -4)", new Number(Math.Sin(-4))},
            new object[] {"sin(   -4)", new Number(Math.Sin(-4))},
            new object[] {"sin(e)", new Number(Math.Sin(Math.E))},
            new object[] {"sin(e)", new Number(Math.Sin(Math.E))},
            new object[] {"sin(-e)", new Number(-Math.Sin(Math.E))},
            new object[] {"sin(-e)", new Number(-Math.Sin(Math.E))},
            new object[] {"-sin(e)", new Number(-Math.Sin(Math.E))},
            new object[] {"sin(50000)", new Number(Math.Sin(50000))},
            new object[] {"sin(50000)", new Number(Math.Sin(50000))},
            new object[] {"sin(0.0005)", new Number(Math.Sin(0.0005))},
            new object[] {"sin(1/2)", new Number(Math.Sin(0.5))},
        };

        [Test]
        [TestCaseSource(nameof(ShouldFailTestCases))]
        public void ShouldFail(object[] currentCase) => ShouldThrowException(currentCase);

        [Test]
        [TestCaseSource(nameof(ShouldPassTestCases))]
        public void ShouldPass(object[] currentCase) => TestBuilderAndCalculator(currentCase);

        [Test]
        public void RadiansThenDegrees()
        {
            ICollection<BaseElement> elements;

            try
            {
                elements = SplitAndValidate.Run("sin(3)");
            }
            catch (Exception ex)
            {
                Assert.Fail("SplitAndValidate.Run() failed. " + ex.Message);
                return;
            }

            Number answer;
            try
            {
                answer = new Calculator(elements).Run();
            }
            catch (Exception ex)
            {
                Assert.Fail("First Calculator.Run() or radians calculation failed. " + ex.Message);
                return;
            }

            answer *= Math.Pow(10, 5);
            answer = new Number(Math.Truncate(answer.AsDouble));
            answer /= Math.Pow(10, 5);
            if (answer != 0.14112)
                Assert.Fail("Wrong answer in radians. Expected " + 0.14112 + " but was " + answer);

            try
            {
                answer = new Calculator(elements).Run(false);
            }
            catch (Exception ex)
            {
                Assert.Fail("Second Calculator.Run() or degrees calculation failed. " + ex.Message);
                return;
            }

            answer *= Math.Pow(10, 5);
            answer = new Number(Math.Truncate(answer.AsDouble));
            answer /= Math.Pow(10, 5);

            if (answer != 0.05233)
                Assert.Fail("Wrong answer in degrees. Expected " + 0.05233 + " but was " + answer);

            Assert.Pass();
        }

        [Test]
        public void Random()
        {
            const int min = 1;
            const int max = 10;
            ICollection<BaseElement> elements;

            try
            {
                elements = SplitAndValidate.Run("random(" + min + "," + max + ")");
            }
            catch (Exception ex)
            {
                Assert.Fail("SplitAndValidate.Run() failed. " + ex.Message);
                return;
            }

            try
            {
                Number answer = new Calculator(elements).Run();
                if (answer < min || answer > max)
                    Assert.Fail("Wrong answer. Expected between " + min + " and " + max + ", but was " + answer);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed Calculator. " + ex.Message);
            }
        }

        [Test]
        public void RandomCanBeMax()
        {
            const int min = 1;
            const int max = 3;
            const int iterations = 1000;
            ICollection<BaseElement> elements;

            try
            {
                elements = SplitAndValidate.Run("random(" + min + "," + max + ")");
            }
            catch (Exception ex)
            {
                Assert.Fail("SplitAndValidate.Run() failed. " + ex.Message);
                return;
            }

            Calculator calculator = new Calculator(elements);

            for (int i = 0; i < iterations; i++)
            {
                Number answer;
                try
                {
                    answer = calculator.Run();
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                    return;
                }

                if (answer == max)
                    Assert.Pass();
                if (i == iterations - 1) Assert.Fail("Not max after " + iterations + " iterations.");
            }

            Assert.Fail("Not max after " + iterations + " iterations.");
        }
    }
}