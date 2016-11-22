using System;
using System.Collections.Generic;

namespace SqlRepo
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity: class, new()
    {
        private readonly ICommandFactory commandFactory;

        public Repository(ICommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public IDeleteCommand<TEntity> Delete()
        {
            return this.commandFactory.CreateDelete<TEntity>();
        }

        public int Delete(TEntity entity)
        {
            return this.commandFactory.CreateDelete<TEntity>()
                       .For(entity)
                       .Go();
        }

        public IInsertCommand<TEntity> Insert()
        {
            return this.commandFactory.CreateInsert<TEntity>();
        }

        public TEntity Insert(TEntity entity)
        {
            return this.commandFactory.CreateInsert<TEntity>()
                       .For(entity)
                       .Go();
        }

        public ISelectCommand<TEntity> Query()
        {
            return this.commandFactory.CreateSelect<TEntity>();
        }

        public IEnumerable<TEntity> ResultsFrom(ISelectCommand<TEntity> query)
        {
            return query.Go();
        }

        public IUpdateCommand<TEntity> Update()
        {
            return this.commandFactory.CreateUpdate<TEntity>();
        }

        public int Update(TEntity entity)
        {
            return this.commandFactory.CreateUpdate<TEntity>()
                       .For(entity)
                       .Go();
        }
    }
}