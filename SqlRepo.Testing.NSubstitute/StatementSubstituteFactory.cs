using System;
using NSubstitute;
using NSubstitute.Extensions;
using SqlRepo.Abstractions;

namespace SqlRepo.Testing.NSubstitute
{
    public static class StatementSubstituteFactory
    {
        public static IDeleteStatement<T> CreateDeleteStatementSubstitute<T>()
            where T: class, new()
        {
            var deleteStatement = Substitute.For<IDeleteStatement<T>>();
            deleteStatement.ReturnsForAll(deleteStatement);

            return deleteStatement;
        }

        public static IInsertStatement<T> CreateInsertStatementSubstitute<T>()
            where T: class, new()
        {
            var insertStatement = Substitute.For<IInsertStatement<T>>();
            insertStatement.ReturnsForAll(insertStatement);

            return insertStatement;
        }

        public static ISelectStatement<T> CreateSelectStatementSubstitute<T>()
            where T: class, new()
        {
            var selectStatement = Substitute.For<ISelectStatement<T>>();
            selectStatement.ReturnsForAll(selectStatement);

            return selectStatement;
        }

        public static IUpdateStatement<T> CreateUpdateStatementSubstitute<T>()
            where T: class, new()
        {
            var updateStatement = Substitute.For<IUpdateStatement<T>>();
            updateStatement.ReturnsForAll(updateStatement);

            return updateStatement;
        }
    }
}