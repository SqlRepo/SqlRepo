using System;
using System.Linq;

namespace SqlRepo.SqlServer
{
    public abstract class SqlCommand<TEntity, TResult> : ClauseBuilder, ISqlCommand<TResult>
        where TEntity: class, new()
    {
        protected SqlCommand(ICommandExecutor commandExecutor, IEntityMapper entityMapper)
        {
            if(commandExecutor == null)
            {
                throw new ArgumentNullException(nameof(commandExecutor));
            }

            if(entityMapper == null)
            {
                throw new ArgumentNullException(nameof(entityMapper));
            }

            this.CommandExecutor = commandExecutor;
            this.EntityMapper = entityMapper;
            this.TableSchema = "dbo";
            this.TableName = typeof(TEntity).Name;
        }

        public string ConnectionString { get; private set; }
        public string TableName { get; protected set; }
        public string TableSchema { get; protected set; }
        protected ICommandExecutor CommandExecutor { get; private set; }
        protected IEntityMapper EntityMapper { get; private set; }
        public abstract TResult Go(string connectionString = null);

        public void UseConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        protected EntityIdentity GetIdByConvention<T>(T entity) where T: class
        {
            var entityType = typeof(TEntity);
            var name = entityType.Name;
            var possibles = new[]
                            {
                                "id",
                                "key",
                                $"{name}id",
                                $"{name}key",
                                $"{name}_id",
                                $"{name}_key"
                            };
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