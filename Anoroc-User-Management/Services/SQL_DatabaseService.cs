using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
        /// <summary>
        /// Connect the Service by adding the Connection string
        /// </summary>
        /// <param name="connection_string">The connection string defined in appsettings.json to connect to the database</param>
        public SQL_DatabaseService(string connection_string)
        {
            Connection = new SqlConnection(connection_string);
        }
        // -----------------------------------------
        // Location SQL
        // -----------------------------------------
        //Setting up the connection to Entity Framework Database Context:

        readonly dbContext _context;
        /// <summary>
        /// Get an instance of the Service to be used locally
        /// </summary>
        /// <param name="context">The instance of service that allows the use of the dbContext object to manage the database</param>
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
            try
            {
                _context.Location.Add(location);
                _context.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete_Location(Location location)
        {
            try
            {
                _context.Location.Remove(location);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Update_Location(Location location)
        {
            try
            {
                _context.Location.Update(location);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }



        // -----------------------------------------
        // Cluster SQL
        // -----------------------------------------
        public List<Cluster> Select_ListClusters()
        {
            return _context.Cluster.ToList();
        }

        public bool Update_Cluster(Cluster cluster)
        {
            try
            {
                _context.Cluster.Update(cluster);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete_Cluster(Cluster cluster)
        {
            try
            {
                _context.Cluster.Remove(cluster);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Insert_Cluster(Cluster cluster)
        {
            try
            {
                _context.Cluster.Add(cluster);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public long Get_Cluster_ID()
        {
            return 0;
        }

        // -----------------------------------------
        // User SQL
        // -----------------------------------------
        public List<User> Select_ListUsers()
        {
            return _context.User.ToList();
        }

        public bool Update_User(User user)
        {
            try
            {
                _context.User.Update(user);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete_User(User user)
        {
            try
            {
                _context.User.Remove(user);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Insert_User(User user)
        {
            try
            {
                _context.User.Add(user);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public void InsertFirebaseToken(string access_token, string firebase_token)
        {
            try
            {
                User updatedUser = (from user in _context.User where user.Access_Token==access_token select user).First();
                updatedUser.Firebase_Token = firebase_token;
                _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
