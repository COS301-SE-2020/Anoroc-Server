using Anoroc_User_Management.Models;
using GeoCoordinatePortable;
using System;
using System.Collections.Generic;


namespace Anoroc_User_Management.Services
{
    /// <summary>
    /// Internal Class helping the cluster service
    /// </summary>
    /// 
    
    internal class Cluster
    {
        public List<Location> Coordinates { get; set; }
        private int Carrier_Data_Points;
        public Cluster()
        {
            // TODO:
            // Create a function that scans through the list of clusters and removes the ones that have been there the longest
        }

        public Cluster(Location loc)
        {
            Coordinates = new List<Location>();
            Coordinates.Add(loc);

            if (loc.Carrier_Data_Point)
                Carrier_Data_Points++;
        }


        /// <summary>
        /// Function that checks if a location provided belongs to this cluster based on the distance between any ONE location already in the cluster.
        /// </summary>
        /// <param name="location"> The location being tested to see if it belongs in the cluster. </param>
        /// <returns> True if the location belongs in the cluster, False otherwise. </returns>
        public bool Check_If_Belong(Location location)
        {
            bool belongs = false;
            foreach(Location loc in Coordinates)
            {
                if(location.Coordinate.GetDistanceTo(loc.Coordinate) <= 1000)
                {
                    belongs = true;
                    AddLocation(location);
                    break;
                }
            }
            return belongs;
        }

        /// <summary>
        ///  Adds a new location point into the cluster
        /// </summary>
        /// <param name="newCoord"> The new location point to be added into the cluster </param>
        private void AddLocation(Location newCoord)
        {
            Coordinates.Add(newCoord);
            if (newCoord.Carrier_Data_Point)
                Carrier_Data_Points++;
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
    }
}