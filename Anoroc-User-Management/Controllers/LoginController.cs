﻿using System;
using System.Collections;
using System.Text.Json;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Guid;


namespace Anoroc_User_Management.Controllers
{   
    /// <summary>
    ///     API Endpoint for accepting the user login from the app and returning a Token 
    /// </summary>
    // TODO: Decide on whether authorization to API is needed
    //[Authorize]
    [ApiController]
    [Route("userManagement/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IMobileMessagingClient _mobileMessagingClient;

        public LoginController(IMobileMessagingClient mobileMessagingClient)
        {
            _mobileMessagingClient = mobileMessagingClient;
        }

        [HttpPost]
        public string Post([FromBody] Login login)
        {
            _mobileMessagingClient.SendNotification(new Location(new GeoCoordinate(5.5, 5.5)));
            // TODO do the actual login
            // TODO ensure there's no token duplicate
            var g = NewGuid();
            var str = Convert.ToBase64String(g.ToByteArray());
            str = str.Replace("=","");
            login.Token = str.Replace("+","");
            return JsonSerializer.Serialize(login);
        }
    }
}