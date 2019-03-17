using System;
using Ninject.Modules;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer.Ninject
{
    public class SqlRepoSqlServerNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IEntityActivatorFactory>()
                .To<EntityActivatorFactory>()
                .InSingletonScope();
            this.Bind<IEntityMapperDefinitionProvider>()
                .To<EntityMapperDefinitionProvider>()
                .InSingletonScope();
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