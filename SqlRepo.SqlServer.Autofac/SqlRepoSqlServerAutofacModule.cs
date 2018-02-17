using System;
using Autofac;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.Autofac
{
    public class SqlRepoSqlServerAutofacModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<RepositoryFactory>()
                            .As<IRepositoryFactory>();
            containerBuilder.RegisterType<StatementExecutor>()
                            .As<IStatementExecutor>();
            containerBuilder.RegisterType<StatementFactory>()
                            .As<IStatementFactory>();
            containerBuilder.RegisterType<DataReaderEntityMapper>()
                            .As<IEntityMapper>();
            containerBuilder.RegisterType<WritablePropertyMatcher>()
                            .As<IWritablePropertyMatcher>();
        }
    }
}