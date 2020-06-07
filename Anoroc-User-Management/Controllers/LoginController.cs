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
        public string Post()
        {
            Login newUser = new Login();
            newUser.Token = "yf8s7auiH&*DHuids89hsdua";
            return JsonSerializer.Serialize(newUser);
        }
    }
}