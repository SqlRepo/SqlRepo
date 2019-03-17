using System;
using System.Linq.Expressions;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityActivatorFactory : IEntityActivatorFactory
    {
        public EntityActivator<T> Create<T>() where T: class, new()
        {
            var type = typeof(T);
            var ctor = type.GetConstructor(Type.EmptyTypes);

            // empty constructor is guaranteed by new constraint
            // ReSharper disable once AssignNullToNotNullAttribute
            var lambda = Expression.Lambda(typeof(EntityActivator<T>), Expression.New(ctor));

            var compiled = (EntityActivator<T>)lambda.Compile();
            return compiled;
        }
    }
}