using System;
using System.Collections.Generic;

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
    }
}