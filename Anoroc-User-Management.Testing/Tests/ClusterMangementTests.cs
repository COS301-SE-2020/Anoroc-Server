using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
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
    public class ClusterMangementTests : IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        public ClusterMangementTests(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public void BeginManagmentTest()
        {
            using var scope = _factory.Services.CreateScope();
            var clusterService = scope.ServiceProvider.GetService<IClusterService>();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));
            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, DateTime.Now, true, new Area("United States", "California", "Mountain View", "A subrub")));

            var clusterManagement = scope.ServiceProvider.GetService<IClusterManagementService>();
            clusterManagement.BeginManagment();

            var areas = database.Select_Unique_Areas();
            int count = 0;
            List<ClusterWrapper> clusters = null;
            areas.ForEach(area =>
            {
                clusters = clusterService.GetClusters(area);
            });

            Assert.NotNull(clusters);
            count = clusters.Count;
            Assert.NotEqual(0, count);
        }
    }
}
