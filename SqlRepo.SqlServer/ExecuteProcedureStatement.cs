using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class ExecuteProcedureStatement<TEntity> : ExecuteStatementBase<IEnumerable<TEntity>>
        where TEntity: class, new()
    {
        private readonly IEntityMapper entityMapper;

        public ExecuteProcedureStatement(IStatementExecutor commandExecutor, IEntityMapper entityMapper)
            : base(commandExecutor)
        {
            this.entityMapper = entityMapper;
        }

        public override IEnumerable<TEntity> Go()
        {
            if(string.IsNullOrWhiteSpace(this.ProcedureName))
            {
                throw new MissingProcedureNameException();
            }

            var procedureName = $"[{this.SchemaName}].[{this.ProcedureName}]";
            using(var reader = this.ParameterDefinitions.Any()
                                   ? this.StatementExecutor.ExecuteStoredProcedure(procedureName,
                                       this.ParameterDefinitions.ToArray())
                                   : this.StatementExecutor.ExecuteStoredProcedure(procedureName))
            {
                return this.entityMapper.Map<TEntity>(reader);
            }

            ;
        }

        public override async Task<IEnumerable<TEntity>> GoAsync()
        {
            if(string.IsNullOrWhiteSpace(this.ProcedureName))
            {
                throw new MissingProcedureNameException();
            }

            var procedureName = $"[{this.SchemaName}].[{this.ProcedureName}]";
            using(var reader = this.ParameterDefinitions.Any()
                                   ? await this.StatementExecutor.ExecuteStoredProcedureAsync(procedureName,
                                         this.ParameterDefinitions.ToArray())
                                   : await this.StatementExecutor.ExecuteStoredProcedureAsync(procedureName))
            {
                return this.entityMapper.Map<TEntity>(reader);
            }

            ;
        }
    }
}