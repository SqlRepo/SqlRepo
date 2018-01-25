using Ninject.Modules;

namespace SqlRepo.SqlServer.NInject
{
    public class SqlRepoSqlServerNInjectModule: NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRepositoryFactory>()
                .To<RepositoryFactory>();
            this.Bind<ICommandExecutor>()
                .To<CommandExecutor>();
            this.Bind<ICommandFactory>()
                .To<CommandFactory>();
            this.Bind<IEntityMapper>()
                .To<DataReaderEntityMapper>();
            this.Bind<IWritablePropertyMatcher>()
                .To<WritablePropertyMatcher>();
        }
    }
}
