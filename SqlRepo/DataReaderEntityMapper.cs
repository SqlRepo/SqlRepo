using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class DataReaderEntityMapper : IEntityMapper
    {
        private static readonly Dictionary<string, object> EntityMapperDefinitions =
            new Dictionary<string, object>();

        public virtual IEnumerable<TEntity> Map<TEntity>(IDataReader reader)
            where TEntity: class, new()
        {
            var list = new List<TEntity>();

            EntityMapperDefinition<TEntity> entityMapper;

            var fullTypeName = typeof(TEntity).FullName;

            if(EntityMapperDefinitions.ContainsKey(fullTypeName))
            {
                entityMapper = (EntityMapperDefinition<TEntity>)EntityMapperDefinitions[fullTypeName];
            }
            else
            {
                entityMapper = new EntityMapperDefinition<TEntity>
                               {
                                   Activator = EntityActivator.GetActivator<TEntity>()
                               };

                foreach(var propertyInfo in typeof(TEntity).GetProperties())
                {
                    entityMapper.PropertySetters.Add(propertyInfo.Name,
                        BuildUntypedSetter<TEntity>(propertyInfo));
                    entityMapper.ColumnTypeMappings.Add(propertyInfo.Name, propertyInfo.PropertyType);
                }

                EntityMapperDefinitions.Add(fullTypeName, entityMapper);
            }

            var isFirst = true;
            var mappingInstructions = new Func<IDataReader, TEntity, TEntity>[reader.FieldCount];

            while(reader.Read())
            {
                var entity = entityMapper.Activator();

                if(isFirst)
                {
                    isFirst = false;

                    for(var i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);

                        if(!entityMapper.PropertySetters.ContainsKey(columnName))
                        {
                            continue;
                        }

                        var propertySetter = entityMapper.PropertySetters[columnName];

                        var fieldIndex = i;

                        if(entityMapper.ColumnTypeMappings.ContainsKey(columnName))
                        {
                            var dataType = entityMapper.ColumnTypeMappings[columnName];

                            if(dataType == typeof(decimal))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             propertySetter(e, reader.GetDecimal(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(decimal?))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetDecimal(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(string))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetString(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(short))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             propertySetter(e, reader.GetInt16(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(short?))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetInt16(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(int))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             propertySetter(e, reader.GetInt32(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(int?))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetInt32(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(long))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             propertySetter(e, reader.GetInt64(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(long?))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetInt64(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(DateTime))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             propertySetter(e,
                                                                 reader.GetDateTime(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(DateTime?))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e,
                                                                 reader.GetDateTime(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(double))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             propertySetter(e, reader.GetDouble(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(double?))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetDouble(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(bool))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             propertySetter(e, reader.GetBoolean(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(bool?))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetBoolean(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(byte))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             propertySetter(e, reader.GetByte(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(byte?))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetByte(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(float))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             propertySetter(e, reader.GetFloat(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else if(dataType == typeof(float?))
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetFloat(fieldIndex));
                                                             return e;
                                                         };
                            }
                            else
                            {
                                mappingInstructions[i] = (dataReader, e) =>
                                                         {
                                                             if(reader.IsDBNull(fieldIndex))
                                                             {
                                                                 return e;
                                                             }

                                                             propertySetter(e, reader.GetValue(fieldIndex));
                                                             return e;
                                                         };
                            }
                        }
                        else
                        {
                            mappingInstructions[i] = (dataReader, e) =>
                                                     {
                                                         if(reader.IsDBNull(fieldIndex))
                                                         {
                                                             return e;
                                                         }

                                                         propertySetter(e, reader.GetValue(fieldIndex));
                                                         return e;
                                                     };
                        }
                    }
                }

                foreach(var mappingInstruction in mappingInstructions)
                {
                    mappingInstruction?.Invoke(reader, entity);
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
            var exBody = Expression.Call(exTarget,
                methodInfo,
                Expression.Convert(exValue, propertyInfo.PropertyType));
            var lambda = Expression.Lambda<Action<T, object>>(exBody, exTarget, exValue);
            var action = lambda.Compile();
            return action;
        }
    }
}