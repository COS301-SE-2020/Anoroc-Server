using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using GeoCoordinatePortable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
            try
            {
                var returnList = new List<Location>();
                //_context.Locations.ToList();
                List<PrimitiveLocation> databaseList = _context.Locations.ToList<PrimitiveLocation>();
                foreach (PrimitiveLocation prim in databaseList)
                {
                    Area area = JsonConvert.DeserializeObject<Area>(prim.Region);
                    GeoCoordinate coord = JsonConvert.DeserializeObject<GeoCoordinate>(prim.Coordinate);
                    Location obj = new Location(coord, prim.Created, area, prim.Carrier_Data_Point);
                    returnList.Add(obj);
                }
                return returnList;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public bool Insert_Location(Location location)
        {
            PrimitiveLocation insertLocation = new PrimitiveLocation(location);
            try
            {
                _context.Locations.Add(insertLocation);
                _context.SaveChanges();
                return true;
            }
            catch  (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete_Location(Location location)
        {
            PrimitiveLocation toDelete = new PrimitiveLocation(location);
            try
            {
                _context.Locations.Remove(toDelete);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool Update_Location(Location location)
        {
            PrimitiveLocation toUpdate = new PrimitiveLocation(location);
            try
            {
                _context.Locations.Update(toUpdate);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        // -----------------------------------------
        // Cluster SQL
        // -----------------------------------------
        public List<Cluster> Select_ListClusters()
        {
            var returnList = new List<Cluster>();
            var databaseList = _context.Clusters.ToList();

            foreach (PrimitiveCluster prim in databaseList)
            {
                var locations = JsonConvert.DeserializeObject<List<Location>>(prim.Coordinates);
                returnList.Add(new Cluster(locations,prim.Cluster_ID));
            }
            return returnList;
        }

        public bool Update_Cluster(Cluster cluster)
        {
            PrimitiveCluster primitiveCluster = new PrimitiveCluster(cluster);
            try
            {
                _context.Clusters.Update(primitiveCluster);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete_Cluster(Cluster cluster)
        {
            PrimitiveCluster primitiveCluster = new PrimitiveCluster(cluster);
            try
            {
                _context.Clusters.Remove(primitiveCluster);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool Insert_Cluster(Cluster cluster)
        {
            PrimitiveCluster primitiveCluster = new PrimitiveCluster(cluster);
            try
            {
                _context.Clusters.Add(primitiveCluster);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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
            return _context.Users.ToList();
        }

        public bool Update_User(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete_User(User user)
        {
            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool Insert_User(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public string getFirebaseToken(string access_token)
        {
            try
            {
                User getUser = (from user in _context.Users where user.Access_Token == access_token select user).First();
                return getUser.Firebase_Token;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "-1";
            }
        }
        public void InsertFirebaseToken(string access_token, string firebase_token)
        {
            try
            {
                User updatedUser = (from user in _context.Users where user.Access_Token  ==  access_token select user).First();
                updatedUser.Firebase_Token = firebase_token;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public void UpdateCarrierStatus(string access_token, string carrier_status)
        {
            bool user_status;
            if (carrier_status.Equals("Positive") || carrier_status.Equals("positive"))
                user_status = true;
            else
                user_status = false;

            try
            {
                User updatedUser = (from user in _context.Users where user.Access_Token == access_token select user).First();
                //var updatedUser = _context.Users.First(a => a.Access_Token == access_token);
                updatedUser.Carrier_Status = user_status;
                
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public void populate()
        {
           /* string json;
            using (StreamReader r = new StreamReader("TempData/Points.json"))
            {
                json = r.ReadToEnd();
                *//*Debug.WriteLine(json);*//*
                Points items = JsonConvert.DeserializeObject<Points>(json);
                foreach (Point point in items.PointArray)
                {
                    _context.Locations.Add(new PrimitiveLocation(point));
                }
                _context.SaveChanges();
            }*/
        }
        public bool validateAccessToken(string access_token)
        {
            try
            {
                var searchUser = _context.Users.Where(user=>user.Access_Token==access_token).FirstOrDefault();
                if (searchUser != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public bool Test_Connection()
        {
            /*try
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
            }*/
            return true;
        }
    }
}
