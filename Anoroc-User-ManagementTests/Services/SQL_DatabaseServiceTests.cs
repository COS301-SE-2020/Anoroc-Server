using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anoroc_User_Management.Interfaces;

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

        [TestMethod()]
        public void populateTest()
        {
            Assert.IsTrue(true);
        }
    }
}