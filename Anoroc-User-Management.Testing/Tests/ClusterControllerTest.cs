using System;
using System.Linq;
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
    public class ClusterControllerTest :
        IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;      

        public ClusterControllerTest(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }


        [Fact]
        public async Task ValidateAddClusterService()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                Location locationMock = new Location(1, 1);
                Cluster mock = new Cluster(locationMock, 999);
                var _database = scope.ServiceProvider.GetService<IDatabaseEngine>();

                var dbtest = scope.ServiceProvider.GetRequiredService<AnorocDbContext>();

                dbtest.Clusters.Add(mock);

                dbtest.SaveChanges();

                var ClusterDto = _database.Get_Cluster_ByID(999);

                Assert.Equal("999", ClusterDto.Cluster_Id.ToString());
            }
        }

        [Fact]
        public async Task ValidateUpdateClusterService()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                Location locationMock = new Location(1, 1);
                Cluster mock = new Cluster(locationMock, 998);
                var _database = scope.ServiceProvider.GetService<IDatabaseEngine>();

                var dbtest = scope.ServiceProvider.GetRequiredService<AnorocDbContext>();
                dbtest.Clusters.Add(mock);                               

                dbtest.SaveChanges();


                var ClusterDto = _database.Get_Cluster_ByID(998);

                var result = dbtest.Clusters.SingleOrDefault(a => a.Cluster_Id == mock.Cluster_Id);
                if (result != null)
                {
                    result.Center_LocationLocation_ID = 998;
                    dbtest.SaveChanges();
                }

                Assert.Equal("998", ClusterDto.Center_LocationLocation_ID.ToString());
            }
        }


        // ManageClusters test
        //[Fact]
        //public async Task Post_Itinerary_ReturnsOkWithCorrectAccessToken()
        //{
        //Itinerary itinerary = new Itinerary();
        //// Arrange
        //var token = new Token()
        //{
        //    access_token = "12345abcd",
        //    Object_To_Server = JsonConvert.SerializeObject(itinerary)
        //};

        //var content = JsonConvert.SerializeObject(token);
        //var buffer = System.Text.Encoding.UTF8.GetBytes(content);
        //var byteContent = new ByteArrayContent(buffer);
        //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //// Act
        //var response = await _client.PostAsync("/Cluster/ManageClusters", byteContent);

        //// Assert
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}
    }
}
