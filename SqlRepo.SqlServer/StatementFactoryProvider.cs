using System;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class StatementFactoryProvider : IStatementFactoryProvider
    {
        private readonly IConnectionProvider connectionProvider;
        private readonly IEntityMapper entityMapper;
        private readonly ISqlLogger sqlLogger;
        private readonly IWritablePropertyMatcher writablePropertyMatcher;

        public StatementFactoryProvider(IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            IConnectionProvider connectionProvider,
            ISqlLogger sqlLogger)
        {
            this.entityMapper = entityMapper;
            this.writablePropertyMatcher = writablePropertyMatcher;
            this.connectionProvider = connectionProvider;
            this.sqlLogger = sqlLogger;
        }

        public IStatementFactory Provide()
        {
            return new StatementFactory(this.sqlLogger,
                this.connectionProvider,
                this.entityMapper,
                this.writablePropertyMatcher);
        }
    }
}