using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Interfaces
{
    public interface IClusterManagementService
    {
        /// <summary>
        /// Begins the task that runs every 4 hours to run the GenerateClusters function
        /// </summary>
        public void BeginManagment();


        public void DeleteLongClusters();
        /// <summary>
        /// Taks the locations that have not been added to a cluster yet and generates new clusters.
        /// This method will also remove the locations that have been in a cluster for too long, this length
        /// refers the when the location itself was created and not the cluster
        /// </summary>
        public void ServiceToGenerateClusters();
        /// <summary>
        /// Checks if the management service is running
        /// </summary>
        /// <returns> Boolean value relating to if the service is running or not </returns>
        public bool IsManagementRunning();

        /// <summary>
        /// Stops the service from executing
        /// </summary>
        public void StopManagment();
    }
}
