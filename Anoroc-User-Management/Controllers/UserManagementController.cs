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
                return Unauthorized("Unauthroized accessed");
            }

        }

        [HttpPost("UploadProfilePhoto")]
        public IActionResult UploadProfilePhoto([FromBody] Token token)
        {
            if (UserManagementService.ValidateUserToken(token.access_token))
            {
                try
                {
                    if (token.Profile_image != null)
                    {
                        byte[] image = token.Profile_image;
                        UserManagementService.SaveProfileImage(token.access_token, image);
                        return Ok("Ok");
                    }
                    else
                        return BadRequest("Invalid request");
                }
                catch(Exception)
                {
                    return BadRequest("Invalid request");
                }
            }
            else
                return Unauthorized("Invalid request");
        }

        [HttpPost("GetUserProfilePicture")]
        public IActionResult GetUserProfilePicture([FromBody] Token token)
        {
            if(UserManagementService.ValidateUserToken(token.access_token))
            {
                return Ok(UserManagementService.GetProfileImage(token.access_token));
            }
            else
                return Unauthorized("Invalid request");
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
                return Unauthorized("Unauthroized accessed");
            }
        }

        [HttpPost("UserIncidents")]
        public IActionResult UserIncidents([FromBody] Token token)
        {
            try
            {
                if(UserManagementService.ValidateUserToken(token.access_token))
                {
                    return Ok(UserManagementService.GetUserIncidents(token.access_token).ToString());
                }
                else
                {
                    return Unauthorized("Unauthorized");
                }
            }
            catch (Exception)
            {
                return BadRequest("Invalid request");
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
