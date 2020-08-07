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
        IDatabaseEngine DatabaseEngine;
        public ItineraryService(IClusterService clusterService, IDatabaseEngine databaseEngine)
        {
            DatabaseEngine = databaseEngine;
            ClusterService = clusterService;
        }


        public ItineraryRiskWrapper GetItineraries(int pagination, string access_token)
        {
            throw new NotImplementedException();
        }


        public ItineraryRiskWrapper ProcessItinerary(Itinerary userItinerary, string access_token)
        {
            double averageClusterDensity = 0;

            ItineraryRisk itinerary = new ItineraryRisk();

            if (userItinerary.Locations != null)
            {
                if (itinerary.LocationItineraryRisks == null)
                    itinerary.LocationItineraryRisks = new Dictionary<Location, int>();

                List<Location> locationList = userItinerary.Locations;

                locationList.ForEach(location =>
                {
                    var clusters = ClusterService.ClustersInRange(location, -1);
                    if (clusters != null)
                    {
                        if (clusters.Count > 0)
                        {
                            averageClusterDensity = 0;
                            clusters.ForEach(cluster =>
                            {
                                averageClusterDensity += CalculateDensity(cluster);
                            });
                            averageClusterDensity /= clusters.Count;
                            averageClusterDensity *= 100;

                            //itinerary.LocationItineraryRisks.Add(location, (int)averageDensity);

                            if(averageClusterDensity > 50)
                                itinerary.LocationItineraryRisks.Add(location, Risk.HIGH_RISK);
                            else
                                itinerary.LocationItineraryRisks.Add(location, Risk.MEDIUM_RISK);
                        }
                    }
                });
                itinerary.TotalItineraryRisk = CalculateTotalRisk(itinerary.LocationItineraryRisks);
                //itinerary.UserEmail = DatabaseEngine.GetUserEmail(access_token);
                //DatabaseEngine.InsertItierary(itinerary);

            }
            return new ItineraryRiskWrapper(itinerary);
        }

        private int CalculateTotalRisk(Dictionary<Location, int> locationItineraryRisks)
        {
            int risk = 0;
            for(int i = 0; i< locationItineraryRisks.Values.Count; i++)
            {
                risk += locationItineraryRisks.Values.ElementAt(i);
            }
            risk /= locationItineraryRisks.Values.Count;
            return risk;
        }

        public double CalculateDensity(Cluster cluster)
        {
            var area = Math.PI * Math.Pow(cluster.Cluster_Radius, 2);

            var density = cluster.Coordinates.Count / area;

            return density;
        }
    }
}
