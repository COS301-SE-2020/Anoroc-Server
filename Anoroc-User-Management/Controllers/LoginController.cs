using System;
using System.Collections;
using System.Text.Json;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Guid;


namespace Anoroc_User_Management.Controllers
{    
    // TODO: Decide on whether authorization to API is needed
    //[Authorize]
    [ApiController]
    [Route("userManagement/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public string Post([FromBody] Login login)
        {
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