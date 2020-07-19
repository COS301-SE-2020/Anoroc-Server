using System;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Anoroc_User_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        public UserManagementController()
        {

        }

        [HttpPost("CarrierStatus")]
        public string CarrierStatus([FromBody] Token token_object) {
            var returnString = token_object.Object_To_Server + "";
            return returnString;
        }
    }
}
