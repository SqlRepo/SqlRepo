using System;
using System.Collections.Generic;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class DataReaderEntityMapper : IEntityMapper
    {
        private readonly IEntityMappingProfileProvider entityMappingProfileProvider;

        public DataReaderEntityMapper(IEntityMappingProfileProvider entityMappingProfileProvider)
        {
            this.entityMappingProfileProvider = entityMappingProfileProvider;
        }

        public virtual IEnumerable<TEntity> Map<TEntity>(IDataReader reader)
            where TEntity: class, new()
        {
            var list = new List<TEntity>();

            var entityMapper = this.entityMappingProfileProvider.Get<TEntity>();

            while(reader.Read())
            {
                var entity = new TEntity();
                entityMapper.Map(entity, reader);
                list.Add(entity);
            }

            return list;
        }
    }
}