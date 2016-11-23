using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlRepo.SqlServer
{
    public class UpdateCommand<TEntity> : SqlCommand<TEntity, int>, IUpdateCommand<TEntity>
        where TEntity : class, new()
    {
        private const string StatementTemplate = "UPDATE [{0}].[{1}]\nSET {2}{3};";

        private readonly IList<Expression<Func<TEntity, object>>> setSelectors =
            new List<Expression<Func<TEntity, object>>>();

        private readonly IList<object> setValues = new List<object>();
        private readonly IWhereClauseBuilder whereClauseBuilder;
        private readonly IWritablePropertyMatcher writablePropertyMatcher;
        private TEntity entity;

        public UpdateCommand(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            IWhereClauseBuilder whereClauseBuilder)
            : base(commandExecutor, entityMapper)
        {
            this.writablePropertyMatcher = writablePropertyMatcher;
            this.whereClauseBuilder = whereClauseBuilder;
        }

        public IUpdateCommand<TEntity> And(Expression<Func<TEntity, bool>> expression)
        {
            whereClauseBuilder.And(expression);
            return this;
        }

        public IUpdateCommand<TEntity> For(TEntity entity)
        {
            if (setSelectors.Any() || !whereClauseBuilder.IsClean)
            {
                throw new InvalidOperationException(
                    "For cannot be used once Set or Where have been used, please create a new command.");
            }

            IsClean = false;
            this.entity = entity;
            return this;
        }

        public override int Go(string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = ConnectionString;
            }
            return CommandExecutor.ExecuteNonQuery(connectionString, Sql());
        }

        public IUpdateCommand<TEntity> NestedAnd(Expression<Func<TEntity, bool>> expression)
        {
            whereClauseBuilder.NestedAnd(expression);
            return this;
        }

        public IUpdateCommand<TEntity> NestedOr(Expression<Func<TEntity, bool>> expression)
        {
            whereClauseBuilder.NestedOr(expression);
            return this;
        }

        public IUpdateCommand<TEntity> Or(Expression<Func<TEntity, bool>> expression)
        {
            whereClauseBuilder.Or(expression);
            return this;
        }

        public IUpdateCommand<TEntity> Set<TMember>(Expression<Func<TEntity, TMember>> selector, TMember @value,
            string tableSchema = null, string tableName = null)
        {
            if (entity != null)
            {
                throw new InvalidOperationException(
                    "Set cannot be used once For has been used, please create a new command.");
            }

            IsClean = false;
            setSelectors.Add(ConvertExpression(selector));
            setValues.Add(@value);
            TableSchema = tableSchema;
            TableName = tableName;
            return this;
        }

        public override string Sql()
        {
            if (entity == null && !setSelectors.Any())
            {
                throw new InvalidOperationException(
                    "Build cannot be used on a statement that has not been initialised using Set or For.");
            }

            return string.Format(StatementTemplate,
                GetTableSchema(),
                GetTableName(),
                GetSetClause(),
                GetWhereClause());
        }

        public IUpdateCommand<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            if (entity != null)
            {
                throw new InvalidOperationException(
                    "Where cannot be used once For has been used, please create a new command.");
            }

            IsClean = false;
            whereClauseBuilder.Where(expression);
            return this;
        }

        private string GetTableName()
        {
            return string.IsNullOrEmpty(TableName) ? TableNameFromType<TEntity>() : TableName;
        }

        private string GetTableSchema()
        {
            return string.IsNullOrEmpty(TableSchema) ? "dbo" : TableSchema;
        }

        private string FormatColumnValuePairs(IEnumerable<string> columnValuePairs)
        {
            return string.Join(", ", columnValuePairs);
        }

        private string GetSetClause()
        {
            return setSelectors.Any() ? GetSetClauseFromSelectors() : GetSetClauseFromEntity();
        }

        private string GetSetClauseFromEntity()
        {
            var columnValuePairs = typeof (TEntity).GetProperties()
                .Where(
                    p =>
                        p.Name != "Id" && p.CanWrite
                        && writablePropertyMatcher.Test(p.PropertyType))
                .Select(
                    p =>
                        "[" + p.Name + "] = " + FormatValue(p.GetValue(entity)));

            return FormatColumnValuePairs(columnValuePairs);
        }

        private string GetSetClauseFromSelectors()
        {
            var columnValuePairs =
                setSelectors.Select(
                    (e, i) => "[" + GetMemberName(e) + "] = " + FormatValue(setValues[i]));

            return FormatColumnValuePairs(columnValuePairs);
        }

        private string GetWhereClause()
        {
            if (entity != null)
            {
                var identity = GetIdByConvention(entity);
                return $"\nWHERE [{identity.Name}] = {identity.Value}";
            }

            var result = whereClauseBuilder.Sql();
            return string.IsNullOrWhiteSpace(result) ? string.Empty : $"\n{result}";
        }
    }
}