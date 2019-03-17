using System;
using System.Collections.Generic;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMapperDefinition<TEntity>: IEntityMapperDefinition
        where TEntity: class, new()
    {
        public EntityMapperDefinition()
        {
            this.PropertySetters = new Dictionary<string, Action<TEntity, object>>();
            this.ColumnTypeMappings = new Dictionary<string, Type>();
        }

        public EntityActivator<TEntity> Activator { get; set; }
        public Dictionary<string, Type> ColumnTypeMappings { get; set; }
        public Dictionary<string, Action<TEntity, object>> PropertySetters { get; set; }
    }
}