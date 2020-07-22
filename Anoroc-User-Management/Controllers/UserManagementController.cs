using System;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;

namespace Anoroc_User_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        IDatabaseEngine DatabaseService;
        public UserManagementController(IDatabaseEngine database)
        {
            DatabaseService = database;
        }

        [HttpPost("CarrierStatus")]
        public IActionResult CarrierStatus([FromBody] Token token_object)
        {
            if (DatabaseService.validateAccessToken(token_object.access_token))
            {
                DatabaseService.UpdateCarrierStatus(token_object.access_token, token_object.Object_To_Server);
                var returnString = token_object.Object_To_Server + "";
                return Ok(returnString);
            }
            else
            {
                JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
                return Unauthorized(jsonConverter.Serialize("Unauthroized accessed"));
                // create http response set response to 401 unauthorize, return json converter.serlizeobject(http response message variable)
            }

            


        }

        [HttpPost("FirebaseToken")]
        public IActionResult FirebaseToken([FromBody] Token token_object)
        {
            if (DatabaseService.validateAccessToken(token_object.access_token))
            {
                DatabaseService.InsertFirebaseToken(token_object.access_token, token_object.Object_To_Server);

                var returnString = token_object.Object_To_Server + "";
                return Ok(returnString);

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
