using System;
using System.Collections.Generic;
using EquationBuilder;
using EquationElements;
using NUnit.Framework;
using static UnitTests.BaseMethods;

namespace UnitTests
{
    [TestFixture]
    internal class ConstantsTests
    {
        static readonly object[] StackedConstantsTestCases =
        {
            new object[]
            {
                "a+b+c", new Number(21),

                new Dictionary<string, string>
                {
                    {"a", "1+b"},
                    {"b", "5"},
                    {"c", "10"}
                }
            }
        };

        [Test]
        [TestCaseSource(nameof(StackedConstantsTestCases))]
        public void StackedConstants(object[] currentCase)
        {
            TestBuilderAndCalculator(currentCase);
        }


        static readonly object[] HasIdenticalConstantTestCases =
        {
            new object[] {"emod", new Number(1)},
            new object[] {"mode", new Number(2)},
            new object[] {"emode", new Number(3)},
            new object[] {"5factorial", new Number(4)},
            new object[] {"factorial5", new Number(5)},
            new object[] {"afactoriala", new Number(6)}
        };

        [Test]
        [TestCaseSource(nameof(HasIdenticalConstantTestCases))]
        public void EquationIsIdenticalToAConstant(object[] currentCase)
        {
            TestBuilderAndCalculator(new[]
            {
                currentCase[0],
                currentCase[1],

                new Dictionary<string, string> {{(string) currentCase[0], currentCase[1].ToString()}}
            });
        }


        static readonly object[] NoMatchingConstantTestCases =
        {
            new object[]
            {
                "emod",
                BuilderExceptionMessages.LastElementInvalidDefault,
                BuilderExceptionMessages.LastElementInvalidBeforeParameter
            },
            new object[]
            {
                "mode",
                BuilderExceptionMessages.FirstElementInvalidDefault,
                BuilderExceptionMessages.FirstElementInvalidBeforeParameter
            }
        };

        [Test]
        [TestCaseSource(nameof(NoMatchingConstantTestCases))]
        public void ShouldFailWhenNotAConstant(object[] currentCase)
        {
            ShouldThrowException(currentCase);
        }


        static readonly object[] LookLikeModShouldPassTestCases =
        {
            new object[]
            {
                "4mod*3",
                new Number(9 * 3),
                new Dictionary<string, string> {{"4mod", "9"}}
            },

            new object[]
            {
                "5mod+5",
                new Number(9 + 5),
                new Dictionary<string, string> {{"5mod", "9"}}
            },

            new object[]
            {
                "6mod-6",
                new Number(11 - 6),
                new Dictionary<string, string> {{"6mod", "11"}}
            }
        };

        [Test]
        [TestCaseSource(nameof(LookLikeModShouldPassTestCases))]
        public void LookLikeModShouldPass(object[] currentCase)
        {
            TestBuilderAndCalculator(currentCase);
        }

        [Test]
        public void ConstantInValidator()
        {
            string actual;
            const string expected = "1*(1+1)*1";

            try
            {
                LinkedList<BaseElement> elements = new LinkedList<BaseElement>();
                elements.AddLast(new Number(1));
                elements.AddLast(new Constant("a", "1+1"));
                elements.AddLast(new Number(1));
                ICollection<BaseElement> validatedElements = new Validator().Run(elements);
                actual = string.Join(null, validatedElements);
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw exception " + ex.Message);
                return;
            }

            if (actual == expected)
                Assert.Pass();
            else
                Assert.Fail("Expected " + expected + " but was " + actual);
        }
    }
}