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

        // Documentation for SQL and JSON:
        // https://docs.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server?view=sql-server-ver15

        protected SqlConnection Connection;
        public SQL_DatabaseService(string connection_string)
        {
            Connection = new SqlConnection(connection_string);
        }
        // -----------------------------------------
        // Location SQL
        // -----------------------------------------
        public List<Location> Select_ListLocations()
        {
            return null;
        }

        public bool Insert_Location(Location location)
        {
            return false;
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
        public List<Cluster> Select_ListClusters()
        {
            return null;
        }

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
            try
            {
                Connection.Open();
                Connection.Close();
                return true;
            }
            catch(SqlException)
            {
                return false;
            }
        }
    }
}
