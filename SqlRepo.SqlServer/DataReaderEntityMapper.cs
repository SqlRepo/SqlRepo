using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlRepo.SqlServer
{
    public class DataReaderEntityMapper : IEntityMapper
    {
        public virtual IEnumerable<TEntity> Map<TEntity>(IDataReader reader) where TEntity : class, new()
        {
            var createdActivator = EntityActivator.GetActivator<TEntity>();

            var list = new List<TEntity>();
            var fieldNames = new string[reader.FieldCount];

            for (var i = 0; i < reader.FieldCount; i++)
                fieldNames[i] = reader.GetName(i);

            var setterList = new List<Action<TEntity, object>>();

            var typeToRetrieve = new Dictionary<string, Type>();

            foreach (var field in fieldNames)
            {
                var propertyInfo = typeof(TEntity).GetProperty(field);
                setterList.Add(BuildUntypedSetter<TEntity>(propertyInfo));

                typeToRetrieve.Add(field, propertyInfo.PropertyType);
            }

            var setterArray = setterList.ToArray();

            while (reader.Read())
            {
                var entity = createdActivator();

                for (var i = 0; i < setterArray.Length; i++)
                {
                    var columnName = reader.GetName(i);

                    if (reader.IsDBNull(i))
                        continue;

                    if (typeToRetrieve.ContainsKey(columnName))
                    {
                        var dataType = typeToRetrieve[columnName];

                        if (dataType == typeof(decimal))
                        {
                            setterArray[i](entity, reader.GetDecimal(i));
                        }
                        else if (dataType == typeof(string))
                        {
                            setterArray[i](entity, reader.GetString(i));
                        }
                        else if (dataType == typeof(int))
                        {
                            setterArray[i](entity, reader.GetInt32(i));
                        }
                        else if (dataType == typeof(long))
                        {
                            setterArray[i](entity, reader.GetInt64(i));
                        }
                        else
                        {
                            var value = reader.GetValue(i);

                            setterArray[i](entity, value);
                        }
                    }
                    else
                    {
                        var value = reader.GetValue(i);

                        setterArray[i](entity, value);
                    }
                }

                list.Add(entity);
            }
            return list;
        }

        public static Action<T, object> BuildUntypedSetter<T>(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType;
            var methodInfo = propertyInfo.GetSetMethod();
            var exTarget = Expression.Parameter(targetType, "t");
            var exValue = Expression.Parameter(typeof(object), "p");
            var exBody = Expression.Call(exTarget, methodInfo,
                Expression.Convert(exValue, propertyInfo.PropertyType));
            var lambda = Expression.Lambda<Action<T, object>>(exBody, exTarget, exValue);
            var action = lambda.Compile();
            return action;
        }
    }
}