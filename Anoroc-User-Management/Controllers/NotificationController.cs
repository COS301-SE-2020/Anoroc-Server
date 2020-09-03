using System;
using System.Collections.Generic;
using System.Text.Json;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Mvc;


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
        NotificationService _notificationService;
        public NotificationContoller(IMobileMessagingClient mobileMessagingClient, IDatabaseEngine databaseEngine)
        {
            _mobileMessagingClient = mobileMessagingClient;
            _databaseEngine = databaseEngine;
            _notificationService = new NotificationService(_databaseEngine);
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

        
        
    }
    
 
}