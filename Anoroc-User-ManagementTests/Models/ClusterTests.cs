using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anoroc_User_Management.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;

namespace Anoroc_User_Management.Services.Tests
{
    [TestClass()]
    public class ClusterTests
    {
        [TestMethod()]
        public void Calculate_CenterTest()
        {
            IClusterService clusterService = new Mock_ClusterService(new SQL_DatabaseService("Data Source=DESKTOP-FGF4947;Initial Catalog=Anoroc;Integrated Security=True"));

            List<Cluster> clusters = clusterService.GetClustersPins(new Area());

            Location center = clusters[0].Center_Location;

            Location TestCenter = new Location(-25.7669207258453, 28.32507578731504, DateTime.Now);

            Assert.AreEqual(TestCenter.Coordinate, center.Coordinate);
        }

        [TestMethod()]
        public void Calculate_RadiusTest()
        {
            IClusterService clusterService = new Mock_ClusterService(new SQL_DatabaseService("Data Source=DESKTOP-FGF4947;Initial Catalog=Anoroc;Integrated Security=True"));

            List<Cluster> clusters = clusterService.GetClustersPins(new Area());


            double ActualRadius = 111.58626995412816;

            Assert.AreEqual(ActualRadius, clusters[0].Cluster_Radius);
        }

        [TestMethod()]
        public void Check_If_BelongTest()
        {
            IClusterService clusterService = new Mock_ClusterService(new SQL_DatabaseService("Data Source=DESKTOP-FGF4947;Initial Catalog=Anoroc;Integrated Security=True"));

            List<Cluster> clusters = clusterService.GetClustersPins(new Area());

            Assert.IsTrue(clusters[0].Check_If_Belong(clusters[0].Coordinates[0]));
        }
    }
}