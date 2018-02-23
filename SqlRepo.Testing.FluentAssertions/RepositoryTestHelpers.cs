using NSubstitute;
using NSubstitute.Extensions;

namespace SqlRepo.Testing.FluentAssertions
{
    public static class RepositoryTestHelpers
    {
        public static ISelectCommand<T> CreateSelectCommandStub<T>(this IRepository<T> repository)
            where T : class, new()
        {
            var selectCommand = Substitute.For<ISelectCommand<T>>();
            selectCommand.ReturnsForAll(selectCommand);

            repository.Query().Returns(selectCommand);

            return selectCommand;
        }

        public static IInsertCommand<T> CreateInsertCommandStub<T>(this IRepository<T> repository)
            where T : class, new()
        {
            var insertCommand = Substitute.For<IInsertCommand<T>>();
            insertCommand.ReturnsForAll(insertCommand);

            repository.Insert().Returns(insertCommand);

            return insertCommand;
        }
    }
}