using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using DBSCAN;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class DBScanClusteringService : IClusterService
    {

        IDatabaseEngine DatabaseService;
        private int NumberOfPointsPerCluster;
        public DBScanClusteringService(IDatabaseEngine database, int _numberofpoints)
        {
            DatabaseService = database;
            NumberOfPointsPerCluster = _numberofpoints;
            //DatabaseService.populate();
        }

        public void AddLocationToCluster(Location location)
        {
            DatabaseService.Insert_Location(location);
        }

        /// <summary>
        ///  Check the clusters in range of the location
        /// </summary>
        /// <param name="location"> the location to be checked </param>
        /// <param name="Distance_To_Cluster_Center"> The distance to the cluster center, if -1 passed it will use the cluster radius </param>
        /// <returns> list of clusters that are in range </returns>
        public List<Cluster> ClustersInRange(Location location, double Distance_To_Cluster_Center)
        {
            var clusterList = DatabaseService.Select_Clusters_By_Area(location.Region);
            if (clusterList != null)
            { 
                var clustersInRange = new List<Cluster>();
                clusterList.ForEach(cluster =>
                {

                    if (Distance_To_Cluster_Center != -1)
                    {
                        var dist = Cluster.HaversineDistance(location, cluster.Center_Location);
                        if (dist <= Distance_To_Cluster_Center)
                        {
                            clustersInRange.Add(cluster);
                        }
                    }
                    else
                    {
                        var dist = Cluster.HaversineDistance(location, cluster.Center_Location);
                        if ( dist <= cluster.Cluster_Radius)
                        {
                            clustersInRange.Add(cluster);
                        }
                    }

                });

                if (clustersInRange.Count > 0)
                    return clustersInRange;
                else
                    return null;
            }
            else
                return null;
        }

        public List<Location> CheckUnclusteredLocations(Location location, double Direct_Distance_To_Location)
        {
            var locationList = DatabaseService.Select_Unclustered_Locations(location.Region);
            if(locationList != null)
            {
                var locationsInRange = new List<Location>();

                locationList.ForEach(loc =>
                {
                    var dist = Cluster.HaversineDistance(location, loc);
                    if (dist <= Direct_Distance_To_Location)
                    {
                        locationsInRange.Add(loc);
                    }
                });

                if (locationsInRange.Count > 0)
                    return locationsInRange;
                else
                    return null;
            }
            else
                return null;
        }



        public List<Cluster> OldClustersInRange(Location location, double Distance_To_Cluster_Center)
        {
            var oldClusterList = DatabaseService.Select_Old_Clusters_By_Area(location.Region);
            if(oldClusterList != null)
            {
                var clustersInRange = new List<Cluster>();

                oldClusterList.ForEach(oldCluster =>
                {   
                    var dist = Cluster.HaversineDistance(location, oldCluster.Center_Location);
                    if (dist <= Distance_To_Cluster_Center)
                    {
                        clustersInRange.Add(oldCluster.toCluster());
                    }
                });

                if (clustersInRange.Count > 0)
                    return clustersInRange;
                else
                    return null;
            }
            else
                return null;
        }

        public List<Location> CheckOldUnclusteredLocations(Location location, double Direct_Distance_To_Location)
        {
            var locationList = DatabaseService.Select_Old_Unclustered_Locations(location.Region);
            if (locationList != null)
            { 
                var locationsInRange = new List<Location>();

                locationList.ForEach(loc =>
                {
                    var dist = Cluster.HaversineDistance(location, loc.toLocation());
                    if (dist <= Direct_Distance_To_Location)
                    {
                        locationsInRange.Add(loc.toLocation());
                    }
                });

                if (locationsInRange.Count > 0)
                    return locationsInRange;
                else
                    return null;
            }
            else
                return null;
        }



        public dynamic GetClusters(Area area)
        {
            var clusters = DatabaseService.Select_List_Clusters();
            return WrapClusters(clusters);
        }
        public dynamic GetClustersPins(Area area)
        {
            var clusters = DatabaseService.Select_List_Clusters();
            return clusters;
        }

        private List<ClusterWrapper> WrapClusters(List<Cluster> clusters)
        {
            List<ClusterWrapper> clusterWrappers = new List<ClusterWrapper>();
            foreach(var cluster in clusters)
            {
                clusterWrappers.Add(new ClusterWrapper(cluster.Coordinates.Count, cluster.Carrier_Data_Points, cluster.Cluster_Radius, cluster.Center_Location));
            }

            return clusterWrappers;
        }

        private bool PostProcessClusters(ClusterSet<IPointData> dbscanClusters)
        {
            foreach(var clusters in dbscanClusters.Clusters)
            {
                var customCluster = new Cluster();
                for(int i = 0; i < clusters.Objects.Count; i++)
                {
                    PointData pointData = (PointData)clusters.Objects[i];

                    Location theLocation = DatabaseService.Select_Locations_By_ID(pointData.Location_ID).FirstOrDefault();

                    customCluster.AddLocation(theLocation);
                }
                customCluster.Structurize();
                DatabaseService.Insert_Cluster(customCluster);
            }
            return true;
        }

        
        public void GenerateClusters()
        {
            IList<IPointData> pointDataList = new List<IPointData>();
            var areaList = DatabaseService.Select_Unique_Areas();
            if (areaList != null)
            {
                areaList.ForEach(area =>
                {
                    var LocationList = DatabaseService.Select_Locations_By_Area(area);


                    if (LocationList != null)
                    {
                        LocationList.ForEach(location =>
                        {
                            pointDataList.Add(new PointData(location.Location_ID, location.Latitude, location.Longitude, location.Carrier_Data_Point, location.Created, location.Region));
                        });

                        var clusters = DBSCAN.DBSCAN.CalculateClusters(pointDataList, epsilon: 0.002, minimumPointsPerCluster: NumberOfPointsPerCluster);
                        var customeClusters = PostProcessClusters(clusters);

                        if(customeClusters)
                        {
                            //successfully added clusters
                            Debug.WriteLine("Calculated the clusters.");
                        }
                        else
                        {
                            // TODO:
                            // Retry logic
                        }
                    }
                    else
                    {
                        // TODO:
                        // Error handleing for no locations being recieved
                    }
                });
            }
            else
            {
                // TODO:
                // Error handleing for no area being recieved
            }
        }
    }
}
