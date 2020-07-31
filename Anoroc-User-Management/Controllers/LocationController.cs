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
        IDatabaseEngine DatabaseEngine;            
        private readonly ICrossedPathsService _crossedPathsService;

        public LocationController(IClusterService clusterService, IMobileMessagingClient mobileMessagingClient, ICrossedPathsService crossedPathsService, IDatabaseEngine dbObject)
        {
            Cluster_Service = clusterService;
            _mobileMessagingClient = mobileMessagingClient;
            _crossedPathsService = crossedPathsService;
            DatabaseEngine = dbObject;
        }


        [HttpPost("GEOLocation")]
        public ObjectResult GEOLocationAsync([FromBody] Token token_object)
        {            
            if (DatabaseEngine.Validate_Access_Token(token_object.access_token))
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
                    return Ok("Processing: " + JsonConvert.SerializeObject(location));
                }
                else
                {
                    HttpResponseMessage responseMessage = new HttpResponseMessage();
                    responseMessage.StatusCode = System.Net.HttpStatusCode.OK;
                    return Ok(JsonConvert.SerializeObject(responseMessage));
                }
                    
            }
            else
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage();
                responseMessage.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                return Unauthorized((JsonConvert.SerializeObject(responseMessage)));

                /*JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
                return JsonConvert.SerializeObject(Unauthorized(jsonConverter.Serialize("Unauthroized accessed")));*/
                // create http response set response to 401 unauthorize, return json converter.serlizeobject(http response message variable)
            }                                            
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
