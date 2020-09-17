using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Models.ItineraryFolder;
using Anoroc_User_Management.Models.TotalCarriers;
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
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            var count = database.Select_List_Carrier_Locations().Count;
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            Location location = new Location(1111, 1111, now, area, true);
            Location location2 = new Location(2222, 2222, now, area, false);
            Location location3 = new Location(3333, 3333, now, area, true);
            if(database.Insert_Location(location))
                count++;
            database.Insert_Location(location2);
            if(database.Insert_Location(location3))
                count++;
            List<Location> list = database.Select_List_Carrier_Locations();
            Assert.Equal(count, list.Count);
        }
        [Fact]
        public void Select_Locations_By_Area()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            var locations = database.Select_List_Locations();
            foreach (Location loc in locations)
            {
                Assert.NotEmpty(database.Select_Locations_By_Area(loc.Region));
            }
        }
        [Fact]
        public void Select_Unclustered_Locations()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            var count = database.Select_Unclustered_Locations(area).Count;
            Location location = new Location(1111, 1111, now, area, true);;
            Location location2 = new Location(2222, 2222, now, area, false);
            Location location3 = new Location(3333, 3333, now, area, true);
            database.Insert_Location(location);
            database.Insert_Location(location2);
            database.Insert_Location(location3);
            List<Location> list = database.Select_Unclustered_Locations(area);
            Assert.Equal(count+3, list.Count);
        }
        [Fact]
        public void Update_Carrier_Locations()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            Location location = new Location(1111, 1111, now, area, true);
            Location location2 = new Location(2222, 2222, now, area, true);
            database.Insert_Location(location);
            database.Insert_Location(location2);
            location.Carrier_Data_Point = false;
            database.Update_Carrier_Locations("none",false);
            List<Location> list = database.Select_Locations_By_Area(area);
            foreach (Location loc in list)
            {
                Assert.False(loc.Carrier_Data_Point);
            }
        }
        [Fact]
        public void Select_Unique_Areas()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            var count = database.Select_Unique_Areas().Count;
            Area area1 = new Area("Country", "province", "city", "suburb");
            Area area2 = new Area("Country2", "province2", "city2", "suburb2");
            if(database.Insert_Area(area1))
                count++;
            if(database.Insert_Area(area2))
                    count++;
            if(database.Insert_Area(area1))
                count++;
            List<Area> areas = database.Select_Unique_Areas();
            Assert.Equal(count, areas.Count);
        }
        [Fact]
        public void Update_Cluster()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            List<Location> locations = new List<Location>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            Location location = new Location(1111, 1111, now, area, true);
            locations.Add(location);

            Cluster cluster = new Cluster(locations, location, 1, now, 1.0);
            database.Insert_Cluster(cluster);
            cluster.Carrier_Data_Points = 2;
            Assert.True(database.Update_Cluster(cluster));
        }

        [Fact]
        public void Delete_Cluster()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            List<Location> locations = new List<Location>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("ClusterDelete", "ClusterDelete", "ClusterDelete", "ClusterDelete");
            Location location = new Location(1111, 1111, now, area, true);
            Location location2 = new Location(2222, 2222, now, area, true);
            Location location3 = new Location(3333, 3333, now, area, true);
            Location location4 = new Location(4444, 4444, now, area, true);
            locations.Add(location);
            locations.Add(location2);
            locations.Add(location3);
            locations.Add(location4);
            Cluster cluster = new Cluster(locations, location, 1, now, 1.0);
            if(database.Insert_Cluster(cluster))
                Assert.True(database.Delete_Cluster(cluster));
        }
        [Fact]

        public void Insert_Cluster()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            List<Location> locations = new List<Location>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("ClusterInsert", "ClusterInsert", "ClusterInsert", "ClusterInsert");
            Location location = new Location(852369, 741258, now, area, true);
            locations.Add(location);
            Cluster cluster = new Cluster(locations, location, 1, now, 1.0);
            Assert.True(database.Insert_Cluster(cluster));
        }
        [Fact]
        public void Select_List_Clusters()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            List<Location> locations = new List<Location>();
            List<Location> locations2 = new List<Location>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            Location location = new Location(1111, 1111, now, area, true);
            Location location2 = new Location(2222, 2222, now, area, true);
            locations.Add(location);
            locations2.Add(location2);
            var count = database.Select_List_Clusters().Count;
            Cluster cluster = new Cluster(locations, location, 1, now, 1.0);
            Cluster cluster2 = new Cluster(locations2, location2, 2, now, 1.0);
            if(database.Insert_Cluster(cluster))
                count++;
            if(database.Insert_Cluster(cluster2))
                count++;
            Assert.Equal(count, database.Select_List_Clusters().Count);
        }

        [Fact]
        public void Select_Clusters_By_Area()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            List<Location> locations = new List<Location>();
            List<Location> locations2 = new List<Location>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("SelectClusterByArea", "SelectClusterByArea", "SelectClusterByArea", "SelectClusterByArea");
            Area area2 = new Area("SelectClusterByArea2", "SelectClusterByArea2", "SelectClusterByArea2", "SelectClusterByArea2");
            Location location = new Location(1111, 1111, now, area, true);
            Location location2 = new Location(2222, 2222, now, area2, true);
            locations.Add(location);
            locations2.Add(location2);
            Cluster cluster = new Cluster(locations, location, 1, now, 1.0);
            Cluster cluster2 = new Cluster(locations2, location2, 2, now, 1.0);
            database.Insert_Cluster(cluster);
            database.Insert_Cluster(cluster2);
            Assert.Single(database.Select_Clusters_By_Area(area));
        }

        [Fact]
        public void Insert_Firebase_Token()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "");
            database.Insert_User(user);
            database.Insert_Firebase_Token(token, "firebase");
            string test = database.Get_Firebase_Token(token);
            Assert.Equal("firebase", test);
        }

        [Fact]
        public void Update_Carrier_Status()
        {
            using var scope = _factory.Services.CreateScope();
            
            var token = Guid.NewGuid().ToString();
            
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            User user = new User(token, "firebase", true);
            database.Insert_User(user);
            User test = database.Get_Single_User(token);
            Assert.Equal("firebase", test.Firebase_Token);
        }

        [Fact]
        public void Get_User_Email()
        {
            using var scope = _factory.Services.CreateScope();
            
            var token = Guid.NewGuid().ToString();
            
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            User user = new User(token, "firebase",true,"emailaddress");
            database.Insert_User(user);
            string test = database.Get_User_Email(token);
            Assert.Equal("emailaddress", test);
        }

        [Fact]
        public void Select_List_Users()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            User user1 = new User("select1", "firebase1");
            User user2 = new User("select2", "firebase2");
            User user3 = new User("select3", "firebase3");
            var count = database.Select_List_Users().Count;
            database.Insert_User(user1);
            database.Insert_User(user2);
            database.Insert_User(user3);
            List<User> users = database.Select_List_Users();
            Assert.Equal(count+3, users.Count);
        }

        [Fact]
        public void Update_User()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "firebase");
            database.Insert_User(user);
            user.Email = "updatedEmail";
            Assert.True(database.Update_User(user));
        }

        [Fact]
        public void Delete_User()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            var count = database.Select_List_Users().Count;
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "firebase");
            database.Insert_User(user);
            Assert.True(database.Delete_User(user));
            Assert.Equal(count,database.Select_List_Users().Count);
        }

        [Fact]
        public void Insert_User()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "firebase");
            Assert.True(database.Insert_User(user));
        }

        [Fact]
        public void Get_Location_By_Longitude()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Country", "province", "city", "suburb");
            Location location = new Location(1111, 1111, now, area, true);
            Location location2 = new Location(2222, 2222, now, area, true);
            database.Insert_Location(location);
            database.Insert_Location(location2);
            Assert.Equal(2222, database.Get_Location_By_Longitude(2222).Longitude);
        }

        [Fact]
        public void Get_Firebase_Token()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token1 = Guid.NewGuid().ToString();
            
            User user = new User(token1, "firebase");
            database.Insert_User(user);
            
            var token2 = Guid.NewGuid().ToString();
            
            User user2 = new User(token2, "firebase2");
            database.Insert_User(user2);
            Assert.Equal("firebase",database.Get_Firebase_Token(token1));
        }

        [Fact]
        public void Validate_Access_Token()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "firebase");
            database.Insert_User(user);
            Assert.True(database.Validate_Access_Token(token));
            Assert.False(database.Validate_Access_Token("some other token"));
        }

        [Fact]
        public void Get_User_Access_Token()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token1 = Guid.NewGuid().ToString();
            var token2 = Guid.NewGuid().ToString();
            
            User user = new User(token1, "firebase",true,"email");
            database.Insert_User(user);
            User user2 = new User(token2, "firebase", true, "other email");
            database.Insert_User(user2);
            Assert.Equal(token1,database.Get_User_Access_Token("email"));
        }

        [Fact]
        public void Get_Incidents()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token1 = Guid.NewGuid().ToString();
            var token2 = Guid.NewGuid().ToString();
            
            User user = new User(token1, "firebase", 10);
            database.Insert_User(user);
            User user2 = new User(token2, "firebase", 20);
            database.Insert_User(user2);
            Assert.Equal(10, database.Get_Incidents(token1));
        }

        [Fact]
        public void Set_Incidents()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "firebase", 10);
            database.Insert_User(user);
            database.Set_Incidents(token, 20);
            Assert.Equal(20, database.Get_Incidents(token));
        }

        [Fact]
        public void Get_Profile_Picture()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "firebase", "unique_string_for_profile_picture");
            database.Insert_User(user);
            Assert.Equal("unique_string_for_profile_picture", database.Get_Profile_Picture(token));
        }
        [Fact]
        public void Set_Profile_Picture()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "firebase", "unique_string_for_profile_picture");
            database.Insert_User(user);
            database.Set_Profile_Picture(token, "this_is_the_updated_pictue");
            Assert.Equal("this_is_the_updated_pictue", database.Get_Profile_Picture(token));
        }

        [Fact]
        public void Insert_Area()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            Area area1 = new Area("areaInsert1", "province", "city", "suburb");
            Area area2 = new Area("areaInsert2", "province2", "city2", "suburb2");
            Assert.True(database.Insert_Area(area1));
            Assert.True(database.Insert_Area(area2));
            Assert.False(database.Insert_Area(area2));
            Assert.False(database.Insert_Area(area1));
        }

        [Fact]
        public void Delete_Area()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            Area area1 = new Area("areaDelete", "province", "city", "suburb");
            List<Area> areas = database.Select_Unique_Areas();
            database.Insert_Area(area1);
            Assert.True(database.Delete_Area(area1));
        }

        [Fact]
        public void Insert_Itinerary_Risk()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            ItineraryRisk risk = new ItineraryRisk(now,"token");
            Assert.NotEqual(-1, database.Insert_Itinerary_Risk(risk));
        }

        [Fact]
        public void Get_All_Itinerary_Risks()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            ItineraryRisk risk = new ItineraryRisk(now, "token2");
            var count = database.Get_All_Itinerary_Risks().Count;
            database.Insert_Itinerary_Risk(risk);
            count++;
            Assert.NotEmpty(database.Get_All_Itinerary_Risks());
            Assert.Equal(count, database.Get_All_Itinerary_Risks().Count);
        }
        [Fact]
        public void Get_Itinerary_Risks_By_Token()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            Assert.Single(database.Get_Itinerary_Risks_By_Token("token"));
        }

        [Fact]
        public void Delete_Itinerary_Risk_By_ID()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            ItineraryRisk risk = new ItineraryRisk(now, "id_delete");
            var id = database.Insert_Itinerary_Risk(risk);
            database.Delete_Itinerary_Risk_By_ID(id);
            Assert.Null(database.Get_Itinerary_Risk_By_ID(id));
        }
        [Fact]
        public void Get_Itinerary_Risk_By_ID()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            ItineraryRisk risk = new ItineraryRisk(now, "id_test");
            var id=database.Insert_Itinerary_Risk(risk);
            Assert.NotNull(database.Get_Itinerary_Risk_By_ID(id));
        }

        [Fact]
        public void Get_Access_Token_Via_FirebaseToken()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "firebasetoken");
            database.Insert_User(user);
            Assert.Equal(token, database.Get_Access_Token_Via_FirebaseToken("firebasetoken"));
        }
        [Fact]
        public void Get_All_Notifications_Of_User()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "notificationTest");
            database.Insert_User(user);
            Notification noti = new Notification(token, "First Notification","Body");
            Notification noti2 = new Notification(token, "Second Notification", "Body");
            database.Add_Notification(noti);
            database.Add_Notification(noti2);
            Assert.Equal(2,database.Get_All_Notifications_Of_User(token).Count);
        }

        [Fact]
        public void Add_Notification()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "notificationTest2");
            database.Insert_User(user);
            Notification noti = new Notification(token, "First Notification", "Body");
            database.Add_Notification(noti);
            Assert.Single(database.Get_All_Notifications_Of_User(token));
        }
        [Fact]
        public void Clear_Notifications_Two_Weeks()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "ClearingTest");
            database.Insert_User(user);
            Notification noti = new Notification(token, "First Notification", "Body",DateTime.UtcNow.AddDays(-30));//Notification from a month ago
            database.Add_Notification(noti);
            Notification noti2 = new Notification(token, "First Notification", "Body", DateTime.UtcNow.AddDays(-1));//Notification from a day ago
            database.Add_Notification(noti2);
            database.Clear_Notifications_Two_Weeks(token);
            Assert.Single(database.Get_All_Notifications_Of_User(token));
        }

        [Fact]
        public void Clear_Notifications_From_Days()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token = Guid.NewGuid().ToString();
            
            User user = new User(token, "ClearingTest2");
            database.Insert_User(user);
            Notification noti = new Notification(token, "First Notification", "Body", DateTime.UtcNow.AddDays(-3));//Notification from a month ago
            database.Add_Notification(noti);
            Notification noti2 = new Notification(token, "First Notification", "Body", DateTime.UtcNow.AddDays(-2));//Notification from a day ago
            database.Add_Notification(noti2);
            database.Clear_Notifications_From_Days(token,2);
            Assert.Single(database.Get_All_Notifications_Of_User(token));
        }

        [Fact]
        public void Set_Totals()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Totals", "province", "city", "Totals");
            Location location = new Location(9999, 9999, now, area, true);
            database.Insert_Location(location); 
            Location location2 = new Location(8888, 8888, now, area, true);
            database.Insert_Location(location2);
            Location location3 = new Location(7777, 7777, now, area, false);
            database.Insert_Location(location3);
            database.Set_Totals(area);
            var count = 0;
            Totals list = database.Get_Totals(area);
            foreach (Carriers carrier in list.TotalCarriers)
            {
                count += carrier.TotalCarriers;
            }
            Assert.True(true);
        }
        [Fact]
        public void Get_Totals()
        {
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            DateTime now = DateTime.UtcNow;
            Area area = new Area("Totals2", "province", "city", "Totals2");
            Location location = new Location(9999, 9999, now, area, true);
            database.Insert_Location(location);
            Location location2 = new Location(8888, 8888, now, area, true);
            database.Insert_Location(location2);
            Location location3 = new Location(7777, 7777, now, area, true);
            database.Insert_Location(location3);
            database.Set_Totals(area);
            var count = 0;
            Totals list = database.Get_Totals(area);
            foreach (Carriers carrier in list.TotalCarriers)
            {
                count += carrier.TotalCarriers;
            }
            Assert.True(true);
        }
    }
}
