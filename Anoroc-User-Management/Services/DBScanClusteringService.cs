using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using DBSCAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class DBScanClusteringService : IClusterService
    {

        IDatabaseEngine DatabaseService;
        public DBScanClusteringService(IDatabaseEngine database)
        {
            DatabaseService = database;
            //DatabaseService.populate();
        }

        public void AddLocationToCluster(Location location)
        {
            throw new NotImplementedException();
        }

        public dynamic ClustersInRage(Location location, double Distance_To_Cluster_Center)
        {
            throw new NotImplementedException();
        }

        public dynamic GetClusters(Area area)
        {
            var clusters = DatabaseService.Select_List_Clusters();
            /*var clustersInArea = new List<Cluster>();

            clusters.ForEach(cluster =>
            {
                if (cluster.Center_Location.Region.Country == area.Country)
                    if (cluster.Center_Location.Region.Province == area.Province)
                        if (cluster.Center_Location.Region.Suburb == area.Suburb)
                            clustersInArea.Add(cluster);
            });*/

            return WrapClusters(clusters);
        }
        public dynamic GetClustersPins(Area area)
        {
            var clusters = DatabaseService.Select_List_Clusters();
            /*var clustersInArea = new List<Cluster>();

            clusters.ForEach(cluster =>
            {
                if (cluster.Center_Location.Region.Country == area.Country)
                    if (cluster.Center_Location.Region.Province == area.Province)
                        if (cluster.Center_Location.Region.Suburb == area.Suburb)
                            clustersInArea.Add(cluster);
            });*/

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

        private List<Cluster> PostProcessClusters(ClusterSet<IPointData> dbscanClusters)
        {
            var clusterWrapper = new List<Cluster>();
            foreach(var clusters in dbscanClusters.Clusters)
            {
                var customCluster = new Cluster();
                for(int i = 0; i < clusters.Objects.Count; i++)
                {
                    PointData pointData = (PointData)clusters.Objects[i];
                    customCluster.AddLocation(new Location(pointData._point.X, pointData._point.Y, pointData.Created, pointData.CarrierDataPoint, pointData.Region));
                }
                customCluster.Structurize();
                clusterWrapper.Add(customCluster);
            }
            return clusterWrapper;
        }

        
        public void GenerateClusters()
        {
            var LocationList = DatabaseService.Select_List_Locations();
           
            IList<IPointData> pointDataList = new List<IPointData>();

            if (LocationList != null)
            {
                LocationList.ForEach(location =>
                {
                    pointDataList.Add(new PointData(location.Latitude, location.Longitude, location.Carrier_Data_Point, location.Created, location.Region));
                });
                var clusters = DBSCAN.DBSCAN.CalculateClusters(pointDataList, epsilon: 0.002, minimumPointsPerCluster: 2);

                var customeClusters = PostProcessClusters(clusters);
            } 
        }
    }
}
