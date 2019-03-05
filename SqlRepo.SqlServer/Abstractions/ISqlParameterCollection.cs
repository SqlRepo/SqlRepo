using System;

namespace SqlRepo.SqlServer.Abstractions {
    public interface ISqlParameterCollection
    {
        void AddWithValue(string name, object value);
    }
}