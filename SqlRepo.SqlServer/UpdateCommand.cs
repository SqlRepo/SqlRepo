using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public class UpdateCommand<TEntity> : SqlCommand<TEntity, int>, IUpdateCommand<TEntity>
        where TEntity: class, new()
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
            this.whereClauseBuilder.And(expression);
            return this;
        }

        public IUpdateCommand<TEntity> For(TEntity entity)
        {
            if(this.setSelectors.Any() || !this.whereClauseBuilder.IsClean)
            {
                throw new InvalidOperationException(
                    "For cannot be used once Set or Where have been used, please use FromScratch to reset the statement before using Set or Where.");
            }

            this.IsClean = false;
            this.entity = entity;
            return this;
        }

        public IUpdateCommand<TEntity> FromScratch()
        {
            this.whereClauseBuilder.FromScratch();
            this.setSelectors.Clear();
            this.entity = null;
            this.IsClean = true;
            return this;
        }

        public override int Go(string connectionString = null)
        {
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = this.ConnectionString;
            }
            return this.CommandExecutor.ExecuteNonQuery(this.Sql());
        }

        public IUpdateCommand<TEntity> NestedAnd(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.NestedAnd(expression);
            return this;
        }

        public IUpdateCommand<TEntity> NestedOr(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.NestedOr(expression);
            return this;
        }

        public IUpdateCommand<TEntity> Or(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.Or(expression);
            return this;
        }

        public IUpdateCommand<TEntity> Set<TMember>(Expression<Func<TEntity, TMember>> selector, TMember @value)
        {
            if(this.entity != null)
            {
                throw new InvalidOperationException(
                    "Set cannot be used once For has been used, please use FromScratch to reset the statement before using Set.");
            }

            this.IsClean = false;
            this.setSelectors.Add(this.ConvertExpression(selector));
            this.setValues.Add(@value);
            return this;
        }

        public override string Sql()
        {
            if(this.entity == null && !this.setSelectors.Any())
            {
                throw new InvalidOperationException(
                    "Build cannot be used on a statement that has not been initialised using Set or For.");
            }

            return string.Format(StatementTemplate,
                this.TableSchema,
                this.TableName,
                this.GetSetClause(),
                this.GetWhereClause());
        }

        public IUpdateCommand<TEntity> UsingTableName(string tableName)
        {
            this.TableName = tableName;
            return this;
        }

        public IUpdateCommand<TEntity> UsingTableSchema(string tableSchema)
        {
            this.TableSchema = tableSchema;
            return this;
        }

        public IUpdateCommand<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            if(this.entity != null)
            {
                throw new InvalidOperationException(
                    "Where cannot be used once For has been used, please use FromScratch to reset the statement before using Where.");
            }

            this.IsClean = false;
            this.whereClauseBuilder.Where(expression);
            return this;
        }

        private string FormatColumnValuePairs(IEnumerable<string> columnValuePairs)
        {
            return string.Join(", ", columnValuePairs);
        }

        private string GetSetClause()
        {
            return this.setSelectors.Any()? this.GetSetClauseFromSelectors(): this.GetSetClauseFromEntity();
        }

        private string GetSetClauseFromEntity()
        {
            var columnValuePairs = typeof(TEntity).GetProperties()
                                                  .Where(
                                                      p =>
                                                      p.Name != "Id" && p.CanWrite
                                                      && this.writablePropertyMatcher.Test(p.PropertyType))
                                                  .Select(
                                                      p =>
                                                      "[" + p.Name + "] = " + this.FormatValue(p.GetValue(this.entity)));

            return this.FormatColumnValuePairs(columnValuePairs);
        }

        private string GetSetClauseFromSelectors()
        {
            var columnValuePairs =
                this.setSelectors.Select(
                    (e, i) => "[" + this.GetMemberName(e) + "] = " + this.FormatValue(this.setValues[i]));

            return this.FormatColumnValuePairs(columnValuePairs);
        }

        private string GetWhereClause()
        {
            if(this.entity != null)
            {
                var identity = this.GetIdByConvention(this.entity);
                return string.Format("\nWHERE [{0}] = {1}", identity.Name, identity.Value);
            }

            var result = this.whereClauseBuilder.Sql();
            return string.IsNullOrWhiteSpace(result)? string.Empty: string.Format("\n{0}", result);
        }
    }
}