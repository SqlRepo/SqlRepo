using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class StatementFactory : IStatementFactory
    {
        private readonly IStatementExecutor statementExecutor;
        private readonly IEntityMapper entityMapper;
        private readonly IWritablePropertyMatcher writablePropertyMatcher;

        public StatementFactory(IStatementExecutor statementExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher)
        {
            this.statementExecutor = statementExecutor;
            this.entityMapper = entityMapper;
            this.writablePropertyMatcher = writablePropertyMatcher;
        }

        public IDeleteStatement<TEntity> CreateDelete<TEntity>() where TEntity : class, new()
        {
            return new DeleteStatement<TEntity>(this.statementExecutor,
                entityMapper,
                new WhereClauseBuilder());
        }

        public IInsertStatement<TEntity> CreateInsert<TEntity>() where TEntity : class, new()
        {
            return new InsertStatement<TEntity>(this.statementExecutor,
                entityMapper,
                writablePropertyMatcher);
        }

        public ISelectStatement<TEntity> CreateSelect<TEntity>() where TEntity : class, new()
        {
            return new SelectStatement<TEntity>(this.statementExecutor,
                entityMapper);
        }

        public IUpdateStatement<TEntity> CreateUpdate<TEntity>() where TEntity : class, new()
        {
            return new UpdateStatement<TEntity>(this.statementExecutor,
                entityMapper,
                writablePropertyMatcher,
                new WhereClauseBuilder());
        }
    }
}