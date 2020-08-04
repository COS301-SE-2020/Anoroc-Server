using System;
using System.Threading.Tasks;
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

        //TODO:
        //Change Return type: IActionResult
        //Return examples: 
        //- Ok() [you can just return this for now]
        //- Unauthorized()
        [HttpPost("Interface")]
        public IActionResult Interface([FromBody] Token token_object)
        {            
            return Ok(token_object.Object_To_Server);
        }
    }
}
