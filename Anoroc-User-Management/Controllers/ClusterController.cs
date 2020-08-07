using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClusterController : ControllerBase
    {
        IClusterService Cluster_Service;
        IClusterManagementService ClusterManagementService;
        IDatabaseEngine DatabaseEngine;

        public ClusterController(IClusterService clusterService, IDatabaseEngine dbObject, IClusterManagementService clusterManagementService)
        {
            ClusterManagementService = clusterManagementService;
            Cluster_Service = clusterService;
            DatabaseEngine = dbObject;
        }

        [HttpPost("ManageClusters")]
        public ObjectResult ManageClusters([FromBody] Token token_object)
        {
            if(DatabaseEngine.Validate_Access_Token(token_object.access_token))
            {
                ClusterManagementService.BeginManagment();
                return Ok("Started Management.");
            }
            else
            {
                return Unauthorized("Invalid Token.");
            }
        }

        [HttpPost("Pins")]
        public ObjectResult Cluster_Pins([FromBody] Token token_object)
        {
            //Area area = token_object.Object_To_Server;
            //return Cluster_Service.GetClustersPins(new Area());
            if (DatabaseEngine.Validate_Access_Token(token_object.access_token))
            {
                //Area area = JsonConvert.DeserializeObject<Area>(token_object.Object_To_Server);
                return Ok(JsonConvert.SerializeObject(Cluster_Service.GetClustersPins(new Area())));
            }
            else
            {
                JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
                return Unauthorized(JsonConvert.SerializeObject(Unauthorized(jsonConverter.Serialize("Invalid Token"))));

                // create http response set response to 401 unauthorize, return json converter.serlizeobject(http response message variable)
            }
        }

        [HttpPost("Test")]
        public ObjectResult Cluster_Test([FromBody] Token token_object)
        {
            IItineraryService itineraryService = new ItineraryService(Cluster_Service, DatabaseEngine);
            var temp = DatabaseEngine.Select_List_Locations();
            var it = new Itinerary();
            it.Locations = temp;
            
            return Ok(JsonConvert.SerializeObject(itineraryService.ProcessItinerary(it, token_object.access_token)));
        }



        [HttpPost("Simplified")]
        public ObjectResult Clusters_ClusterWrapper([FromBody] Token token_object)
        {
            if (DatabaseEngine.Validate_Access_Token(token_object.access_token))
            {
                Area area2 = new Area();
                return Ok(new JavaScriptSerializer().Serialize(Cluster_Service.GetClusters(area2)));
            }
            else
            {
                JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
                return Unauthorized(jsonConverter.Serialize("Unauthroized accessed"));
                // create http response set response to 401 unauthorize, return json converter.serlizeobject(http response message variable)
            }
        }
    }
}
