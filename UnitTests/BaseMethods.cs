using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EquationBuilder;
using EquationCalculator;
using EquationElements;
using NumberFormats;
using NUnit.Framework;

namespace UnitTests
{
    [SuppressMessage("ReSharper", "LocalizableElement")]
    internal class BaseMethods
    {
        static ICollection<BaseElement> elements;

        /// <summary>
        ///     <para>0 = Equation as string.</para>
        ///     <para>1 = Expected answer as Number.</para>
        ///     <para>2 (optional) = User-defined constants as Dictionary(string, string).</para>
        /// </summary>
        /// <param name="args"></param>
        public static bool TestBuilderAndCalculator(object[] args)
        {
            if (TestElementBuilder(args) == false)
                return false;

            if (TestCalculator(args) == false)
                return false;

            if (TestEquationIsValid(args) == false)
                return false;

            return true;
        }

        public static bool TestEquationIsValid(object[] args)
        {
            switch (args.Length)
            {
                case 2:
                    if (EquationIsValid.Run((string) args[0]) == false)
                    {
                        Assert.Fail("EquationIsValid failed " + (string) args[0] + ".");
                        return false;
                    }

                    break;

                case 3:
                    if (EquationIsValid.Run((string) args[0], (Dictionary<string, string>) args[2]) == false)
                    {
                        Assert.Fail("EquationIsValid failed " + (string) args[0] + ".");
                        return false;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(null, "Wrong number of arguments.");
            }

            return true;
        }

        private static bool TestElementBuilder(object[] args)
        {
            try
            {
                elements = args.Length switch
                {
                    2 => SplitAndValidate.Run((string) args[0]),
                    3 => SplitAndValidate.Run((string) args[0], (Dictionary<string, string>) args[2]),
                    _ => throw new ArgumentOutOfRangeException(null, "Wrong number of arguments.")
                };

                return true;
            }
            catch (Exception ex1)
            {
                Assert.Fail("SplitAndValidate().Run() failed " + (string) args[0] + ". " + ex1.Message);
                return false;
            }
        }

        private static bool TestCalculator(object[] args)
        {
            Number actualAnswer;
            try
            {
                actualAnswer = new Calculator(elements).Run();
            }
            catch (Exception ex2)
            {
                Assert.Fail("Calculator failed " + (string) args[0] + ". " + ex2.Message);
                return false;
            }

            if (actualAnswer != (Number) args[1])
            {
                Assert.Fail("Calculator for " + (string) args[0] + " returned wrong answer. It returned " +
                            actualAnswer + ". Expected " + (Number) args[1] + ".");
                return false;
            }

            return true;
        }


        /// <summary>
        ///     <para>0 = Equation (string).</para>
        ///     <para>1+ = Acceptable messages to be thrown.</para>
        /// </summary>
        /// <param name="args"></param>
        public static void ShouldThrowException(object[] args)
        {
            Exception thrownException = null;
            Number answer = new(int.MaxValue);

            try
            {
                elements = SplitAndValidate.Run((string) args[0]);
                answer = new Calculator(elements).Run();
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            if (thrownException is null)
            {
                Assert.Fail("No exception was thrown for " + (string) args[0] + ". Returned " + answer + ".");
                return;
            }

            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case null when thrownException is ArgumentNullException:
                        Assert.Pass();
                        return;
                    case string expectedMessage when thrownException.Message.StartsWith(expectedMessage):
                        Assert.Pass("Correctly threw " + expectedMessage);
                        return;
                }
            }

            Assert.Fail("Actual exception message was " + thrownException.Message);
        }


        /// <summary>
        ///     <para>0 = Number.</para>
        ///     <para>1 = Expected two DP display.</para>
        ///     <para>2 = Expected top-heavy fraction display.</para>
        ///     <para>3 = Expected mixed-fraction display.</para>
        ///     <para>4 (optional) = Expected top-heavy IsApproximately display.</para>
        /// </summary>
        /// <param name="args"></param>
        public static void FractionConversionTest(string[] args)
        {
            Number number = new(args[0]);
            string actual;

            try
            {
                actual = new DecimalNumber2DPNumberFormat().Display(number);
            }
            catch (Exception ex)
            {
                Assert.Fail(args[0] + " to 2dp failed. " + ex.Message);
                return;
            }

            if (actual != args[1])
            {
                Assert.Fail("Wrong answer for " + args[0] + " to 2dp. Expected " + args[1] + " but was " + actual);
                return;
            }

            try
            {
                actual = new TopHeavyFractionNumberFormat().Display(number);
            }
            catch (Exception ex)
            {
                Assert.Fail(args[0] + " to top-heavy fraction failed. " + ex.Message);
                return;
            }

            if (args.Length == 5)
            {
                if (actual != args[4])
                {
                    Assert.Fail("Wrong answer for " + args[0] +
                                " to top-heavy fraction IsApproximately. Expected " + args[4] + " but was " +
                                actual);
                    return;
                }
            }
            else
            {
                if (actual != args[2])
                {
                    Assert.Fail("Wrong answer for " + args[0] + " to top-heavy fraction. Expected " + args[2] +
                                " but was " + actual);
                    return;
                }
            }

            try
            {
                actual = new MixedFractionNumberFormat().Display(number);
            }
            catch (Exception ex)
            {
                Assert.Fail(args[0] + " to mixed-fraction failed. " + ex.Message);
                return;
            }

            if (actual != args[3])
            {
                Assert.Fail("Wrong answer for " + args[0] + " to mixed-fraction. Expected " + args[3] + " but was " +
                            actual);
                return;
            }

            Assert.Pass();
        }
    }
}