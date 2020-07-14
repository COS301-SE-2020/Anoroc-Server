using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
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
        public int Next_Cluster_ID { get; set; }
        public SQL_DatabaseService(string connection_string)
        {
            Connection = new SqlConnection(connection_string);
        }
        // -----------------------------------------
        // Location SQL
        // -----------------------------------------
        public void Insert_Location(Location location)
        {

        }

        public bool Delete_Location(Location location)
        {
            return false;
        }

        public bool Update_Location(Location location)
        {
            return false;
        }



        // -----------------------------------------
        // Cluster SQL
        // -----------------------------------------
        public bool Update_Cluster(Cluster cluster)
        {
            return false;
        }

        public bool Delete_Cluster(Cluster cluster)
        {
            return false;
        }

        public bool Insert_Cluster(Cluster cluster)
        {
            return false;
        }

        public int Get_Cluster_ID()
        {
            return 0;
        }

        public bool Test_Connection()
        {
            throw new NotImplementedException();
        }
    }
}
