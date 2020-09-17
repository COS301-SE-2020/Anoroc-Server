using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Anoroc_User_Management.Testing.Tests
{
    public class ItineraryServiceTests : IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        User user;
        public ItineraryServiceTests(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            user = new User();
        }

        [Fact]
        public void ProcessItineraryTest()
        {
            using var scope = _factory.Services.CreateScope();
            var itineraryService = scope.ServiceProvider.GetRequiredService<IItineraryService>();
            var userService = scope.ServiceProvider.GetService<IUserManagementService>();
            var clusterService = scope.ServiceProvider.GetService<IClusterService>();
            user.Email = "test@anoroc.com";
            user.FirstName = "anoroc";
            user.UserSurname = "asd";

            if (userService.UserAccessToken(user.Email) == null)
                user.AccessToken = userService.addNewUser(user);
            else
                user.AccessToken = userService.UserAccessToken(user.Email);

            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));

            clusterService.GenerateClusters();

            List<Cluster> listCluster = clusterService.GetClustersPins(new Area());

            var firstCluster = listCluster.FirstOrDefault();
            
            var itinerary = new Itinerary(firstCluster.Coordinates.ToList());

            var result = itineraryService.ProcessItinerary(itinerary, user.AccessToken);

            Assert.Equal(4, result.TotalItineraryRisk);
        }

        [Fact]
        public void GetItinerariesTest()
        {
            using var scope = _factory.Services.CreateScope();
            var itineraryService = scope.ServiceProvider.GetRequiredService<IItineraryService>();
            var userService = scope.ServiceProvider.GetService<IUserManagementService>();
            var clusterService = scope.ServiceProvider.GetService<IClusterService>();
            user.Email = "test@anoroc.com";
            user.FirstName = "anoroc";
            user.UserSurname = "asd";

            if (userService.UserAccessToken(user.Email) == null)
                user.AccessToken = userService.addNewUser(user);
            else
                user.AccessToken = userService.UserAccessToken(user.Email);

            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));

            clusterService.GenerateClusters();

            List<Cluster> listCluster = clusterService.GetClustersPins(new Area());

            var firstCluster = listCluster.FirstOrDefault();

            var itinerary = new Itinerary(firstCluster.Coordinates.ToList());

            var result = itineraryService.ProcessItinerary(itinerary, user.AccessToken);

            var useritinerary = itineraryService.GetItineraries(10, user.AccessToken);

            var count = 0;
            useritinerary.ForEach(itin =>
            {
                if (itin.ID == result.ID)
                {
                    count = itin.ID;
                }
            });

            Assert.Equal(result.ID, count);
        }

    }
}
