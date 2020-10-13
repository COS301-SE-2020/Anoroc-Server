using System;
using System.Collections.Generic;
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
    [ApiController]
    public class NotificationContoller : ControllerBase
    {
        
        private readonly IMobileMessagingClient _mobileMessagingClient;
        IDatabaseEngine _databaseEngine;
        INotificationService _notificationService;
        IUserManagementService _userService;
        public NotificationContoller(IMobileMessagingClient mobileMessagingClient, IDatabaseEngine databaseEngine, IUserManagementService userEngine, INotificationService notificationEngine)
        {
            _mobileMessagingClient = mobileMessagingClient;
            _databaseEngine = databaseEngine;
            _notificationService = notificationEngine;
            _userService = userEngine;

        }
        [HttpGet("notification/test")] 
        public string GetAll()
        {

            //_mobileMessagingClient.SendNotification(new Location(new GeoCoordinate(5.5, 5.5)));

            
            string access_Token = _databaseEngine.Get_Access_Token_Via_FirebaseToken("c_UAmMUOemE:APA91bFBcMx7y7oLTxebLLVQq2X9bvcM34rwohGVLjWw_fFQw_Q2Ei2_rPWtxcNCXQ2Bn4h4TV8GjYV8cC3m1EM6QuN1pXp3BO7qAjndZrnjtmDQ3hxNfzAP3VebmjPuseNNzMKHg_Gs");
            //Creating notification object
            string body = "You have come into contact with a carrier. Click for more info";
            string title = "Contagion Encounter";
            Anoroc_User_Management.Models.Notification save = new Anoroc_User_Management.Models.Notification(access_Token, title, body);
            //Saving notification object with access token to the database.
            _notificationService.SaveNotificationToDatabase(save);

            return "Notification Saved";
        }

        [HttpPost("notification/save")]
        public void saveNotification([FromBody] Token token_object)
        {
            var temp = JsonConvert.DeserializeObject<Notification>(token_object.Object_To_Server);
            _notificationService.SaveNotificationToDatabase(temp);

        }

        [HttpPost("allNotification/get")]
        public IActionResult getAllNotification([FromBody] Token token_object)
        {

            /*string accessToken = _databaseEngine.Get_Access_Token_Via_FirebaseToken(token_object.access_token);

            t = _databaseEngine.Get_All_Notifications_Of_User(accessToken);*/
            if(_userService.ValidateUserToken(token_object.access_token))
            {
                return Ok(JsonConvert.SerializeObject(_notificationService.SendNotificationToApp(token_object.access_token)));
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
            /*var wtf = JsonConvert.DeserializeObject<Notification>(token_object.Object_To_Server);
            _notificationService.SendNotificationToApp(token_object.access_token); 
            var result = JsonConvert.SerializeObject(wtf);

            return result;
            */
        }


    }
    
 
}