using System;

namespace SqlRepo.SqlServer
{
    public class AliasRequiredException : Exception
    {
        public AliasRequiredException()
            : base("An alias is required when joining a table using the same entity type.") {}
    }
}