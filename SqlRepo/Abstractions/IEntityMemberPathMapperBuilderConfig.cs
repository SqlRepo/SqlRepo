using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityMemberPathMapperBuilderConfig<T, TMember>
        where T: class, new() { }
}