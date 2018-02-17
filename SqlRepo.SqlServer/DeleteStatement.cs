using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class DeleteStatement<TEntity> : SqlStatement<TEntity, int>, IDeleteStatement<TEntity>
        where TEntity: class, new()
    {
        private const string StatementTemplate = "DELETE [{0}].[{1}]{2};";
        private readonly IWhereClauseBuilder whereClauseBuilder;
        private TEntity entity;

        public DeleteStatement(IStatementExecutor statementExecutor,
            IEntityMapper entityMapper,
            IWhereClauseBuilder whereClauseBuilder)
            : base(statementExecutor, entityMapper)
        {
            this.whereClauseBuilder = whereClauseBuilder;
        }

        public IDeleteStatement<TEntity> And(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.And(expression);
            return this;
        }

        public IDeleteStatement<TEntity> For(TEntity entity)
        {
            if(!this.whereClauseBuilder.IsClean)
            {
                throw new InvalidOperationException(
                    "For cannot be used once Where has been used, please use FromScratch to reset the statement before using Where.");
            }

            this.IsClean = false;
            this.entity = entity;
            return this;
        }

        public override int Go(string connectionString = null)
        {
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = this.ConnectionString;
            }
            return this.StatementExecutor.ExecuteNonQuery(this.Sql());
        }

        public override async Task<int> GoAsync(string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = this.ConnectionString;
            }
            return await this.StatementExecutor.ExecuteNonQueryAsync(this.Sql());
        }

        public IDeleteStatement<TEntity> NestedAnd(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.NestedAnd(expression);
            return this;
        }

        public IDeleteStatement<TEntity> NestedOr(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.NestedOr(expression);
            return this;
        }

        public IDeleteStatement<TEntity> Or(Expression<Func<TEntity, bool>> expression)
        {
            this.whereClauseBuilder.Or(expression);
            return this;
        }

        public override string Sql()
        {
            return string.Format(StatementTemplate, this.TableSchema, this.TableName, this.GetWhereClause());
        }

        public IDeleteStatement<TEntity> UsingTableName(string tableName)
        {
            this.TableName = tableName;
            return this;
        }

        public IDeleteStatement<TEntity> UsingTableSchema(string tableSchema)
        {
            this.TableSchema = tableSchema;
            return this;
        }

        public IDeleteStatement<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            if(this.entity != null)
            {
                throw new InvalidOperationException(
                    "Where cannot be used once For has been used, please use FromScratch to reset the statement before using Where.");
            }

            this.IsClean = false;
            this.whereClauseBuilder.Where(expression, tableName: TableName, tableSchema: TableSchema);
            return this;
        }

        public IDeleteStatement<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values)
        {
            this.whereClauseBuilder.WhereIn(selector, values, tableName: TableName, tableSchema: TableSchema);
            return this;
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