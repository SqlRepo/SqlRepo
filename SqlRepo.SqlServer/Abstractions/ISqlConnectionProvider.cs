using System;

namespace SqlRepo.SqlServer.Abstractions
{
    public interface ISqlConnectionProvider
    {
        ISqlConnection Provide();
    }
}