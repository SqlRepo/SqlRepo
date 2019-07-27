using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class ExecuteQuerySqlStatement<TEntity> : ExecuteSqlStatement<IEnumerable<TEntity>>,
        IExecuteQuerySqlStatement<TEntity>
        where TEntity: class, new()
    {
        private readonly IEntityMapper entityMapper;

        public ExecuteQuerySqlStatement(IStatementExecutor commandExecutor, IEntityMapper entityMapper)
            : base(commandExecutor)
        {
            this.entityMapper = entityMapper;
        }

        public override IEnumerable<TEntity> Go()
        {
            this.ThrowIfSqlMissing();

            using(var reader = this.StatementExecutor.ExecuteReader(this.Sql))
            {
                return this.entityMapper.Map<TEntity>(reader);
            }
        }

        public override async Task<IEnumerable<TEntity>> GoAsync()
        {
            this.ThrowIfSqlMissing();

            using(var reader = await this.StatementExecutor.ExecuteReaderAsync(this.Sql))
            {
                return this.entityMapper.Map<TEntity>(reader);
            }
        }

        private void ThrowIfSqlMissing()
        {
            if(string.IsNullOrWhiteSpace(this.Sql))
            {
                throw new MissingSqlException();
            }
        }

        public IExecuteQuerySqlStatement<TEntity> UsingMappingProfile(IEntityMappingProfile mappingProfile)
        {
            this.entityMapper.UseMappingProfile(mappingProfile);
            return this;
        }
    }
}