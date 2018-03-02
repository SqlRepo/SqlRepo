using System;

namespace SqlRepo.Abstractions
{
    public interface IWritablePropertyMatcher {
        bool Test(Type type);
    }
}