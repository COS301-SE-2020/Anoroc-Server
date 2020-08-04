using System;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Anoroc_User_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InterfaceController : ControllerBase
    {
        public InterfaceController()
        {
        }

        [HttpPost("Interface")]
        public String Interface([FromBody] Token token_object)
        {
            return token_object.Object_To_Server;
        }
    }
}
