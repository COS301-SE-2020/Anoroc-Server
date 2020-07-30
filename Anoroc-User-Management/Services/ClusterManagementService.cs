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
            IsRunning = false;
        }
        private static bool IsRunning;
        public void BeginManagment()
        {
            IsRunning = true;
            Task.Run(async () =>
            {
                while (IsRunning)
                {
                    ServiceToGenerateClusters();
                    await Task.Delay((int)TimeSpan.FromMinutes(240).TotalMilliseconds);
                }
            });
        }

        public void DeleteLongClusters()
        {
            throw new NotImplementedException();
        }

        public void ServiceToGenerateClusters()
        {
            ClusterService.GenerateClusters();
        }

        public bool IsManagementRunning()
        {
            return IsRunning;
        }

        public void StopManagment()
        {
            IsRunning = false;
        }
    }
}
