using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlRepo.Abstractions
{
    public interface IExecuteProcedureStatement<TReturn>
    {
        IList<ParameterDefinition> ParameterDefinitions { get; }
        TReturn Go();
        Task<TReturn> GoAsync();
        IExecuteProcedureStatement<TReturn> WithName(string procedureName);
        IExecuteProcedureStatement<TReturn> WithParameter(string name, object value);
        IExecuteProcedureStatement<TReturn> WithSchema(string schemaName);
        IExecuteProcedureStatement<TReturn> UseConnectionProvider(IConnectionProvider connectionProvider);
    }
}