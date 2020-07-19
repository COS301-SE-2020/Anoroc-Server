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
        public bool CarrierDataPoint { get; set; }
        public DateTime Created { get; set; }
        public Area Region { get; set; }

        // Token of the user owning this point
        public string Token { get; set; }

        public Location(SimpleLocation simpleLocation)
        {
            Coordinate = new GeoCoordinate(simpleLocation.Latitude, simpleLocation.Longitude);
            Token = simpleLocation.Token;
            Created = DateTime.Now;
        }
        public Location(double latitude, double longitude)
        {
            Coordinate = new GeoCoordinate(latitude, longitude);
            Created = DateTime.Now;
        }
        
        public Location(double latCoord, double longCoord, DateTime created, Area area)
        {
            Coordinate = new GeoCoordinate(latCoord, longCoord);
            Created = created;
            CarrierDataPoint = false;
            Region = area;
        }

        public Location(double lat, double longCoord, DateTime created)
        {
            Coordinate = new GeoCoordinate(lat, longCoord);
            Created = created;
            CarrierDataPoint = false;
        }

        public Location(GeoCoordinate coord)
        {
            Coordinate = coord;
            CarrierDataPoint = false;
            Created = DateTime.Now;
        }

        public override string ToString()
        {
            return "Lat: " + Coordinate.Latitude + " Long: " + Coordinate.Longitude;
        }
    }
}
