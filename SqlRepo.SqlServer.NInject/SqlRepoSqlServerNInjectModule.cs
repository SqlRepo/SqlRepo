using Ninject.Modules;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.NInject
{
    public class SqlRepoSqlServerNInjectModule: NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRepositoryFactory>()
                .To<RepositoryFactory>();
            this.Bind<IStatementFactoryProvider>()
                .To<StatementFactoryProvider>();
            this.Bind<IEntityMapper>()
                .To<DataReaderEntityMapper>();
            this.Bind<IWritablePropertyMatcher>()
                .To<WritablePropertyMatcher>();
            this.Bind<ISqlLogger>()
                .To<SqlLogger>();
        }
    }
}
