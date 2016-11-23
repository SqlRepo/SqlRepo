using SqlRepo.Testing;
using System.Collections.Generic;

namespace SqlRepo.SqlServer.Tests
{
    public abstract class SelectCommandTestBase :
        SqlCommandTestBase<SelectCommand<TestEntity>, IEnumerable<TestEntity>>
    {
        protected const string IdPropertyName = "Id";
        protected const string InnerEntityName = "InnerEntity";
        protected const string IntPropertyName = "IntProperty";
        protected const string StringPropertyName = "StringProperty";
        protected const string TestEntityName = "TestEntity";

        protected override SelectCommand<TestEntity> CreateCommand(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            string connectionString)
        {
            var command = new SelectCommand<TestEntity>(commandExecutor, entityMapper);
            command.UseConnectionString(connectionString);
            return command;
        }
    }
}