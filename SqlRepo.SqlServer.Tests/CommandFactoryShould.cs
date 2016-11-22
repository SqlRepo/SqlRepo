using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class CommandFactoryShould
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeCommandExecutorIsInitialised();
            this.AssumeEntityMapperIsInitialised();
            this.AssumeWritableProperyMatcherIsInitialised();
            this.commandFactory = new CommandFactory(this.commandExecutor,
                this.entityMapper,
                this.writablePropertyMatcher);
        }

        [Test]
        public void ReturnInsertOnRequest()
        {
            var result = this.commandFactory.CreateInsert<TestEntity>();
            result.Should()
                  .NotBeNull();
            result.Should()
                  .BeAssignableTo<ISqlCommand<TestEntity>>();
            result.Should()
                  .BeAssignableTo<InsertCommand<TestEntity>>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForInsert()
        {
            this.commandFactory.Invoking(e => e.CreateInsert<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void ReturnSelectOnRequest()
        {
            var result = this.commandFactory.CreateSelect<TestEntity>();
            result.Should()
                  .NotBeNull();
            result.Should()
                  .BeAssignableTo<IClauseBuilder>();
            result.Should()
                  .BeAssignableTo<SelectCommand<TestEntity>>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForSelect()
        {
            this.commandFactory.Invoking(e => e.CreateSelect<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void ReturnUpdateOnRequest()
        {
            var result = this.commandFactory.CreateUpdate<TestEntity>();
            result.Should()
                  .NotBeNull();
            result.Should()
                  .BeAssignableTo<ISqlCommand<int>>();
            result.Should()
                  .BeAssignableTo<UpdateCommand<TestEntity>>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForUpdate()
        {
            this.commandFactory.Invoking(e => e.CreateUpdate<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void ReturnDeleteOnRequest()
        {
            var result = this.commandFactory.CreateDelete<TestEntity>();
            result.Should()
                  .NotBeNull();
            result.Should()
                  .BeAssignableTo<ISqlCommand<int>>();
            result.Should()
                  .BeAssignableTo<DeleteCommand<TestEntity>>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForDelete()
        {
            this.commandFactory.Invoking(e => e.CreateDelete<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        private ICommandExecutor commandExecutor;
        private ICommandFactory commandFactory;
        private IEntityMapper entityMapper;
        private IWritablePropertyMatcher writablePropertyMatcher;

        private void AssumeCommandExecutorIsInitialised()
        {
            this.commandExecutor = Substitute.For<ICommandExecutor>();
        }

        private void AssumeEntityMapperIsInitialised()
        {
            this.entityMapper = Substitute.For<IEntityMapper>();
        }

        private void AssumeWritableProperyMatcherIsInitialised()
        {
            this.writablePropertyMatcher = Substitute.For<IWritablePropertyMatcher>();
        }
    }
}