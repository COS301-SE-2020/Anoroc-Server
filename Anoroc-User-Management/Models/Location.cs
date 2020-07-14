using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model class used to store and work with GEO Location Points
    /// </summary>
    public class Location
    {
        public GeoCoordinate Coordinate { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public DateTime Created { get; set; }
        public Area Region { get; set; }

        public string User_Email { get; set; }

        public Location(double latCoord, double longCoord, DateTime created, Area area)
        {
            Coordinate = new GeoCoordinate(latCoord, longCoord);
            Created = created;
            Carrier_Data_Point = false;
            Region = area;
        }

        public Location(double lat, double longCoord, DateTime created)
        {
            Coordinate = new GeoCoordinate(lat, longCoord);
            Created = created;
            Carrier_Data_Point = false;
        }

        public Location(GeoCoordinate coord)
        {
            Coordinate = coord;
            Carrier_Data_Point = false;
            Created = DateTime.Now;
        }

        public override string ToString()
        {
            return "Lat: " + Coordinate.Latitude + " Long: " + Coordinate.Longitude;
        }
    }
}
