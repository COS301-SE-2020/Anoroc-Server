using System.Collections.Generic;
using System.Text.Json;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;


namespace Anoroc_User_Management.Controllers
{
    [ApiController]
    public class NotificationContoller : ControllerBase
    {
        [HttpGet("notification/all")] 
        public ActionResult<IEnumerable<Notification>> GetAll()
        {
            return new []
            {
                new Notification { description = "Alert: Risk Detected!" },
            };
        }
    }
    
 
}