using System;
using System.Collections;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Anoroc_User_Management.Controllers
{    
    // TODO: Decide on whether authorization to API is needed
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public string Post([FromBody] Login login)
        {
            Console.WriteLine(login.email);
            // Login newUser = new Login();
            // newUser.Token = "yf8s7auiH&*DHuids89hsdua";
            return JsonSerializer.Serialize(login);
        }
    }
}