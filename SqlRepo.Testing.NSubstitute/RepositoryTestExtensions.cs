using System;
using NSubstitute;
using NSubstitute.Extensions;

namespace SqlRepo.Testing.NSubstitute
{
    public static class RepositoryTestExtensions
    {
        public static IDeleteCommand<T> CreateDeleteCommandSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var deleteCommand = Substitute.For<IDeleteCommand<T>>();
            deleteCommand.ReturnsForAll(deleteCommand);

            repository.Delete()
                      .Returns(deleteCommand);

            return deleteCommand;
        }

        public static IInsertCommand<T> CreateInsertCommandSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var insertCommand = Substitute.For<IInsertCommand<T>>();
            insertCommand.ReturnsForAll(insertCommand);

            repository.Insert()
                      .Returns(insertCommand);

            return insertCommand;
        }

        public static ISelectCommand<T> CreateSelectCommandSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var selectCommand = Substitute.For<ISelectCommand<T>>();
            selectCommand.ReturnsForAll(selectCommand);

            repository.Query()
                      .Returns(selectCommand);

            return selectCommand;
        }

        public static IUpdateCommand<T> CreateUpdateCommandSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var updateCommand = Substitute.For<IUpdateCommand<T>>();
            updateCommand.ReturnsForAll(updateCommand);

            repository.Update()
                      .Returns(updateCommand);

            return updateCommand;
        }
    }
}