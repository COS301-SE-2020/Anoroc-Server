using System;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Anoroc_User_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        IDatabaseEngine DatabaseService;
        public UserManagementController(IDatabaseEngine database)
        {
            DatabaseService = database;
        }

        [HttpPost("CarrierStatus")]
        public string CarrierStatus([FromBody] Token token_object) {
            var returnString = token_object.Object_To_Server + "";
            return returnString;
        }

        [HttpPost("FirebaseToken")]
        public string FirebaseToken([FromBody] Token token_object)
        {

            DatabaseService.InsertFirebaseToken(token_object.access_token, (token_object.Object_To_Server+""));

            var returnString = token_object.Object_To_Server + "";
            return returnString;
        }
    }
}
