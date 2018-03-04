using System;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;

namespace SqlRepo.Tests {
    [TestFixture]
    public class RepositoryUseConnectionProviderShould : RepositoryTestBase
    {
        [Test]
        public void SetConnectionProviderOnStatementFactory()
        {
            var connectionProvider = Substitute.For<IConnectionProvider>();
            this.Repository.UseConnectionProvider(connectionProvider);
            this.StatementFactory.Received()
                .UseConnectionProvider(connectionProvider);
        }
    }
}