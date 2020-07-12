using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class CSM_ClusterService : IClusterService
    {
        IDatabaseEngine Database_Engine;
        public CSM_ClusterService(IDatabaseEngine databse)
        {
            Database_Engine = databse;
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
            throw new NotImplementedException();
        }
    }
}
