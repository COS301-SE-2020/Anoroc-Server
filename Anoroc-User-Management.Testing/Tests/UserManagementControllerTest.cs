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
        User user;
        public UserManagementControllerTest(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            user = new User();
        }

        // Carrier Status test
        [Fact]
        public async Task Post_UpdateUserContagionStatus_ReturnsOkWithCorrectAccessToken()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetService<IUserManagementService>();

            user.Email = "test@anoroc.com";
            user.FirstName = "anoroc";
            user.UserSurname = "asd";

            if (userService.UserAccessToken(user.Email) == null)
                user.AccessToken = userService.addNewUser(user);
            else
                user.AccessToken = userService.UserAccessToken(user.Email);

            // Arrange
            var token = new Token()
            {
                access_token = user.AccessToken,
                Object_To_Server = "TOKEN"
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
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetService<IUserManagementService>();

            user.Email = "test@anoroc.com";
            user.FirstName = "anoroc";
            user.UserSurname = "asd";

            if (userService.UserAccessToken(user.Email) == null)
                user.AccessToken = userService.addNewUser(user);
            else
                user.AccessToken = userService.UserAccessToken(user.Email);

            // Arrange
            var token = new Token()
            {
                access_token = user.AccessToken,
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
            using var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<IDatabaseEngine>();
            
            var token1 = Guid.NewGuid().ToString();
            
            var user = new User(token1, "notificationTest2");
            database.Insert_User(user);
            // Arrange
            var token = new Token()
            {
                access_token = token1,
                Object_To_Server = JsonConvert.SerializeObject(new User()
                {
                    Email = "tn.selahle@gmail.com"
                })
            };

            var content = JsonConvert.SerializeObject(token);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var userservice = scope.ServiceProvider.GetService<IUserManagementService>();
            byteContent.Headers.Add("X-XamarinKey", userservice.getXamarinKeyForTest());
            // Act
            var response = await _client.PostAsync("/UserManagement/UserLoggedIn", byteContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // UserLoggedIn test, wrong access token
        [Fact]
        public async Task Post_UserLoggedIn_ReturnsUnauthorizedWithIncorrectAccessToken()
        {
            using var scope = _factory.Services.CreateScope();
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
            var userservice = scope.ServiceProvider.GetService<IUserManagementService>();
            byteContent.Headers.Add("X-XamarinKey", userservice.getXamarinKeyForTest());
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
                var userService = scope.ServiceProvider.GetService<IUserManagementService>();
                var _database = scope.ServiceProvider.GetService<IDatabaseEngine>();

                user.Email = "test@anoroc.com";
                user.FirstName = "anoroc";
                user.UserSurname = "asd";

                if (userService.UserAccessToken(user.Email) == null)
                    user.AccessToken = userService.addNewUser(user);
                else
                    user.AccessToken = userService.UserAccessToken(user.Email);

                // Arrange
                var token = new Token()
                {
                    access_token = user.AccessToken,
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
                Assert.Equal("TOKEN", _database.Get_Firebase_Token(user.AccessToken));
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