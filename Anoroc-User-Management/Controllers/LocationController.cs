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
           /* area should =
            * {
                "Area":
                {
                    "HandShake": "Hello"
                }
              }
           */
            return Cluster_Service.GetClustersPins(area);
        }

        [HttpPost("Clustering/MLNet")]
        public string MLNet([FromBody]Point point)
        {
            MLNetClustering mLNetClustering = new MLNetClustering();
            return mLNetClustering.Check_Close_To_Cluster(point);
            //return mLNetClustering.Clustering();

           /* KMeans.Example();
            return KMeans.GetCnetroids();*/
        }
      
        [HttpPost("Clusters/Simplified")]
        public string Clusters_Cluster([FromBody] Area area)
        {
            /* area should =
            * {
                "Area":
                {
                    "HandShake": "Hello"
                }
              }
           */

            return Cluster_Service.GetClusters(area);
        }


       /* [HttpPost("GEOLocation")]
        public string GEOLocationAsync()
        {
            return Cluster_Service.ReadJson();
        }*/

        //Function for Demo purposes, get the lcoation from the database to show funcitonality
        [HttpGet("toString")]
        public string toString()
        {
            return "stuff";
        }
    }
}
