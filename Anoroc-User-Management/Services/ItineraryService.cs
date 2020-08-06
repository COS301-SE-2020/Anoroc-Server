using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class ItineraryService : IItineraryService
    {
        IClusterService ClusterService;
        public ItineraryService(IClusterService clusterService)
        {
            ClusterService = clusterService;
        }

        public void ProcessItinerary(Itinerary itinerary)
        {
            if (itinerary.Locations != null)
            {
                List<Location> locationList = itinerary.Locations;
                locationList.ForEach(location =>
                {
                    var clusters = ClusterService.ClustersInRange(location, -1);
                    if (clusters != null)
                    {
                        if (clusters.Count > 0)
                        {
                            double averageDensity = 0;
                            clusters.ForEach(cluster =>
                            {
                                averageDensity += CalculateDensity(cluster);
                            });
                            averageDensity /= clusters.Count;

                            itinerary.LocationItineraryRisks.Add(location, 1);
                        }
                    }
                });
            }
        }

        public double CalculateDensity(Cluster cluster)
        {
            var area = Math.PI * Math.Pow(cluster.Cluster_Radius, 2);

            var density = cluster.Coordinates.Count / area;

            return density;
        }
    }
}
