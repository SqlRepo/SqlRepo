using System;
using System.Collections.Generic;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer.Static
{
    public static class RepoFactory
    {
        private static readonly WritablePropertyMatcher WritablePropertyMatcher =
            new WritablePropertyMatcher();
        private static readonly DataReaderEntityMapper DataReaderEntityMapper = new DataReaderEntityMapper();
        private static IRepositoryFactory repositoryFactory;
        private static ISqlLogWriter sqlLogWriter;
        private static IConnectionProvider connectionProvider;

        public static IRepository<TEntity> Create<TEntity>()
            where TEntity: class, new()
        {
            EnsureRepositoryFactoryInstance();
            return repositoryFactory.Create<TEntity>();
        }

        public static void UseConnectionProvider(IConnectionProvider connectionProvider)
        {
            RepoFactory.connectionProvider = connectionProvider;
        }

        public static void UseLogger(ISqlLogWriter sqlLogWriter)
        {
            RepoFactory.sqlLogWriter = sqlLogWriter;
            EnsureRepositoryFactoryInstance();
        }

        private static void EnsureRepositoryFactoryInstance()
        {
            if(repositoryFactory != null)
            {
                return;
            }

            if(connectionProvider == null)
            {
                throw new InvalidOperationException(
                    "Create cannot be used until an IConnectionProvider has been set, have you forgotten to call UseConnectionProvider(...)");
            }

            if(sqlLogWriter == null)
            {
                sqlLogWriter = new NoOpSqlLogger();
            }

            var statementFactoryProvider = new StatementFactoryProvider(DataReaderEntityMapper,
                WritablePropertyMatcher,
                connectionProvider,
                new SqlLogger(new List<ISqlLogWriter>
                              {
                                  sqlLogWriter
                              }));

            repositoryFactory = new RepositoryFactory(statementFactoryProvider);
        }
    }
}