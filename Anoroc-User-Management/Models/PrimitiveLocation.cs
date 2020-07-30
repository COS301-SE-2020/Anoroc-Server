using System;
using Anoroc_User_Management.Services;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model class used to store and work with GEO Location Points
    /// </summary>
    public class PrimitiveLocation
    {
        public long Location_ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public DateTime Created { get; set; }
        public string Region { get; set; }

        // To identify the user for the notification, pass the token and get the user detials from that and not the location class, otherwise we are
        // sending the details of the user everywhere
        //public string User_Email { get; set; }
        public PrimitiveLocation(Location location)
        {
            
            Latitude = location.Longitude;
            Longitude = location.Longitude;
            Carrier_Data_Point = location.Carrier_Data_Point;
            Region = JsonConvert.SerializeObject(location.Region);
            Carrier_Data_Point = location.Carrier_Data_Point;
            Created =location.Created;
        }
        public PrimitiveLocation()
        {

        }
        /*public PrimitiveLocation(Point point)
        {
            Created = DateTime.Now;
            Carrier_Data_Point = false;
            Region = "";
            Coordinate = JsonConvert.SerializeObject(new GeoCoordinate(point.Latitude, point.Longitude));
        }*/
        public override string ToString()
        {
            return "";
        }
        public void toggleCarrierStatus()
        {
            Carrier_Data_Point = !Carrier_Data_Point;
        }
    }
}
