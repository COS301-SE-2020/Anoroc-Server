using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
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
        public int MaxDate;
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

        readonly AnorocDbContext _context;
        /// <summary>
        /// Get an instance of the Service to be used locally
        /// </summary>
        /// <param name="context">The instance of service that allows the use of the dbContext object to manage the database</param>
        public SQL_DatabaseService(AnorocDbContext context, int maxdate)
        {
            MaxDate = maxdate;
            _context = context;
        }
        public List<Location> Select_List_Locations()
        {
            try
            {
                return _context.Locations
                    .Include(b => b.Region)
                .Include(b => b.Cluster)
                    .ToList();
                /*databaseList.ForEach(location =>
                {
                    location.Region = Select_Area_By_Id(location.RegionArea_ID);
                });*/
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
        public List<Location> Select_List_Carrier_Locations()
        {
            return _context.Locations
                .Where(l => l.Carrier_Data_Point == true)
                .Include(l => l.Region)
                .Include(b => b.Cluster)
                .ToList(); ;
        }
        public List<Location> Select_Locations_By_Area(Area area)
        {
            return _context.Locations
                .Where(loc => loc.Region==area)
                .Include(l => l.Region)
                .Include(b => b.Cluster)
                .ToList();
        }
        public List<Location> Select_Locations_By_ID(long id)
        {
            return _context.Locations
                .Where(l => l.Location_ID == id)
                .Include(l => l.Region)
                .Include(b => b.Cluster)
                .ToList();
        }
        public List<Location> Select_Unclustered_Locations(Area area)
        {
            return _context.Locations
                .Where(l => l.ClusterReferenceID==null)
                .Where(c => c.Cluster!=null)
                .Include(a => a.Region)
                .Include(b => b.Cluster)
                .ToList();
        }
        public void Update_Carrier_Locations(string access_token, bool status)
        {
            _context.Locations
                .Where(l => l.UserAccessToken == access_token)
                .ToList()
                .ForEach(l => l.Carrier_Data_Point = status);
            _context.SaveChanges();
            Update_Old_Carrier_Locations(access_token, status);
        }
        public void Update_Locations_Access_Token(string old_Token, string new_token)
        {
            _context.Locations
                .Where(l => l.UserAccessToken == old_Token)
                .ToList()
                .ForEach(l => l.UserAccessToken = new_token);
            _context.SaveChanges();
        }
        public List<Area> Select_Unique_Areas()
        {
            var returnList = new List<Area>();
            var nonDistincList = _context.Areas.ToList();
            foreach(Area area in nonDistincList)
            {
                if (!returnList.Contains(area))
                    returnList.Add(area);
            }
            return returnList;
        }
        public bool Insert_Location(Location location)
        {
            var areas = Select_Unique_Areas(); 
            try
            {
                Area areadb = AreaListContains(areas, location.Region);

                if (areadb == null)
                {
                    Insert_Area(location.Region);
                }
                else
                {
                    location.Region = areadb;
                }

                _context.Locations.Add(location);
                _context.SaveChanges();
                return true;
            }
            catch  (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private Area AreaListContains(List<Area> areas, Area locationArea)
        {
            Area returnVal = null;
            if(areas != null)
            {
                areas.ForEach(area =>
                {
                    if (locationArea.AreaEquals(area))
                        returnVal = area;
                });
            }
            return returnVal;
        }
        public bool Delete_Location(Location location)
        {
            try
            {
                _context.Locations.Remove(location);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public List<Location> Select_Location_By_Cluster_Reference(long reference)
        {
            return _context.Locations
                .Where(l => l.ClusterReferenceID == reference)
                .ToList();
        }
        public bool Update_Location(Location location)
        {
            try
            {
                _context.Locations.Update(location);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete_Locations_Older_Than_Hours(int hours)
        {         
            try
            {
                var locations = _context.Locations.Where(l =>
                l.Created.DayOfYear==DateTime.Now.DayOfYear &&
                l.Created.Hour < DateTime.Now.AddHours(-hours).Hour
                ).ToList();
                foreach(Location location in locations)
                {
                    Insert_Old_Location(location);
                    Delete_Location(location);
                }
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
        public List<Cluster> Select_List_Clusters()
        {
            //go through list and add the Coordinates to this list since this is only returning data from Cluster table and not from both tables
            var returnList = _context.Clusters
                .Include(c => c.Coordinates)
                .Include("Coordinates.Region")
                .Include(l => l.Center_Location)
                .ToList();
            foreach (var item in returnList)
            {
                foreach (var location in item.Coordinates)
                {
                    location.Cluster = null;
                }
            }
            /*foreach (var item in returnList)
            {
                Select_Location_By_Cluster_Reference(item.Cluster_Id).ToList().ForEach(location =>
                {
                    location.Cluster = null;
                    location.Region = Select_Area_By_Id(location.RegionArea_ID);
                    item.Coordinates.Add(location);
                });
                item.Center_Location = Select_Locations_By_ID(item.Center_LocationLocation_ID)
                    .FirstOrDefault();
            }*/
            return returnList;
        }

        public bool Update_Cluster(Cluster cluster)
        {
            try
            {
                _context.Clusters.Update(cluster);
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
            try
            {
                _context.Clusters.Remove(cluster);
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
            try
            {
                _context.Clusters.Add(cluster);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public List<Cluster> Select_Clusters_By_Area(Area area)
        {
            return _context.Clusters
                .Where(cl => cl.Center_Location.RegionArea_ID == area.Area_ID)
                .Include(c => c.Coordinates)
                .Include(l => l.Center_Location)
                .ToList();

            /*foreach (var item in returnList)
            {
                Select_Location_By_Cluster_Reference(item.Cluster_Id).ToList().ForEach(location =>
                {
                    location.Cluster = null;
                    location.Region = Select_Area_By_Id(location.RegionArea_ID);
                    item.Coordinates.Add(location);
                });
                item.Center_Location = Select_Locations_By_ID(item.Center_LocationLocation_ID)
                    .FirstOrDefault();
            }*/

        }
        public long Get_Cluster_ID()
        {
            return 0;
        }
        // -----------------------------------------
        // User SQL
        // -----------------------------------------
        public List<User> Select_List_Users()
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
        public string Get_Firebase_Token(string access_token)
        {
            try
            {
                User getUser = (from user in _context.Users where user.AccessToken == access_token select user).First();
                return getUser.Firebase_Token;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "-1";
            }
        }
        public void Insert_Firebase_Token(string access_token, string firebase_token)
        {
            try
            {
                User updatedUser = (from user in _context.Users where user.AccessToken  ==  access_token select user).First();
                updatedUser.Firebase_Token = firebase_token;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public void Update_Carrier_Status(string access_token, string carrier_status)
        {
            bool user_status;
            if ((carrier_status.ToUpper()).Equals("POSITIVE"))
                user_status = true;
            else
                user_status = false;
            try
            {
<<<<<<< HEAD
                User updatedUser = (from user in _context.Users where user.AccessToken == access_token select user).First();
                //var updatedUser = _context.Users.First(a => a.Access_Token == access_token);
                updatedUser.carrierStatus = user_status;
                
=======
                User updatedUser = _context.Users
                    .Where(u => u.Access_Token== access_token)
                    .FirstOrDefault();
                updatedUser.Carrier_Status = user_status;
                _context.Users.Update(updatedUser);
                Update_Carrier_Locations(updatedUser.Access_Token, updatedUser.Carrier_Status);
>>>>>>> 98b96d1e9b0f503f92f821f42c6576bd848e2171
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public string GetUserEmail(string access_token)
        {
            return _context.Users.Where(user => user.AccessToken == access_token).FirstOrDefault().Email;
        }
        public bool updateUserToken(User user, string token)
        {
            try
            {
                var updatedUser = _context.Users
                    .Where(u => u.Email == user.Email)
                    .FirstOrDefault();
                string old_token = updatedUser.Access_Token;
                updatedUser.Access_Token = token;
                _context.Users.
                    Update(updatedUser);
                _context.SaveChanges();
                Update_Locations_Access_Token(old_token, token);
                Update_Old_Locations_Access_Token(old_token, token);
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public void populate()
        {
            string json;
            using (StreamReader r = new StreamReader("TempData/Points.json"))
            {
                json = r.ReadToEnd();
                Points items = JsonConvert.DeserializeObject<Points>(json);
                foreach (Point point in items.PointArray)
                {
                    var location = new Location(point.Latitude, point.Longitude, DateTime.Now, new Area("South Africa", "Gauteng", "Pretoria"));
                    if (Insert_Location(location))
                    {
                        Debug.WriteLine("Inserted Location: " + JsonConvert.SerializeObject(location));
                    }
                }
            }
        }
        public bool Validate_Access_Token(string access_token)
        {
            try
            {
                var searchUser = _context.Users.Where(user=>user.AccessToken==access_token).FirstOrDefault();
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
        // -----------------------------------------
        // Area Table SQL
        // -----------------------------------------
        public bool Insert_Area(Area area)
        {
            try
            {
                var check =Select_Unique_Areas();
                if (!check.Contains(area))
                    _context.Areas.Add(area);
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete_Area(Area area)
        {
            try
            {
                var check = Select_Unique_Areas();
                if (check.Contains(area))
                    _context.Areas.Remove(area);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public Area Select_Area_By_Id(long id)
        {
            return _context.Areas
                .Where(area => area.Area_ID == id).FirstOrDefault();
        }
        // -----------------------------------------
        // Old Cluster Table SQL
        // -----------------------------------------   Old must not return anything older than 8 days
        public List<OldCluster> Select_All_Old_Clusters()
        {
            return _context.OldClusters
                .Where(ol => ol.Cluster_Created > DateTime.Now.AddDays(-MaxDate))
                .ToList();
        }
        public List<OldCluster> Select_Old_Clusters_By_Area(Area area)
        {
            return _context.OldClusters
                .Where(oc => oc.Center_Location.Region == area)
                .ToList();
        }
        public bool Insert_Old_Cluster(Cluster cluster)
        {
            try
            {
                OldCluster old = new OldCluster(cluster);
                _context.OldClusters.Add(old);
                _context.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        // -----------------------------------------
        // Old Locations Table SQL
        // -----------------------------------------
        public List<OldLocation> Select_Old_Unclustered_Locations(Area area)
        {
            return _context.OldLocations
                .Where(ol => ol.Created > DateTime.Now.AddDays(-MaxDate))
                .ToList();
        }
        public void Update_Old_Carrier_Locations(string access_token, bool status)
        {
            _context.OldLocations
               .Where(l => l.UserAccessToken == access_token)
               .ToList()
               .ForEach(l => l.Carrier_Data_Point = status);
            _context.SaveChanges();
        }
        public void Update_Old_Locations_Access_Token(string old_Token, string new_token)
        {
            _context.OldLocations
               .Where(l => l.UserAccessToken == old_Token)
               .ToList()
               .ForEach(l => l.UserAccessToken = new_token);
            _context.SaveChanges();
        }
        public bool Insert_Old_Location(Location location)
        {
            try
            {
                OldLocation old = new OldLocation(location);
                _context.OldLocations.Add(old);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
