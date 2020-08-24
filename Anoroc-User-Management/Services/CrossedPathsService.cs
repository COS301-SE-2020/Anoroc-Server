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
        public readonly double ProximityToCarrier;
        public CrossedPathsService(IClusterService clusterService, IMobileMessagingClient mobileMessagingClient, IDatabaseEngine databaseEngine, double proximityToCarrier)
        {
            _clusterService = clusterService;
            _mobileMessagingClient = mobileMessagingClient;
            _databaseEngine = databaseEngine;
            ProximityToCarrier = proximityToCarrier;
        }

        /// <summary>
        /// Processes a location point.
        /// If the point falls within a cluster (in danger), the user has to be notified.
        /// </summary>
        /// <param name="location">A location point that has to be processed</param>
        /// TODO Consider making this async
        public void ProcessLocation(Location location, string token)
        {
            string firebaseToken = _databaseEngine.Get_Firebase_Token(token);
            int risk = 0;
            var clusters = _clusterService.ClustersInRange(location, -1);
            if (clusters != null)
            {
                if (clusters.Count > 0)
                {
                    risk = RISK.MEDIUM_RISK;

                    clusters.ForEach(cluster =>
                    {
                        var coordinates = cluster.Coordinates;
                        
                        for(int i = 0; i < coordinates.Count; i++)
                        {
                            if(Cluster.HaversineDistance(location, coordinates.ElementAt(i)) <= ProximityToCarrier)
                            {
                                risk = RISK.HIGH_RISK;
                            }
                        }
                    });

                    _mobileMessagingClient.SendNotification(location, firebaseToken, risk);

                }
                else
                {
                    ProcessWithUnclusteredLocations(location, firebaseToken);
                }
            }
            else
            {
                ProcessWithUnclusteredLocations(location, firebaseToken);
            }
        }

        private void ProcessWithUnclusteredLocations(Location location, string firebaseToken)
        {
            int risk = 0;
            var unclusteredLocations = _clusterService.CheckUnclusteredLocations(location, ProximityToCarrier);
            if(unclusteredLocations != null)
            {
                if(unclusteredLocations.Count > 0)
                {
                    risk = RISK.HIGH_RISK;
                    _mobileMessagingClient.SendNotification(location, firebaseToken, risk);
                }
                else
                {
                    ProcessOldClusterLocations(location, firebaseToken);
                }
            }
            else
            {
                ProcessOldClusterLocations(location, firebaseToken);
            }
        }

        private void ProcessOldClusterLocations(Location location, string firebaseToken)
        {
            int risk = 0;
            var oldClusters = _clusterService.OldClustersInRange(location, -1);
            if(oldClusters != null)
            {
                if(oldClusters.Count > 0)
                {
                    risk = RISK.LOW_RISK;
                    oldClusters.ForEach(cluster =>
                    {
                        var coordinates = cluster.Coordinates;
                        for(int i = 0; i < coordinates.Count; i++)
                        {
                            if(Cluster.HaversineDistance(location, coordinates.ElementAt(i)) <= ProximityToCarrier)
                            {
                                risk = RISK.MODERATE_RISK;
                            }
                        }
                    });

                    _mobileMessagingClient.SendNotification(location, firebaseToken, risk);
                }
                else
                {
                    ProcessOldUnclusteredLocations(location, firebaseToken);
                }
            }
            else
            {
                ProcessOldUnclusteredLocations(location, firebaseToken);
            }
        }

        private void ProcessOldUnclusteredLocations(Location location, string firebaseToken)
        {
            int risk = 0;
            var unclusteredLocations = _clusterService.CheckOldUnclusteredLocations(location, ProximityToCarrier);
            if (unclusteredLocations != null)
            {
                if (unclusteredLocations.Count > 0)
                {
                    risk = RISK.MODERATE_RISK;
                    _mobileMessagingClient.SendNotification(location, firebaseToken, risk);
                }
                else
                {
                    LocationNotInRiks(location, firebaseToken);
                }
            }
            else
            {
                LocationNotInRiks(location, firebaseToken);
            }
        }

        private void LocationNotInRiks(Location location, string firebaseToken)
        {
           // TODO:
           // Do something if the user is found to be at no risk?
        }
    }
}