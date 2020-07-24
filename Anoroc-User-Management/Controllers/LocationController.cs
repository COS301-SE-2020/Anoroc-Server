using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Nancy.Json;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net.Http;

namespace Anoroc_User_Management.Controllers
{
    /// <summary>
    /// API Endpoint for all location data from client
    /// </summary>



    //[Produces("application/json")]
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        IClusterService Cluster_Service;
        private readonly IMobileMessagingClient _mobileMessagingClient;
        private readonly ICrossedPathsService _crossedPathsService;

        public LocationController(IClusterService clusterService, IMobileMessagingClient mobileMessagingClient, ICrossedPathsService crossedPathsService)
        {
            Cluster_Service = clusterService;
            _mobileMessagingClient = mobileMessagingClient;
            _crossedPathsService = crossedPathsService;
        }

       
        [HttpPost("Clusters/Pins")]
        public string Cluster_Pins([FromBody] Token token_object)
        {
            //Area area = token_object.Object_To_Server;
            //return Cluster_Service.GetClustersPins(new Area());
            return "";
        }

        
      
        [HttpPost("Clusters/Simplified")]
        public string Clusters_ClusterWrapper([FromBody] Token token_object)
        {
            Area area2 = new Area();
            return new JavaScriptSerializer().Serialize(Cluster_Service.GetClusters(area2));
        }


        [HttpPost("GEOLocation")]
        public string GEOLocationAsync([FromBody] Token token_object)
        {
            if(token_object.access_token == "thisisatoken")
            {
                if (token_object.error_descriptions != "Integration Testing")
                {
                    var data = token_object.Object_To_Server;
                    Debug.WriteLine(JsonConvert.SerializeObject(token_object));

                    Location location = JsonConvert.DeserializeObject<Location>(token_object.Object_To_Server);
                    location.UserAccessToken = token_object.access_token;

                    if (location.Carrier_Data_Point)
                    {
                        Console.WriteLine("Processing: " + location);
                        _crossedPathsService.ProcessLocation(location);
                    }
                    else
                    {
                        Console.WriteLine("Non Carrier: " + location);
                    }
                    return "Hello";
                }
                else
                {
                    HttpResponseMessage responseMessage = new HttpResponseMessage();
                    responseMessage.StatusCode = System.Net.HttpStatusCode.OK;
                    return JsonConvert.SerializeObject(responseMessage);
                }
                    
            }
            else
                return "No";
        }

        //Function for Demo purposes, get the lcoation from the database to show funcitonality
        [HttpGet("toString")]
        public string toString()
        {
            return "stuff";
        }

        [HttpPost("update")]
        public string UpdateLocation([FromBody] SimpleLocation simpleLocation)
        {
            var location = new Location(simpleLocation);
            //_mobileMessagingClient.SendNotification(location);
            return System.Text.Json.JsonSerializer.Serialize(location);
        }

        [HttpPost("test")]
        public string Test()
        {
            //_mobileMessagingClient.SendNotification(new Location(new GeoCoordinate(5.5, 5.5)));
            return "Notification sent";
        }
    }
}
