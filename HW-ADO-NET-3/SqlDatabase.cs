using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_ADO_NET_3
{
    class SqlDatabase : Database
    {
        public SqlDatabase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public override IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }
        public override IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            return new SqlCommand(commandText, (SqlConnection)connection);
        }
        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        public override IDbDataAdapter CreateAdapter()
        {
            return new SqlDataAdapter();
        }
        public override IDataParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new SqlParameter(parameterName, parameterValue);
        }
    }
}
