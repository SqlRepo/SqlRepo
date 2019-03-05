﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer.Abstractions
{
    public abstract class ExecuteProcedureStatement<TResult> : IExecuteProcedureStatement<TResult>
    {
        protected ExecuteProcedureStatement(IStatementExecutor statementExecutor)
        {
            this.StatementExecutor = statementExecutor;
            this.ParameterDefinitions = new List<ParameterDefinition>();
        }

        protected string ProcedureName { get; private set; }
        protected string SchemaName { get; private set; } = "dbo";
        protected IStatementExecutor StatementExecutor { get; }

        public abstract TResult Go();
        public abstract Task<TResult> GoAsync();

        public IList<ParameterDefinition> ParameterDefinitions { get; }

        public IExecuteProcedureStatement<TResult> UseConnectionProvider(IConnectionProvider connectionProvider)
        {
            this.StatementExecutor.UseConnectionProvider(connectionProvider);
            return this;
        }

        public IExecuteProcedureStatement<TResult> WithName(string procedureName)
        {
            this.ProcedureName = procedureName;
            return this;
        }

        public IExecuteProcedureStatement<TResult> WithParameter(string name, object value)
        {
            this.ParameterDefinitions.Add(new ParameterDefinition
                                          {
                                              Name = name,
                                              Value = value
                                          });
            return this;
        }

        public IExecuteProcedureStatement<TResult> WithSchema(string schemaName)
        {
            this.SchemaName = schemaName;
            return this;
        }
    }
}