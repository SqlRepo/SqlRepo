using System;
using Autofac;

namespace SqlRepo.SqlServer.Autofac
{
    public class DataAutofacModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<RepositoryFactory>()
                            .As<IRepositoryFactory>();
            containerBuilder.RegisterType<CommandExecutor>()
                            .As<ICommandExecutor>();
            containerBuilder.RegisterType<CommandFactory>()
                            .As<ICommandFactory>();
            containerBuilder.RegisterType<DataReaderEntityMapper>()
                            .As<IEntityMapper>();
            containerBuilder.RegisterType<WritablePropertyMatcher>()
                            .As<IWritablePropertyMatcher>();
        }
    }
}