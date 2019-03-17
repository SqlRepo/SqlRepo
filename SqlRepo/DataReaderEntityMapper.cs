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
        private readonly IEntityMapperDefinitionProvider entityMapperDefinitionProvider;
        private static readonly Dictionary<string, object> EntityMapperDefinitions =
            new Dictionary<string, object>();

        public DataReaderEntityMapper(IEntityMapperDefinitionProvider entityMapperDefinitionProvider)
        {
            this.entityMapperDefinitionProvider = entityMapperDefinitionProvider;
        }
        public virtual IEnumerable<TEntity> Map<TEntity>(IDataReader reader)
            where TEntity: class, new()
        {
            var list = new List<TEntity>();

            var entityMapper = this.entityMapperDefinitionProvider.Get<TEntity>();

            var isFirst = true;
            var mappingInstructions = new Func<IDataReader, TEntity, TEntity>[reader.FieldCount];

            while(reader.Read())
            {
                var entity = entityMapper.Activator();

                if(isFirst)
                {
                    isFirst = false;

                    this.PopulateMappingInstructions(mappingInstructions, reader, entityMapper);
                }

                foreach(var mappingInstruction in mappingInstructions)
                {
                    mappingInstruction?.Invoke(reader, entity);
                }

                list.Add(entity);
            }

            return list;
        }

        private void PopulateMappingInstructions<TEntity>(IList<Func<IDataReader, TEntity, TEntity>> mappingInstructions,
            IDataRecord record,
            EntityMapperDefinition<TEntity> entityMapper)
            where TEntity: class, new()
        {
            for(var i = 0; i < record.FieldCount; i++)
            {
                var columnName = record.GetName(i);

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
                                                     propertySetter(e, record.GetDecimal(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(decimal?))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetDecimal(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(string))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetString(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(short))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     propertySetter(e, record.GetInt16(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(short?))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetInt16(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(int))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     propertySetter(e, record.GetInt32(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(int?))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetInt32(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(long))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     propertySetter(e, record.GetInt64(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(long?))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetInt64(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(DateTime))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     propertySetter(e, record.GetDateTime(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(DateTime?))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetDateTime(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(double))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     propertySetter(e, record.GetDouble(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(double?))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetDouble(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(bool))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     propertySetter(e, record.GetBoolean(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(bool?))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetBoolean(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(byte))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     propertySetter(e, record.GetByte(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(byte?))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetByte(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(float))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     propertySetter(e, record.GetFloat(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(float?))
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetFloat(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else
                    {
                        mappingInstructions[i] = (dataReader, e) =>
                                                 {
                                                     if(record.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, record.GetValue(fieldIndex));
                                                     return e;
                                                 };
                    }
                }
                else
                {
                    mappingInstructions[i] = (dataReader, e) =>
                                             {
                                                 if(record.IsDBNull(fieldIndex))
                                                 {
                                                     return e;
                                                 }

                                                 propertySetter(e, record.GetValue(fieldIndex));
                                                 return e;
                                             };
                }
            }
        }
    }
}