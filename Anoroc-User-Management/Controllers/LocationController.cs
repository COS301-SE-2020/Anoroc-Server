using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;

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

        public LocationController(IClusterService clusterService)
        {
            Cluster_Service = clusterService;
        }

       
        [HttpPost("Clusters/Pins")]
        public string Cluster_Pins([FromBody] Area area)
        {
            return Cluster_Service.GetClustersPins(area);
        }

        
      
        [HttpPost("Clusters/Simplified")]
        public string Clusters_ClusterWrapper([FromBody] Area area)
        {
            Area area2 = new Area();
            return new JavaScriptSerializer().Serialize(Cluster_Service.GetClusters(area2));
        }


        [HttpPost("GEOLocation")]
        public string GEOLocationAsync()
        {
            return "Hello";
        }

        //Function for Demo purposes, get the lcoation from the database to show funcitonality
        [HttpGet("toString")]
        public string toString()
        {
            return "stuff";
        }
    }
}
