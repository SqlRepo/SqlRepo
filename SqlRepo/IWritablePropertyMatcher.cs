using System;

namespace SqlRepo
{
    public interface IWritablePropertyMatcher {
        bool Test(Type type);
    }
}