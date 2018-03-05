using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer.Abstractions
{
    public abstract class ExecuteStatementBase<TResult> : IExecuteStatement<TResult>
    {
        protected ExecuteStatementBase(IStatementExecutor statementExecutor)
        {
            this.StatementExecutor = statementExecutor;
            this.ParameterDefinitions = new List<ParameterDefinition>();
        }

        public string ProcedureName { get; private set; }
        public string SchemaName { get; private set; } = "dbo";
        protected IStatementExecutor StatementExecutor { get; }

        public abstract TResult Go();
        public abstract Task<TResult> GoAsync();

        public IList<ParameterDefinition> ParameterDefinitions { get; }

        public IExecuteStatement<TResult> UseConnectionProvider(IConnectionProvider connectionProvider)
        {
            this.StatementExecutor.UseConnectionProvider(connectionProvider);
            return this;
        }

        public IExecuteStatement<TResult> WithName(string procedureName)
        {
            this.ProcedureName = procedureName;
            return this;
        }

        public IExecuteStatement<TResult> WithParameter(string name, object value)
        {
            this.ParameterDefinitions.Add(new ParameterDefinition
                                          {
                                              Name = name,
                                              Value = value
                                          });
            return this;
        }

        public IExecuteStatement<TResult> WithSchema(string schemaName)
        {
            this.SchemaName = schemaName;
            return this;
        }
    }
}