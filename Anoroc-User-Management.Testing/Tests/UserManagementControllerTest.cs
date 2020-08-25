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
    public class UserManagementControllerTest : 
        IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;

        public UserManagementControllerTest(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        // Carrier Status test
        [Fact]
        public async Task Post_UpdateUserContagionStatus_ReturnsOkWithCorrectAccessToken()
        {
            // Arrange
            var token = new Token()
            {
                access_token = "12345abcd",
                Object_To_Server = "Positive"
            };
            
            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/UserManagement/CarrierStatus", byteContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Carrier Status test, wrong access token
        [Fact]
        public async Task Post_UpdateUserContagionStatus_ReturnsUnauthorizedWithIncorrectAccessToken()
        {
            // Arrange
            var token = new Token()
            {
                access_token = "12345abcdWRONG",
                Object_To_Server = "Positive"
            };
            
            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/UserManagement/CarrierStatus", byteContent);
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        // Firebase test
        [Fact]
        public async Task Post_FirebaseToken_ReturnsOkWithCorrectAccessToken()
        {
            // Arrange
            var token = new Token()
            {
                access_token = "12345abcd",
                Object_To_Server = "TOKEN"
            };

            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/UserManagement/FirebaseToken", byteContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Firebase test, wrong access token
        [Fact]
        public async Task Post_FirebaseToken_ReturnsUnauthorizedWithIncorrectAccessToken()
        {
            // Arrange
            var token = new Token()
            {
                access_token = "12345abcdWRONG",
                Object_To_Server = "TOKEN"
            };

            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/UserManagement/FirebaseToken", byteContent);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        // UserLoggedIn test
        [Fact]
        public async Task Post_UserLoggedIn_ReturnsOkWithCorrectAccessToken()
        {
            // Arrange
            var token = new Token()
            {
                access_token = "12345abcd",
                Object_To_Server = JsonConvert.SerializeObject(new User()
                {
                    Email = "tn.selahle@gmail.com"
                })
            };

            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/UserManagement/UserLoggedIn", byteContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // UserLoggedIn test, wrong access token
        [Fact]
        public async Task Post_UserLoggedIn_ReturnsUnauthorizedWithIncorrectAccessToken()
        {
            // Arrange
            var token = new Token()
            {
                access_token = "12345abcdWRONG",
                Object_To_Server = "TOKEN"
            };

            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/UserManagement/UserLoggedIn", byteContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_FirebaseToken_TokenIsSuccessfullyAddedToDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                var _database = scope.ServiceProvider.GetService<IDatabaseEngine>();
                var token = new Token()
                {
                    access_token = "12345abcd",
                    Object_To_Server = "testToken"
                };

                var content = JsonConvert.SerializeObject(token);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Act
                var response = await _client.PostAsync("/UserManagement/FirebaseToken", byteContent);
                
                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("testToken", _database.Get_Firebase_Token(token.access_token));
            }
        }


        [Fact]
        public async Task ValidateUpdateFirebase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                var _database = scope.ServiceProvider.GetService<IDatabaseEngine>();

                User mock = new User("UpdateTest");

                var dbtest = scope.ServiceProvider.GetRequiredService<AnorocDbContext>();

                dbtest.Users.Add(mock);

                dbtest.SaveChanges();

                var result = dbtest.Users.SingleOrDefault(u => u.AccessToken == mock.AccessToken);
                if (result != null)
                {
                    result.Firebase_Token = "updated";
                    dbtest.SaveChanges();
                }

                var userDto = _database.Get_User_ByID("UpdateTest");
                
                Assert.Equal("updated", userDto.Firebase_Token);

            }
        }


    }
}