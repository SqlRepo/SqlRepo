using System;
using System.Collections.Generic;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.ConnectionProviders;

namespace SqlRepo.SqlServer.IntegrationTests.Examples
{
    public abstract class ExampleBase
    {
        protected RepositoryFactory RepositoryFactory { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.RepositoryFactory = new RepositoryFactory(new StatementFactoryProvider(
                new DataReaderEntityMapper(),
                new WritablePropertyMatcher(),
                new AppConfigFirstConnectionProvider(),
                new SqlLogger(new List<ISqlLogWriter>
                              {
                                  new ConsoleSqlLogger()
                              })));
        }
    }
}