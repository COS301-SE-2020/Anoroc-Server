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
    public class ClusterControllerTest :
        IClassFixture<CustomWebApplicationFactory<Anoroc_User_Management.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Anoroc_User_Management.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;      

        public ClusterControllerTest(CustomWebApplicationFactory<Anoroc_User_Management.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        // ManageClusters test
        //[Fact]
        //public async Task Post_Itinerary_ReturnsOkWithCorrectAccessToken()
        //{
            //Itinerary itinerary = new Itinerary();
            //// Arrange
            //var token = new Token()
            //{
            //    access_token = "12345abcd",
            //    Object_To_Server = JsonConvert.SerializeObject(itinerary)
            //};

            //var content = JsonConvert.SerializeObject(token);
            //var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            //var byteContent = new ByteArrayContent(buffer);
            //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //// Act
            //var response = await _client.PostAsync("/Cluster/ManageClusters", byteContent);

            //// Assert
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}
    }
}
