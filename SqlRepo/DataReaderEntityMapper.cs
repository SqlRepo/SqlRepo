using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class DataReaderEntityMapper : IEntityMapper
    {
        private readonly IEntityMapperDefinitionProvider entityMapperDefinitionProvider;

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

        private void PopulateMappingInstructions<TEntity>(
            IList<Func<IDataReader, TEntity, TEntity>> mappingInstructions,
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
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     propertySetter(e, r.GetDecimal(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(decimal?))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetDecimal(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(string))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetString(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(short))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     propertySetter(e, r.GetInt16(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(short?))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetInt16(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(int))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     propertySetter(e, r.GetInt32(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(int?))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetInt32(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(long))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     propertySetter(e, r.GetInt64(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(long?))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetInt64(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(DateTime))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     propertySetter(e, r.GetDateTime(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(DateTime?))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetDateTime(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(double))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     propertySetter(e, r.GetDouble(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(double?))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetDouble(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(bool))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     propertySetter(e, r.GetBoolean(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(bool?))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetBoolean(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(byte))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     propertySetter(e, r.GetByte(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(byte?))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetByte(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(float))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     propertySetter(e, r.GetFloat(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else if(dataType == typeof(float?))
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetFloat(fieldIndex));
                                                     return e;
                                                 };
                    }
                    else
                    {
                        mappingInstructions[i] = (r, e) =>
                                                 {
                                                     if(r.IsDBNull(fieldIndex))
                                                     {
                                                         return e;
                                                     }

                                                     propertySetter(e, r.GetValue(fieldIndex));
                                                     return e;
                                                 };
                    }
                }
                else
                {
                    mappingInstructions[i] = (r, e) =>
                                             {
                                                 if(r.IsDBNull(fieldIndex))
                                                 {
                                                     return e;
                                                 }

                                                 propertySetter(e, r.GetValue(fieldIndex));
                                                 return e;
                                             };
                }
            }
        }
    }
}