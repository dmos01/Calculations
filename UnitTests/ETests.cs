using System;
using System.Collections.Generic;
using EquationBuilder;
using EquationElements;
using EquationElements.Functions;
using EquationElements.Operators;
using NUnit.Framework;
using static UnitTests.BaseMethods;

namespace UnitTests
{
    [TestFixture]
    public class ETests
    {
        static readonly object[] ShouldFailTestCases =
        {
            new object[]
            {
                "5E308", ElementsExceptionMessages.ExponentTooLargeOrSmallDefault,
                ElementsExceptionMessages.ExponentTooLargeOrSmallBeforeParameter
            },
            new object[]
            {
                "5E-308", ElementsExceptionMessages.ExponentTooLargeOrSmallDefault,
                ElementsExceptionMessages.ExponentTooLargeOrSmallBeforeParameter
            },
            new object[]
            {
                "5E+308", ElementsExceptionMessages.ExponentTooLargeOrSmallDefault,
                ElementsExceptionMessages.ExponentTooLargeOrSmallBeforeParameter
            },
            new object[]
            {
                "5E-E", ElementsExceptionMessages.ExponentIsNotIntegerDefault,
                ElementsExceptionMessages.ExponentIsNotIntegerBeforeParameter
            },
            new object[]
            {
                "23E23E23", BuilderExceptionMessages.InvalidUseOfExponentEDefault,
                ElementsExceptionMessages.ExponentIsNotIntegerDefault,
                ElementsExceptionMessages.ExponentIsNotIntegerBeforeParameter
            },
            new object[]
            {
                "5E+E", ElementsExceptionMessages.ExponentIsNotIntegerDefault,
                ElementsExceptionMessages.ExponentIsNotIntegerBeforeParameter
            },
            new object[]
            {
                "5EAbs(5)", BuilderExceptionMessages.InvalidUseOfExponentEDefault
            }
        };

        [Test]
        [TestCaseSource(nameof(ShouldFailTestCases))]
        public void ShouldFail(object[] currentCase) => ShouldThrowException(currentCase);


        static readonly object[] ShouldPassTestCases =
        {
            new object[] {"log(e,5)", new Number(Math.Log(5))},
            new object[] {"ln(5)", new Number(Math.Log(5))},
            new object[] {"E", new Number(Math.E)},
            new object[] {"-E", new Number(0 - Math.E)},
            new object[] {"E+1", new Number(Math.E + 1)},
            new object[] {"-E+1", new Number(0 - Math.E + 1)},
            new object[] {"1+E", new Number(1 + Math.E)},
            new object[] {"1+-E", new Number(1 - Math.E)},
            new object[] {"5*E-E", new Number(5 * Math.E - Math.E)},
            new object[] {"5*E+E", new Number(6 * Math.E)},
            new object[] {"5-E0", new Number(5)},
            new object[] {"5E-0", new Number(5 * Math.Pow(10, 0))},
            new object[] {"5+E0", new Number(5e0)},
            new object[] {"5E27", new Number(5e27)},
            new object[] {"5E-27", new Number(5e-27)},
            new object[] {"5E+27", new Number(5e27)},
            new object[] {"5E28", new Number(5e28)},
            new object[] {"5E-28", new Number(5e-28)},
            new object[] {"5E+28", new Number(5e28)},
            new object[] {"5E307", new Number(5e307)},
            new object[] {"5E-307", new Number(5e-307)},
            new object[] {"5E+307", new Number(5e307)},
            new object[] {"5*EE", new Number(5 * Math.E * Math.E)},
            new object[] {"EE", new Number(Math.E * Math.E)},
            new object[] {"-EE", new Number(0 - Math.E * Math.E)},
            new object[] {"EE-5", new Number(Math.E * Math.Pow(10, -5))},
            new object[] {"EE+5", new Number(Math.E * Math.Pow(10, 5))},
            new object[] {"E*E-5", new Number(Math.E * Math.E - 5)},
            new object[] {"E*E+5", new Number(Math.E * Math.E + 5)},
            new object[] {"EE5", new Number(Math.E * Math.Pow(10, 5))},
            new object[] {"E*E5", new Number(Math.E * Math.E * 5)},
            new object[] {"5EE", new Number(5 * Math.E * Math.E)},
            new object[] {"EEE", new Number(Math.E * Math.E * Math.E)},
        };

        [Test]
        [TestCaseSource(nameof(ShouldPassTestCases))]
        public void ShouldPass(object[] currentCase) => TestBuilderAndCalculator(currentCase);
    }
}