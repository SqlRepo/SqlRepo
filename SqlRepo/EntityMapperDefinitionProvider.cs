using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMapperDefinitionProvider : IEntityMapperDefinitionProvider
    {
        private readonly Dictionary<Type, IEntityMapperDefinition> cache =
            new Dictionary<Type, IEntityMapperDefinition>();
        private readonly IEntityActivatorFactory entityActivatorFactory;

        public EntityMapperDefinitionProvider(IEntityActivatorFactory entityActivatorFactory)
        {
            this.entityActivatorFactory = entityActivatorFactory;
        }

        public EntityMapperDefinition<T> Get<T>()
            where T: class, new()
        {
            var entityType = typeof(T);
            if(!this.cache.TryGetValue(entityType, out var result))
            {
                var mapper = new EntityMapperDefinition<T>
                             {
                                 Activator = this.entityActivatorFactory.Create<T>()
                             };

                foreach(var propertyInfo in entityType.GetProperties())
                {
                    mapper.PropertySetters.Add(propertyInfo.Name, this.BuildUntypedSetter<T>(propertyInfo));
                    mapper.ColumnTypeMappings.Add(propertyInfo.Name, propertyInfo.PropertyType);
                }

                this.cache.Add(entityType, mapper);
                result = mapper;
            }

            return result as EntityMapperDefinition<T>;
        }

        private Action<T, object> BuildUntypedSetter<T>(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType;
            var methodInfo = propertyInfo.GetSetMethod();
            var exTarget = Expression.Parameter(targetType, "t");
            var exValue = Expression.Parameter(typeof(object), "p");
            var exBody = Expression.Call(exTarget,
                methodInfo,
                Expression.Convert(exValue, propertyInfo.PropertyType));
            var lambda = Expression.Lambda<Action<T, object>>(exBody, exTarget, exValue);
            var action = lambda.Compile();
            return action;
        }
    }
}