using System;
using System.Collections.Generic;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity: class, new()
    {
        private readonly IStatementFactory statementFactory;

        public Repository(IStatementFactory statementFactory)
        {
            this.statementFactory = statementFactory;
        }

        public IDeleteStatement<TEntity> Delete()
        {
            return this.statementFactory.CreateDelete<TEntity>();
        }

        public int Delete(TEntity entity)
        {
            return this.statementFactory.CreateDelete<TEntity>()
                       .For(entity)
                       .Go();
        }

        public IExecuteNonQueryProcedureStatement ExecuteNonQueryProcedure()
        {
            return this.statementFactory.CreateExecuteNonQueryProcedure();
        }

        public IExecuteNonQuerySqlStatement ExecuteNonQuerySql()
        {
            return this.statementFactory.CreateExecuteNonQuerySql();
        }

        public IExecuteQueryProcedureStatement<TEntity> ExecuteQueryProcedure()
        {
            return this.statementFactory.CreateExecuteQueryProcedure<TEntity>();
        }

        public IExecuteQuerySqlStatement<TEntity> ExecuteQuerySql()
        {
            return this.statementFactory.CreateExecuteQuerySql<TEntity>();
        }

        public IInsertStatement<TEntity> Insert()
        {
            return this.statementFactory.CreateInsert<TEntity>();
        }

        public TEntity Insert(TEntity entity)
        {
            return this.statementFactory.CreateInsert<TEntity>()
                       .For(entity)
                       .Go();
        }

        public ISelectStatement<TEntity> Query()
        {
            return this.statementFactory.CreateSelect<TEntity>();
        }

        public IEnumerable<TEntity> ResultsFrom(ISelectStatement<TEntity> query)
        {
            return query.Go();
        }

        public IUpdateStatement<TEntity> Update()
        {
            return this.statementFactory.CreateUpdate<TEntity>();
        }

        public int Update(TEntity entity)
        {
            return this.statementFactory.CreateUpdate<TEntity>()
                       .For(entity)
                       .Go();
        }

        public IRepository<TEntity> UseConnectionProvider(IConnectionProvider connectionProvider)
        {
            this.statementFactory.UseConnectionProvider(connectionProvider);
            return this;
        }
    }
}