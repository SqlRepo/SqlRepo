using System;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class CommandFactoryShould
    {
        [SetUp]
        public void SetUp()
        {
            AssumeCommandExecutorIsInitialised();
            AssumeEntityMapperIsInitialised();
            AssumeWritableProperyMatcherIsInitialised();
            commandFactory = new CommandFactory(commandExecutor,
                entityMapper,
                writablePropertyMatcher);
        }

        private ICommandExecutor commandExecutor;
        private ICommandFactory commandFactory;
        private IEntityMapper entityMapper;
        private IWritablePropertyMatcher writablePropertyMatcher;

        private void AssumeCommandExecutorIsInitialised()
        {
            commandExecutor = Substitute.For<ICommandExecutor>();
        }

        private void AssumeEntityMapperIsInitialised()
        {
            entityMapper = Substitute.For<IEntityMapper>();
        }

        private void AssumeWritableProperyMatcherIsInitialised()
        {
            writablePropertyMatcher = Substitute.For<IWritablePropertyMatcher>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForDelete()
        {
            commandFactory.Invoking(e => e.CreateDelete<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForInsert()
        {
            commandFactory.Invoking(e => e.CreateInsert<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForSelect()
        {
            commandFactory.Invoking(e => e.CreateSelect<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForUpdate()
        {
            commandFactory.Invoking(e => e.CreateUpdate<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void ReturnDeleteOnRequest()
        {
            var result = commandFactory.CreateDelete<TestEntity>();
            result.Should()
                .NotBeNull();
            result.Should()
                .BeAssignableTo<ISqlCommand<int>>();
            result.Should()
                .BeAssignableTo<DeleteCommand<TestEntity>>();
        }

        [Test]
        public void ReturnInsertOnRequest()
        {
            var result = commandFactory.CreateInsert<TestEntity>();
            result.Should()
                .NotBeNull();
            result.Should()
                .BeAssignableTo<ISqlCommand<TestEntity>>();
            result.Should()
                .BeAssignableTo<InsertCommand<TestEntity>>();
        }

        [Test]
        public void ReturnSelectOnRequest()
        {
            var result = commandFactory.CreateSelect<TestEntity>();
            result.Should()
                .NotBeNull();
            result.Should()
                .BeAssignableTo<IClauseBuilder>();
            result.Should()
                .BeAssignableTo<SelectCommand<TestEntity>>();
        }

        [Test]
        public void ReturnUpdateOnRequest()
        {
            var result = commandFactory.CreateUpdate<TestEntity>();
            result.Should()
                .NotBeNull();
            result.Should()
                .BeAssignableTo<ISqlCommand<int>>();
            result.Should()
                .BeAssignableTo<UpdateCommand<TestEntity>>();
        }
    }
}