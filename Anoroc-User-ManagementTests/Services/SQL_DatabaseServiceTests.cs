using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anoroc_User_Management.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Anoroc_User_Management.Interfaces;
using Microsoft.IdentityModel.Protocols;

namespace Anoroc_User_Management.Services.Tests
{
    [TestClass()]
    public class SQL_DatabaseServiceTests
    {
        [TestMethod()]
        public void Test_ConnectionTest()
        {
            IDatabaseEngine database = new SQL_DatabaseService("Data Source=DESKTOP-FGF4947;Initial Catalog=Anoroc;Integrated Security=True");

            Assert.IsTrue(database.Test_Connection());
        }
    }
}