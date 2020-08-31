using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        IUserManagementService UserManagementService;
        IDatabaseEngine DatabaseEngine;
        public ClusterController(IClusterService clusterService, IUserManagementService userService, IClusterManagementService clusterManagementService, IDatabaseEngine databaseEngine)
        {
            ClusterManagementService = clusterManagementService;
            Cluster_Service = clusterService;
            UserManagementService = userService;
            DatabaseEngine = databaseEngine;
            //databaseEngine.populate();
        }

        [HttpPost("ManageClusters")]
        public ObjectResult ManageClusters([FromBody] Token token_object)
        {
            if(UserManagementService.ValidateUserToken(token_object.access_token))
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
            if (UserManagementService.ValidateUserToken(token_object.access_token))
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

        [HttpPost("OldClustersSimplified")]
        public IActionResult OldClustersSimplified([FromBody] Token token_object)
        {
            try
            {
                var days = Convert.ToInt32(token_object.Object_To_Server);
                if (days > 0 && days <= 8)
                    return Ok(JsonConvert.SerializeObject(Cluster_Service.GetOldClustersDaysAgo(days)));
                else
                    return BadRequest("Out of range days.");
            }
            catch (FormatException e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest("Expected days to be an integer.");
            }
        }

        [HttpPost("Test")]
        public ObjectResult Cluster_Test([FromBody] Token token_object)
        {
            Location location = JsonConvert.DeserializeObject<Location>(token_object.Object_To_Server);
            return Ok(JsonConvert.SerializeObject(Cluster_Service.ClustersInRange(location, -1)));
        }



        [HttpPost("Simplified")]
        public ObjectResult Clusters_ClusterWrapper([FromBody] Token token_object)
        {
            Area area2 = new Area();
            return Ok(new JavaScriptSerializer().Serialize(Cluster_Service.GetClusters(area2)));
        }
    }
}
