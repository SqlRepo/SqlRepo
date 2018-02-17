using Ninject.Modules;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.NInject
{
    public class SqlRepoSqlServerNInjectModule: NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRepositoryFactory>()
                .To<RepositoryFactory>();
            this.Bind<IStatementExecutor>()
                .To<StatementExecutor>();
            this.Bind<IStatementFactory>()
                .To<StatementFactory>();
            this.Bind<IEntityMapper>()
                .To<DataReaderEntityMapper>();
            this.Bind<IWritablePropertyMatcher>()
                .To<WritablePropertyMatcher>();
        }
    }
}
