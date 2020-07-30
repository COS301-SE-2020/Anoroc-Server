using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Anoroc_User_Management.Models;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Services.Tests
{
    [TestClass()]
    public class PrimitiveClusterTests
    {
        [TestMethod()]
        public void PrimitiveClusterTest()
        {
            Location testingLocation = new Location(1.23, 4.56, DateTime.Now);
            Cluster cluster = new Cluster(testingLocation,1);
            PrimitiveCluster primitive = new PrimitiveCluster(cluster);
            Assert.AreEqual(primitive.Cluster_ID, cluster.Cluster_Id);
            Assert.AreEqual(primitive.Coordinates, JsonConvert.SerializeObject(cluster.Coordinates));
            Assert.AreEqual(primitive.Center_Location, JsonConvert.SerializeObject(cluster.Center_Location));
            Assert.AreEqual(primitive.Carrier_Data_Points, cluster.Carrier_Data_Points);
            Assert.AreEqual(primitive.Cluster_Created, cluster.Cluster_Created);
            Assert.AreEqual(primitive.Cluster_Radius, cluster.Cluster_Radius);
        }
    }
}