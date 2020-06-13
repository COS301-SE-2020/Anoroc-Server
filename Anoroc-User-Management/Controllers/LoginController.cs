using System;
using System.Collections;
using System.Text.Json;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
            // BUG set the Token key otherwise unit test will fail
            return JsonSerializer.Serialize(login);
        }
    }
}