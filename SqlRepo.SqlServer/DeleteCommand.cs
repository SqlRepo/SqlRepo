using System;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public class DeleteCommand<TEntity> : SqlCommand<TEntity, int>, IDeleteCommand<TEntity>
        where TEntity : class, new()
    {
        private const string StatementTemplate = "DELETE [{0}].[{1}]{2};";
        private readonly IWhereClauseBuilder whereClauseBuilder;
        private TEntity entity;

        public DeleteCommand(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            IWhereClauseBuilder whereClauseBuilder)
            : base(commandExecutor, entityMapper)
        {
            this.whereClauseBuilder = whereClauseBuilder;
        }

        public IDeleteCommand<TEntity> And(Expression<Func<TEntity, bool>> expression)
        {
            whereClauseBuilder.And(expression);
            return this;
        }

        public IDeleteCommand<TEntity> For(TEntity entity)
        {
            if (!whereClauseBuilder.IsClean)
            {
                throw new InvalidOperationException(
                    "For cannot be used once Where has been used, please use FromScratch to reset the statement before using Where.");
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

        public IDeleteCommand<TEntity> NestedAnd(Expression<Func<TEntity, bool>> expression)
        {
            whereClauseBuilder.NestedAnd(expression);
            return this;
        }

        public IDeleteCommand<TEntity> NestedOr(Expression<Func<TEntity, bool>> expression)
        {
            whereClauseBuilder.NestedOr(expression);
            return this;
        }

        public IDeleteCommand<TEntity> Or(Expression<Func<TEntity, bool>> expression)
        {
            whereClauseBuilder.Or(expression);
            return this;
        }

        public override string Sql()
        {
            return string.Format(StatementTemplate, TableSchema, TableName, GetWhereClause());
        }

        public IDeleteCommand<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            if (entity != null)
            {
                throw new InvalidOperationException(
                    "Where cannot be used once For has been used, please use FromScratch to reset the statement before using Where.");
            }

            IsClean = false;
            whereClauseBuilder.Where(expression);
            return this;
        }

        public IDeleteCommand<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values)
        {
            whereClauseBuilder.WhereIn(selector, values);
            return this;
        }

        public IDeleteCommand<TEntity> UsingTableName(string tableName)
        {
            TableName = tableName;
            return this;
        }

        public IDeleteCommand<TEntity> UsingTableSchema(string tableSchema)
        {
            TableSchema = tableSchema;
            return this;
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