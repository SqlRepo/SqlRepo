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
            TEntity entity = null;
            while(reader.Read())
            {
                if(entity == null || !this.mappingProfile.DataRecordMatchesEntity(entity, reader))
                {
                    entity = new TEntity();
                    list.Add(entity);
                }
                this.mappingProfile.Map(entity, reader);
            }

            return list;
        }

        public void UseMappingProfile(IEntityMappingProfile mappingProfile)
        {
            this.mappingProfile = mappingProfile;
        }
    }
}