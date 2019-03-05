using System;
using System.Linq.Expressions;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer.Abstractions
{
    public interface IFromClauseBuilder : IClauseBuilder
    {
        IFromClauseBuilder From<T>(string tableAlias = null,
            string tableName = null,
            string tableSchema = null);

        IFromClauseBuilder InnerJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null);

        IFromClauseBuilder LeftOuterJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null);

        IFromClauseBuilder On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        IFromClauseBuilder RightOuterJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null);

        TableDefinition TableDefinition<T>(string alias = null);
    }
}