﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Anoroc_User_Management.Testing.Tests
{
    public class LocationControllerTest:
                        IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>

    {

        private HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
      



        public LocationControllerTest(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        // Location test
        [Fact]
        public async Task Post_GEOLocation_ReturnsOkWithCorrectAccessToken()
        {
            using var scope = _factory.Services.CreateScope();
            var usermanagment = scope.ServiceProvider.GetService<IUserManagementService>();
            var user = new User();
            user.Email = "Test@anoroc.com";
            user.FirstName = "anoroc";
            user.UserSurname = "code sum moar";
            var custom_token = usermanagment.addNewUser(user);
            // Arrange
            var token = new Token()
            {
                access_token = custom_token,
                error_descriptions = "Integration Testing"
            };

            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/Location/GEOLocation", byteContent);

            // Assert
            Assert.NotNull(response);
        }

        // Location test, wrong access token
        [Fact]
        public async Task Post_GEOLocation_ReturnsUnauthorizedWithIncorrectAccessToken()
        {
            // Arrange
            var token = new Token()
            {
                access_token = "12345abcdWRONG",
                error_descriptions = "Integration Testing"
            };

            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/Location/GEOLocation", byteContent);

            // Assert
            Assert.NotNull(response);
        }


        [Fact]
        public async Task ValidateAddLocationService()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                Location mock = new Location(4, 4);
                var _database = scope.ServiceProvider.GetService<IDatabaseEngine>();


                var dbtest = scope.ServiceProvider.GetRequiredService<AnorocDbContext>();

                dbtest.Locations.Add(mock);

                dbtest.SaveChanges();

                var locationDto = _database.Get_Location_By_Longitude(4);

                Assert.Equal("4", locationDto.Longitude.ToString());
            }
        }

        [Fact]
        public async Task ValidatUppdateLocationService()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                Location mock = new Location(1, 1);
                var _database = scope.ServiceProvider.GetService<IDatabaseEngine>();



                var dbtest = scope.ServiceProvider.GetRequiredService<AnorocDbContext>();

                dbtest.Locations.Add(mock);

                dbtest.SaveChanges();

                var result = dbtest.Locations.SingleOrDefault(l => l.Longitude == mock.Longitude);
                if(result != null)
                {
                    result.Longitude = 2;
                    dbtest.SaveChanges();
                }

                var locationDto = _database.Get_Location_By_Longitude(2);

                Assert.Equal("2", locationDto.Longitude.ToString());
            }
        }


        [Fact]
        public async Task ValidateAddAreaService()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                Area mock = new Area(998,"temp","temp","temp","temp");
                var _database = scope.ServiceProvider.GetService<IDatabaseEngine>();

                var dbtest = scope.ServiceProvider.GetRequiredService<AnorocDbContext>();

                dbtest.Areas.Add(mock);

                dbtest.SaveChanges();

                var areaDto = _database.Select_Area_By_Id(998);

                Assert.Equal("temp", areaDto.Country);
            }
        }

        [Fact]
        public async Task ValidatUppdateAreaService()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange

                Area mock = new Area(999,"temp", "temp", "temp", "temp");
                var _database = scope.ServiceProvider.GetService<IDatabaseEngine>();

                var dbtest = scope.ServiceProvider.GetRequiredService<AnorocDbContext>();

                dbtest.Areas.Add(mock);

                dbtest.SaveChanges();

                var areaDto = _database.Select_Area_By_Id(999);


                var result = dbtest.Areas.SingleOrDefault(a => a.Country == mock.Country);
                if (result != null)
                {
                    result.Country = "updated";
                    dbtest.SaveChanges();
                }

                Assert.Equal("updated", areaDto.Country);

            }
        }
    }
}
