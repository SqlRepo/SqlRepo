using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlRepo.Abstractions
{
    public interface IExecuteStatement<TReturn>
    {
        IList<ParameterDefinition> ParameterDefinitions { get; }
        TReturn Go();
        Task<TReturn> GoAsync();
        IExecuteStatement<TReturn> WithName(string procedureName);
        IExecuteStatement<TReturn> WithParameter(string name, object value);
        IExecuteStatement<TReturn> WithSchema(string schemaName);
        IExecuteStatement<TReturn> UseConnectionProvider(IConnectionProvider connectionProvider);
    }
}