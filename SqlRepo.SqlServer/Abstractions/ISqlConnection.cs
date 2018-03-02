using System;
using System.Threading.Tasks;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer.Abstractions {
    public interface ISqlConnection : IConnection
    {
        void Open();
        ISqlCommand CreateCommand();

        Task OpenAsync();
    }
}