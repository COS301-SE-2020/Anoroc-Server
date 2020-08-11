using System;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Controllers
{
    //TODO: 
    //change return types of endpoints to RESTful return types
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
        public String CarrierStatus([FromBody] Token token_object)
        {
            if (DatabaseService.Validate_Access_Token(token_object.access_token))
            {
                DatabaseService.Update_Carrier_Status(token_object.access_token, token_object.Object_To_Server);
                var returnString = token_object.Object_To_Server + "";
                return JsonConvert.SerializeObject(Ok(returnString));
            }
            else
            {
                JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
                return JsonConvert.SerializeObject(Unauthorized(jsonConverter.Serialize("Unauthroized accessed")));
                // create http response set response to 401 unauthorize, return json converter.serlizeobject(http response message variable)
            }

        }

        [HttpPost("FirebaseToken")]
        public String FirebaseToken([FromBody] Token token_object)
        {
            if (DatabaseService.Validate_Access_Token(token_object.access_token))
            {
                DatabaseService.Insert_Firebase_Token(token_object.access_token, token_object.Object_To_Server);

                var returnString = token_object.Object_To_Server + "";
                return JsonConvert.SerializeObject(Ok(returnString));

            }
            else
            {
                JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
                return JsonConvert.SerializeObject(Unauthorized(jsonConverter.Serialize("Unauthroized accessed")));
                // create http response set response to 401 unauthorize, return json converter.serlizeobject(http response message variable)
            }

        }
    }
}
