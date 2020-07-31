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
        public ClusterManagementService(IClusterService clusterService)
        {
            ClusterService = clusterService;
        }
     
        public void BeginManagment()
        {
            // TODO:
            // Manage clusters
            ServiceToGenerateClusters();
        }

        public void DeleteLongClusters()
        {
            throw new NotImplementedException();
        }

        public void ServiceToGenerateClusters()
        {
            ClusterService.GenerateClusters();
        }
    }
}
