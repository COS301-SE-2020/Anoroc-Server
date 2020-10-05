using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly string XamarinKey;
        IUserManagementService UserManagementService;
        public UserManagementController(IUserManagementService userManagementService, IConfiguration configurationManager)
        {
            XamarinKey = configurationManager["XamarinKey"];
            UserManagementService = userManagementService;
        }

        [HttpPost("DownloadUserData")]
        public IActionResult DownloadUserData([FromBody] Token token)
        {
            if(UserManagementService.ValidateUserToken(token.access_token))
            {
                if (UserManagementService.SendData(token.access_token))
                    return Ok("Email Sent.");
                else
                    return Ok("Failed to send.");
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
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
                    var image = token.Object_To_Server;
                    UserManagementService.SaveProfileImage(token.access_token, image);
                    return Ok("Ok");
                }
                catch(Exception)
                {
                    return BadRequest("Invalid request");
                }
            }
            else
                return Unauthorized("Invalid token");
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

        [HttpPost("ToggleAnonmity")]
        public IActionResult ToggleAnonmity([FromBody] Token token)
        {
            if(UserManagementService.ValidateUserToken(token.access_token))
            { 
                return Ok("Anonomity set to: " + UserManagementService.ToggleUserAnonomity(token.access_token));
            }
            else
            {
                return Unauthorized("Unauthorized.");
            }
        }

        [HttpPost("UserLoggedIn")]
        public IActionResult UserLoggedIn([FromBody] Token token)
        {
            var user2 = new User();
            user2.FirstName = "Andrew";
            user2.UserSurname = "Wilson";
            user2.Email = "u15191223@tuks.co.za";
            Debug.WriteLine(JsonConvert.SerializeObject(user2));
            try
            {
                if (Request.Headers.ContainsKey("X-XamarinKey"))
                {
                    if (UserManagementService.CheckXamarinKey(Request.Headers["X-XamarinKey"]))
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
                    else
                    {
                        return Unauthorized("Unauthorized");
                    }
                }
                else
                {
                    return Unauthorized("Unauthorized");
                }
            }
            catch(Exception e)
            {
                return BadRequest("Invalid object");
            }
        }
    }
}
