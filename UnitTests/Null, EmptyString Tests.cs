﻿using EquationBuilder;
using NUnit.Framework;
using static UnitTests.BaseMethods;

namespace UnitTests
{
    [TestFixture]
    internal class NullOrEmptyEquationTests
    {
        [Test]
        public void Null() =>
            ShouldThrowException(
                new object[]
                {
                    null,
                    null,
                    BuilderExceptionMessages.NoEquationDefault
                });

        [Test]
        public void Empty() =>
            ShouldThrowException(
                new object[]
                {
                    "",
                    null,
                    BuilderExceptionMessages.NoEquationDefault
                });
    }
}