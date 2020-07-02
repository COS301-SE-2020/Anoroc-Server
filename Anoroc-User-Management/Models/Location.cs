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
