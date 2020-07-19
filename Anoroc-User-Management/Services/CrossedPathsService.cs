using System.Collections.Generic;
using System.Linq;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;

namespace Anoroc_User_Management.Services
{
    /// <summary>
    /// Class responsible for providing functionality to check if users have crossed paths or not.
    /// </summary>
    public class CrossedPathsService
    {
        private readonly IClusterService _clusterService;
        private readonly IMobileMessagingClient _mobileMessagingClient;
        public CrossedPathsService(IClusterService clusterService, IMobileMessagingClient mobileMessagingClient)
        {
            _clusterService = clusterService;
            _mobileMessagingClient = mobileMessagingClient;
            _mobileMessagingClient.SendNotification();
        }

        /// <summary>
        /// Processes a location point.
        /// If the point falls within a cluster (in danger), the user has to be notified.
        /// </summary>
        /// <param name="location">A location point that has to be processed</param>
        /// TODO Consider making this async
        public void ProcessLocation(Location location)
        {
            // figure out what area to use.
            List<Cluster> clusters = _clusterService.GetClusters(new Area());
            // Find cluster that the point resides in. cluster will be null if no area is found
            var cluster = clusters.FirstOrDefault(tempCluster => tempCluster.Contains(location));

            if (cluster != null)
            {
                // TODO Notify user of danger (requires Notification service)
                // Consider checking point timestamp to compare when the infection occured so you can alert other points in the area
                
                
            }
        }
    }
}