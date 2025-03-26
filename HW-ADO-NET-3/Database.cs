using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_ADO_NET_3
{
    public abstract class Database
    {
        public string ConnectionString { get; set; }
        public abstract IDbConnection CreateConnection();
        public abstract IDbCommand CreateCommand();
        public abstract IDbCommand CreateCommand(string commandText, IDbConnection connection);
        public abstract IDataParameter CreateParameter(string parameterName, object parameterValue);
        public abstract IDbDataAdapter CreateAdapter();
    }

}
