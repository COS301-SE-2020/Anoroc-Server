using Anoroc_User_Management.Services;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
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

        [Key]
        public long Location_ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public DateTime Created { get; set; }
        public long AreaReferenceID { get; set; }
        public Area Region { get; set; }
        [ForeignKey("Cluster_ID")]
        public long? ClusterReferenceID { get; set; }
        public Cluster Cluster { get; set; }

        // Token of the user owning this point
        public string Token { get; set; }
        public string UserAccessToken { get; set; }

        public Location(SimpleLocation simpleLocation)
        {
            Latitude = simpleLocation.Latitude;
            Longitude = simpleLocation.Longitude;
            Token = simpleLocation.Token;
            Created = DateTime.Now;
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
        public Location(double lat, double longCoord, DateTime created)
        {
            Latitude = lat;
            Longitude = longCoord;
            Created = created;
            Carrier_Data_Point = false;
        }

        public Location(GeoCoordinate coord)
        {
            Latitude = coord.Latitude;
            Longitude = coord.Longitude;
            Carrier_Data_Point = false;
            Created = DateTime.Now;
        }
        public Location(GeoCoordinate coord, DateTime creted, Area area)
        {
            Latitude = coord.Latitude;
            Longitude = coord.Longitude;
            Carrier_Data_Point = false;
            Created = creted;
            Region = area;
        }
        public Location(GeoCoordinate coord, DateTime creted, Area area, bool carrier)
        {
            Latitude = coord.Latitude;
            Longitude = coord.Longitude;
            Created = creted;
            Region = area;
            Carrier_Data_Point = carrier;
        }
        public Location()
        {

        }

        public Location(double lat, double longCoord, DateTime created, bool carrierDataPoint, Area region) : this(lat, longCoord, created)
        {
            this.carrierDataPoint = carrierDataPoint;
            Region = region;
        }

        public override string ToString()
        {
            return "Lat: " + Latitude + " Long: " + Longitude;
        }

        public void toggleCarrierStatus()
        {
            Carrier_Data_Point = !Carrier_Data_Point;
        }
    }
}
