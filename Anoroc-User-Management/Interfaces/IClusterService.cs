using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Interfaces
{
    public interface IClusterService
    {
        /// <summary>
        ///  Get the summerized cluster information defined for the Area provided
        /// </summary>
        /// <param name="area"> The Area of the clusters that must be returned eg. Gauteng </param>
        /// <returns> A list of the clusters for that area, each cluster has a center location, radius, pin count, carrier pin count </returns>
        public dynamic GetClusters(Area area);

        /// <summary>
        ///  Get the clusters and the pins in each cluster defnined in the Area
        /// </summary>
        /// <param name="area"> The Area of the clusters that must be returned eg. Gauteng</param>
        /// <returns> A list of the clusters and in each cluster is a list of pins for that cluster </returns>
        public dynamic GetClustersPins(Area area);

        /// <summary>
        ///  Adds the location to the cluster it belongs in based on the cluster service algorthim
        /// </summary>
        /// <param name="location"> The location to be added to a cluster </param>
        public void AddLocationToCluster(Location location);

        /// <summary>
        ///  Checks the user's locaiton and determine which cluster they are close too, using a param defined in the crossed path service
        /// </summary>
        /// <param name="location"> The location to find the closest cluster to in Distance_To_Cluster_Center meters</param>
        /// <param name="Distance_To_Cluster_Center"> Meters distance of the locaiton point and the center of a cluster that would make the locaiton considered as close to</param>
        /// <returns> List of clusters the locaiton is close to </returns>
        public dynamic ClustersInRage(Location location, double Distance_To_Cluster_Center);
    }
}
