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
            throw new NotImplementedException();
        }

        public dynamic GetClustersPins(Area area)
        {
            var LocationList = DatabaseService.Select_ListLocations();
            IList<DBSCAN.Point> PointList = new List<DBSCAN.Point>();
            IList<IPointData> pointDataList = new List<IPointData>();

            foreach(Location location in LocationList)
            {
                pointDataList.Add(new PointData(location.Coordinate.Latitude, location.Coordinate.Longitude));
            }

            var clusters = DBSCAN.DBSCAN.CalculateClusters(pointDataList, epsilon: 1.0, minimumPointsPerCluster: 4);
            return clusters;
        }
    }
}
