using System;
using System.Threading.Tasks;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Anoroc_User_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        public ItineraryController()
        {
        }

        [HttpPost("Itinerary")]
        public IActionResult Itinerary([FromBody] Token token_object)
        {            
            return Ok(token_object.Object_To_Server);
        }
    }
}
