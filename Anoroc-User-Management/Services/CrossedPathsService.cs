using System;
using System.Collections.Generic;
using System.Linq;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;

namespace Anoroc_User_Management.Services
{
    /// <summary>
    /// Class responsible for providing functionality to check if users have crossed paths or not.
    /// </summary>
    public class CrossedPathsService : ICrossedPathsService
    {
        private readonly IClusterService _clusterService;
        private readonly IMobileMessagingClient _mobileMessagingClient;
        public readonly IDatabaseEngine _databaseEngine;
        public CrossedPathsService(IClusterService clusterService, IMobileMessagingClient mobileMessagingClient, IDatabaseEngine databaseEngine)
        {
            _clusterService = clusterService;
            _mobileMessagingClient = mobileMessagingClient;
            _databaseEngine = databaseEngine;
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
            List<Cluster> clusters = _clusterService.ClustersInRage(location, 5000.0);
            // Find cluster that the point resides in. cluster will be null if no area is found
            var cluster = clusters.FirstOrDefault(tempCluster => tempCluster.Contains(location));
            Console.WriteLine(cluster);
            if (cluster != null)
            {
                Console.WriteLine("Sending message...");
                // TODO Consider checking point timestamp to compare when the infection occured so you can alert other points in the area
                string firebaseToken = _databaseEngine.getFirebaseToken("thisisatoken");
                _mobileMessagingClient.SendNotification(location, firebaseToken);
            }
            else
            {
                Console.WriteLine("No cluster found!");
            }
        }
    }
}