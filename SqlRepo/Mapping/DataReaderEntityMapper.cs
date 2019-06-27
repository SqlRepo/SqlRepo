using System;
using System.Collections.Generic;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class DataReaderEntityMapper : IEntityMapper
    {
        private IEntityMappingProfile mappingProfile;

        public virtual IEnumerable<TEntity> Map<TEntity>(IDataReader reader)
            where TEntity: class, new()
        {
            if(this.mappingProfile == null)
            {
                this.mappingProfile = new DefaultEntityMappingProfile(typeof(TEntity));
            }

            var list = new List<TEntity>();

            while(reader.Read())
            {
                var entity = new TEntity();
                this.mappingProfile.Map(entity, reader);
                list.Add(entity);
            }

            return list;
        }

        public void UseMappingProfile(IEntityMappingProfile mappingProfile)
        {
            this.mappingProfile = mappingProfile;
        }
    }
}