using System;
using System.Transactions;
using SqlRepo.Abstractions;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Insert
{
    public class InsertCustomerBenchmarkSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public InsertCustomerBenchmarkSqlRepo(IBenchmarkHelpers benchmarkHelpers,
            IRepositoryFactory repositoryFactory)
            : base(benchmarkHelpers, Component.SqlRepo)
        {
            this._repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            using(var transaction = new TransactionScope())
            {
                var address = this._repositoryFactory.Create<Address>()
                                  .Insert(new Address
                                          {
                                              ZipCode = "ABC 123"
                                          });

                var customer = this._repositoryFactory.Create<Customer>()
                                   .Insert(new Customer
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