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
    public class UsermangementServiceTests : IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        User user;
        public UsermangementServiceTests(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
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
        public void DeleteUserTest()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetService<IUserManagementService>();

            user.Email = "test@anoroc.com";
            user.FirstName = "anoroc";
            user.UserSurname = "asd";

            if (userService.UserAccessToken(user.Email) == null)
                user.AccessToken = userService.addNewUser(user);
            else
                user.AccessToken = userService.UserAccessToken(user.Email);

            var clusterService = scope.ServiceProvider.GetService<IClusterService>();

            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, true, DateTime.UtcNow.AddHours(-5), new Area("United States", "California", "Mountain View", "A subrub"), user.AccessToken));
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            userService.CompletelyDeleteUser(user.AccessToken);

            Assert.Null(database.Get_Single_User(user.AccessToken));
            Assert.Empty(database.Select_Locations_By_Access_Token(user.AccessToken));
        }

        [Fact]
        public void SendData()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetService<IUserManagementService>();

            user.Email = "test@anoroc.com";
            user.FirstName = "anoroc";
            user.UserSurname = "asd";

            if (userService.UserAccessToken(user.Email) == null)
                user.AccessToken = userService.addNewUser(user);
            else
                user.AccessToken = userService.UserAccessToken(user.Email);

            var clusterService = scope.ServiceProvider.GetService<IClusterService>();

            clusterService.AddLocationToCluster(new Location(37.4219984444444, -122.084, true, DateTime.UtcNow.AddHours(-5), new Area("United States", "California", "Mountain View", "A subrub"), user.AccessToken));

            Assert.NotNull(userService.ReturnUserData(user.AccessToken));
        }
    }
}
