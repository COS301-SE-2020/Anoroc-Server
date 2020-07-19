using System;
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
        public string CarrierStatus([FromBody]string status) {
            return status;
        }
    }
}
