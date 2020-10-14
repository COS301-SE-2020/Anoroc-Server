using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        IItineraryService ItineraryService;
        IUserManagementService UserManagementService;
        public ItineraryController(IItineraryService itineraryService, IUserManagementService userManagement)
        {
            UserManagementService = userManagement;
            ItineraryService = itineraryService;
        }

        [HttpPost("ProcessItinerary")]
        public IActionResult Itinerary([FromBody] Token token_object)
        {
            if(UserManagementService.ValidateUserToken(token_object.access_token))
            {
                Itinerary userItinerary = JsonConvert.DeserializeObject<Itinerary>(token_object.Object_To_Server);
                ItineraryRiskWrapper itineraryRiskWrapper = ItineraryService.ProcessItinerary(userItinerary, token_object.access_token);
                return Ok(JsonConvert.SerializeObject(itineraryRiskWrapper));
            }
            else
            {
                return Unauthorized("Invalid Token");
            }
        }

        [HttpPost("DeleteItinerary")]
        public IActionResult DeleteItinerary([FromBody] Token token)
        {
            if (UserManagementService.ValidateUserToken(token.access_token))
            {
                try
                {
                    int ID = Convert.ToInt32(token.Object_To_Server);
                    ItineraryService.DeleteUserItinerary(token.access_token,ID);
                    return Ok("Deleted itinerary");
                }
                catch (FormatException e)
                {
                    return BadRequest(e);
                }
            }
            else
                return Unauthorized("Invalid token.");
        }

        [HttpPost("GetUserItinerary")]
        public IActionResult GetItinerarys([FromBody] Token token)
        {
            if(UserManagementService.ValidateUserToken(token.access_token))
            {
                try
                {
                    int paginiation = Convert.ToInt32(token.Object_To_Server);
                    return Ok(JsonConvert.SerializeObject(ItineraryService.GetItineraries(paginiation, token.access_token)));
                }
                catch(FormatException e)
                {
                    Debug.WriteLine(e.Message);
                    return BadRequest("Expected an int in Object_To_Server");
                }
            }
            else
            {
                return Unauthorized("Invalid Token");
            }
        }


        [HttpPost("ProcessItineraryWebApp")]
        public IActionResult ItineraryWebApp([FromBody] Token token_object)
        {
            if (UserManagementService.ValidateUserToken(token_object.access_token))
            {
                Itinerary userItinerary = JsonConvert.DeserializeObject<Itinerary>(token_object.Object_To_Server);
                ItineraryRiskWrapper itineraryRiskWrapper = ItineraryService.ProcessItinerary(userItinerary, token_object.access_token);
                return Ok(JsonConvert.SerializeObject(itineraryRiskWrapper.TotalItineraryRisk));
            }
            else
            {
                return Unauthorized("Invalid Token");
            }
        }
    }
}
