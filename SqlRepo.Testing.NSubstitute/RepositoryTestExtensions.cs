using System;
using NSubstitute;
using SqlRepo.Abstractions;

namespace SqlRepo.Testing.NSubstitute
{
    public static class RepositoryTestExtensions
    {
        public static IDeleteStatement<T> CreateDeleteStatementSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var deleteStatement = StatementSubstituteFactory.CreateDeleteStatementSubstitute<T>();
            repository.Delete()
                      .Returns(deleteStatement);

            return deleteStatement;
        }

        public static IInsertStatement<T> CreateInsertStatementSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var insertStatement = StatementSubstituteFactory.CreateInsertStatementSubstitute<T>();

            repository.Insert()
                      .Returns(insertStatement);

            return insertStatement;
        }

        public static ISelectStatement<T> CreateSelectStatementSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var selectStatement = StatementSubstituteFactory.CreateSelectStatementSubstitute<T>();

            repository.Query()
                      .Returns(selectStatement);

            return selectStatement;
        }

        public static IUpdateStatement<T> CreateUpdateStatementSubstitute<T>(this IRepository<T> repository)
            where T: class, new()
        {
            var updateStatement = StatementSubstituteFactory.CreateUpdateStatementSubstitute<T>();

            repository.Update()
                      .Returns(updateStatement);

            return updateStatement;
        }
    }
}