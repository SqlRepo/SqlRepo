using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityMemberPathMapper<T>
        where T: class, new() { }
}