using System.Transactions;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Insert
{
    public class InsertCustomerBenchmarkSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public InsertCustomerBenchmarkSqlRepo(IBenchmarkHelpers benchmarkHelpers, IRepositoryFactory repositoryFactory)
            : base(benchmarkHelpers,
                Component.SqlRepo)
        {
            _repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            using (var transaction = new TransactionScope())
            {
                var address = _repositoryFactory.Create<Address>().Insert(new Address
                {
                    ZipCode = "ABC 123"
                });

                var customer = _repositoryFactory.Create<Customer>().Insert(new Customer
                {
                    AddressId = address.Id,
                    FirstName = "John",
                    LastName = "Doe",
                    Gender = Gender.Male
                });

                transaction.Complete();
            }
        }
    }
}