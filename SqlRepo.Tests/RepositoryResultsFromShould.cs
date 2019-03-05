﻿using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryResultsFromShould : RepositoryTestBase
    {
        [Test]
        public void ExecuteQuery()
        {
            var query = Substitute.For<ISelectStatement<TestEntity>>();
            this.AssumeResultsFromIsRequested(query);
            query.Received()
                 .Go();
        }

        [Test]
        public void ReturnResultsFromQuery()
        {
            var query = Substitute.For<ISelectStatement<TestEntity>>();
            var expected = new[] {new TestEntity(), new TestEntity()};
            query.Go()
                 .Returns(expected);
            var actual = this.AssumeResultsFromIsRequested(query);
            actual.Should()
                  .BeEquivalentTo(expected);
        }
    }
}