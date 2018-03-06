using System;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class StatementFactory : IStatementFactory
    {
        private readonly IEntityMapper entityMapper;
        private readonly ISqlLogger sqlLogger;
        private readonly IWritablePropertyMatcher writablePropertyMatcher;
        private IConnectionProvider connectionProvider;

        public StatementFactory(ISqlLogger sqlLogger,
            IConnectionProvider connectionProvider,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher)
        {
            this.sqlLogger = sqlLogger;
            this.connectionProvider = connectionProvider;
            this.entityMapper = entityMapper;
            this.writablePropertyMatcher = writablePropertyMatcher;
        }

        public IDeleteStatement<TEntity> CreateDelete<TEntity>()
            where TEntity: class, new()
        {
            return new DeleteStatement<TEntity>(this.CreateStatementExecutor(),
                this.entityMapper,
                new WhereClauseBuilder());
        }

        public IExecuteNonQueryProcedureStatement CreateExecuteNonQueryProcedure()
        {
            return new ExecuteNonQueryProcedureStatement(this.CreateStatementExecutor());
        }

        public IExecuteNonQuerySqlStatement CreateExecuteNonQuerySql()
        {
            return new ExecuteNonQuerySqlStatement(this.CreateStatementExecutor());
        }

        public IExecuteQueryProcedureStatement<TEntity> CreateExecuteQueryProcedure<TEntity>()
            where TEntity: class, new()
        {
            return new ExecuteQueryProcedureStatement<TEntity>(this.CreateStatementExecutor(),
                this.entityMapper);
        }

        public IExecuteQuerySqlStatement<TEntity> CreateExecuteQuerySql<TEntity>()
            where TEntity: class, new()
        {
            return new ExecuteQuerySqlStatement<TEntity>(this.CreateStatementExecutor(), this.entityMapper);
        }

        public IInsertStatement<TEntity> CreateInsert<TEntity>()
            where TEntity: class, new()
        {
            return new InsertStatement<TEntity>(this.CreateStatementExecutor(),
                this.entityMapper,
                this.writablePropertyMatcher);
        }

        public ISelectStatement<TEntity> CreateSelect<TEntity>()
            where TEntity: class, new()
        {
            return new SelectStatement<TEntity>(this.CreateStatementExecutor(), this.entityMapper);
        }

        public IUpdateStatement<TEntity> CreateUpdate<TEntity>()
            where TEntity: class, new()
        {
            return new UpdateStatement<TEntity>(this.CreateStatementExecutor(),
                this.entityMapper,
                this.writablePropertyMatcher,
                new WhereClauseBuilder());
        }

        public IStatementFactory UseConnectionProvider(IConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
            return this;
        }

        private IStatementExecutor CreateStatementExecutor()
        {
            return new StatementExecutor(this.sqlLogger, this.connectionProvider);
        }
    }
}