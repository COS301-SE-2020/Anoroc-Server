using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Anoroc_User_Management.Services
{
    public class SQL_DatabaseService : IDatabaseEngine
    {

        // Documentation for SQL and JSON:
        // https://docs.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server?view=sql-server-ver15

        //The Following 4 lines connect to the database but not using Entity Framework
        protected SqlConnection Connection;
        public SQL_DatabaseService(string connection_string)
        {
            Connection = new SqlConnection(connection_string);
        }
        // -----------------------------------------
        // Location SQL
        // -----------------------------------------
        //Setting up the connection to Entity Framework Database Context:

        readonly dbContext _context;

        public SQL_DatabaseService(dbContext context)
        {
            _context = context;
        }
        public List<Location> Select_ListLocations()
        {
            return _context.Location.ToList();
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
                _context.Database.OpenConnection();
                if (_context.Database.CanConnect())
                {
                    _context.Database.CloseConnection();
                    return true;
                }
                else
                {
                    _context.Database.CloseConnection();
                    return false;
                }
            }
            catch(SqlException)
            {
                return false;
            }
        }
    }
}
