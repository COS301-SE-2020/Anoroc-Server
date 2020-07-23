using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anoroc_User_Management.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Anoroc_User_Management.Models;

namespace Anoroc_User_Management.Services.Tests
{
    [TestClass()]
    public class Mock_ClusterServiceTests
    {
        [TestMethod()]
        public void AddLocationToClusterTest()
        {
            Mock_ClusterService clusterService = new Mock_ClusterService(true);

            clusterService.ReadJsonForTests();
            Location mockLocation = new Location(-26.275312, 28.065452);
            clusterService.AddLocationToCluster(mockLocation);
            Location secondMockLoc = new Location(-26.275355, 28.065396);
            List<Cluster> checkInRange = clusterService.ClustersInRage(secondMockLoc, 35.00);
            bool tester = false;
            foreach(Cluster yourCluster in checkInRange)
            {
                if (yourCluster.Coordinates.Contains(mockLocation))
                    tester = true;
            }
            Assert.IsTrue(tester);
        }
    }
}