using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Anoroc_User_Management.Testing.Tests
{
    public class CrossedpathServiceTest : IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        public CrossedpathServiceTest(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public void ProcessLocationTest()
        {
            using var scope = _factory.Services.CreateScope();
            var clusterService = scope.ServiceProvider.GetService<IClusterService>();
            var crossedpathSservice = scope.ServiceProvider.GetService<ICrossedPathsService>();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            var userService = scope.ServiceProvider.GetService<IUserManagementService>();
            
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));

            clusterService.GenerateClusters();

            var initialcount = userService.GetUserIncidents("12345abcd");

            crossedpathSservice.ProcessLocation(new Location(37.4219984444444, -122.084, DateTime.Now, false, new Area("United States", "California", "Mountain View", "A subrub")), "12345abcd");

            var newCount = userService.GetUserIncidents("12345abcd");

            Assert.Equal(initialcount + 1, newCount);
        }

        [Fact]
        public void ProcessWithUnclusteredLocationsTest()
        {
            using var scope = _factory.Services.CreateScope();
            var clusterService = scope.ServiceProvider.GetService<IClusterService>();
            var crossedpathSservice = scope.ServiceProvider.GetService<ICrossedPathsService>();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            var userService = scope.ServiceProvider.GetService<IUserManagementService>();

            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));

            var initialcount = userService.GetUserIncidents("12345abcd");

            crossedpathSservice.ProcessLocation(new Location(37.4219984444444, -122.084, DateTime.Now, false, new Area("United States", "California", "Mountain View", "A subrub")), "12345abcd");

            var newCount = userService.GetUserIncidents("12345abcd");

            Assert.Equal(initialcount + 1, newCount);
        }

        [Fact]
        public void ProcessOldClusterLocationsTest()
        {
            using var scope = _factory.Services.CreateScope();
            var clusterService = scope.ServiceProvider.GetService<IClusterService>();
            var crossedpathSservice = scope.ServiceProvider.GetService<ICrossedPathsService>();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            var userService = scope.ServiceProvider.GetService<IUserManagementService>();

            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.UtcNow.AddHours(-5), true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.UtcNow.AddHours(-5), true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.UtcNow.AddHours(-5), true, new Area("United States", "California", "Mountain View", "A subrub")));
            

            var locations = database.Select_All_Old_Locations();

            var oldCluster = new Cluster();
            locations.ForEach(loc =>
            {
                oldCluster.AddLocation(loc);
            });

            oldCluster.Structurize();
            database.Insert_Cluster(oldCluster);

            var initialcount = userService.GetUserIncidents("12345abcd");

            crossedpathSservice.ProcessLocation(new Location(37.4219984444444, -122.084, DateTime.Now, false, new Area("United States", "California", "Mountain View", "A subrub")), "12345abcd");

            var newCount = userService.GetUserIncidents("12345abcd");

            Assert.Equal(initialcount+1, newCount);
        }

    }
}
