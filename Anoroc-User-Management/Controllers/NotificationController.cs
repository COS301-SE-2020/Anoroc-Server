using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Controllers
{
    /// <summary>
    /// API Endpoint for Notifications
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class NotificationContoller : ControllerBase
    {
        
        private readonly IMobileMessagingClient _mobileMessagingClient;
        IDatabaseEngine _databaseEngine;
        IUserManagementService UserManagementService;
        NotificationService _notificationService;
        public NotificationContoller(IMobileMessagingClient mobileMessagingClient, IDatabaseEngine databaseEngine, IUserManagementService userManagementService)
        {
            _mobileMessagingClient = mobileMessagingClient;
            _databaseEngine = databaseEngine;
            _notificationService = new NotificationService(_databaseEngine);
            UserManagementService = userManagementService;
        }


        [HttpPost("RetrieveNotification")]
        public ObjectResult RetrieveNotification([FromBody] Token token_object)
        {
            if (UserManagementService.ValidateUserToken(token_object.access_token))
            {
                if (token_object.error_descriptions != "Integration Testing")
                {
                    var data = token_object.Object_To_Server;
                    Debug.WriteLine(JsonConvert.SerializeObject(token_object));


                    return Ok("Processing: ");
                }
                else
                {
                    HttpResponseMessage responseMessage = new HttpResponseMessage();
                    responseMessage.StatusCode = System.Net.HttpStatusCode.OK;
                    return Ok(JsonConvert.SerializeObject(responseMessage));
                }

            }
            else
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage();
                responseMessage.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                return Unauthorized((JsonConvert.SerializeObject(responseMessage)));

                /*JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
                return JsonConvert.SerializeObject(Unauthorized(jsonConverter.Serialize("Unauthroized accessed")));*/
                // create http response set response to 401 unauthorize, return json converter.serlizeobject(http response message variable)
            }
        }


        
        
    }
    
 
}