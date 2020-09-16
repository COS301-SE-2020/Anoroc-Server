using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Models.TotalCarriers;
using Anoroc_User_Management.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Controllers
{

    [ApiController]
    public class PredictionContoller : ControllerBase
    {

        IDatabaseEngine _databaseEngine;
        PredictionService _predictionService;
        public PredictionContoller(IDatabaseEngine databaseEngine)
        {
            _databaseEngine = databaseEngine;
            _predictionService = new PredictionService();
        }

        [HttpGet("prediction/generate")]
        public string generateLocation()
        {

            //_mobileMessagingClient.SendNotification(new Location(new GeoCoordinate(5.5, 5.5)));

            _databaseEngine.Integrated_Populate();

            return "Location Generated";
        }

        [HttpGet("prediction/predict")]
        public string suburbPrediction()
        {

            //_mobileMessagingClient.SendNotification(new Location(new GeoCoordinate(5.5, 5.5)));

            Area temp = _databaseEngine.Get_Area_By_ID(1);
            _databaseEngine.Set_Totals(temp);
            var areas = _databaseEngine.Select_Unique_Areas();
            if (areas != null)
            {
                areas.ForEach(area =>
                {
                    Totals obj = _databaseEngine.Get_Totals(area);
                    _predictionService.predicateSuburbConfirmedViaDatabase(obj);
                });
            }

            return "Location Generated";
        }


        [HttpGet("test")]
        public string Test()
        {

            return "Notification Saved";

        }
    }

}
