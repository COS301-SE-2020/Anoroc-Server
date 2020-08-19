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
    public class RegisterControllerTest:
                IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>

    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public RegisterControllerTest(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        // Register test
        [Fact]
        public async Task Post_Register_ReturnsOkWithCorrectAccessToken()
        {
            // Arrange
            var register = new Register()
            {
                Token = "yf8s7auiH&*DHuids89hsdua"
            };

            var content = JsonConvert.SerializeObject(register);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/Register/Post", byteContent);

            // Assert
            Assert.Equal(register.Token, response.ToString());
        }
    }
}
