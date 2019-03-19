using System;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMemberPathMapper<T> : IEntityMemberPathMapper<T>
        where T: class, new() { }
}