using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Class used to simplify the clusters so that we don't send too much data to the Mobile app.
    /// </summary>
    public class ClusterWrapper
    {
        public ClusterWrapper(DateTime _created, int pins, int carrier, double radius, Location center)
        {
            Pin_Count = pins;
            Carrier_Pin_Count = carrier;
            Cluster_Radius = radius;
            Center_Pin = center;
            Created = _created;
        }
        /// <summary>
        /// Attributes used to store cluster information
        /// </summary>
        /// <param name="Pin_Count"> number of locations in the cluster </param>
        /// <param name="Carrier_Pin_Count"> number of contagent carrier location points in the cluster </param>
        /// <param name="Cluster_Radius"> max (distance between any two points in the cluster) </param>
        /// <param name="Center_Pin"> Location variable of the center pin in the cluster</param>
        public int Pin_Count { get; set; }
        public int Carrier_Pin_Count { get; set; }
        public double Cluster_Radius { get; set; }
        public Location Center_Pin { get; set; }
        public DateTime Created { get; set; }
    }
}
