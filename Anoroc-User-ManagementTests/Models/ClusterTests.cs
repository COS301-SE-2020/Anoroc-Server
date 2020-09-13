using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anoroc_User_Management.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using System.Linq;

namespace Anoroc_User_Management.Services.Tests
{
    [TestClass()]
    public class ClusterTests
    {
        [TestMethod()]
        public void Calculate_CenterTest()
        {
            Mock_ClusterService clusterService = new Mock_ClusterService(true);

            List<Cluster> clusters = clusterService.ReadJsonForTests();

            Location center = clusters[0].Center_Location;

            Location TestCenter = new Location(-25.7669207258453, 28.32507578731504, DateTime.Now);

            Assert.AreEqual(TestCenter.Latitude, center.Latitude);
            Assert.AreEqual(TestCenter.Longitude, center.Longitude);
        }

        [TestMethod()]
        public void Calculate_RadiusTest()
        {
            Mock_ClusterService clusterService = new Mock_ClusterService(true);

            List<Cluster> clusters = clusterService.ReadJsonForTests();


            double ActualRadius = 111.49002209304558;

            Assert.AreEqual(ActualRadius, clusters[0].Cluster_Radius);
        }

        [TestMethod()]
        public void Check_If_BelongTest()
        {
            Mock_ClusterService clusterService = new Mock_ClusterService(true);

            List<Cluster> clusters = clusterService.ReadJsonForTests();

            Assert.IsTrue(clusters[0].Check_If_Belong(clusters[0].Coordinates.ElementAt(0)));
        }

        [TestMethod()]
        public void HaversineDistanceTest()
        {
            Location location1 = new Location(37.4219983333333, -122.084, DateTime.Now, true, new Area("United States", "California","Los Angeles", "Mountain View"));
            Location location2 = new Location(37.4219983333333, -122.084, DateTime.Now, true, new Area("United States", "California", "Los Angeles", "Mountain View"));

            double distance = Cluster.HaversineDistance(location1, location2);

            Assert.AreEqual(distance, 0.0);

            Location location3 = new Location(49.3804135, 8.6744913, DateTime.Now, true, new Area());
            Location location4 = new Location(-25.7545444, 28.2292589, DateTime.Now, true, new Area());

            double distance2 = Cluster.HaversineDistance(location3, location4);
         
            Assert.AreEqual(distance2, 8576574.472085688);
        }
    }
}