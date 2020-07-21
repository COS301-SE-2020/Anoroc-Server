using System;
using Anoroc_User_Management.Services;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model class used to store and work with GEO Location Points
    /// </summary>
    public class PrimitiveLocation : DbContext
    {
        public long Location_ID { get; set; }
        public string Coordinate { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public DateTime Created { get; set; }
        public string Region { get; set; }

        // To identify the user for the notification, pass the token and get the user detials from that and not the location class, otherwise we are
        // sending the details of the user everywhere
        //public string User_Email { get; set; }

        public PrimitiveLocation(long LocID, double latCoord, double longCoord, DateTime created, string area)
        {
            Location_ID = LocID;
            Coordinate = "{'lat':" + latCoord + ",'long':" + longCoord + "}";
            Created = created;
            Carrier_Data_Point = false;
            Region = area;
        }
        public PrimitiveLocation(double latCoord, double longCoord, DateTime created, string area)
        {

            Coordinate = "{'lat':" + latCoord + ",'long':" + longCoord + "}";
            Created = created;
            Carrier_Data_Point = false;
            Region = area;
        }

        public PrimitiveLocation(double lat, double longCoord, DateTime created)
        {
            Coordinate = "{'lat':"+lat+",'long':"+longCoord+"}";
            Created = created;
            Carrier_Data_Point = false;
        }
        public PrimitiveLocation(long locID, double lat, double longCoord)
        {
            Location_ID = locID;
            Coordinate = "{'lat':" + lat + ",'long':" + longCoord + "}";
            //Created = created;
            Carrier_Data_Point = false;
        }

        public PrimitiveLocation(string coord)
        {
            Coordinate = coord;
            Carrier_Data_Point = false;
            Created = DateTime.Now;
        }
        public PrimitiveLocation()
        {

        }
        public PrimitiveLocation(Location location)
        {
            Coordinate = JsonConvert.SerializeObject(location.Coordinate);
            Carrier_Data_Point = location.Carrier_Data_Point;
            Region = JsonConvert.SerializeObject(location.Region);
            Carrier_Data_Point = location.Carrier_Data_Point;
        }

        public PrimitiveLocation(Point point)
        {
            Created = DateTime.Now;
            Carrier_Data_Point = false;
            Region = "";
            Coordinate = JsonConvert.SerializeObject(new GeoCoordinate(point.Latitude, point.Longitude));
        }

        public override string ToString()
        {
            return Coordinate;
        }

        public void toggleCarrierStatus()
        {
            Carrier_Data_Point = !Carrier_Data_Point;
        }
    }
}
