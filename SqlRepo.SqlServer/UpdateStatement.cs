﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class UpdateStatement<TEntity> : SqlStatement<TEntity, int>, IUpdateStatement<TEntity>
        where TEntity: class, new()
    {
        private const string StatementTemplate = "UPDATE [{0}].[{1}]\nSET {2}{3};";

        private readonly IList<Expression<Func<TEntity, object>>> setSelectors =
            new List<Expression<Func<TEntity, object>>>();

        private readonly IList<object> setValues = new List<object>();
        private readonly IWhereClauseBuilder whereClauseBuilder;
        private readonly IWritablePropertyMatcher writablePropertyMatcher;
        private TEntity entity;

        public UpdateStatement(IStatementExecutor statementExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            IWhereClauseBuilder whereClauseBuilder)
            : base(statementExecutor, entityMapper)
        {
            this.writablePropertyMatcher = writablePropertyMatcher;
            this.whereClauseBuilder = whereClauseBuilder;
        }

        public IUpdateStatement<TEntity> And(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.And(expression);
            return this;
        }

        public IUpdateStatement<TEntity> For(TEntity entity)
        {
            if(this.setSelectors.Any() || !this.whereClauseBuilder.IsClean)
            {
                throw new InvalidOperationException(
                    "For cannot be used once Set or Where have been used, please create a new command.");
            }

            this.IsClean = false;
            this.entity = entity;
            return this;
        }

        public override int Go()
        {
            return this.StatementExecutor.ExecuteNonQuery(this.Sql());
        }

        public override async Task<int> GoAsync()
        {
            return await this.StatementExecutor.ExecuteNonQueryAsync(this.Sql());
        }

        public IUpdateStatement<TEntity> NestedAnd(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.NestedAnd(expression);
            return this;
        }

        public IUpdateStatement<TEntity> NestedOr(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.NestedOr(expression);
            return this;
        }

        public IUpdateStatement<TEntity> Or(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.Or(expression);
            return this;
        }

        public IUpdateStatement<TEntity> Set<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember @value,
            string tableSchema = null,
            string tableName = null)
        {
            if(this.entity != null)
            {
                throw new InvalidOperationException(
                    "Set cannot be used once For has been used, please create a new command.");
            }

            this.IsClean = false;
            this.setSelectors.Add(this.ConvertExpression(selector));
            this.setValues.Add(@value);
            this.TableSchema = tableSchema;
            this.TableName = tableName;
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
                this.GetTableSchema(),
                this.GetTableName(),
                this.GetSetClause(),
                this.GetWhereClause());
        }

        public IUpdateStatement<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            if(this.entity != null)
            {
                throw new InvalidOperationException(
                    "Where cannot be used once For has been used, please create a new command.");
            }

            this.IsClean = false;
            this.whereClauseBuilder.Where(expression);
            return this;
        }
        
        public IUpdateStatement<TEntity> WhereIn<TMember>(Expression<Func<TEntity, TMember>> selector, TMember[] values)
        {
            if (this.entity != null)
            {
                throw new InvalidOperationException(
                    "Where cannot be used once For has been used, please create a new command.");
            }
            this.IsClean = false;
            this.whereClauseBuilder.WhereIn<TEntity, TMember>(selector, values);
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
                                                  .Where(p => p.Name != "Id" && p.CanWrite
                                                              && this.writablePropertyMatcher.Test(p
                                                                  .PropertyType))
                                                  .Select(p => "[" + p.Name + "] = "
                                                               + this.FormatValue(p.GetValue(this.entity)));

            return this.FormatColumnValuePairs(columnValuePairs);
        }

        private string GetSetClauseFromSelectors()
        {
            var columnValuePairs =
                this.setSelectors.Select((e, i) => "[" + this.GetMemberName(e) + "] = "
                                                   + this.FormatValue(this.setValues[i]));

            return this.FormatColumnValuePairs(columnValuePairs);
        }

        private string GetTableName()
        {
            return string.IsNullOrEmpty(this.TableName)? this.TableNameFromType<TEntity>(): this.TableName;
        }

        private string GetTableSchema()
        {
            return string.IsNullOrEmpty(this.TableSchema)? "dbo": this.TableSchema;
        }

        private string GetWhereClause()
        {
            if(this.entity != null)
            {
                var identity = this.GetIdByConvention(this.entity);
                return $"\nWHERE [{identity.Name}] = {identity.Value}";
            }

            var result = this.whereClauseBuilder.Sql();
            return string.IsNullOrWhiteSpace(result)? string.Empty: $"\n{result}";
        }
    }
}