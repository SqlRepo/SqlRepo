using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityValueMemberMapperBuilder : IEntityValueMemberMapperBuilderConfig
    {
        IEntityValueMemberMapper Build();
    }
}