﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class ExecuteQuerySqlStatement<TEntity> : ExecuteSqlStatement<IEnumerable<TEntity>>, IExecuteQuerySqlStatement<TEntity>
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
            if(string.IsNullOrWhiteSpace(this.Sql))
            {
                throw new MissingSqlException();
            }

            using(var reader = this.StatementExecutor.ExecuteReader(this.Sql))
            {
                return this.entityMapper.Map<TEntity>(reader);
            }
        }

        public override async Task<IEnumerable<TEntity>> GoAsync()
        {
            if(string.IsNullOrWhiteSpace(this.Sql))
            {
                throw new MissingSqlException();
            }

            using(var reader = await this.StatementExecutor.ExecuteReaderAsync(this.Sql))
            {
                return this.entityMapper.Map<TEntity>(reader);
            }
        }
    }
}