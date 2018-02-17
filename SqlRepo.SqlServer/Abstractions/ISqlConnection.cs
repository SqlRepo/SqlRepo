using System;
using System.Threading.Tasks;

namespace SqlRepo.SqlServer.Abstractions {
    public interface ISqlConnection : IDisposable
    {
        void Open();
        ISqlCommand CreateCommand();

        Task OpenAsync();
    }
}