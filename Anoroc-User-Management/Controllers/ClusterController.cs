using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Controllers
{
     [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]")]
    [ApiController]
    public class ClusterController : ControllerBase
    {
        IClusterService Cluster_Service;
        IClusterManagementService ClusterManagementService;
        IUserManagementService UserManagementService;
        IDatabaseEngine DatabaseEngine;
        private readonly string Azure_Key;
        public ClusterController(IClusterService clusterService, IUserManagementService userService, IClusterManagementService clusterManagementService, IDatabaseEngine databaseEngine, IConfiguration configurationManager)
        {
            ClusterManagementService = clusterManagementService;
            Cluster_Service = clusterService;
            UserManagementService = userService;
            DatabaseEngine = databaseEngine;
            //databaseEngine.Integrated_Populate();
            //databaseEngine.Set_Totals(new Area("", "", "", "Brooklyn"));
            //var list = databaseEngine.Get_Totals(new Area("", "", "", "Brooklyn"));
            Azure_Key = configurationManager["AzureToken"];
        }

        [HttpPost("ManageClusters")]
        public ObjectResult ManageClusters([FromHeader] string key)
        {
            if (Request.Headers.ContainsKey("X-AzureKey"))
            {
                if (Request.Headers["X-AzureKey"].Equals(Azure_Key))
                {
                    ClusterManagementService.BeginManagment();
                    return Ok("Started Management.");
                }
                else
                {
                    return Unauthorized("Invalid Token.");
                }
            }
            else
                return BadRequest("Bad Request");
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
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

        [EnableCors(origins: "*", headers: "*", methods: "*")]
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

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost("OldClusterPins")]
        public IActionResult OldClusterPins([FromBody] Token token)
        {
            if (UserManagementService.ValidateUserToken(token.access_token))
            {
                try
                {
                    int days = Convert.ToInt32(token.Object_To_Server);
                    return Ok(JsonConvert.SerializeObject(Cluster_Service.GetOldClustersPinsDaysAgo(days)));
                }
                catch(Exception)
                {
                    return BadRequest("Bad request");
                }
            }
            else
                return Unauthorized("Invalid token");
        }

        [HttpPost("Test")]
        public ObjectResult Cluster_Test([FromBody] Token token_object)
        {
            Location location = JsonConvert.DeserializeObject<Location>(token_object.Object_To_Server);
            return Ok(JsonConvert.SerializeObject(Cluster_Service.ClustersInRange(location, -1)));
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost("Simplified")]
        public ObjectResult Clusters_ClusterWrapper([FromBody] Token token_object)
        {
            Area area2 = new Area();
            var data = Cluster_Service.GetClusters(area2);
            return Ok(new JavaScriptSerializer().Serialize(data));
        }
    }
}
