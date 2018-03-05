using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SqlRepo.SqlServer.Abstractions
{
    public class SqlParameterCollectionAdapter: ISqlParameterCollection
    {
        private readonly SqlParameterCollection parameters;

        public SqlParameterCollectionAdapter(SqlParameterCollection parameters)
        {
            this.parameters = parameters;
        }

        public void AddWithValue(string name, object value)
        {
            this.parameters.AddWithValue(name, value);
        }
    }
}
