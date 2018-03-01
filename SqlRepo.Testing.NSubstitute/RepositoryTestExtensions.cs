using System;
using NSubstitute;
using NSubstitute.Extensions;

namespace SqlRepo.Testing.NSubstitute
{
    public static class RepositoryTestExtensions
    {
        public static IDeleteStatement<T> CreateDeleteStatementSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var deleteStatement = Substitute.For<IDeleteStatement<T>>();
            deleteStatement.ReturnsForAll(deleteStatement);

            repository.Delete()
                      .Returns(deleteStatement);

            return deleteStatement;
        }

        public static IInsertStatement<T> CreateInsertStatementSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var insertStatement = Substitute.For<IInsertStatement<T>>();
            insertStatement.ReturnsForAll(insertStatement);

            repository.Insert()
                      .Returns(insertStatement);

            return insertStatement;
        }

        public static ISelectStatement<T> CreateSelectStatementSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var selectStatement = Substitute.For<ISelectStatement<T>>();
            selectStatement.ReturnsForAll(selectStatement);

            repository.Query()
                      .Returns(selectStatement);

            return selectStatement;
        }

        public static IUpdateStatement<T> CreateUpdateStatementSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var updateStatement = Substitute.For<IUpdateStatement<T>>();
            updateStatement.ReturnsForAll(updateStatement);

            repository.Update()
                      .Returns(updateStatement);

            return updateStatement;
        }
    }
}