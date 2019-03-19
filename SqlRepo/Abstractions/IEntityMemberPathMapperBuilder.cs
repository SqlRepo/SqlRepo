using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityMemberPathMapperBuilder<T, TMember>: IEntityMemberPathMapperBuilderConfig<T, TMember>
        where T: class, new()
    {
        IEntityMemberPathMapper<T> Build();
    }
}