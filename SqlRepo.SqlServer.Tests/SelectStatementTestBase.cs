using SqlRepo.Testing;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.Tests
{
    public abstract class SelectStatementTestBase :
        SqlStatementTestBase<SelectStatement<TestEntity>, IEnumerable<TestEntity>>
    {
        protected const string IdPropertyName = "Id";
        protected const string InnerEntityName = "InnerEntity";
        protected const string IntPropertyName = "IntProperty";
        protected const string StringPropertyName = "StringProperty";
        protected const string TestEntityName = "TestEntity";

        protected override SelectStatement<TestEntity> CreateStatement(IStatementExecutor statementExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            ISqlConnectionProvider connectionProvider)
        {
            var statement = new SelectStatement<TestEntity>(statementExecutor, entityMapper);
            statement.UseConnectionString(connectionProvider);
            return statement;
        }
    }
}