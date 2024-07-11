using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace WebApplication1.packages
{
    public class pkg_base
    {
        private string _connectionString;

        public pkg_base(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected OracleConnection GetConnection()
        {
            return new OracleConnection(_connectionString);
        }
    }

}
