using System;
using FluentAssertions;
using NUnit.Framework;

namespace SqlRepo.Testing.Tests
{
    [TestFixture]
    public class ExpressionHelperShould
    {
        [SetUp]
        public void SetUp()
        {
            this.target = new ExpressionHelper();
        }

        private ExpressionHelper target;
    }
}