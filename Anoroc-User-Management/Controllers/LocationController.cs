using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Anoroc_User_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        [HttpPost]
        public string Post()
        {
            return "this is a post";
        }

        //Looking for Latitude, Longitude, Altitude
        [HttpPost("GEOlocation")]
        public async Task<string> GEOlocationAsync()
        {
            var form = await HttpContext.Request.ReadFormAsync();
           
            return form["Latitude"] + ", " + form["Longitude"] + ", " + form["Altitude"];
        }


        //Function for Demo purposes, get the lcoation from the database to show funcitonality
        [HttpGet("toString")]
        public string toString()
        {
            
            return "stuff";
        }
    }
}
