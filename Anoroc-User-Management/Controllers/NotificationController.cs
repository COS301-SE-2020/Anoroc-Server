using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;


namespace Anoroc_User_Management.Controllers
{
    /// <summary>
    /// API Endpoint for Notifications
    /// </summary>
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

        [HttpPost("Notification/RetrieveNotification")]
        public List<Notification> RetrieveNotification([FromBody] Token token_object)
        {
            if (UserManagementService.ValidateUserToken(token_object.access_token))
            {
                return _notificationService.getNotificationFromDatabase(token_object.access_token);
            }
            else
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage();
                responseMessage.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                return null;

                /*JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
                return JsonConvert.SerializeObject(Unauthorized(jsonConverter.Serialize("Unauthroized accessed")));*/
                // create http response set response to 401 unauthorize, return json converter.serlizeobject(http response message variable)
            }
        }

        [HttpGet("Notification/test")]
        public string test()
        {
            return "OK!";
        }






    }
    
 
}