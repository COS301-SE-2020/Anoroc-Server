using Anoroc_User_Management.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class SQL_DatabaseService : IDatabaseEngine
    {

        public SqlConnection Connection;
        public SQL_DatabaseService(string connection_string)
        {
            Connection = new SqlConnection(connection_string);
        }

        public dynamic Execute_Query(string sql, string table)
        {
            throw new NotImplementedException();
        }

        public bool Test_Connection()
        {
            throw new NotImplementedException();
        }
    }
}
