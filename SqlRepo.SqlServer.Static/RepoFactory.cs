using System;

namespace SqlRepo.SqlServer.Static
{
    public static class RepoFactory
    {
        private static IRepositoryFactory repositoryFactory;
        private static ISqlLogger sqlLogger;
        private static readonly WritablePropertyMatcher WritablePropertyMatcher = new WritablePropertyMatcher();
        private static readonly DataReaderEntityMapper DataReaderEntityMapper = new DataReaderEntityMapper();

        public static IRepository<TEntity> Create<TEntity>()
            where TEntity: class, new()
        {
            EnsureRepositoryFactoryInstance();
            return repositoryFactory.Create<TEntity>();
        }

        private static void EnsureRepositoryFactoryInstance()
        {
            if(repositoryFactory != null)
            {
                return;
            }

            if(sqlLogger == null)
            {
                sqlLogger = new NoOpSqlLogger();
            }

            repositoryFactory = new RepositoryFactory(new CommandFactory(new CommandExecutor(sqlLogger),
                DataReaderEntityMapper,
                WritablePropertyMatcher));
        }

        public static void UseLogger(ISqlLogger sqlLogger)
        {
            RepoFactory.sqlLogger = sqlLogger;
            EnsureRepositoryFactoryInstance();
        }
    }
}
