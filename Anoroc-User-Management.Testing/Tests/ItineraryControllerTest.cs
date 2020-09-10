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
    public class ItineraryControllerTest :
                        IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public ItineraryControllerTest(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public void DatabaseServiceSuccessfullyInsertsItineraryToDatabse()
        {
            using var scope = _factory.Services.CreateScope();
            // Arrange
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            var itineraryRisk = new ItineraryRisk(DateTime.Now, "12345abcd");
            
            // Act
            int itineraryID = database.Insert_Itinerary_Risk(itineraryRisk);
            var resultFromGetItineraryRisksByToken = database.Get_Itinerary_Risks_By_Token("12345abcd");
            var resultFromGetAllItineraryRisks = database.Get_All_Itinerary_Risks();
            
            // Assert
            Assert.NotEmpty(resultFromGetAllItineraryRisks);
            Assert.NotEmpty(resultFromGetItineraryRisksByToken);
        }

        [Fact]
        public void ProcessItineraryTest()
        {
            using var scope = _factory.Services.CreateScope();
            var itineraryService = scope.ServiceProvider.GetService<IItineraryService>();

            var locationList = new List<Location>();

            locationList.Add(new Location(37.4219983333333, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View")));
            locationList.Add(new Location(37.4219983333333, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View")));
            locationList.Add(new Location(37.4219983333333, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View")));

            var userItinerary = new Itinerary(locationList);

            var itineraryRisk = itineraryService.ProcessItinerary(userItinerary, "12345abcd");

            Assert.NotNull(itineraryRisk);
            Assert.Equal(0, itineraryRisk.TotalItineraryRisk);
        }

        [Fact]
        public void ProcessItineraryAtRiskTest()
        {
            using var scope = _factory.Services.CreateScope();
            var itineraryService = scope.ServiceProvider.GetService<IItineraryService>();

            var locationList = new List<Location>();

            locationList.Add(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View")));
            locationList.Add(new Location(37.4219985555555, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View")));
            locationList.Add(new Location(37.4219986666666, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View")));

            var userItinerary = new Itinerary(locationList);

            var clusterService = scope.ServiceProvider.GetService<IClusterService>();
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View")));
            clusterService.AddLocationToCluster(new Location(37.4219985555555, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View")));
            clusterService.AddLocationToCluster(new Location(37.4219986666666, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View")));


            var clusterManagementService = scope.ServiceProvider.GetService<IClusterManagementService>();
            clusterManagementService.BeginManagment();

            var itineraryRiskAtRisk = itineraryService.ProcessItinerary(userItinerary, "12345abcd");

            Assert.Equal(0, itineraryRiskAtRisk.TotalItineraryRisk);
        }
    }
}
