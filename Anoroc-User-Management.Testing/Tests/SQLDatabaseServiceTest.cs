using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Anoroc_User_Management.Testing.Tests
{
    public class SQLDatabaseServiceTest : IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        public SQLDatabaseServiceTest(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public void Insert_Location()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            Location location = new Location( 1111,  1111, now, area, true);
            Assert.True(database.Insert_Location(location));
        }

        [Fact]
        public void Delete_Location()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            Location location = new Location(1111, 1111, now, area, true);
            database.Insert_Location(location);
            Assert.True(database.Delete_Location(location));
        }
        [Fact]
        public void Update_Location()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            Location location = new Location(1111, 1111, now, area, true);
            database.Insert_Location(location);
            location.Carrier_Data_Point = false;
            Assert.True(database.Update_Location(location));
        }
        [Fact]
        public void Select_List_Locations()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            Location location = new Location(1111, 1111, now, area, true);
            database.Insert_Location(location);
            List<Location> list = database.Select_List_Locations();
            Assert.Single(list);
        }
        [Fact]
        public void Select_List_Carrier_Locations()
        {

        }
        [Fact]
        public void Select_Locations_By_Area()
        {

        }
        [Fact]
        public void Select_Locations_By_ID()
        {

        }
        [Fact]
        public void Select_Unclustered_Locations()
        {

        }
        [Fact]
        public void Update_Carrier_Locations()
        {

        }
        [Fact]
        public void Select_Unique_Areas()
        {

        }
        [Fact]
        public void Update_Cluster()
        {
            
        }

        [Fact]
        public void Delete_Cluster()
        {

        }
        [Fact]

        public void Insert_Cluster()
        {

        }
        [Fact]
        public void Select_List_Clusters(){}

        [Fact]
        public void Select_Clusters_By_Area(){}

        [Fact]
        public void Get_Cluster_ID(){}

        [Fact]
        public void Insert_Firebase_Token(){}

        [Fact]
        public void Update_Carrier_Status(){}

        [Fact]
        public void Get_User_Email(){}

        [Fact]
        public void Select_List_Users(){}

        [Fact]
        public void Update_User(){}

        [Fact]
        public void Delete_User(){}

        [Fact]
        public void Insert_User(){}

        [Fact]
        public void Get_Location_By_Longitude(){}


        [Fact]
        public void Get_Cluster_ByID(){ }
        [Fact]
        public void Get_Firebase_Token(){}

        [Fact]
        public void Validate_Access_Token(){}

        [Fact]
        public void Get_User_Access_Token(){}

        [Fact]
        public void Increment_Incidents(){}

        [Fact]
        public void Get_Incidents(){}

        [Fact]
        public void Set_Incidents(){}

        [Fact]
        public void Get_Profile_Picture(){ }
        [Fact]
        public void Set_Profile_Picture(){}

        [Fact]
        public void Get_User_ByID(){}

        [Fact]
        public void Insert_Area(){}

        [Fact]
        public void Delete_Area(){}

        [Fact]
        public void Select_Area_By_Id(){ }
        [Fact]
        public void Select_All_Old_Clusters(){ }
        [Fact]
        public void Select_Old_Clusters_By_Area(){}

        [Fact]
        public void Insert_Old_Cluster(){}

        [Fact]
        public void Populate_Coordinates(){}

        [Fact]
        public void Select_Old_Unclustered_Locations(){ }
        [Fact]
        public void Select_All_Old_Locations(){}

        [Fact]
        public void Insert_Old_Location(){ }
        [Fact]
        public void Insert_Itinerary_Risk(){}

        [Fact]
        public void Get_All_Itinerary_Risks(){ }
        [Fact]
        public void Get_Itinerary_Risks_By_Token(){}

        [Fact]
        public void Delete_Itinerary_Risk_By_ID(){ }
        [Fact]
        public void Get_Itinerary_Risk_By_ID(){}

        [Fact]
        public void Get_Access_Token_Via_FirebaseToken(){ }
        [Fact]
        public void Get_All_Notifications_Of_User(){}

        [Fact]
        public void Get_Area_By_ID(){ }
        [Fact]
        public void Add_Notification(){ }
        [Fact]
        public void Clear_Notifications_Two_Weeks(){}

        [Fact]
        public void Clear_Notifications_From_Days(){}

        [Fact]
        public void Set_Totals(){ }
        [Fact]
        public void Get_Totals(){}
    }
}
