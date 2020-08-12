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
                var databaseList = _context.Locations
                    .Include(b => b.Region)
                    .ToList();
                /*databaseList.ForEach(location =>
                {
                    location.Region = Select_Area_By_Id(location.RegionArea_ID);
                });*/
                return databaseList;
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
                .ToList(); ;
        }
        public List<Location> Select_Locations_By_Area(Area area)
        {
            return _context.Locations
                .Where(loc => loc.Region==area)
                .Include(l => l.Region)
                .ToList();
        }
        public List<Location> Select_Locations_By_ID(long id)
        {
            return _context.Locations
                .Where(l => l.Location_ID == id)
                .Include(l => l.Region)
                .ToList();
        }
        public List<Location> Select_Unclustered_Locations(Area area)
        {
            //select all unclusted locations, but center locations from clusters are also added so must not select these.
            return null;
        }
        public List<Area> Select_Unique_Areas()
        {
            //return _context.Areas.Distinct(new AreaEqualityComparer()).ToList();
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
            var returnList = _context.Clusters.ToList();
            foreach (var item in returnList)
            {
                Select_Location_By_Cluster_Reference(item.Cluster_Id).ToList().ForEach(location =>
                {
                    location.Cluster = null;
                    location.Region = Select_Area_By_Id(location.RegionArea_ID);
                    item.Coordinates.Add(location);
                });
                item.Center_Location = Select_Locations_By_ID(item.Center_LocationLocation_ID)
                    .FirstOrDefault();
            }
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
            var returnList = _context.Clusters.Where(cl => cl.Center_Location.RegionArea_ID == area.Area_ID).ToList();

            foreach (var item in returnList)
            {
                Select_Location_By_Cluster_Reference(item.Cluster_Id).ToList().ForEach(location =>
                {
                    location.Cluster = null;
                    location.Region = Select_Area_By_Id(location.RegionArea_ID);
                    item.Coordinates.Add(location);
                });
                item.Center_Location = Select_Locations_By_ID(item.Center_LocationLocation_ID)
                    .FirstOrDefault();
            }

            return returnList;
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
                User getUser = (from user in _context.Users where user.Access_Token == access_token select user).First();
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
                User updatedUser = (from user in _context.Users where user.Access_Token  ==  access_token select user).First();
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
            string upper = carrier_status.ToUpper();
            if (upper.Equals("POSITIVE"))
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
        public string GetUserEmail(string access_token)
        {
            return _context.Users.Where(user => user.Access_Token == access_token).FirstOrDefault().Email;
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
