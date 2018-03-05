using System;
using System.Linq;
using System.Threading.Tasks;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public abstract class SqlStatement<TEntity, TResult> : ClauseBuilder, ISqlStatement<TResult>
        where TEntity: class, new()
    {
        protected SqlStatement(IStatementExecutor statementExecutor, IEntityMapper entityMapper)
        {
            this.StatementExecutor =
                statementExecutor ?? throw new ArgumentNullException(nameof(statementExecutor));
            this.EntityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
            this.TableSchema = "dbo";
            this.TableName = typeof(TEntity).Name;
        }

        protected IStatementExecutor StatementExecutor { get; }
        protected IEntityMapper EntityMapper { get; }

        public string TableName { get; protected set; }
        public string TableSchema { get; protected set; }
        public abstract TResult Go();
        public abstract Task<TResult> GoAsync();

        public ISqlStatement<TResult> UseConnectionProvider(IConnectionProvider connectionProvider)
        {
            this.StatementExecutor.UseConnectionProvider(connectionProvider);
            return this;
        }

        protected EntityIdentity GetIdByConvention<T>(T entity)
            where T: class
        {
            var entityType = typeof(TEntity);
            var name = entityType.Name;
            var possibles = new[] {"id", "key", $"{name}id", $"{name}key", $"{name}_id", $"{name}_key"};
            var properties = entityType.GetProperties();
            var property = properties.FirstOrDefault(p => possibles.Contains(p.Name.ToLowerInvariant()));

            var identiy = new EntityIdentity
                          {
                              Name = "Id"
                          };
            if(property != null)
            {
                identiy.Name = property.Name;
                identiy.Value = property.GetValue(entity);
            }

            return identiy;
        }
    }
}