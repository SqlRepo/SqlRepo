using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Insert
{
    public class InsertCustomerBenchmarkDapper : BenchmarkOperationBase
    {
        public InsertCustomerBenchmarkDapper(IBenchmarkHelpers benchmarkHelpers)
            : base(benchmarkHelpers,
                Component.Dapper)
        {
        }

        public override void Execute()
        {
            using (var transaction = new TransactionScope())
            {
                var sqlConnection = new SqlConnection(ConnectionString.Value);

                sqlConnection.QuerySingle("INSERT ");

                /*var address = _repositoryFactory.Create<Address>().Insert(new Address
                {
                    ZipCode = "ABC 123"
                });

                var customer = _repositoryFactory.Create<Customer>().Insert(new Customer
                {
                    AddressId = address.Id,
                    FirstName = "John",
                    LastName = "Doe",
                    Gender = Gender.Male
                });*/

                transaction.Complete();
            }
        }
    }
}