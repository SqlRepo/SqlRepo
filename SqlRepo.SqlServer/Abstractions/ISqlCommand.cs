using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlRepo.SqlServer.Abstractions {
    public interface ISqlCommand : IDisposable
    {
        int CommandTimeout { get; set; }
        CommandType CommandType { get; set; }
        string CommandText { get; set; }
        ISqlParameterCollection Parameters { get; }
        int ExecuteNonQuery();
        Task<int> ExecuteNonQueryAsync();
        IDataReader ExecuteReader(CommandBehavior closeConnection);
        Task<IDataReader> ExecuteReaderAsync(CommandBehavior closeConnection);
    }
}   