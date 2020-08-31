using System;
using System.Collections.Generic;
using System.Text.Json;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
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

        public NotificationContoller(IMobileMessagingClient mobileMessagingClient)
        {
            _mobileMessagingClient = mobileMessagingClient;
        }
        [HttpGet("notification/all")] 
        public string GetAll()
        {

            //_mobileMessagingClient.SendNotification(new Location(new GeoCoordinate(5.5, 5.5)));

            return "Notification sent";
        }

        
        
    }
    
 
}