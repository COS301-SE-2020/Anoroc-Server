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
        int HoursConsideredOld;
        int DaysAllowedToStore;
        public ClusterManagementService(IClusterService clusterService, IDatabaseEngine database, int youngAgeHours, int oldAgeHours)
        {
            ClusterService = clusterService;
            DatabaseEngine = database;
            HoursConsideredOld = youngAgeHours;
            DaysAllowedToStore = oldAgeHours;
        }
     
        public void BeginManagment()
        {
            //DatabaseEngine.Delete_Locations_Older_Than_Hours(HoursConsideredOld);
            //DeleteLongClusters();
            ServiceToGenerateClusters();
            var timeNow = DateTime.UtcNow.AddDays(-DaysAllowedToStore);
            var areas = DatabaseEngine.Select_Unique_Areas();
            if(areas != null)
            {
                areas.ForEach(area =>
                {
                    var clusters = DatabaseEngine.Select_Old_Clusters_By_Area(area).Where(cluster => cluster.Cluster_Created <= timeNow).ToList();
                    if(clusters != null)
                    {
                        clusters.ForEach(c =>
                        {
                            DatabaseEngine.Delete_Cluster(c);
                        });
                    }
                });
            }
            //DatabaseEngine.Delete_Old_Locations_Older_Than_Days(DaysAllowedToStore);
            //DatabaseEngine.Delete_Old_Clusters_Older_Than_Days(DaysAllowedToStore);
        }

        public void InsertClustersToOldClusters()
        {
            var clusterList = DatabaseEngine.Select_List_Clusters();
            clusterList.ForEach(cluster =>
            {
                //DatabaseEngine.Insert_Old_Cluster(cluster);
                //DatabaseEngine.Delete_Cluster(cluster);
            });
        }

        public void ServiceToGenerateClusters()
        {
            ClusterService.GenerateClusters();
        }
    }
}
