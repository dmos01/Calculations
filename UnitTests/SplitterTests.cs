using System;
using System.Collections.Generic;
using EquationBuilder;
using EquationElements;
using NUnit.Framework;

// ReSharper disable IdentifierTypo

namespace UnitTests
{
    public class SplitterTests
    {
        static readonly object[] FirstLoopUnrecognizedElementPossibilityCases =
        {
            new object[] {"1mod2", new Number(1 % 2)},
            new object[] {"3e4", new Number(3E4)},
            new object[] {"5fox6", new Number(5 * 2 * 6), new Dictionary<string, string> {{"fox", "2"}}},
            new object[] {"7foxmod8", new Number(7 * 2 * 8), new Dictionary<string, string> {{"foxmod", "2"}}},
            new object[] {"9foxmod10", new Number(9 * 2 % 10), new Dictionary<string, string> {{"fox", "2"}}},
            new object[] {"11foxmod12", new Number(5 % 12), new Dictionary<string, string> {{"11fox", "5"}}},
            new object[] {"13mode14", new Number(13 % 5), new Dictionary<string, string> {{"e14", "5"}}},
            new object[]
            {
                "17emode18", new Number(6 % 4),
                new Dictionary<string, string>
                {
                    {"17e", "6"}, {"e18", "4"}
                }
            },
            new object[]
            {
                "19amodb20cd", new Number(6 % 4),
                new Dictionary<string, string>
                {
                    {"19a", "6"},
                    {
                        "b20cd", "4"
                    }
                }
            },

            new object[]
            {
                "foxmodQ", new Number(6 % 4),
                new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
                {
                    {"fox", "6"}, {"q", "4"}
                }
            },

            new object[] {"21e", new Number(21 * Math.E)},
            new object[] {"e22", new Number(Math.E * 22)},
            new object[] {"21emode22", new Number(21 * Math.E % Math.E * 22)},

            new object[] {"23e*e24", new Number(5 * Math.E * 24), new Dictionary<string, string> {{"23e", "5"}}},

            new object[] {"25ee26", new Number(25 * Math.E * Math.Pow(10, 26))}
        };

        static readonly object[] SecondLoopUnrecognizedElementPossibilityCases =
        {
            new object[] {"fox", new Number(2), new Dictionary<string, string> {{"fox", "2"}}},

            new object[]
            {
                "foxmod", new Number(2), new Dictionary<string, string> {{"foxmod", "2"}}
            },

            new object[]
            {
                "mode", new Number(5), new Dictionary<string, string> {{"mode", "5"}}
            },
            new object[]
            {
                "emode", new Number(6 % 6),
                new Dictionary<string, string> {{"e", "6"}}
            },
            new object[] {"emode", new Number(Math.E % Math.E)}
        };

        static readonly object[] KnownBugCases =
        {
            new object[]
            {
                "23mode24", new Number(6 * 4),
                new Dictionary<string, string>
                {
                    {"23mod", "6"}, {"e24", "4"}
                }
            }
        };

        /// <summary>
        /// Single-Element Constants on both sides are valid.
        /// </summary>
        static readonly object[] UndeterminedUsesOfECases =
        {
            new object[]
            {
                "fez",
                new Dictionary<string, string>
                {
                    {"f", "1"}, {"z", "3+4"}
                }
            },
            new object[] {"15mode16", new Dictionary<string, string> {{"15mod", "1+2"}}}
        };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCaseSource(nameof(FirstLoopUnrecognizedElementPossibilityCases))]
        public void FirstLoopUnrecognizedElementPossibilities(object[] args) => BaseMethods.TestBuilderAndCalculator(args);


        [Test]
        [TestCaseSource(nameof(SecondLoopUnrecognizedElementPossibilityCases))]
        public void SecondLoopUnrecognizedElementPossibilities(object[] args) => BaseMethods.TestBuilderAndCalculator(args);

        [Test]
        [TestCaseSource(nameof(KnownBugCases))]
        public void KnownBugs(object[] args) => BaseMethods.TestBuilderAndCalculator(args);


        [Test]
        [TestCaseSource(nameof(UndeterminedUsesOfECases))]
        public void UndeterminedUsesOfE(object[] args)
        {
            if (EquationIsValid.Run((string) args[0], (Dictionary<string, string>) args[1]))
                Assert.Fail((string) args[0] + " is actually valid.");
            else
                Assert.Pass();
        }

        #region First loop UnrecognizedElement possibilities:

        /* 
         * 1mod2
         * 3e4
         * fez
         * 5fox6
         * 7foxmod8
         * 9mode10
         * 11emode12
         * 13amodb14cd
         * foxmodQ
         * foxsin

           Will not be mod, pi, functions, constants or numbers (on their own).
        */

        #endregion

        #region First loop mod and e calls enable:

        /*
         * 1mod2
         * 3e4
         * fez where f and z are constants.
         * 7foxmod8 where 7fox is a constant.
         * 9mode10 where e10 is a constant.
         * 9mode10 where 9mod is a constant.
         * 11emode12 where 11e and e12 are constants.
         * 11emode12 (11 e mod e 12).
         * 13amodb14cd where 13a and b14cd are constants.
         * foxmodQ where fox and Q are constants.
         */

        #endregion

        #region Second loop Constants test enables:

        /*
         * 5fox6 where fox is a constant.
         * 7foxmod8 where foxmod is a constant.
         * 9mode10 where mode is a constant.
         * 13amodb14cd where amodb and cd are constants.
         */

        #endregion

        #region Second loop UnrecognizedElement possibilities:

        /*
         * fez
         * fox
         * foxmod
         * mode
         * emode ?
         * amodb
         * cd
         * foxmodQ where foxmodQ, fox and Q are not constants.
         * foxsin

           Will not be mod, pi, functions or constants (on their own).
        */

        #endregion

        #region Second loop mod and e calls enable:

        /*
         * foxmod where fox is a constant.
         * mode where e is a constant, mod is a constant or neither is a constant.
         * amodb where a and b are constants.
         */

        #endregion
    }
}