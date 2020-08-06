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
            List<Location> locationList = itinerary.Locations;
            locationList.ForEach(location =>
            {
                var clusters = ClusterService.ClustersInRange(location, -1);
                if(clusters.Count > 0)
                {
                    itinerary.LocationItineraryRisks.Add(location, 1);
                }
            });
        }
    }
}
