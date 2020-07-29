using Anoroc_User_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace Anoroc_User_Management.Services
{
    /// <summary>
    /// Class helping the cluster service
    /// </summary>
    /// 
    
    public class Cluster : DbContext
    {
        public long Cluster_ID { get; }
        public List<Location> Coordinates { get; set; }
        public Location Center_Location { get; set; }
        public int Carrier_Data_Points;
        public DateTime Cluster_Created { get; set; }

        public double Cluster_Radius { get; set; }
        public Cluster()
        {
            // TODO:
        }
            // Create a function that scans through the list of clusters and removes the ones that have been there the longest
        

        public Cluster(Location loc, long cluster_id)
        {

            Coordinates = new List<Location>();

            Coordinates.Add(loc);

            Cluster_Created = DateTime.Now;

            Cluster_ID = cluster_id;

            if (loc.Carrier_Data_Point)
                Carrier_Data_Points++;

            Structurize();
        }
        public Cluster(List<Location> coords, long cluster_id)
        {
            Coordinates = coords;
            Cluster_ID = cluster_id;
            foreach(Location loc in coords)
                if (loc.Carrier_Data_Point)
                    Carrier_Data_Points++;

            Structurize();
        }

        public void Structurize()
        {
            Calculate_Center();
            Calculate_Radius();
        }


        /// <summary>
        /// Function that checks if a location provided belongs to this cluster based on the distance between any ONE location already in the cluster.
        /// </summary>
        /// <param name="location"> The location being tested to see if it belongs in the cluster. </param>
        /// <returns> True if the location belongs in the cluster, False otherwise. </returns>
        public bool Check_If_Belong(Location location)
        {
            bool belongs = false;
            if (location.Coordinate.GetDistanceTo(Center_Location.Coordinate) <= 200)
            { 
                belongs = true;
            }
            return belongs;
        }

        /// <summary>
        /// Function that checks if a location provided belongs to this cluster based on the distance between any ONE location already in the cluster.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>true if the cluster contains the location. false if it doesn't</returns>
        public bool Contains(Location location)
        {
            var contains = location.Coordinate.GetDistanceTo(Center_Location.Coordinate) <= 200;
            return contains;
        }

        /// <summary>
        ///  Adds a new location point into the cluster
        /// </summary>
        /// <param name="newCoord"> The new location point to be added into the cluster </param>
        public void AddLocation(Location newCoord)
        {
            Coordinates.Add(newCoord);
            if (newCoord.Carrier_Data_Point)
                Carrier_Data_Points++;
            Structurize();
        }


        /// <summary>
        /// Gets the percentage of the cluster that is made up of location points from carriers
        /// </summary>
        /// <returns> The percentage of carriers in the cluster </returns>
        public double Percentage_Carrier()
        {
            double percentage;
            if (Carrier_Data_Points > 0)
            {
                percentage = Carrier_Data_Points / Coordinates.Count;
                return Math.Round(percentage, 2, MidpointRounding.AwayFromZero);
            }
            else
                return 0;
        }

        /// <summary>
        /// Calculate center point of a cluster, code adapated from: https://gist.github.com/tlhunter - average-geolocation.js
        /// </summary>
        public void Calculate_Center()
        {
            Center_Location = null;
            if (Coordinates.Count == 1)
            {
                Center_Location = Coordinates[0];
            }

            var x = 0.0;
            var y = 0.0;
            var z = 0.0;

            foreach (var coord in Coordinates)
            {
                var latitude = coord.Coordinate.Latitude * Math.PI / 180;
                var longitude = coord.Coordinate.Longitude * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
            }

            var total = Coordinates.Count;

            x = x / total;
            y = y / total;
            z = z / total;

            var centralLongitude = Math.Atan2(y, x);
            var centralSquareRoot = Math.Sqrt(x * x + y * y);
            var centralLatitude = Math.Atan2(z, centralSquareRoot);

            Center_Location = new Location((centralLatitude * 180 / Math.PI), (centralLongitude * 180 / Math.PI), Cluster_Created);
        }



        /// <summary>
        /// Calculate the radius of the  cluster for drawing a circle on the map. The radius is calculated as the max(distance from a point to the center point)
        /// </summary>
        public void Calculate_Radius()
        {
            double radius = 0;
            int cluster_size = Coordinates.Count;
            double temp_distance;
            for (int i = 0; i < cluster_size - 1; i++)
            {
                temp_distance = Coordinates[i].Coordinate.GetDistanceTo(Center_Location.Coordinate);
                if (temp_distance > radius)
                    radius = temp_distance;
            }
            Cluster_Radius = radius;
        }
    }
}
