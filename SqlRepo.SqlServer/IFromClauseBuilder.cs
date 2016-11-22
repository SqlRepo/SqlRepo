using System;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public interface IFromClauseBuilder : IClauseBuilder
    {
        IFromClauseBuilder From<T>( string tableAlias = null, string tableName = null, string tableSchema = null);
        IFromClauseBuilder FromScratch();
        IFromClauseBuilder InnerJoin<TLeft, TRight>(string leftTableAlias = null, string rightTableAlias = null, string rightTableName = null, string rightTableSchema = null);
        IFromClauseBuilder On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression, string leftTableAlias = null, string rightTableAlias = null);
        IFromClauseBuilder LeftOuterJoin<TLeft, TRight>(string leftTableAlias = null, string rightTableAlias = null, string rightTableName = null, string rightTableSchema = null);
        IFromClauseBuilder RightOuterJoin<TLeft, TRight>(string leftTableAlias = null, string rightTableAlias = null, string rightTableName = null, string rightTableSchema = null);
    }
}