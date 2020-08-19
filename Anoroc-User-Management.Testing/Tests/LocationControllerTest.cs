using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Anoroc_User_Management.Testing.Tests
{
    public class LocationControllerTest:
                        IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>

    {

        private readonly HttpClient _client;
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
            // Arrange
            var token = new Token()
            {
                access_token = "12345abcd",
                error_descriptions = "Integration Testing"
            };

            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/Location/GEOLocation", byteContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
