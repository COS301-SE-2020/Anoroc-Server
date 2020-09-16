using Anoroc_User_Management.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model class used to store and work with GEO Location Points
    /// </summary>
    public class Location
    {
        private bool carrierDataPoint;
        private Location center_Location;

        [Key]
        public long Location_ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey("RegionArea_ID")]
        public long RegionArea_ID { get; set; }
        public Area Region { get; set; }
        [ForeignKey("Cluster_ID")]
        public long? ClusterReferenceID { get; set; }
        [JsonIgnore]
        public Cluster Cluster { get; set; }
        public User User { get; set; }

        // Token of the user owning this point
        [ForeignKey("AccessToken")]
        public string AccessToken { get; set; }

        public Location(SimpleLocation simpleLocation)
        {
            Latitude = simpleLocation.Latitude;
            Longitude = simpleLocation.Longitude;
            Created = DateTime.Now;
        }
        public Location(OldLocation oldLocation)
        {
            Location_ID = oldLocation.Old_Location_ID;
            Latitude = oldLocation.Latitude;
            Longitude = oldLocation.Longitude;
            carrierDataPoint = oldLocation.Carrier_Data_Point;
            Created = oldLocation.Created;
            RegionArea_ID = oldLocation.Area_Reference_ID;
            Region = oldLocation.Region;
            ClusterReferenceID = oldLocation.Old_Cluster_Reference_ID;
            Cluster = new Cluster(oldLocation.Cluster);
            User = oldLocation.User;
        }
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Created = DateTime.Now;
        }
        
        // To identify the user for the notification, pass the token and get the user detials from that and not the location class, otherwise we are
        // sending the details of the user everywhere
        

        public Location(double latCoord, double longCoord, DateTime created, Area area)
        {
            Latitude = latCoord;
            Longitude = longCoord;
            Created = created;
            Carrier_Data_Point = false;
            Region = area;
        }
        public Location(double latCoord, double longCoord, DateTime created, Area area, bool carrier)
        {
            Latitude = latCoord;
            Longitude = longCoord;
            Created = created;
            Carrier_Data_Point = false;
            Region = area;
            Carrier_Data_Point = carrier;
            AccessToken = "none";
        }
        public Location(double lat, double longCoord, DateTime created)
        {
            Latitude = lat;
            Longitude = longCoord;
            Created = created;
            Carrier_Data_Point = false;
            AccessToken = "none";
        }
        public Location()
        {

        }

        public Location(double lat, double longCoord, DateTime created, bool carrierDataPoint, Area region) : this(lat, longCoord, created)
        {
            this.carrierDataPoint = carrierDataPoint;
            Region = region;
        }

        public Location(Location location)
        {
            Location_ID = location.Location_ID;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            Carrier_Data_Point = location.Carrier_Data_Point;
            Created = location.Created;
            //AreaReferenceID = location.AreaReferenceID;
            Region = new Area(location.Region);
            //ClusterReferenceID = location.ClusterReferenceID;
            Cluster = new Cluster(location.Cluster);
        }

        public Location(double latitude, double longitude, bool carrier_Data_Point, DateTime created, Area region, string userAccessToken) : this(latitude, longitude)
        {
            Carrier_Data_Point = carrier_Data_Point;
            Created = created;
            Region = region;
            AccessToken = userAccessToken;
        }

/*
        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }*/

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void toggleCarrierStatus()
        {
            Carrier_Data_Point = !Carrier_Data_Point;
        }
    }
}
