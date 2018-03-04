using System;

namespace SqlRepo.Abstractions
{
    public interface IConnectionProvider
    {
        TConnection Provide<TConnection>() where TConnection: class, IConnection;
    }
}
