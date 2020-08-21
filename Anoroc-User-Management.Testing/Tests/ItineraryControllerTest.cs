using System;
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
            database.Insert_Itinerary_Risk(itineraryRisk);
            var resultFromGetItineraryRisksByToken = database.Get_Itinerary_Risks_By_Token("12345abcd");
            var resultFromGetAllItineraryRisks = database.Get_All_Itinerary_Risks();
            
            // Assert
            Assert.NotEmpty(resultFromGetAllItineraryRisks);
            Assert.NotEmpty(resultFromGetItineraryRisksByToken);
        }
        
    }
}
