﻿using GeoCoordinatePortable;
using System;
using Microsoft.EntityFrameworkCore;

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

        // To identify the user for the notification, pass the token and get the user detials from that and not the location class, otherwise we are
        // sending the details of the user everywhere
        public string UserAccessToken { get; set; }

        public Location(long LocID, double latCoord, double longCoord, DateTime created, Area area)
        {
            Location_ID = LocID;
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
