using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlRepo.SqlServer
{
    public class DataReaderEntityMapper : IEntityMapper
    {
        public virtual IEnumerable<TEntity> Map<TEntity>(IDataReader reader) where TEntity: class, new()
        {
            var result = new List<TEntity>();
            var type = typeof(TEntity);
            var properties = type.GetProperties()
                                 .Where(p => p.CanWrite)
                                 .ToList();

            var columnMap = Enumerable.Range(0, reader.FieldCount)
                                      .Select(n => new ColumnMapping(n, reader.GetName(n)))
                                      .ToList();

            while(reader.Read())
            {
                var entity = new TEntity();
                foreach(var mapping in columnMap)
                {
                    var columnValue = reader[mapping.Index];
                    var property = properties.FirstOrDefault(p => p.Name == mapping.Name);

                    if(property != null && columnValue != null && columnValue.GetType() != typeof(DBNull))
                    {
                        property.SetValue(entity, columnValue);
                    }
                }
                result.Add(entity);
            }

            return result;
        }
    }
}