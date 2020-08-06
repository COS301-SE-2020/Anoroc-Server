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

        public Dictionary<Location, int> ProcessItinerary(Itinerary itinerary)
        {
            double averageDensity = 0;
            if (itinerary.Locations != null)
            {
                if (itinerary.LocationItineraryRisks == null)
                    itinerary.LocationItineraryRisks = new Dictionary<Location, int>();

                List<Location> locationList = itinerary.Locations;
                locationList.ForEach(location =>
                {
                    var clusters = ClusterService.ClustersInRange(location, -1);
                    if (clusters != null)
                    {
                        if (clusters.Count > 0)
                        {
                            averageDensity = 0;
                            clusters.ForEach(cluster =>
                            {
                                averageDensity += CalculateDensity(cluster);
                            });
                            averageDensity /= clusters.Count;
                            averageDensity *= 100;

                            //itinerary.LocationItineraryRisks.Add(location, (int)averageDensity);

                            if(averageDensity > 50)
                                itinerary.LocationItineraryRisks.Add(location, 4);
                            else
                                itinerary.LocationItineraryRisks.Add(location, 3);
                        }
                    }
                });
            }
            return itinerary.LocationItineraryRisks;
        }

        public double CalculateDensity(Cluster cluster)
        {
            var area = Math.PI * Math.Pow(cluster.Cluster_Radius, 2);

            var density = cluster.Coordinates.Count / area;

            return density;
        }
    }
}
