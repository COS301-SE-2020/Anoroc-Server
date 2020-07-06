using System;
using System.Collections;
using System.Text.Json;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Anoroc_User_Management.Controllers
{    
    /// <summary>
    /// API Endpoint for users registering an Anoroc Account instead of the Social Media Logins
    /// </summary>
    // TODO: Decide on whether authorization to API is needed
    //[Authorize]
    [ApiController]
    [Route("userManagement/[controller]")]
    public class RegisterController : ControllerBase
    {
        [HttpPost]
        public string Post([FromBody] Register register)
        {
            register.Token = "yf8s7auiH&*DHuids89hsdua";
            return JsonSerializer.Serialize(register);
        }
    }
}