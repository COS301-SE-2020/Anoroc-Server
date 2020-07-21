using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anoroc_User_Management.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Models.Tests
{
    [TestClass()]
    public class PrimitiveLocationTests
    {
        [TestMethod()]
        public void PrimitiveLocationTest()
        {
            Location location = new Location(1.23, 4.56, DateTime.Now);
            PrimitiveLocation primitive = new PrimitiveLocation(location);
            Assert.AreEqual(primitive.Location_ID, location.Location_ID);
            Assert.AreEqual(primitive.Coordinate, JsonConvert.SerializeObject(location.Coordinate));
            Assert.AreEqual(primitive.Carrier_Data_Point, location.Carrier_Data_Point);
            Assert.AreEqual(primitive.Created, location.Created);
        }
    }
}