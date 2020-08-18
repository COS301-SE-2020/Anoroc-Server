using System;
using System.Diagnostics;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Controllers
{
    //TODO: 
    //change return types of endpoints to RESTful return types
    [Route("[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
      
        IUserManagementService UserManagementService;
        public UserManagementController(IUserManagementService userManagementService)
        {
            UserManagementService = userManagementService;
        }

        [HttpPost("CarrierStatus")]
        public IActionResult CarrierStatus([FromBody] Token token_object)
        {
            if (UserManagementService.ValidateUserToken(token_object.access_token))
            {
                UserManagementService.UpdateCarrierStatus(token_object.access_token, token_object.Object_To_Server);
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
            if (UserManagementService.ValidateUserToken(token_object.access_token))
            {
                UserManagementService.InsertFirebaseToken(token_object.access_token, token_object.Object_To_Server);

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

        [HttpPost("UserLoggedIn")]
        public IActionResult UserLoggedIn([FromBody] Token token)
        {
            try
            {
                User user = JsonConvert.DeserializeObject<User>(token.Object_To_Server);
                var userToken = UserManagementService.UserAccessToken(user.Email);
                if (userToken != null)
                {
                    return Ok(userToken);
                }
                else
                {
                    string custom_token = UserManagementService.addNewUser(user);
                    return Ok(custom_token);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("FAILED TO CONVERT USER FROM JSON IN USER CONTROLLER: " + e.Message);
                return BadRequest("Invalid object");
            }
        }
    }
}
