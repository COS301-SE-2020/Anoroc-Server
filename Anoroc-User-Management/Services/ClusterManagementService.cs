using Anoroc_User_Management.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class ClusterManagementService : IClusterManagementService
    {
        IClusterService ClusterService;
        IDatabaseEngine DatabaseEngine;
        public ClusterManagementService(IClusterService clusterService, IDatabaseEngine database)
        {
            ClusterService = clusterService;
            DatabaseEngine = database;
        }
     
        public void BeginManagment()
        {
            DatabaseEngine.Delete_Locations_Older_Than_Hours(4);
            DeleteLongClusters();
            ServiceToGenerateClusters();
        }

        public void DeleteLongClusters()
        {
            var clusterList = DatabaseEngine.Select_List_Clusters();
            clusterList.ForEach(cluster =>
            {
                DatabaseEngine.Insert_Old_Cluster(cluster);
                DatabaseEngine.Delete_Cluster(cluster);
            });
        }

        public void ServiceToGenerateClusters()
        {
            ClusterService.GenerateClusters();
        }
    }
}
