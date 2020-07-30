using GeoCoordinatePortable;
using System;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model class used to store and work with GEO Location Points
    /// </summary>
    public class Location //:DbContext
    {
        public long Location_ID { get; set; }
        public GeoCoordinate Coordinate { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public DateTime Created { get; set; }
        public Area Region { get; set; }

        // Token of the user owning this point
        public string Token { get; set; }
        public string UserAccessToken { get; set; }

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
        
        // To identify the user for the notification, pass the token and get the user detials from that and not the location class, otherwise we are
        // sending the details of the user everywhere
        

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
        public Location(double lat, double longCoord, DateTime created, bool carrier, Area area)
        {
            Region = area;
            Coordinate = new GeoCoordinate(lat, longCoord);
            Created = created;
            Carrier_Data_Point = carrier;
        }
        public Location(long locID, double lat, double longCoord)
        {
            Location_ID = locID;
            Coordinate = new GeoCoordinate(lat, longCoord);
            //Created = created;
            Carrier_Data_Point = false;
        }

        public Location(GeoCoordinate coord)
        {
            Coordinate = coord;
            Carrier_Data_Point = false;
            Created = DateTime.Now;
        }
        public Location(GeoCoordinate coord, DateTime creted, Area area)
        {
            Coordinate = coord;
            Carrier_Data_Point = false;
            Created = creted;
            Region = area;
        }
        public Location(GeoCoordinate coord, DateTime creted, Area area, bool carrier)
        {
            Coordinate = coord;
            Created = creted;
            Region = area;
            Carrier_Data_Point = carrier;
        }
        public Location()
        {

        }

        public override string ToString()
        {
            return "Lat: " + Coordinate.Latitude + " Long: " + Coordinate.Longitude;
        }

        public void toggleCarrierStatus()
        {
            Carrier_Data_Point = !Carrier_Data_Point;
        }
    }
}
