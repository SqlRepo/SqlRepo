using System;
using System.Collections.Generic;
using System.Text;

namespace SqlRepo
{
    public interface IConnectionProvider
    {
        T Provide<T>() where T: class, IConnection;
    }
}
