using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        IDataService DataService;
        public DataController(IDataService dataService)
        {
            DataService = dataService;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet("SouthAfricaOverTime")]
        public async Task<IActionResult> SouthAfricaOverTime()
        {
            var response = await DataService.GetCasesPerDate();
            return Ok(response);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet("PredictAreas")]
        public IActionResult PredictAreas()
        {
            var response = DataService.PredictionAreas();
            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}