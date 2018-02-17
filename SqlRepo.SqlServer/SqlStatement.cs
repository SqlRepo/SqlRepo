using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public abstract class SqlStatement<TEntity, TResult> : ClauseBuilder, ISqlStatement<TResult>
        where TEntity: class, new()
    {
        protected SqlStatement(IStatementExecutor statementExecutor, IEntityMapper entityMapper)
        {
            this.StatementExecutor = statementExecutor ?? throw new ArgumentNullException(nameof(statementExecutor));
            this.EntityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
            this.TableSchema = "dbo";
            this.TableName = typeof(TEntity).Name;
        }

        public string ConnectionString { get; private set; }
        public string TableName { get; protected set; }
        public string TableSchema { get; protected set; }
        protected IStatementExecutor StatementExecutor { get; }
        protected IEntityMapper EntityMapper { get; }
        public abstract TResult Go(string connectionString = null);
        public abstract Task<TResult> GoAsync(string connectionString = null);

        public void UseConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;
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