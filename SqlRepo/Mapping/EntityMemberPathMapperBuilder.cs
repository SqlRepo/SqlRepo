using System;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMemberPathMapperBuilder<T, TMember> : IEntityMemberPathMapperBuilder<T, TMember>
        where T: class, new()
    {
        private readonly IEntityMemberPathMapper<T> mapper;

        public EntityMemberPathMapperBuilder()
        {
            this.mapper = new EntityMemberPathMapper<T>();
        }

        public IEntityMemberPathMapper<T> Build()
        {
            return this.mapper;
        }
    }
}