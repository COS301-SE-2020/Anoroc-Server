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
            var LocationList = DatabaseService.Select_ListLocations();
            IList<DBSCAN.Point> PointList = new List<DBSCAN.Point>();
            IList<IPointData> pointDataList = new List<IPointData>();

            foreach (Location location in LocationList)
            {
                pointDataList.Add(new PointData(location.Coordinate.Latitude, location.Coordinate.Longitude, location.Carrier_Data_Point, location.Created));
            }

            var clusters = DBSCAN.DBSCAN.CalculateClusters(pointDataList, epsilon: 0.001, minimumPointsPerCluster: 2);
            return WrapClusters(PostProcessClusters(clusters));
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
                    customCluster.AddLocation(new Location(pointData._point.X, pointData._point.Y, pointData.Created, pointData.CarrierDataPoint));
                }
                clusterWrapper.Add(customCluster);
            }
            return clusterWrapper;
        }

        public dynamic GetClustersPins(Area area)
        {
            var LocationList = DatabaseService.Select_ListLocations();
            IList<DBSCAN.Point> PointList = new List<DBSCAN.Point>();
            IList<IPointData> pointDataList = new List<IPointData>();

            foreach(Location location in LocationList)
            {
                pointDataList.Add(new PointData(location.Coordinate.Latitude, location.Coordinate.Longitude,location.Carrier_Data_Point, location.Created));
            }

            var clusters = DBSCAN.DBSCAN.CalculateClusters(pointDataList, epsilon: 0.001, minimumPointsPerCluster: 2);
            return clusters;
        }
    }
}
