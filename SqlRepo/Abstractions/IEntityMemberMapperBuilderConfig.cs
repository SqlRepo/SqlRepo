using System;

namespace SqlRepo.Abstractions {
    public interface IEntityMemberMapperBuilderConfig<T, TMember>
        where T: class, new() { }
}