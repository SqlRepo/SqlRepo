using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class ExecuteQueryProcedureStatement<TEntity> : ExecuteProcedureStatement<IEnumerable<TEntity>>,
        IExecuteQueryProcedureStatement<TEntity>
        where TEntity: class, new()
    {
        private readonly IEntityMapper entityMapper;

        public ExecuteQueryProcedureStatement(IStatementExecutor commandExecutor, IEntityMapper entityMapper)
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

        public IExecuteQueryProcedureStatement<TEntity> UsingMappingProfile(
            IEntityMappingProfile mappingProfile)
        {
            this.entityMapper.UseMappingProfile(mappingProfile);
            return this;
        }
    }
}